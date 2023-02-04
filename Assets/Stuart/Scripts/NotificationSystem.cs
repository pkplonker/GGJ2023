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

		private void Awake()
		{
			text = GetComponent<TextMeshProUGUI>();
			text.text = "";
		}

		private void Start() => WinTarget.OnPlayerWin += PlayerWin;
		private void OnDisable() => WinTarget.OnPlayerWin -= PlayerWin;

		private void PlayerWin(int id) => ShowText($"Player {id} outgrew the pot!");

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