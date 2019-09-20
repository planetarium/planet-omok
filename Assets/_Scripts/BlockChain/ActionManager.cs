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

        public IObservable<ActionBase.ActionEvaluation<JoinSession>> JoinSesion(
            string a_sessionID)
        {
            var action = new JoinSession
            {
                sessionID = a_sessionID,
            };
            ProcessAction(action);

            return ActionBase.EveryRender<JoinSession>()
                .SkipWhile(eval => !eval.Action.Id.Equals(action.Id))
                .Take(1)
                .Last()
                .ObserveOnMainThread();
        }
        #endregion
    }
}
