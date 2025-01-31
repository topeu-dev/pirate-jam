using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Utility;

public class StageEffectController : MonoBehaviour
{
    public Volume volume;
    public float transitionTime = 4f;

    public CinemachineCamera cameraForStages;

    private float _initialMusicVolume; 
    public AudioSource stage1Music;
    
    public GameObject inquisitorToFocusOn2nStage;
    public GameObject inquisitorsFor2ndStage;
    public AudioSource stage2Music;
    
    public GameObject inquisitorToFocusOn3rdStage;
    public GameObject inquisitorsFor3rdStage;
    public AudioSource stage3Music;
    
    public AudioSource audioSourceToPlayOnStageChange;
    
    public float timeToFocus;

    private ShadowsMidtonesHighlights smh;
    private ChromaticAberration chromaticAberration;
    private ColorAdjustments colorAdjustments;

    private void Awake()
    {
        // _initialMusicVolume = stage1Music.volume;
    }

    void Start()
    {
        if (!volume.profile.TryGet(out smh))
        {
            Debug.LogError("Shadows Midtones Highlights не добавлено в Volume Profile!");
        }

        if (!volume.profile.TryGet(out chromaticAberration))
        {
            Debug.LogError("Chromatic Aberration не добавлено в Volume Profile!");
        }
        
        if (!volume.profile.TryGet(out colorAdjustments))
        {
            Debug.LogError("Color adjustments не добавлено в Volume Profile!");
        }
    }

    private void OnEnable()
    {
        EventManager.GameProgressEvent.OnStage2 += OnStage2;
        EventManager.GameProgressEvent.OnStage3 += OnStage3;
    }

    private void OnDisable()
    {
        EventManager.GameProgressEvent.OnStage2 -= OnStage2;
        EventManager.GameProgressEvent.OnStage3 -= OnStage3;
    }

    private void OnStage2(Component arg0)
    {
        stage1Music.enabled = false;
        stage2Music.enabled = true;
        
        inquisitorsFor2ndStage.SetActive(true);

        StartCoroutine(SwitchCameraForNSeconds(
            inquisitorToFocusOn2nStage,
            timeToFocus
        ));
        audioSourceToPlayOnStageChange.Play();
        StartCoroutine(LerpParams(
                new Vector4(1f, 0.88f, 0.95f, 0f),
                new Vector4(1f, 1f, 1f, 0f),
                new Vector4(1f, 1f, 1f, 0f),
                0.15f,
                -10f
            )
        );
    }

    private void OnStage3(Component arg0)
    {
        stage2Music.enabled = false;
        stage3Music.enabled = true;
        
        inquisitorsFor3rdStage.SetActive(true);

        StartCoroutine(SwitchCameraForNSeconds(
            inquisitorToFocusOn3rdStage,
            timeToFocus
        ));
        audioSourceToPlayOnStageChange.Play();
        StartCoroutine(LerpParams(
                new Vector4(1f, 0.80f, 0.95f, 0f),
                new Vector4(1f, 1f, 1f, 0f),
                new Vector4(1f, 1f, 1f, 0f),
                0.25f,
                -20f
            )
        );
    }


    private IEnumerator LerpParams(
        Vector4 targetShadows,
        Vector4 targetMidtones,
        Vector4 targetHighlights,
        float targetChromaticAberration,
        float targetHueShift
    )
    {
        Vector4 initialShadows = smh.shadows.value;
        Vector4 initialMidtones = smh.midtones.value;
        Vector4 initialHighlights = smh.highlights.value;
        float initialChromaticAberration = chromaticAberration.intensity.value;
        float initialHueShiftValue = colorAdjustments.hueShift.value;

        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionTime;

            smh.shadows.value = Vector4.Lerp(initialShadows, targetShadows, t);
            smh.midtones.value = Vector4.Lerp(initialMidtones, targetMidtones, t);
            smh.highlights.value = Vector4.Lerp(initialHighlights, targetHighlights, t);
            chromaticAberration.intensity.value = Mathf.Lerp(initialChromaticAberration, targetChromaticAberration, t);
            colorAdjustments.hueShift.value = Mathf.Lerp(initialHueShiftValue, targetHueShift, t);

            yield return null;
        }
    }


    private IEnumerator SwitchCameraForNSeconds(
        GameObject inquisitorToFocus,
        float time
    )
    {
        cameraForStages.Priority.Value = 50;
        cameraForStages.Follow = inquisitorToFocus.transform;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraForStages.Priority.Value = 0;
    }
}