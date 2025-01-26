using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public static class EventManager
    {
        public static readonly SelectableObjectEvents SelectableObject = new SelectableObjectEvents();
        public static readonly CameraEvents CameraEvent = new CameraEvents();
        public static readonly MoneyEvents MoneyEvent = new MoneyEvents();
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

        public class MoneyEvents
        {
            // source - currentAmount - amountToAdd
            public UnityAction<Component, int, int> OnIncome;

            // source - currentAmount - chargedAmount
            public UnityAction<Component, int, int> OnPurchase;
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