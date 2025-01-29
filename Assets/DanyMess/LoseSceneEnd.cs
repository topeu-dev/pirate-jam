using UnityEngine;
using UnityEngine.SceneManagement;


public class LoseSceneEnd : MonoBehaviour
{
    
    // ��������� ���������� ��� ������� �����
    public int sceneIndexRestart = 1;
    public int sceneIndexMenu = 0;

    

  

    // �����, ������� ����������� ��� ����� �� ������
    public void RestartLoad()
    {

        SceneManager.LoadScene(sceneIndexRestart);
    }
    public void MenuLoad()
    {

        SceneManager.LoadScene(sceneIndexMenu);
    }
}