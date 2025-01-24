using UnityEngine;
using Utility;

namespace Game
{
    public class GameState : MonoBehaviour
    {
        private int _citizenLeft;

        private void Start()
        {
            var allCitizen = FindObjectsByType<CitizenController>(FindObjectsSortMode.None);
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
        }
    }
}