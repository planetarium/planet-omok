using System.Linq;
using Libplanet;
using LibplanetUnity;
using LibplanetUnity.Action;
using Omok.Action;

namespace Omok.BlockChain
{
    /// <summary>
    /// 게임의 Action을 생성하고 Agent에 넣어주는 역할을 한다.
    /// </summary>
    public class ActionManager : MonoSingleton<ActionManager>
    {
        private static void ProcessAction(GameAction action)
        {
            Agent.instance.MakeTransaction(new[] { action }.Cast<ActionBase>());
        }

        #region Actions

        public void JoinSesion(string sessionID)
        {
            var action = new JoinSession()
            {
                SessionID = sessionID,
            };
            ProcessAction(action);
        }

        public void Place(int index)
        {
            var action = new PlaceAction()
            {
                Index = index,
            };
            ProcessAction(action);
        }
        
        #endregion
    }
}
