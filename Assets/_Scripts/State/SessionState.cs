using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bencodex.Types;
using Libplanet;

namespace Nekoyume.State
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

        public readonly Dictionary<string, List<Address>> sessions = new Dictionary<string, List<Address>>();

        public SessionState() : base(Address)
        {

        }

        public SessionState(Bencodex.Types.Dictionary bdict) : base(Address)
        {
            var dict = bdict
                   .Select(kv => new KeyValuePair<string, IValue>((Text)kv.Key, kv.Value))
                   .ToImmutableDictionary();
            sessions = new Dictionary<string, List<Address>>();
            ((Bencodex.Types.Dictionary)dict["sessions"]).Select(kv =>
                 sessions[((Bencodex.Types.Text)kv.Key).Value] = 
                    ((Bencodex.Types.List)kv.Value).Select(addr => addr.ToAddress()).ToList()
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
                        kv.Value.Select(addr => addr.Serialize()).Serialize()
                    )
                ))
            }.Union((Bencodex.Types.Dictionary) base.Serialize()));
    }
}
