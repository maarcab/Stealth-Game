using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLogic : MonoBehaviour
{
    public void ButtonExit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void ButtonToScene(string scene)
    {
        SceneManager.LoadScene(sceneName: scene);
    }
}
