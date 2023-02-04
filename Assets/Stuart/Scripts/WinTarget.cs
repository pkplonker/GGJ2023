using System;
using System.Collections;
using System.Collections.Generic;
using Stuart;
using UnityEngine;

public class WinTarget : MonoBehaviour
{
    public static event Action<int> OnPlayerWin; 
    private void OnTriggerEnter(Collider other)
    {
        var invent = other.GetComponent<Inventory>();
        if (invent == null) return;
        Debug.Log($"Winner is Player {invent.playerId}");
        OnPlayerWin?.Invoke(invent.playerId);
    }
}
