using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Для работы с UI компонентами
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    // Публичная переменная для задержки в секундах
    public float delayTime = 3.0f;

    // Публичная переменная для индекса сцены
    public int sceneIndex = 1;

    // Публичная кнопка для запуска загрузки сцены
    public Button loadButton;

    // Ссылка на объект, который нужно включить
    public GameObject targetObject;

    // Скорость исчезновения кнопки
    public float fadeSpeed = 2.0f;

    private CanvasGroup buttonCanvasGroup;

    void Start()
    {
        // Получаем CanvasGroup компонента кнопки
        buttonCanvasGroup = loadButton.GetComponent<CanvasGroup>();

        // Если CanvasGroup отсутствует, добавляем его
        if (buttonCanvasGroup == null)
        {
            buttonCanvasGroup = loadButton.gameObject.AddComponent<CanvasGroup>();
        }

        // Подписываемся на клик кнопки
        loadButton.onClick.AddListener(StartSceneLoad);
    }

    // Метод, который запускается при клике на кнопку
    void StartSceneLoad()
    {
        // Включаем GameObject до запуска корутины
        if (targetObject != null)
        {
            targetObject.SetActive(true);  // Включаем объект
            Debug.Log("GameObject был включен до начала корутины.");
        }
        else
        {
            Debug.LogWarning("Не удалось найти GameObject для включения.");
        }

        // Запускаем корутину исчезновения кнопки
        StartCoroutine(FadeOutButton());

        // Запускаем корутину с задержкой
        StartCoroutine(LoadSceneWithDelay());
    }

    // Корутина для исчезновения кнопки
    IEnumerator FadeOutButton()
    {
        while (buttonCanvasGroup.alpha > 0)
        {
            buttonCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Отключаем интерактивность кнопки после исчезновения
        buttonCanvasGroup.interactable = false;
        buttonCanvasGroup.blocksRaycasts = false;
    }

    // Корутина для загрузки сцены с задержкой
    IEnumerator LoadSceneWithDelay()
    {
        // Ждем заданное время
        yield return new WaitForSeconds(delayTime);

        // Загружаем сцену по индексу
        SceneManager.LoadScene(sceneIndex);
    }
}