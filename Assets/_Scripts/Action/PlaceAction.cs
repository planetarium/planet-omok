using System;
using System.Collections.Generic;
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
    [ActionType("place")]
    public class PlaceAction : GameAction
    {
        public int Index;
        
        protected override IImmutableDictionary<string, IValue> PlainValueInternal =>
            new Dictionary<string, IValue>()
            {
                ["index"] = (Integer)Index,
            }.ToImmutableDictionary();

        public PlaceAction()
        {
        }

        protected override void LoadPlainValueInternal(IImmutableDictionary<string, IValue> plainValue)
        {
            Index = ((Integer)plainValue["index"]).ToInt();
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
            else if (gameState.Players[gameState.Turn] != ctx.Signer)
            {
                Debug.Log("Not signer's turn.");
            }
            else if (gameState.GameBoard[Index] != -1)
            {
                Debug.Log($"Already placed on that node. {Index}");
            }
            else if (gameState.Winner != default(Address))
            {
                Debug.Log("Game already finished.");
            }
            else
            {
                var playerIndex = gameState.Players[0] == ctx.Signer ? 0 : 1;
                gameState.GameBoard[Index] = playerIndex;
                gameState.Turn = 1 - gameState.Turn;
                sessionState.sessions[SessionID] = gameState;
            }
            return states.SetState(SessionState.Address, sessionState.Serialize());
        }

        public override void Render(IActionContext context, IAccountStateDelta nextStates)
        {
            GameState gameState;
            if(nextStates.TryGetState(SessionState.Address, out Bencodex.Types.Dictionary bdict))
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
                    GameManager.instance.gameboard?.PlaceNode(false, gameState.Turn, Index);
                    GameManager.instance.SetMyTurn(gameState.Turn == gameState.Players.IndexOf(Agent.instance.Address));
                    GameManager.instance.gameboard.UpdateInfo();
                });
            }
        }

        public override void Unrender(IActionContext context, IAccountStateDelta nextStates)
        {
        }
    }
}
