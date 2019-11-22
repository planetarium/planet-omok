using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Bencodex.Types;
using Libplanet;
using Libplanet.Action;
using LibplanetUnity;
using LibplanetUnity.Action;
using Nekoyume.State;
using UnityEngine;

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

        public JoinSession(string sessionID) : base(sessionID)
        {
            
        }


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

            SessionState sessionState;
            if(states.TryGetState(SessionState.Address, out Bencodex.Types.Dictionary bdict))
            {
                sessionState = new SessionState(bdict);
            }
            else
            {
                sessionState = new SessionState();
            }

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

        public override void Render(IActionContext context, IAccountStateDelta nextStates)
        {
            SessionState sessionState;
            if(nextStates.TryGetState(SessionState.Address, out Bencodex.Types.Dictionary bdict))
            {
                sessionState = new SessionState(bdict);
            }
            else
            {
                sessionState = new SessionState();
            }
            
            Agent.instance.RunOnMainThread(() => 
            {
                GameManager.instance.sessionUI.UpdateUI(sessionState, sessionID);
            });
        }

        public override void Unrender(IActionContext context, IAccountStateDelta nextStates)
        {
        }
    }
}
