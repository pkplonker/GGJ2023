using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Stuart
{
    public class GameController : MonoBehaviour
    {
        public static event Action OnGameStart;
        private void Awake() => GameStatRecorder.StartGame();

        private void OnEnable()
        {
            WinTarget.OnPlayerWin += PlayerWin;
            MapGenerator.OnMapGenerated += MapGenerated;
        } 
        private void OnDisable()
        { 
            WinTarget.OnPlayerWin -= PlayerWin;
            MapGenerator.OnMapGenerated -= MapGenerated;

        }

        private void MapGenerated(GameObject background, float scale)
        {
            StartCoroutine(StartCountdown());
        }

        private IEnumerator StartCountdown()
        {
            float timer = 0;
            float target = 3;
            while (timer<target)
            {
                timer+=Time.deltaTime;
                NotificationSystem.instance.ShowText((target-Mathf.FloorToInt(timer)).ToString());
                yield return null;
            }
            NotificationSystem.instance.ShowText("Go!");
            OnGameStart?.Invoke();
        }

        private void PlayerWin(int winnerId) =>
            GameStatRecorder.StopGame(winnerId, FindObjectsOfType<Inventory>().ToList());

    }
}
