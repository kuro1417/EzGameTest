using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.FightingScene);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
