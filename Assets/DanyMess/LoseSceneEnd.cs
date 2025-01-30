using UnityEngine;
using UnityEngine.SceneManagement;


public class LoseSceneEnd : MonoBehaviour
{
    
    // Публичная переменная для индекса сцены
    public int sceneIndexRestart = 2;
    public int sceneIndexMenu = 0;

    

  

    // Метод, который запускается при клике на кнопку
    public void RestartLoad()
    {

        SceneManager.LoadScene(sceneIndexRestart);
    }
    public void MenuLoad()
    {

        SceneManager.LoadScene(sceneIndexMenu);
    }
}