using System;
using System.Collections.Generic;
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
