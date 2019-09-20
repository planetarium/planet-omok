using System;
using System.Collections.Generic;
using System.Linq;
using Libplanet;
using Nekoyume.Action;
using UniRx;

namespace Nekoyume.BlockChain
{
    /// <summary>
    /// 게임의 Action을 생성하고 Agent에 넣어주는 역할을 한다.
    /// </summary>
    public class ActionManager : MonoSingleton<ActionManager>
    {
        private static void ProcessAction(GameAction action)
        {
            AgentController.Agent.EnqueueAction(action);
        }
        
        #region Actions
        
        public IObservable<ActionBase.ActionEvaluation<HackAndSlash>> HackAndSlash(
            int stage)
        {
            var action = new HackAndSlash
            {
                stage = stage,
                avatarAddress = States.Instance.currentAvatarState.Value.address,
            };
            ProcessAction(action);

            return ActionBase.EveryRender<HackAndSlash>()
                .SkipWhile(eval => !eval.Action.Id.Equals(action.Id))
                .Take(1)
                .Last()
                .ObserveOnMainThread();
        }
        #endregion
    }
}
