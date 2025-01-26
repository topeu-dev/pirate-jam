using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace HUD
{
    public class HUDController : MonoBehaviour
    {
        private int _initialCitizenLeft;
        private int _citizenLeft;

        public TextMeshProUGUI moneyCounter;
        public TextMeshProUGUI citizenCounter;
        
        public TextMeshProUGUI notificationText;


        private void OnEnable()
        {
            EventManager.MoneyEvent.OnIncome += OnIncome;
            EventManager.MoneyEvent.OnPurchase += OnPurchase;
            EventManager.GameProgressEvent.OnEnchant += DecreaseCitizenLeftCounter;
            EventManager.GameProgressEvent.OnStartGame += SaveInitialCitizenCount;
            EventManager.NotificationEvent.OnWrongSpawnPoint += ShowWrongSpawnNotification;
        }

        private void ShowWrongSpawnNotification(Component source)
        {
            notificationText.alpha = 1f;
            notificationText.text = "Sir, I can't spawn there.";
            StartCoroutine(LerpNotificationAlpha());
        }

        private IEnumerator LerpNotificationAlpha()
        {
            float elapsedTime = 0f;

            while (elapsedTime < 2f)
            {
                elapsedTime += Time.deltaTime;
                notificationText.alpha = Mathf.Lerp(1,
                    0, elapsedTime / 2f);
                yield return null;
            }
        }


        private void OnDisable()
        {
            EventManager.MoneyEvent.OnIncome -= OnIncome;
            EventManager.MoneyEvent.OnPurchase -= OnPurchase;
            EventManager.GameProgressEvent.OnEnchant -= DecreaseCitizenLeftCounter;
            EventManager.GameProgressEvent.OnStartGame -= SaveInitialCitizenCount;
        }

        private void SaveInitialCitizenCount(Component arg0, int citizenLeft)
        {
            _initialCitizenLeft = citizenLeft;
            _citizenLeft = citizenLeft;
        }

        private void DecreaseCitizenLeftCounter(Component arg0)
        {
            _citizenLeft--;
            citizenCounter.text = _citizenLeft + "/" + _initialCitizenLeft;
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