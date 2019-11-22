using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bencodex.Types;
using Nekoyume.State;

namespace Nekoyume.Action
{
    [Serializable]
    public abstract class GameAction : ActionBase
    {
        public string sessionID;
        public Guid Id { get; private set; }

        public override IValue PlainValue =>
            new Bencodex.Types.Dictionary(
                PlainValueInternal
                    .SetItem("id", Id.Serialize())
                    .SetItem("sessionID", (Text) sessionID)
                    .Select(kv => new KeyValuePair<IKey, IValue>((Text) kv.Key, kv.Value))
            );
        protected abstract IImmutableDictionary<string, IValue> PlainValueInternal { get; }

        protected GameAction(string sessionID)
        {
            Id = Guid.NewGuid();
            this.sessionID = sessionID;
        }

        public override void LoadPlainValue(IValue plainValue)
        {
            var dict = ((Bencodex.Types.Dictionary) plainValue)
                .Select(kv => new KeyValuePair<string, IValue>((Text) kv.Key, kv.Value))
                .ToImmutableDictionary();
            Id = dict["id"].ToGuid();
            sessionID = ((Text) dict["sessionID"]).Value;
            LoadPlainValueInternal(dict);
        }
        
        protected abstract void LoadPlainValueInternal(IImmutableDictionary<string, IValue> plainValue);
    }
}
