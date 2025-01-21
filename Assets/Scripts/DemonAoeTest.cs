using System.Collections.Generic;
using UnityEngine;

public class DemonAoeTest : MonoBehaviour
{
    public float aoe = 4f;
    public float timeToLive = 10f;

    private readonly List<CitizenController> _enchantedCitizens = new();

    private LineRenderer _lineRenderer;

    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = aoe;
        Destroy(gameObject, timeToLive);
    }

    private void Update()
    {
        DrawCircle();
    }

    private void OnDestroy()
    {
        for (var i = 0; i < _enchantedCitizens.Count; i++)
        {
            _enchantedCitizens[i].StopEnchanting();
        }
    }


    void DrawCircle()
    {
        _lineRenderer.positionCount = 50 + 1;
        _lineRenderer.useWorldSpace = false;

        float angle = 0f;
        for (int i = 0; i <= 50; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * aoe;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * aoe;

            _lineRenderer.SetPosition(i, new Vector3(x, y, 0f));

            angle += 360f / 50;
        }
    }

    public void AddToEnchantingList(CitizenController citizen)
    {
        _enchantedCitizens.Add(citizen);
    }
}