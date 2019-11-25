using System;
using System.Collections.Generic;
using System.Linq;
using Bencodex.Types;
using Libplanet;

namespace Omok.State
{
    /// <summary>
    /// 세션의 상태 모델이다.
    /// </summary>
    [Serializable]
    public class SessionState : State, ICloneable
    {
        public static readonly Address Address = new Address(new byte[]
            {
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0
            }
        );

        public readonly Dictionary<string, GameState> sessions = new Dictionary<string, GameState>();

        public SessionState() : base(Address)
        {

        }

        public SessionState(Bencodex.Types.Dictionary bdict) : base(Address)
        {
            var rawSessions = (Bencodex.Types.Dictionary) bdict["sessions"];
            sessions = rawSessions.ToDictionary(
                kv => kv.Key.ToString(),
                kv => new GameState((Bencodex.Types.Dictionary) kv.Value)
            );
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
        
        public override IValue Serialize() =>
            new Bencodex.Types.Dictionary(new Dictionary<IKey, IValue>
            {
                [(Text) "sessions"] = new Bencodex.Types.Dictionary(sessions.Select(kv =>
                    new KeyValuePair<IKey, IValue>(
                        (Text) kv.Key,
                        kv.Value.Serialize()
                    )
                ))
            }.Union((Bencodex.Types.Dictionary) base.Serialize()));
    }
}
