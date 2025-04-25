using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.FightingScene);
        });

        menuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MenuScene);
        });
    }

    private void Start()
    {
        BoxingGameManager.Instance.OnStateChanged += BoxingGameManager_OnStateChanged;
        Hide();
    }

    private void BoxingGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (BoxingGameManager.Instance.IsGameOver())
        {
            Show();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
