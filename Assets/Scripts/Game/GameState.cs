using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace Game
{
    public class GameState : MonoBehaviour
    {
        private int _citizenLeft;
        private int _initialCitizenCount;


        public int initialDemonCount = 10;
        private int _currentDemonCount;

        public float timeToFadeIn = 3f;
        private Canvas _canvasForFadeIn;


        private void Awake()
        {
            _canvasForFadeIn = GetComponent<Canvas>();
        }

        private void Start()
        {
            var allCitizen = FindObjectsByType<CitizenController>(FindObjectsSortMode.None);
            _initialCitizenCount = allCitizen.Length;
            _citizenLeft = allCitizen.Length;
            EventManager.GameProgressEvent.OnStartGame(this, allCitizen.Length);

            _currentDemonCount = initialDemonCount;
            EventManager.DemonChargeEvent.OnStartGame?.Invoke(this, initialDemonCount);
        }

        private void OnEnable()
        {
            EventManager.GameProgressEvent.OnEnchant += DecreaseCounter;
            EventManager.DemonChargeEvent.OnDemonKilledByInqEvent += DecreaseDemonCounter;
        }

        private void OnDisable()
        {
            EventManager.GameProgressEvent.OnEnchant -= DecreaseCounter;
            EventManager.DemonChargeEvent.OnDemonKilledByInqEvent -= DecreaseDemonCounter;
        }


        private void DecreaseCounter(Component source)
        {
            _citizenLeft--;
            if (_citizenLeft <= 0)
            {
                StartCoroutine(FadeInAndLoadScene("WinScene"));
            }

            if (1 - (float)_citizenLeft / _initialCitizenCount >= 0.33f)
            {
                EventManager.GameProgressEvent.OnStage2?.Invoke(this);
            }

            if (1 - (float)_citizenLeft / _initialCitizenCount >= 0.66f)
            {
                EventManager.GameProgressEvent.OnStage3?.Invoke(this);
            }
        }


        private void DecreaseDemonCounter(Component arg0)
        {
            _currentDemonCount--;
            if (_currentDemonCount <= 0)
            {
                StartCoroutine(FadeInAndLoadScene("LoseScene"));
            }
        }


        private IEnumerator FadeInAndLoadScene(string sceneName)
        {
            float elapsedTime = 0f;
            _canvasForFadeIn.enabled = true;
            _canvasForFadeIn.sortingOrder = 200;
            var img = _canvasForFadeIn.GetComponent<Image>();
            while (elapsedTime < timeToFadeIn)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / timeToFadeIn;

                img.color = Color.Lerp(img.color, Color.black, t);

                yield return null;
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}