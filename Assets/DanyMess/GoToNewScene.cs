using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToNewScene : MonoBehaviour
{
    public Animator cameraAnimator; // Аниматор для камеры
    public Animator gateAnimator; // Аниматор для ворот
    public AudioSource audioSource; // Источник звука
    public AudioClip transitionSound; // Звук перехода
    public Image fadeImage; // Черный экран для затемнения
    public Button transitionButton; // Кнопка, которая должна исчезнуть

    public float cameraDelay = 0.5f; // Задержка перед анимацией камеры
    public float gateDelay = 0.5f; // Задержка перед анимацией ворот
    public float soundDelay = 0.5f; // Задержка перед воспроизведением звука
    public float fadeDelay = 0.5f; // Задержка перед затемнением экрана
    public float fadeDuration = 1f; // Длительность затемнения
    public float finalDelay = 0.5f; // Дополнительная задержка перед сменой сцены
    public float buttonFadeSpeed = 2f; // Скорость исчезновения кнопки

    public void LoadSceneWithEffects(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(TransitionSequence(sceneIndex));
        }
        else
        {
            Debug.LogError("Индекс сцены некорректен!");
        }
    }

    private System.Collections.IEnumerator TransitionSequence(int sceneIndex)
    {
        // 1. Исчезновение кнопки
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

            // Отключаем кнопку после исчезновения
            transitionButton.interactable = false;
            transitionButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Кнопка перехода не назначена!");
        }

        // 2. Анимация камеры
        if (cameraAnimator != null)
        {
            cameraAnimator.enabled = true;
            cameraAnimator.SetTrigger("Play");
            yield return new WaitForSeconds(cameraDelay);
        }
        else
        {
            Debug.LogWarning("Аниматор камеры не назначен!");
        }

        // 3. Анимация ворот
        if (gateAnimator != null)
        {
            gateAnimator.enabled = true;
            gateAnimator.SetTrigger("Open");
            yield return new WaitForSeconds(gateDelay);
        }
        else
        {
            Debug.LogWarning("Аниматор ворот не назначен!");
        }

        // 4. Проигрывание звука
        if (audioSource != null && transitionSound != null)
        {
            yield return new WaitForSeconds(soundDelay);
            audioSource.PlayOneShot(transitionSound);
        }
        else
        {
            Debug.LogWarning("AudioSource или звук перехода не назначены!");
        }

        // 5. Затемнение экрана
        if (fadeImage != null)
        {
            yield return new WaitForSeconds(fadeDelay);
            yield return StartCoroutine(FadeToBlack());
        }
        else
        {
            Debug.LogWarning("fadeImage не назначен!");
        }

        // 6. Финальная задержка перед сменой сцены
        yield return new WaitForSeconds(finalDelay);

        // 7. Переход на новую сцену
        SceneManager.LoadScene(sceneIndex);
    }

    private System.Collections.IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration); // Меняем прозрачность
            fadeImage.color = color;
            yield return null;
        }
    }
}