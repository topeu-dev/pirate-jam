using System.Collections.Generic;
using UnityEngine;

public class DemonAoeTest : MonoBehaviour
{
    private float aoe = 4f;
    private Collider[] _sphereHitsBuffer = new Collider[40];
    private List<string> appliesTo = new(new[] { "Citizen" });

    private List<CitizenController> enchantedCitezens = new();
    
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        DrawCircle();
        ClearHitsBuffer();
        Physics.OverlapSphereNonAlloc(transform.position, aoe, _sphereHitsBuffer);
        for (var i = 0; i < _sphereHitsBuffer.Length; i++)
        {
            if (!_sphereHitsBuffer[i])
                continue;
            Debug.Log("Not null " + _sphereHitsBuffer[i].gameObject.name);

            if (appliesTo.Contains(_sphereHitsBuffer[i].gameObject.tag))
            {
                var citizenControl = _sphereHitsBuffer[i].gameObject.GetComponent<CitizenController>();
                citizenControl.EnchantTo(transform.position);
                enchantedCitezens.Add(citizenControl);
            }
        }
    }

    private void ClearHitsBuffer()
    {
        for (int i = 0; i < _sphereHitsBuffer.Length; i++)
        {
            _sphereHitsBuffer[i] = null;
        }
    }

    private void OnDestroy()
    {
        for (var i = 0; i < enchantedCitezens.Count; i++)
        {
            enchantedCitezens[i].StopEnchanting();
        }
    }
    
    
    void DrawCircle()
    {
        lineRenderer.positionCount = 50 + 1; 
        lineRenderer.useWorldSpace = false;

        float angle = 0f;
        for (int i = 0; i <= 50; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * aoe;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * aoe;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));

            angle += 360f / 50;
        }
    }
}