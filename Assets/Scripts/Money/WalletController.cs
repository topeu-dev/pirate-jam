using UnityEngine;
using Utility;

namespace Money
{
    public class WalletController : MonoBehaviour
    {
        public float timeToIncome;
        public int income;
        public int amountFromStart;

        private int _currentAmount;


        private float _elapsedTime;

        private void Start()
        {
            _currentAmount = amountFromStart;
            EventManager.MoneyEvent.OnIncome(this, _currentAmount, 0);
        }

        private void Update()
        {
            if (_elapsedTime < timeToIncome)
            {
                _elapsedTime += Time.deltaTime;
                return;
            }

            _currentAmount += income;
            EventManager.MoneyEvent.OnIncome(this, _currentAmount, income);
            _elapsedTime = 0f;
        }


        public bool HasEnoughMoney(int amount)
        {
            return _currentAmount - amount >= 0;
        }

        public void Buy(int chargedAmount)
        {
            _currentAmount -= chargedAmount;
            EventManager.MoneyEvent.OnPurchase(this, _currentAmount, chargedAmount);
        }

        // public void DecreaseAmount(float amount)
        // {
        //     if (_currentAmount  - amount < 0)
        //     {
        //         
        //     }
        //     _currentAmount -= amount;
        // }
    }
}