using UnityEngine;
using Nekoyume.State;
using UniRx;

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

        public readonly ReactiveProperty<AgentState> agentState = new ReactiveProperty<AgentState>();
        public readonly ReactiveProperty<SessionState> sessionState = new ReactiveProperty<SessionState>();

        private States()
        {
            sessionState.Subscribe(SubscribeSession);
        }

        private void SubscribeAgent(AgentState value)
        {
            
        }

        private void SubscribeSession(SessionState state)
        {
            if (state is null)
                return;
            Debug.LogWarning(state.sessions[GameManager.instance.currentSession].Count);
        }
    }
}
