using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public static class EventManager
    {
        public static readonly SelectableObjectEvents SelectableObject = new SelectableObjectEvents();
        public static readonly CameraEvents CameraEvent = new CameraEvents();
        public static readonly MoneyEvents MoneyEvent = new MoneyEvents();


        public class SelectableObjectEvents
        {
            public UnityAction<Component, GameObject> OnObjectSelectedEvent;
        }

        public class CameraEvents
        {
            public UnityAction<Component, GameObject> OnPlayableCharacterFocusEvent;
        }

        public class MoneyEvents
        {
            // source - currentAmount - amountToAdd
            public UnityAction<Component, int, int> OnIncome;

            // source - currentAmount - chargedAmount
            public UnityAction<Component, int, int> OnPurchase;
        }
    }
}