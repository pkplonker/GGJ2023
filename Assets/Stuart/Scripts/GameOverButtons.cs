using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Stuart
{
	public class GameOverButtons : MonoBehaviour
	{
		[SerializeField] private AudioClip clickClip;
		private AudioSource source;
		private List<Button> buttons = new();
		private void Awake() => source = GetComponent<AudioSource>();

		private void Start()
		{
			buttons = GetComponentsInChildren<Button>().ToList();
			SetButtonsActive(false);
			GameController.OnGameEnd += PlayerWin;
		}

		private void PlayerWin(int obj, WinReason x)
		{
			SetButtonsActive(true);
		}

		private void OnDisable() => GameController.OnGameEnd -= PlayerWin;

		private void SetButtonsActive(bool state)
		{
			foreach (var b in buttons)
			{
				b.gameObject.SetActive(state);
			}
		}

		public void Quit(bool playsound = true)
		{
			if (playsound) PlaySound();
			SceneManager.LoadScene(0);
		}

		public void Continue(bool playsound = true)
		{
			if (playsound) PlaySound();
			SceneManager.LoadScene(1);
		}

		private void PlaySound() => AudioController.instance.PlaySound(clickClip);
	}
}