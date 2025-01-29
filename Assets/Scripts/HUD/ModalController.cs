using TMPro;
using UnityEngine;

namespace HUD
{
    public class ModalController : MonoBehaviour
    {
        public GameObject[] modals;
        public TextMeshProUGUI modalCounter;

        private int _currentModalIndex = 0;


        public void NextModal()
        {
            _currentModalIndex = (_currentModalIndex + 1) % modals.Length;
            SetOneActiveModal(_currentModalIndex);
            SetIndexToCounter(_currentModalIndex);
        }

        public void PreviousModal()
        {
            _currentModalIndex = (_currentModalIndex - 1 + modals.Length) % modals.Length;
            SetOneActiveModal(_currentModalIndex);
            SetIndexToCounter(_currentModalIndex);
        }

        private void OnEnable()
        {
            modalCounter.text = "1/" + modals.Length;
            SetOneActiveModal(0);
            SetIndexToCounter(0);
        }


        private void SetIndexToCounter(int modalIndex)
        {
            modalCounter.text = modalIndex + 1 + "/" + modals.Length;
        }

        private void SetOneActiveModal(int modalIndex)
        {
            foreach (var modal in modals)
            {
                modal.SetActive(false);
            }

            modals[modalIndex].SetActive(true);
        }
    }
}