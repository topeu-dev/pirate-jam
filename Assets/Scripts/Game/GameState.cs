using UnityEngine;
using Utility;

namespace Game
{
    public class GameState : MonoBehaviour
    {
        private int _citizenLeft;
        private int _initialCitizenCount;

        private void Start()
        {
            var allCitizen = FindObjectsByType<CitizenController>(FindObjectsSortMode.None);
            _initialCitizenCount = allCitizen.Length;
            _citizenLeft = allCitizen.Length;
            EventManager.GameProgressEvent.OnStartGame(this, allCitizen.Length);
        }

        private void OnEnable()
        {
            EventManager.GameProgressEvent.OnEnchant += DecreaseCounter;
        }

        private void OnDisable()
        {
            EventManager.GameProgressEvent.OnEnchant -= DecreaseCounter;
        }

        private void DecreaseCounter(Component source)
        {
            _citizenLeft--;
            if (_citizenLeft <= 0)
            {
                Debug.Log("----------------------Game Over----------------------");
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
    }
}