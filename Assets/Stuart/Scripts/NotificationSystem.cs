using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Stuart
{
	public class NotificationSystem : MonoBehaviour
	{
		[SerializeField] private float messageTime = 3f;
		private TextMeshProUGUI text;
		private Coroutine messageCor;
		public static NotificationSystem instance { get; private set; }
		private void Awake()
		{
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy(this);
			text = GetComponent<TextMeshProUGUI>();
			text.text = "";
		}

		private void Start() => GameController.OnGameEnd += PlayerWin;
		private void OnDisable() => GameController.OnGameEnd -= PlayerWin;

		private void PlayerWin(int id, WinReason winReason)
		{
			switch (winReason)
			{
				case WinReason.ReachedEnd:
					ShowText($"Player {id} outgrew the pot!");
					break;
				case WinReason.OtherPlayerTrapped:
					var ans = id == 1 ? 2 : 1;
					ShowText($"Player {ans} got stuck!");
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(winReason), winReason, "invalid win reason");
			}
		} 

		public void ShowText(string message)
		{
			if (messageCor != null) StopCoroutine(messageCor);
			messageCor = StartCoroutine(MessageCor(message));
		}

		private IEnumerator MessageCor(string message, float time =-1 )
		{
			if (time == -1) time = messageTime;
			var timer = 0f;
			text.enabled = true;
			text.text = message;
			while (timer < time)
			{
				timer += Time.deltaTime;
				yield return null;
			}
			text.enabled = false;

		}


	}
}