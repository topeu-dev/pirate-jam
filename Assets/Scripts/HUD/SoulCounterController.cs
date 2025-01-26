using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace HUD
{
    public class SoulCounterController : MonoBehaviour
    {
        public Image slider;
        public TextMeshProUGUI textCounter;
        public Animator textCounterAnimator;
        public Image stage1;
        public Image stage2;
        public Image stage3;

        private int _initialCitizenCount;
        private int _currentCitizenCount;


        private void Awake()
        {
            SetAlpha(stage1, 1f);
            SetAlpha(stage2, 0.2f);
            SetAlpha(stage3, 0.2f);
        }

        private void OnEnable()
        {
            EventManager.GameProgressEvent.OnEnchant += DecreaseCitizenLeftCounter;
            EventManager.GameProgressEvent.OnStartGame += SaveInitialCitizenCount;
            EventManager.GameProgressEvent.OnStage2 += OnStage2;
            EventManager.GameProgressEvent.OnStage3 += OnStage3;
        }


        private void OnDisable()
        {
            EventManager.GameProgressEvent.OnEnchant -= DecreaseCitizenLeftCounter;
            EventManager.GameProgressEvent.OnStartGame -= SaveInitialCitizenCount;
            EventManager.GameProgressEvent.OnStage2 -= OnStage2;
            EventManager.GameProgressEvent.OnStage3 -= OnStage3;
        }


        private void SaveInitialCitizenCount(Component arg0, int initialCitizenCount)
        {
            _initialCitizenCount = initialCitizenCount;
            _currentCitizenCount = initialCitizenCount;
            slider.fillAmount = 0;
            textCounter.text = _initialCitizenCount.ToString();
        }

        private void DecreaseCitizenLeftCounter(Component arg0)
        {
            _currentCitizenCount--;
            StartCoroutine(LerpCounter());
            textCounterAnimator.SetTrigger("CounterAdd");
            textCounter.text = _currentCitizenCount.ToString();
        }

        private IEnumerator LerpCounter()
        {
            float elapsedTime = 0f;

            while (elapsedTime < 5f)
            {
                elapsedTime += Time.deltaTime;
                slider.fillAmount = Mathf.Lerp(slider.fillAmount,
                    1 - (float)_currentCitizenCount / _initialCitizenCount, elapsedTime / 5f);
                yield return null;
            }
        }


        private void OnStage2(Component arg0)
        {
            SetAlpha(stage2, 1f);
        }

        private void OnStage3(Component arg0)
        {
            SetAlpha(stage3, 1f);
        }

        private void SetAlpha(Image image, float alpha)
        {
            var oldColor = image.color;
            image.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        }
    }
}