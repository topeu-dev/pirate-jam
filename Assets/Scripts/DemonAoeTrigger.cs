using System;
using UnityEngine;

public class DemonAoeTrigger : MonoBehaviour
{
    private DemonAoeTest demonAoeTest;
    
    private void Awake()
    {
        demonAoeTest = GetComponent<DemonAoeTest>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fov"))
        {
            var inquisitor = other.GetComponentInParent<InquisitorController>();
            inquisitor.ChaseDemon(transform.gameObject);
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