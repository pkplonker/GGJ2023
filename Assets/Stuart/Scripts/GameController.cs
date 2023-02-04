using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stuart
{
    public class GameController : MonoBehaviour
    {
        private void Awake() => GameStatRecorder.StartGame();
        private void OnEnable() => WinTarget.OnPlayerWin += PlayerWin;
        private void OnDisable() => WinTarget.OnPlayerWin -= PlayerWin;

        private void PlayerWin(int winnerId) =>
            GameStatRecorder.StopGame(winnerId, FindObjectsOfType<Inventory>().ToList());

    }
}
