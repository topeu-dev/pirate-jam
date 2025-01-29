using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DemonAoeTest : MonoBehaviour
{
    public float aoe = 4f;
    public float timeToLive = 10f;
    public GameObject deathVFX;
    public float timeToFade = 2f;
    public GameObject demonModel;
    public TextMeshProUGUI timeToLiveCounterText;

    private List<Material> _materialsToFade = new();
    private List<Color> _materialsToFadeColors = new();
    private bool _isChased;
    private readonly List<CitizenController> _enchantedCitizens = new();
    private Animator _animator;

    private LineRenderer _lineRenderer;

    private SphereCollider _sphereCollider;

    private float _timeToLiveCounter;

    private void Awake()
    {
        var allRenderers = demonModel.GetComponentsInChildren<Renderer>();
        for (var i = 0; i < allRenderers.Length; i++)
        {
            var mat = allRenderers[i].material;
            if (mat.HasProperty("_Color"))
            {
                _materialsToFade.Add(mat);
                _materialsToFadeColors.Add(mat.color);
            }
        }

        _animator = GetComponentInChildren<Animator>();
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = aoe;
    }

    private void Start()
    {
        _timeToLiveCounter = timeToLive;
        Destroy(gameObject, timeToLive);
    }

    private void Update()
    {
        _timeToLiveCounter -= Time.deltaTime;
        var timeToLiveCounterAsInt = ((int)_timeToLiveCounter);
        if (timeToLiveCounterAsInt >= 0)
        {
            timeToLiveCounterText.text = timeToLiveCounterAsInt.ToString(); 
        }
        else
        {
            timeToLiveCounterText.text = "";
        }
        
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

    public void Death()
    {
        _animator.speed = 0f;
        EnableDeathVFX();
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < timeToFade)
        {
            elapsedTime += Time.deltaTime;
            for (var i = 0; i < _materialsToFade.Count; i++)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / timeToFade);
                _materialsToFade[i].color = new Color(
                    _materialsToFadeColors[i].r,
                    _materialsToFadeColors[i].g,
                    _materialsToFadeColors[i].b,
                    alpha
                );
            }

            yield return null;
        }
    }

    private void EnableDeathVFX()
    {
        StartCoroutine(FadeOutCoroutine());
        deathVFX.SetActive(true);
        Destroy(gameObject, 2.5f);
    }
}