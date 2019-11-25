using System.Collections.Immutable;
using Bencodex.Types;
using Libplanet.Action;
using LibplanetUnity;
using LibplanetUnity.Action;
using Omok.State;

namespace Omok.Action
{
    [ActionType("join_session")]
    public class JoinSession : GameAction
    {
        protected override IImmutableDictionary<string, IValue> PlainValueInternal =>
            ImmutableDictionary<string, IValue>.Empty;

        public JoinSession()
        {
        }

        protected override void LoadPlainValueInternal(IImmutableDictionary<string, IValue> plainValue)
        {
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

            if (sessionState.sessions.ContainsKey(SessionID))
            {
                sessionState.sessions[SessionID].Players.Add(ctx.Signer);
            }
            else
            {
                var gameState = new GameState(SessionID);
                gameState.Players.Add(ctx.Signer);
                sessionState.sessions.Add(SessionID, gameState);
            }
            GameManager.instance.currentSession = SessionID;
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
                GameManager.instance.sessionUI?.UpdateUI(sessionState, SessionID);
            });
        }

        public override void Unrender(IActionContext context, IAccountStateDelta nextStates)
        {
        }
    }
}
