using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField] private TextMeshProUGUI _livesText;
    
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI _firefliesText;
    [SerializeField] private TextMeshProUGUI _distanceText;

    [Header("Mega Charge")] 
    [SerializeField] private Slider _megaChargeSlider;
    
    // References
    private GameManager _gameManager;
    private PlayerController _playerController;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        UpdateLives();
        UpdateScore();
        UpdateMegaChargeGauge();
    }

    private void UpdateLives()
    {
        // Display hearts based on current lives
        string lives = "";
        for (int i = 0; i < _gameManager.CurrentLives; i++)
        {
            lives += "LIFE ";
        }
        _livesText.text = lives;
    }

    private void UpdateScore()
    {
        _firefliesText.text = "Ô " + _gameManager.Fireflies;
        _distanceText.text = Mathf.Round(_gameManager.Distance) + "m";
    }

    private void UpdateMegaChargeGauge()
    {
        _megaChargeSlider.value = _playerController.MegaChargeGauge;
        _megaChargeSlider.maxValue = _playerController.MegaChargeGaugeMax;
    }
}
