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
            SubscribeJoinSession();
        }

        public void Stop()
        {
            _disposables.DisposeAllAndClear();
        }

        private AgentState GetAgentState<T>(ActionBase.ActionEvaluation<T> evaluation) where T : ActionBase
        {
            var agentAddress = States.Instance.agentState.Value.address;
            return (AgentState)evaluation.OutputStates.GetState(agentAddress);
        }

        private void SubscribeJoinSession()
        {
            ActionBase.EveryRender<JoinSession>()
                .ObserveOnMainThread()
                .Subscribe(UpdateSessionState).AddTo(_disposables);
        }

        private static void UpdateSessionState(ActionBase.ActionEvaluation<JoinSession> action)
        {
            States.Instance.sessionState.Value = (SessionState)action.OutputStates.GetState(SessionState.Address);
        }
    }
}
