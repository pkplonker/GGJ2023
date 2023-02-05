using System;
using System.Collections;
using System.Collections.Generic;
using Stuart;
using UnityEngine;

namespace Stuart
{
    public class WinTarget : MonoBehaviour
    {
        public static event Action<int> OnPlayerWin;
        private int winner = -1;
        private void OnTriggerEnter(Collider other)
        {
            if (winner!=-1) return;
            var invent = other.GetComponent<Inventory>();
            if (invent == null) return;
            winner = invent.playerId;
            Debug.Log($"Winner is Player {invent.playerId}");
            OnPlayerWin?.Invoke(invent.playerId);
        }
    }
}