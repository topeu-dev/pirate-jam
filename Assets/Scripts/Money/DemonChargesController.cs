using System;
using UnityEngine;
using Utility;

namespace Money
{
    public class DemonChargesController : MonoBehaviour
    {
        public int initialCount = 10;
        private int _currentCount;

        private void Awake()
        {
            _currentCount = initialCount;
        }

        private void Start()
        {
            EventManager.DemonChargeEvent.OnStartGame?.Invoke(this, initialCount);
        }

        private void OnEnable()
        {
            EventManager.DemonChargeEvent.OnDemonKilledByInqEvent += DecreaseCounter;
        }

        private void OnDisable()
        {
            EventManager.DemonChargeEvent.OnDemonKilledByInqEvent -= DecreaseCounter;
        }

        private void DecreaseCounter(Component source)
        {
            _currentCount--;
            if (_currentCount <= 0)
            {
                Debug.Log("---GGWP---");
            }
        }
    }
}