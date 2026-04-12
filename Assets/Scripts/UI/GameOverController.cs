using TMPro;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [Header("UI Elements")]
    
    [SerializeField] private TextMeshProUGUI _distanceText;
    [SerializeField] private TextMeshProUGUI _fireflyText;

    private void Start()
    {
        // load saved data to display scores
        GameData data = SaveManager.Instance.Load();

        _distanceText.text = "Distance : " + Mathf.Round(GameManager.Instance.Distance);
        _fireflyText.text = "firefly : " + GameManager.Instance.Fireflies;
    }
    
    // called by the Replay button
    public void OnReplayButton()
    {
        GameManager.Instance.ResetGame();
        SceneLoader.Instance.LoadGameplay();
    }
    
    // called by the Menu Button
    public void OnMenuButton()
    {
        SceneLoader.Instance.LoadMainMenu();
    }
    
}
