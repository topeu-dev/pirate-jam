using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace HUD
{
    public class DemonChargesCounterController : MonoBehaviour
    {
        public Image slider;
        public TextMeshProUGUI textCounter;

        public Animator textCounterAnimator;
        // public Image stage1;
        // public Image stage2;
        // public Image stage3;

        private int _initialDemonChargesCount;
        private int _currentDemonChargesCount;


        private void Awake()
        {
            // SetAlpha(stage1, 1f);
            // SetAlpha(stage2, 0.2f);
            // SetAlpha(stage3, 0.2f);
        }

        private void OnEnable()
        {
            EventManager.DemonChargeEvent.OnDemonKilledByInqEvent += DecreaseDemonChargesLeftCounter;
            EventManager.DemonChargeEvent.OnStartGame += SaveInitialDemonCharges;
        }


        private void OnDisable()
        {
            EventManager.DemonChargeEvent.OnDemonKilledByInqEvent -= DecreaseDemonChargesLeftCounter;
            EventManager.DemonChargeEvent.OnStartGame -= SaveInitialDemonCharges;
        }


        private void SaveInitialDemonCharges(Component arg0, int initialCitizenCount)
        {
            _initialDemonChargesCount = initialCitizenCount;
            _currentDemonChargesCount = initialCitizenCount;
            slider.fillAmount = 1;
            textCounter.text = _initialDemonChargesCount.ToString();
        }

        private void DecreaseDemonChargesLeftCounter(Component arg0)
        {
            _currentDemonChargesCount--;
            StartCoroutine(LerpCounter());
            textCounterAnimator.SetTrigger("CounterAdd");
            textCounter.text = _currentDemonChargesCount.ToString();
        }

        private IEnumerator LerpCounter()
        {
            float elapsedTime = 0f;

            while (elapsedTime < 5f)
            {
                elapsedTime += Time.deltaTime;
                slider.fillAmount = Mathf.Lerp(slider.fillAmount,
                    (float)_currentDemonChargesCount / _initialDemonChargesCount, elapsedTime / 5f);
                yield return null;
            }
        }
    }
}