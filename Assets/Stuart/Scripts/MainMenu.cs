using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject newGamePanel;
    [SerializeField] private GameObject settingsPanel;

    private void Awake()
    {
        OpenMain();
    }

    public void OpenMain()
    {
        mainMenuPanel.SetActive(true);
        newGamePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
    public void Settings()
    {
        mainMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void NewGame()
    {
        mainMenuPanel.SetActive(false);
        newGamePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
    public void StartGame()
    {
        Debug.Log("Not yet linked");
        //todo change scene
    }
}
