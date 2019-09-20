using System.IO;
using Nekoyume.Action;
using Nekoyume.State;
using UniRx;
using UnityEngine;

namespace Nekoyume.BlockChain
{
    /// <summary>
    /// 각 주소 고유의 상태들을 모아서 데이터 지역성을 확보한다.
    /// </summary>
    public class States
    {
        private static class Singleton
        {
            internal static readonly States Value = new States();

            static Singleton()
            {
            }
        }

        public static readonly States Instance = Singleton.Value;

        public SessionState sessionState;
        public AgentState agentState;
        public GameState gameState;

        private States()
        {
            sessionState = new SessionState();
        }

    }
}
