using System;
using System.Collections.Immutable;
using Bencodex.Types;
using Libplanet;
using Libplanet.Action;
using LibplanetUnity;
using LibplanetUnity.Action;
using Omok.State;
using UnityEngine;

namespace Omok.Action
{
    [ActionType("resign")]
    public class ResignAction : GameAction
    {
        public int Index;

        protected override IImmutableDictionary<string, IValue> PlainValueInternal =>
            ImmutableDictionary<string, IValue>.Empty;

        public ResignAction()
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

            GameState gameState;
            SessionState sessionState;
            if (states.TryGetState(SessionState.Address, out Bencodex.Types.Dictionary bdict))
            {
                sessionState = new SessionState(bdict);
                sessionState.sessions.TryGetValue(SessionID, out gameState);
            }
            else
            {
                throw new Exception("SessionState should not be empty.");
            }

            if (gameState is null)
            {
                Debug.LogError("GameState does not exists.");
                return states;
            }

            if (!gameState.Players.Contains(ctx.Signer))
            {
                Debug.Log("Signer does not belongs to session.");
            }
            else if (gameState.Winner != default(Address))
            {
                Debug.Log("Game already finished.");
            }
            else
            {
                var winnerIndex = gameState.Players[0] == ctx.Signer ? 1 : 0;
                gameState.Winner = gameState.Players[winnerIndex];
                sessionState.sessions[SessionID] = gameState;
            }
            return states.SetState(SessionState.Address, sessionState.Serialize());
        }

        public override void Render(IActionContext context, IAccountStateDelta nextStates)
        {
            GameState gameState;
            if (nextStates.TryGetState(SessionState.Address, out Bencodex.Types.Dictionary bdict))
            {
                var sessionState = new SessionState(bdict);
                sessionState.sessions.TryGetValue(SessionID, out gameState);
            }
            else
            {
                Debug.LogError("SessionState is empty on render.");
                return;
            }

            if (gameState is null)
            {
                Debug.LogError("GameState is null on rendering.");
                return;
            }

            if (gameState.SessionID == GameManager.instance.currentSession)
            {
                Agent.instance.RunOnMainThread(() =>
                {
                    Debug.LogWarning("Render Resign Action");
                    GameManager.instance.gameboard?.SetResult(gameState.Winner == Agent.instance.Address);
                });
            }
        }

        public override void Unrender(IActionContext context, IAccountStateDelta nextStates)
        {
        }
    }
}
