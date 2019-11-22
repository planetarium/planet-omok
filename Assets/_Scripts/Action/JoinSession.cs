using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Bencodex.Types;
using Libplanet;
using Libplanet.Action;
using Nekoyume.State;

namespace Nekoyume.Action
{
    [ActionType("join_session")]
    public class JoinSession : GameAction
    {
        public string sessionID;

        protected override IImmutableDictionary<string, IValue> PlainValueInternal =>
            new Dictionary<string, IValue>
            {
                ["sessionID"] = new Bencodex.Types.Text(sessionID),
            }.ToImmutableDictionary();


        protected override void LoadPlainValueInternal(IImmutableDictionary<string, IValue> plainValue)
        {
            sessionID = ((Bencodex.Types.Text) plainValue["sessionID"]).Value;
        }

        public override IAccountStateDelta Execute(IActionContext ctx)
        {
            var states = ctx.PreviousStates;

            if (ctx.Rehearsal)
            {
                states = states.SetState(SessionState.Address, MarkChanged);
                return states.SetState(ctx.Signer, MarkChanged);
            }

            var sessionState = (SessionState)states.GetState(SessionState.Address) ?? new SessionState();
            if (sessionState.sessions.ContainsKey(sessionID))
            {
                sessionState.sessions[sessionID].Add(ctx.Signer);
            }
            else
            {
                sessionState.sessions.Add(sessionID, new List<Address> { ctx.Signer });
            }
            GameManager.instance.currentSession = sessionID;
            return states.SetState(SessionState.Address, sessionState.Serialize());
        }
    }
}
