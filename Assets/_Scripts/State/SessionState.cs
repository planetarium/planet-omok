using System;
using System.Collections.Generic;
using System.Data;
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

        public readonly Dictionary<string, List<Address>> sessions;

        public SessionState() : base(Address)
        {
            sessions = new Dictionary<string, List<Address>>();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
