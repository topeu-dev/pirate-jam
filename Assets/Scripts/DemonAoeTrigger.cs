using Inquisitor;
using UnityEngine;

public class DemonAoeTrigger : MonoBehaviour
{
    private DemonAoeTest demonAoeTest;

    private bool isChased;

    private void Awake()
    {
        demonAoeTest = GetComponent<DemonAoeTest>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isChased)
        {
            return;
        }
        
        if (other.CompareTag("Fov"))
        {
            var inquisitor = other.GetComponentInParent<InquisitorController>();
            inquisitor.setTargetToChase(transform.gameObject);
            isChased = true;
        }

        if (other.CompareTag("Citizen"))
        {
            var citizen = other.GetComponentInParent<CitizenController>();
            if (!citizen.ConvertedSoul)
            {
                demonAoeTest.AddToEnchantingList(citizen);
                citizen.EnchantTo(transform.position);
            }
        }
    }
}