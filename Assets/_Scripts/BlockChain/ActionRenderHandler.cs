using System;
using System.Collections.Generic;
using System.IO;
using Nekoyume.Action;
using Nekoyume.State;
using UniRx;
using UnityEngine;

namespace Nekoyume.BlockChain
{
    /// <summary>
    /// 현상태 : 각 액션의 랜더 단계에서 즉시 게임 정보에 반영시킴. 아바타를 선택하지 않은 상태에서 이전에 성공시키지 못한 액션을 재수행하고
    ///       이를 핸들링하면, 즉시 게임 정보에 반영시길 수 없기 때문에 에러가 발생함.
    /// 참고 : 이후 언랜더 처리를 고려한 해법이 필요함.
    /// 해법 1: 랜더 단계에서 얻는 `eval` 자체 혹은 변경점을 queue에 넣고, 게임의 상태에 따라 꺼내 쓰도록.
    ///
    /// ToDo. `ActionRenderHandler`의 형태가 완성되면, `ActionUnrenderHandler`도 작성해야 함.
    /// </summary>
    public class ActionRenderHandler
    {
        private static class Singleton
        {
            internal static readonly ActionRenderHandler Value = new ActionRenderHandler();
        }

        public static readonly ActionRenderHandler Instance = Singleton.Value;

        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        private ActionRenderHandler()
        {
        }

        public void Start()
        {
            LoadLocalAvatarState();
            HackAndSlash();
        }

        public void Stop()
        {
            _disposables.DisposeAllAndClear();
        }
        
        private bool ValidateEvaluationForAgentState<T>(ActionBase.ActionEvaluation<T> evaluation) where T : ActionBase
        {
            if (States.Instance.agentState.Value == null)
            {
                return false;
            }
            return evaluation.OutputStates.UpdatedAddresses.Contains(States.Instance.agentState.Value.address);
        }

        private bool ValidateEvaluationForCurrentAvatarState<T>(ActionBase.ActionEvaluation<T> evaluation)
            where T : ActionBase =>
            !(States.Instance.currentAvatarState.Value is null)
            && evaluation.OutputStates.UpdatedAddresses.Contains(States.Instance.currentAvatarState.Value.address);

        private AgentState GetAgentState<T>(ActionBase.ActionEvaluation<T> evaluation) where T : ActionBase
        {
            var agentAddress = States.Instance.agentState.Value.address;
            return (AgentState) evaluation.OutputStates.GetState(agentAddress);
        }

        private void UpdateAgentState<T>(ActionBase.ActionEvaluation<T> evaluation) where T : ActionBase
        {
            Debug.LogFormat("Called UpdateAgentState<{0}>. Updated Addresses : `{1}`", evaluation.Action,
                string.Join(",", evaluation.OutputStates.UpdatedAddresses));
            var state = GetAgentState(evaluation);
            UpdateAgentState(state);
        }

        private void UpdateAvatarState<T>(ActionBase.ActionEvaluation<T> evaluation, int index) where T : ActionBase
        {
            Debug.LogFormat("Called UpdateAvatarState<{0}>. Updated Addresses : `{1}`", evaluation.Action,
                string.Join(",", evaluation.OutputStates.UpdatedAddresses));
            if (!States.Instance.agentState.Value.avatarAddresses.ContainsKey(index))
            {
                States.Instance.avatarStates.Remove(index);
                AvatarManager.DeleteAvatarPrivateKey(index);
                return;
            }

            var avatarAddress = States.Instance.agentState.Value.avatarAddresses[index];
            var avatarState = (AvatarState) evaluation.OutputStates.GetState(avatarAddress);
            if (avatarState == null)
            {
                return;
            }

            UpdateAvatarState(avatarState, index);
        }
        
        private void UpdateCurrentAvatarState<T>(ActionBase.ActionEvaluation<T> evaluation) where T : ActionBase
        {
            UpdateAvatarState(evaluation, States.Instance.currentAvatarKey.Value);
        }

        private void HackAndSlash()
        {
            ActionBase.EveryRender<HackAndSlash>()
                .Where(ValidateEvaluationForCurrentAvatarState)
                .ObserveOnMainThread()
                .Subscribe(UpdateCurrentAvatarState).AddTo(_disposables);
        }

        private static void UpdateAvatarState(AvatarState avatarState, int index)
        {

            if (States.Instance.avatarStates.ContainsKey(index))
            {
                // 게임에서 업데이트한 아바타 상태를 따라잡았을때만 업데이트
                if (avatarState.BlockIndex >= States.Instance.avatarStates[index].BlockIndex)
                    States.Instance.avatarStates[index] = avatarState;
            }
            else
            {
                States.Instance.avatarStates.Add(index, avatarState);
            }
        }

        private static void UpdateAgentState(AgentState state)
        {
            States.Instance.agentState.Value = state;
        }

        public static void UpdateLocalAvatarState(AvatarState avatarState, int index)
        {
            Debug.LogFormat("Update local avatarState. agentAddress: {0} address: {1} BlockIndex: {2}",
                avatarState.agentAddress, avatarState.address, avatarState.BlockIndex);
            UpdateAvatarState(avatarState, index);
        }

        public static void UpdateLocalAgentState(AgentState agentState)
        {
            Debug.LogFormat("Update local agentSTate. agentAddress: {0} BlockIndex: {1}",
                agentState.address, AgentController.Agent.BlockIndex);
            UpdateAgentState(agentState);
        }

        private void LoadLocalAvatarState()
        {
            if (!(States.Instance.agentState?.Value is null))
            {
                foreach (var avatarAddress in States.Instance.agentState.Value.avatarAddresses)
                {
                    var fileName = string.Format(States.CurrentAvatarFileNameFormat, States.Instance.agentState.Value.address,
                        avatarAddress.Value);
                    var path = Path.Combine(Application.persistentDataPath, fileName);
                    if (File.Exists(path))
                    {
                        var avatarState =
                            ByteSerializer.Deserialize<AvatarState>(File.ReadAllBytes(path));
                        Debug.LogFormat("Load local avatarState. agentAddress: {0} address: {1} BlockIndex: {2}",
                            avatarState.agentAddress, avatarState.address, avatarState.BlockIndex);
                        UpdateLocalAvatarState(avatarState, avatarAddress.Key);
                        File.Delete(path);
                    }
                }
            }
        }
    }
}
