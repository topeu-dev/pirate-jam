using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToNewScene : MonoBehaviour
{
    public Animator cameraAnimator; // �������� ��� ������
    public Animator gateAnimator; // �������� ��� �����
    public AudioSource audioSource; // �������� �����
    public AudioClip transitionSound; // ���� ��������
    public Image fadeImage; // ������ ����� ��� ����������
    public Button transitionButton; // ������, ������� ������ ���������

    public float cameraDelay = 0.5f; // �������� ����� ��������� ������
    public float gateDelay = 0.5f; // �������� ����� ��������� �����
    public float soundDelay = 0.5f; // �������� ����� ���������������� �����
    public float fadeDelay = 0.5f; // �������� ����� ����������� ������
    public float fadeDuration = 1f; // ������������ ����������
    public float finalDelay = 0.5f; // �������������� �������� ����� ������ �����
    public float buttonFadeSpeed = 2f; // �������� ������������ ������

    public void LoadSceneWithEffects(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(TransitionSequence(sceneIndex));
        }
        else
        {
            Debug.LogError("������ ����� �����������!");
        }
    }

    private System.Collections.IEnumerator TransitionSequence(int sceneIndex)
    {
        // 1. ������������ ������
        if (transitionButton != null)
        {
            CanvasGroup buttonCanvasGroup = transitionButton.GetComponent<CanvasGroup>();
            if (buttonCanvasGroup == null)
            {
                buttonCanvasGroup = transitionButton.gameObject.AddComponent<CanvasGroup>();
            }

            while (buttonCanvasGroup.alpha > 0)
            {
                buttonCanvasGroup.alpha -= Time.deltaTime * buttonFadeSpeed;
                yield return null;
            }

            // ��������� ������ ����� ������������
            transitionButton.interactable = false;
            transitionButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("������ �������� �� ���������!");
        }

        // 2. �������� ������
        if (cameraAnimator != null)
        {
            cameraAnimator.enabled = true;
            cameraAnimator.SetTrigger("Play");
            yield return new WaitForSeconds(cameraDelay);
        }
        else
        {
            Debug.LogWarning("�������� ������ �� ��������!");
        }

        // 3. �������� �����
        if (gateAnimator != null)
        {
            gateAnimator.enabled = true;
            gateAnimator.SetTrigger("Open");
            yield return new WaitForSeconds(gateDelay);
        }
        else
        {
            Debug.LogWarning("�������� ����� �� ��������!");
        }

        // 4. ������������ �����
        if (audioSource != null && transitionSound != null)
        {
            yield return new WaitForSeconds(soundDelay);
            audioSource.PlayOneShot(transitionSound);
        }
        else
        {
            Debug.LogWarning("AudioSource ��� ���� �������� �� ���������!");
        }

        // 5. ���������� ������
        if (fadeImage != null)
        {
            yield return new WaitForSeconds(fadeDelay);
            yield return StartCoroutine(FadeToBlack());
        }
        else
        {
            Debug.LogWarning("fadeImage �� ��������!");
        }

        // 6. ��������� �������� ����� ������ �����
        yield return new WaitForSeconds(finalDelay);

        // 7. ������� �� ����� �����
        SceneManager.LoadScene(sceneIndex);
    }

    private System.Collections.IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration); // ������ ������������
            fadeImage.color = color;
            yield return null;
        }
    }
}