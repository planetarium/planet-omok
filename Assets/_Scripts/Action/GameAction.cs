using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bencodex.Types;
using LibplanetUnity.Action;

namespace Omok.Action
{
    [Serializable]
    public abstract class GameAction : ActionBase
    {
        public string SessionID;

        public override IValue PlainValue =>
            new Bencodex.Types.Dictionary(
                PlainValueInternal
                    .SetItem("sessionID", (Text) SessionID)
                    .Select(kv => new KeyValuePair<IKey, IValue>((Text) kv.Key, kv.Value))
            );
        protected abstract IImmutableDictionary<string, IValue> PlainValueInternal { get; }

        public GameAction()
        {
        }

        public override void LoadPlainValue(IValue plainValue)
        {
            var dict = ((Bencodex.Types.Dictionary) plainValue)
                .Select(kv => new KeyValuePair<string, IValue>((Text) kv.Key, kv.Value))
                .ToImmutableDictionary();
            SessionID = ((Text) dict["sessionID"]).Value;
            LoadPlainValueInternal(dict);
        }
        
        protected abstract void LoadPlainValueInternal(IImmutableDictionary<string, IValue> plainValue);
    }
}
