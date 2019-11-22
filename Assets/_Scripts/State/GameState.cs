using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Bencodex.Types;
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

        public string SessionID;
        public int Turn;
        public Address Winner;
        public List<Address> Players;
        public List<int> GameBoard;

        public GameState(string sessionID) : base(Address)
        {
            SessionID = sessionID;
            Turn = 0;
            Winner = default(Address);
            Players = new List<Address>();
            GameBoard = Enumerable.Repeat(-1, 13 * 14).ToList();
        }

        public GameState(Bencodex.Types.Dictionary bdict) : base(Address)
        {
            var rawSessionID = (Bencodex.Types.Text) bdict["sessionID"];
            var rawTurn = (Integer) bdict["turn"];
            var rawWinner = (Bencodex.Types.Binary) bdict["winner"];
            var rawPlayers = (Bencodex.Types.List) bdict["players"];
            var rawGameBoard = (Bencodex.Types.List) bdict["gameBoard"];
            SessionID = rawSessionID.Value;
            Turn = rawTurn.ToInt();
            Winner = new Address(rawWinner);
            Players = rawPlayers.ToList(value => new Address((Binary)value));
            GameBoard = rawGameBoard.ToList(value => value.ToInt());
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
        
        public override IValue Serialize() =>
            new Bencodex.Types.Dictionary(new Dictionary<IKey, IValue>
            {
                [(Text) "sessionID"] = (Text) SessionID,
                [(Text) "turn"] = (Integer) Turn,
                [(Text) "winner"] = Winner.Serialize(),
                [(Text) "players"] = 
                    new Bencodex.Types.List(Players.Select(value => value.Serialize())),
                [(Text) "gameBoard"] =
                    new Bencodex.Types.List(GameBoard.Select(value => (IValue)((Integer) value))),
            }.Union((Bencodex.Types.Dictionary) base.Serialize()));
    }
}
