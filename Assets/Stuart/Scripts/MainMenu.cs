    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.Serialization;

namespace Stuart
{

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject helpPanel;
        [SerializeField] private GameObject creditsPanel;
        [SerializeField] private AudioClip clickClip;

        private void Awake()
        {
            OpenMain(false);
        }

        public void OpenMain(bool playsound = true)
        {
            if (playsound) PlaySound();
            mainMenuPanel.SetActive(true);
            helpPanel.SetActive(false);
            creditsPanel.SetActive(false);
        }

        public void Quit(bool playsound = true)
        {
            if (playsound) PlaySound();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }

        private void PlaySound() => AudioController.instance.PlaySound(clickClip);


        public void Settings(bool playsound = true)
        {
            if (playsound) PlaySound();
            mainMenuPanel.SetActive(false);
            helpPanel.SetActive(true);
            creditsPanel.SetActive(false);
        }

        public void Credits(bool playsound = true)
        {
            if (playsound) PlaySound();
            mainMenuPanel.SetActive(false);
            helpPanel.SetActive(false);
            creditsPanel.SetActive(true); 
        }

        public void StartGame(bool playsound = true)
        {
            if (playsound) PlaySound();
            SceneManager.LoadScene(1);
        }
    }
}
