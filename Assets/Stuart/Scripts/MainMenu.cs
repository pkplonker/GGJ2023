using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject newGamePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private AudioClip clickClip;
    private AudioSource source;
    private void Awake()
    {
        OpenMain(false);
        source = GetComponent<AudioSource>();
    }

    public void OpenMain(bool playsound = true)
    {
        if(playsound) PlaySound();
        mainMenuPanel.SetActive(true);
        newGamePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void Quit(bool playsound = true)
    {
        if(playsound) PlaySound();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    private void PlaySound()=>source.PlayOneShot(clickClip);
    

    public void Settings(bool playsound = true)
    {
        if(playsound) PlaySound();
        mainMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void NewGame(bool playsound = true)
    {
        if(playsound) PlaySound();
        mainMenuPanel.SetActive(false);
        newGamePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
    public void StartGame(bool playsound = true)
    {
        if(playsound) PlaySound();
        Debug.Log("Not yet linked");
        //todo change scene
    }
}
