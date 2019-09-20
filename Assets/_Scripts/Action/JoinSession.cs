using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libplanet.Action;

namespace Nekoyume.Action
{
    class JoinSession : GameAction
    {
        protected override IImmutableDictionary<string, object> PlainValueInternal => throw new NotImplementedException();

        public override IAccountStateDelta Execute(IActionContext ctx)
        {
            throw new NotImplementedException();
        }

        protected override void LoadPlainValueInternal(IImmutableDictionary<string, object> plainValue)
        {
            throw new NotImplementedException();
        }
    }
}
