using TMPro;
using UnityEngine;
using Utility;

namespace HUD
{
    public class HUDController : MonoBehaviour
    {
        public TextMeshProUGUI moneyCounter;


        private void OnEnable()
        {
            EventManager.MoneyEvent.OnIncome += OnIncome;
            EventManager.MoneyEvent.OnPurchase += OnPurchase;
        }

        private void OnDisable()
        {
            EventManager.MoneyEvent.OnIncome -= OnIncome;
            EventManager.MoneyEvent.OnPurchase -= OnPurchase;
        }

        private void OnIncome(Component source, int currentAmount, int amountToAdd)
        {
            moneyCounter.text = currentAmount.ToString();
        }

        private void OnPurchase(Component source, int currentAmount, int chargedAmount)
        {
            moneyCounter.text = currentAmount.ToString();
        }
    }
}