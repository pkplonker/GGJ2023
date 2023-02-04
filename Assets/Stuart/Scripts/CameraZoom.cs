using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stuart
{
	public class CameraZoom : MonoBehaviour
	{
		private Camera playerCamera;

		private void Awake()
		{
			playerCamera = Camera.main;
		}

		private void OnEnable() => WinTarget.OnPlayerWin += PlayerWin;

		private void PlayerWin(int id)
		{
			foreach (var cam in FindObjectsOfType<Camera>())
			{
				if (cam == playerCamera) continue;
				Destroy(cam);
			}
		}

		private void OnDisable() => WinTarget.OnPlayerWin -= PlayerWin;
	}
}