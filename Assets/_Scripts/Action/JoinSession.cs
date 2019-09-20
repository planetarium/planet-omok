using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Libplanet;
using Libplanet.Action;
using Nekoyume.State;

namespace Nekoyume.Action
{
    [ActionType("join_session")]
    public class JoinSession : GameAction
    {
        public string sessionID;

        public JoinSession()
        {
            type = ActionType.JoinSession;
        }

        protected override IImmutableDictionary<string, object> PlainValueInternal =>
            new Dictionary<string, object>
            {
                ["sessionID"] = ByteSerializer.Serialize(sessionID),
            }.ToImmutableDictionary();


        protected override void LoadPlainValueInternal(IImmutableDictionary<string, object> plainValue)
        {
            sessionID = ByteSerializer.Deserialize<string>((byte[])plainValue["sessionID"]);
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

            return states.SetState(SessionState.Address, sessionState);
        }
    }
}
