using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Central game controller managing game states, scenes, and match flow.
/// Handles initialization, victory conditions, and game transitions.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private Character playerCharacter;
    [SerializeField] private Character opponentCharacter;
    [SerializeField] private float matchTimer = 180f; // 3 minutes per round
    [SerializeField] private int maxRounds = 3;
    
    private int currentRound = 1;
    private float roundTimeRemaining;
    private GameState currentState;
    private static GameManager instance;
    
    public enum GameState
    {
        Menu,
        CharacterSelect,
        Loading,
        Playing,
        Paused,
        RoundOver,
        GameOver,
        Victory,
        Defeat
    }
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    
    // Events
    public static event Action<GameState> OnGameStateChanged;
    public static event Action OnRoundStart;
    public static event Action OnRoundEnd;
    public static event Action OnGameOver;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        roundTimeRemaining = matchTimer;
        SetGameState(GameState.Playing);
        OnRoundStart?.Invoke();
    }
    
    private void Update()
    {
        if (currentState != GameState.Playing)
            return;
        
        UpdateRoundTimer();
        CheckVictoryConditions();
    }
    
    private void UpdateRoundTimer()
    {
        roundTimeRemaining -= Time.deltaTime;
        
        if (roundTimeRemaining <= 0)
        {
            roundTimeRemaining = 0;
            DetermineWinnerByTime();
        }
    }
    
    private void CheckVictoryConditions()
    {
        // Check if player is defeated
        if (playerCharacter.stats.CurrentHP <= 0)
        {
            OnRoundEnd?.Invoke();
            SetGameState(GameState.Defeat);
            return;
        }
        
        // Check if opponent is defeated
        if (opponentCharacter.stats.CurrentHP <= 0)
        {
            OnRoundEnd?.Invoke();
            SetGameState(GameState.Victory);
            return;
        }
    }
    
    private void DetermineWinnerByTime()
    {
        float playerHealth = playerCharacter.stats.CurrentHP;
        float opponentHealth = opponentCharacter.stats.CurrentHP;
        
        if (playerHealth > opponentHealth)
        {
            SetGameState(GameState.Victory);
        }
        else if (opponentHealth > playerHealth)
        {
            SetGameState(GameState.Defeat);
        }
        else
        {
            // Draw - replay round
            ResetRound();
        }
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
        SetGameState(GameState.Paused);
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        SetGameState(GameState.Playing);
    }
    
    public void ResetRound()
    {
        playerCharacter.ResetToDefault();
        opponentCharacter.ResetToDefault();
        roundTimeRemaining = matchTimer;
        SetGameState(GameState.Playing);
        OnRoundStart?.Invoke();
    }
    
    public void NextRound()
    {
        if (currentRound >= maxRounds)
        {
            SetGameState(GameState.GameOver);
            OnGameOver?.Invoke();
        }
        else
        {
            currentRound++;
            ResetRound();
        }
    }
    
    public void LoadScene(string sceneName)
    {
        SetGameState(GameState.Loading);
        SceneManager.LoadScene(sceneName);
    }
    
    public void SetGameState(GameState newState)
    {
        if (currentState == newState)
            return;
        
        currentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }
    
    public GameState GetCurrentState() => currentState;
    public float GetRoundTimeRemaining() => roundTimeRemaining;
    public int GetCurrentRound() => currentRound;
    public int GetMaxRounds() => maxRounds;
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
