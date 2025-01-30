using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace Game
{
    public class GameState : MonoBehaviour
    {
        private int _citizenLeft;
        private int _initialCitizenCount;

        public SceneState sceneState;

        public int initialDemonCount = 10;
        private int _currentDemonCount;

        public float timeToFadeIn = 3f;
        private Canvas _canvasForFadeIn;

        private bool _stage2Started = false;
        private bool _stage3Started = false;


        private void Awake()
        {
            _canvasForFadeIn = GetComponent<Canvas>();
        }

        private void Start()
        {
            var allCitizen = FindObjectsByType<CitizenController>(FindObjectsSortMode.None);

            _initialCitizenCount = allCitizen.Length - 3;
            if (_initialCitizenCount < 0)
            {
                _initialCitizenCount = 3;
            }

            _citizenLeft = _initialCitizenCount;
            EventManager.GameProgressEvent.OnStartGame(this, _initialCitizenCount);

            _currentDemonCount = initialDemonCount;
            EventManager.DemonChargeEvent.OnStartGame?.Invoke(this, initialDemonCount);
        }

        private void OnEnable()
        {
            EventManager.GameProgressEvent.OnEnchant += DecreaseCounter;
            EventManager.DemonChargeEvent.OnDemonKilledByInqEvent += DecreaseDemonCounter;
            InputActionSingleton.GeneralInputActions.Gameplay.ShowFov.performed += ToggleFovState;
        }

        private void ToggleFovState(InputAction.CallbackContext obj)
        {
            sceneState.isFovEnabled = !sceneState;
        }

        private void OnDisable()
        {
            EventManager.GameProgressEvent.OnEnchant -= DecreaseCounter;
            EventManager.DemonChargeEvent.OnDemonKilledByInqEvent -= DecreaseDemonCounter;
            InputActionSingleton.GeneralInputActions.Gameplay.ShowFov.performed -= ToggleFovState;
        }


        private void DecreaseCounter(Component source)
        {
            _citizenLeft--;
            if (_citizenLeft <= 0)
            {
                StartCoroutine(FadeInAndLoadScene("WinScene"));
            }

            if (!_stage2Started)
            {
                if (1 - (float)_citizenLeft / _initialCitizenCount >= 0.33f)
                {
                    EventManager.GameProgressEvent.OnStage2?.Invoke(this);
                    _stage2Started = true;
                }
            }


            if (!_stage3Started)
            {
                if (1 - (float)_citizenLeft / _initialCitizenCount >= 0.66f)
                {
                    EventManager.GameProgressEvent.OnStage3?.Invoke(this);
                    _stage3Started = true;
                }
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