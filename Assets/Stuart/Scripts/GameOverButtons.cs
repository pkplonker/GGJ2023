using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
			WinTarget.OnPlayerWin += PlayerWin;
		}

		private void PlayerWin(int obj)
		{
			SetButtonsActive(true);
		}

		private void OnDisable() => WinTarget.OnPlayerWin -= PlayerWin;

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
			//todo move to main menu
		}

		public void Continue(bool playsound = true)
		{
			if (playsound) PlaySound();
			//todo load next scene
		}

		private void PlaySound() => AudioController.instance.PlaySound(clickClip);
	}
}