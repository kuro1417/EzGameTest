using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class BoxingGameManager : MonoBehaviour
{
    public static BoxingGameManager Instance { get; private set; }

    [SerializeField] private int enemyKillScore = 10;
    [SerializeField] private int bossKillScore = 50;

    public event EventHandler OnStateChanged;
    public event EventHandler OnScoreChanged;
    public event EventHandler OnGamePause;
    public event EventHandler OnGameUnPause;
    private enum GameState
    {
        GamePlaying,
        GameOver
    }

    private GameState gameState;
    private float gamePlayTimer;
    private int gameScore;
    private bool isPauseGame = false;
    private void Awake()
    {
        Instance = this;

        gameState = GameState.GamePlaying;
        EnemyControl.OnEnemyDeath += EnemyControl_OnEnemyDeath;
    }

    private void Start()
    {
        GameInput.Instance.OnPausedAction += GameInput_OnPausedAction;
    }

    private void GameInput_OnPausedAction(object sender, EventArgs e)
    {
        TogglePause();
    }

    private void OnDestroy()
    {
        EnemyControl.OnEnemyDeath -= EnemyControl_OnEnemyDeath;
    }

    private void EnemyControl_OnEnemyDeath(EnemyControl enemy)
    {
        if (gameState != GameState.GamePlaying) return;

        if (enemy.IsBoss())
        {
            AddScore(bossKillScore);
        }
        else
        {
            AddScore(enemyKillScore);
        }
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.GamePlaying:
                gamePlayTimer += Time.deltaTime;
                if (PlayerHealth.Instance.IsDead())
                {
                    gameState = GameState.GameOver;
                    OnStateChanged?.Invoke(this, new EventArgs());
                }
                break;
            case GameState.GameOver:
                break;
        }
        //Debug.Log(gameScore);
    }

    public void TogglePause()
    {
        isPauseGame = !isPauseGame;
        if (isPauseGame)
        {
            Time.timeScale = 0f;
            OnGamePause?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnPause?.Invoke(this, EventArgs.Empty);
        }
    }
    public void AddScore(int amount)
    {
        gameScore += amount;
        OnScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGamePlaying()
    {
        return gameState == GameState.GamePlaying;
    }

    public bool IsGameOver()
    {
        return gameState == GameState.GameOver;
    }
    public float GetPlayTime()
    {
        return gamePlayTimer;
    }
    public int GetScore()
    {
        return gameScore;
    }

    public bool IsPause()
    {
        return isPauseGame;
    }
}
