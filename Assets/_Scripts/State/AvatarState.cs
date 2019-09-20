using System;
using System.Collections.Generic;
using Libplanet;

namespace Nekoyume.State
{
    /// <summary>
    /// Agent가 포함하는 각 Avatar의 상태 모델이다.
    /// </summary>
    [Serializable]
    public class AvatarState : State, ICloneable
    {
        public string name;
        public int characterId;
        public int level;
        public long exp;
        public int worldStage;
        public DateTimeOffset updatedAt;
        public DateTimeOffset? clearedAt;
        public Address agentAddress;
        public long BlockIndex;

        public AvatarState(Address address, Address agentAddress, long blockIndex, string name = null) : base(address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));                
            }
            
            this.name = name ?? "";
            level = 1;
            exp = 0;
            worldStage = 1;
            updatedAt = DateTimeOffset.UtcNow;
            this.agentAddress = agentAddress;
            BlockIndex = blockIndex;
        }
        
        public AvatarState(AvatarState avatarState) : base(avatarState.address)
        {
            if (avatarState == null)
            {
                throw new ArgumentNullException(nameof(avatarState));
            }
            
            name = avatarState.name;
            characterId = avatarState.characterId;
            level = avatarState.level;
            exp = avatarState.exp;
            worldStage = avatarState.worldStage;
            updatedAt = avatarState.updatedAt;
            clearedAt = avatarState.clearedAt;
            agentAddress = avatarState.agentAddress;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
