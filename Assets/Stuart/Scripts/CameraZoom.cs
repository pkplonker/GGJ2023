using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stuart
{
	public class CameraZoom : MonoBehaviour
	{
		private Camera playerCamera;
		[SerializeField] private float zoomTime;
		private void Awake() => playerCamera = Camera.main;

		private void Start() => WinTarget.OnPlayerWin += PlayerWin;
		private void OnDisable() => WinTarget.OnPlayerWin -= PlayerWin;

		private void PlayerWin(int id)
		{
			foreach (var cam in FindObjectsOfType<Camera>())
			{
				if (cam == playerCamera) continue;
				cam.enabled = false;
			}

			StartCoroutine(CameraMove());
		}

		private IEnumerator CameraMove()
		{
			var timer = 0f;
			var camStart = new Vector2(playerCamera.rect.width, playerCamera.rect.height);
			var sizeStart = playerCamera.orthographicSize;
			var targetSize = FindObjectOfType<MapGenerator>().sizeScale * 5;
			while (timer < zoomTime)
			{
				timer += Time.deltaTime;
				var progress = timer / zoomTime;
				var width = Mathf.Lerp(camStart.x, 1, progress);
				var height = Mathf.Lerp(camStart.y, 1, progress);
				playerCamera.rect = new Rect(playerCamera.rect.x, playerCamera.rect.y, width, height);
				playerCamera.orthographicSize = Mathf.Lerp(sizeStart, targetSize, progress);
				yield return null;
			}
		}
	}
}