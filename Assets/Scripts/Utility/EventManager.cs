using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public static class EventManager
    {
        public static readonly SelectableObjectEvents SelectableObject = new SelectableObjectEvents();
        public static readonly CameraEvents CameraEvent = new CameraEvents();
        public static readonly InGameMenuEvents InGameMenuEvent = new InGameMenuEvents();
        
        public static readonly MoneyEvents MoneyEvent = new MoneyEvents();
        public static readonly DemonChargesEvents DemonChargeEvent = new DemonChargesEvents();
        public static readonly GameProgressEvents GameProgressEvent = new GameProgressEvents();
        public static readonly NotificationEvents NotificationEvent = new NotificationEvents();


        public class SelectableObjectEvents
        {
            public UnityAction<Component, GameObject> OnObjectSelectedEvent;
        }

        public class CameraEvents
        {
            public UnityAction<Component, GameObject> OnPlayableCharacterFocusEvent;
        }
        
        public class InGameMenuEvents
        {
            public UnityAction<Component> OnPressBackEvent;
        }

        public class MoneyEvents
        {
            // source - currentAmount - amountToAdd
            public UnityAction<Component, int, int> OnIncome;

            // source - currentAmount - chargedAmount
            public UnityAction<Component, int, int> OnPurchase;
        }

        public class DemonChargesEvents
        {
            // source - initial-demon-charge-count
            public UnityAction<Component, int > OnStartGame;
            public UnityAction<Component, AudioSource[]> OnSummon;
            public UnityAction<Component> OnDemonKilledByInqEvent;
        }

        public class GameProgressEvents
        {
            // source - count of citizen
            public UnityAction<Component, int> OnStartGame;

            // source 
            public UnityAction<Component> OnEnchant;

            public UnityAction<Component> OnStage2;

            public UnityAction<Component> OnStage3;
        }


        public class NotificationEvents
        {
            public UnityAction<Component> OnWrongSpawnPoint;
        }
    }
}