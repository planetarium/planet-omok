using System;
using System.Linq;
using System.Collections.Immutable;
using Libplanet;
using Libplanet.Action;
using UniRx;
using System.Collections.Generic;

namespace Nekoyume.Action
{
    [Serializable]
    public abstract class ActionBase : IAction
    {
        public const string MarkChanged = "";
        public ActionType type;

        public abstract IImmutableDictionary<string, object> PlainValue { get; }
        public abstract void LoadPlainValue(IImmutableDictionary<string, object> plainValue);
        public abstract IAccountStateDelta Execute(IActionContext ctx);

        private static readonly Dictionary<ActionType, EventHandler<IActionContext>> EventHandlers =
            new Dictionary<ActionType, EventHandler<IActionContext>>();

        public enum ActionType
        {
            JoinSession = 0,
        }

        public void Render(IActionContext context, IAccountStateDelta nextStates)
        {
            EventHandlers[type]?.Invoke(this, context);
        }

        public void Unrender(IActionContext context, IAccountStateDelta nextStates)
        {

        }

        public static void AddRenderHandler(ActionType actionAddress, EventHandler<IActionContext> handler)
        {
            if (!EventHandlers.ContainsKey(actionAddress))
                EventHandlers[actionAddress] = null;

            EventHandlers[actionAddress] += handler;
        }

        public static void RemoveRenderHandler(ActionType actionAddress, EventHandler<IActionContext> handler)
        {
            EventHandlers[actionAddress] -= handler;
        }
    }
}
