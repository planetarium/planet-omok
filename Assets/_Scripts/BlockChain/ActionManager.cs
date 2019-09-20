using System;
using System.Collections.Generic;
using System.Linq;
using Libplanet;
using Libplanet.Action;
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

        public void JoinSession(
            string a_sessionID, EventHandler<IActionContext> handler)
        {
            var action = new JoinSession
            {
                sessionID = a_sessionID
            };
            ProcessAction(action);

            ActionBase.AddRenderHandler(ActionBase.ActionType.JoinSession, handler);
        }
        #endregion
    }
}
