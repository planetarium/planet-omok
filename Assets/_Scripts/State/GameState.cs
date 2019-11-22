using System;
using System.Collections.Generic;
using System.Data;
using Libplanet;

namespace Omok.State
{
    /// <summary>
    /// Agent의 상태 모델이다.
    /// </summary>
    [Serializable]
    public class GameState : State, ICloneable
    {
        // FIXME: This should be distinguishable for each session
        public static readonly Address Address = new Address(new byte[]
            {
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1
            }
        );

        public int turnCount = 0;
        public readonly Dictionary<int, Address> players;
        public readonly List<int> gameBoard;

        public GameState(Address address) : base(address)
        {
            players = new Dictionary<int, Address>();
            gameBoard = new List<int>();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
