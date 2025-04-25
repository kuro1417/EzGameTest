using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.FightingScene);
        });
        MainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MenuScene);
        });
        closeButton.onClick.AddListener(() =>
        {
            BoxingGameManager.Instance.TogglePause();
        });
    }

    private void Start()
    {
        BoxingGameManager.Instance.OnGamePause += BoxingGameManager_OnGamePause;
        BoxingGameManager.Instance.OnGameUnPause += BoxingGameManager_OnGameUnPause;
        Hide();
    }

    private void BoxingGameManager_OnGameUnPause(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void BoxingGameManager_OnGamePause(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
