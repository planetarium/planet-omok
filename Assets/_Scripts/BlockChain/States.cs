using UnityEngine;
using Omok.State;

namespace Omok.BlockChain
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

        public SessionState SessionState;

        private States()
        {
        }
    }
}
