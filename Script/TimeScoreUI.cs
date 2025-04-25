using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        BoxingGameManager.Instance.OnStateChanged += BoxingGameManager_OnStateChanged;
        BoxingGameManager.Instance.OnScoreChanged += BoxingGameManager_OnScoreChanged;

        UpdateScoreUI();
        UpdateTimerUI();
    }

    private void OnDestroy()
    {
        if (BoxingGameManager.Instance != null)
        {
            BoxingGameManager.Instance.OnStateChanged -= BoxingGameManager_OnStateChanged;
            BoxingGameManager.Instance.OnScoreChanged -= BoxingGameManager_OnScoreChanged;
        }
    }

    private void BoxingGameManager_OnScoreChanged(object sender, System.EventArgs e)
    {
        UpdateTimerUI();
    }

    private void BoxingGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        UpdateScoreUI();
    }

    private void Update()
    {
        if (BoxingGameManager.Instance.IsGamePlaying())
        {
            UpdateTimerUI();
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {BoxingGameManager.Instance.GetScore()}";
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            float time = BoxingGameManager.Instance.GetPlayTime();
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }
}
