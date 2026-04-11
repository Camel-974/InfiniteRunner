using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // callde by the Play Button
    public void OnPlayButton()
    {
        SceneLoader.Instance.LoadGameplay();
    }
    
    // called by the Quit Button
    public void OnQuitButton()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
