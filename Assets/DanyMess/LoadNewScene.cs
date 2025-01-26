using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // ��� ������ � UI ������������
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    // ��������� ���������� ��� �������� � ��������
    public float delayTime = 3.0f;

    // ��������� ���������� ��� ������� �����
    public int sceneIndex = 1;

    // ��������� ������ ��� ������� �������� �����
    public Button loadButton;

    // ������ �� ������, ������� ����� ��������
    public GameObject targetObject;

    // �������� ������������ ������
    public float fadeSpeed = 2.0f;

    private CanvasGroup buttonCanvasGroup;

    void Start()
    {
        // �������� CanvasGroup ���������� ������
        buttonCanvasGroup = loadButton.GetComponent<CanvasGroup>();

        // ���� CanvasGroup �����������, ��������� ���
        if (buttonCanvasGroup == null)
        {
            buttonCanvasGroup = loadButton.gameObject.AddComponent<CanvasGroup>();
        }

        // ������������� �� ���� ������
        loadButton.onClick.AddListener(StartSceneLoad);
    }

    // �����, ������� ����������� ��� ����� �� ������
    void StartSceneLoad()
    {
        // �������� GameObject �� ������� ��������
        if (targetObject != null)
        {
            targetObject.SetActive(true);  // �������� ������
            Debug.Log("GameObject ��� ������� �� ������ ��������.");
        }
        else
        {
            Debug.LogWarning("�� ������� ����� GameObject ��� ���������.");
        }

        // ��������� �������� ������������ ������
        StartCoroutine(FadeOutButton());

        // ��������� �������� � ���������
        StartCoroutine(LoadSceneWithDelay());
    }

    // �������� ��� ������������ ������
    IEnumerator FadeOutButton()
    {
        while (buttonCanvasGroup.alpha > 0)
        {
            buttonCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // ��������� ��������������� ������ ����� ������������
        buttonCanvasGroup.interactable = false;
        buttonCanvasGroup.blocksRaycasts = false;
    }

    // �������� ��� �������� ����� � ���������
    IEnumerator LoadSceneWithDelay()
    {
        // ���� �������� �����
        yield return new WaitForSeconds(delayTime);

        // ��������� ����� �� �������
        SceneManager.LoadScene(sceneIndex);
    }
}