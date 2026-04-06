using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }
    
    [Header("settings")]
    [SerializeField] private int _maxLives = 3;
    [SerializeField] private int _startingLives = 3;
    
    // Score
    public int Fireflies { get; private set; }
    public int SavedCows { get ; private set; }
    public float Distance { get; private set; }
    
    // Lives
    public int CurrentLives { get; private set; }
    
    private bool _isGameOver = false;
    private GameData _gameData;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return;}
        Instance = this;
    }

    private void Start()
    {
        //start with the correct numbver of lives
        CurrentLives = _startingLives;
        
        //load saved data
        _gameData = SaveManager.Instance.Load();
        Debug.Log("Distance record : " + _gameData.DistanceRecord);
        
    }

    private void Update()
    {
        if (_isGameOver) return;
        
        //increase distance over time
        Distance += Time.deltaTime * TrackController.Instance.TrackSpeed;
    }

    public void LoseLife()
    {
        if (_isGameOver) return;

        CurrentLives--;
        Debug.Log("lives remaining: " + CurrentLives);

        if (CurrentLives <= 0)
        {
            GameOver();
        }
    }
    
    // called when the player collects a firefly
    public void AddFireFly()
    {
        Fireflies++;
        Debug.Log("fireflies: " + Fireflies);
    }
    
    // called when the player save a cow
    public void AddSavedCow()
    {
        SavedCows++;
        Debug.Log("saved cow: " + SavedCows);
    }

    public void GameOver()
    {
        _isGameOver = true;
        Debug.Log("Game Over ! Distance : " + Distance);
        // TODO : Load GameOver scene
        
        // update record if beaten
        if (Distance > _gameData.DistanceRecord)
        {
            _gameData.DistanceRecord = Distance;
        }
        
        // add Firefly and cows to total
        _gameData.TotalFireflies += Fireflies;
        _gameData.TotalSavedCows += SavedCows;
        
        // save the game
        SaveManager.Instance.Save(_gameData);
            
    }
}
