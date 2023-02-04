using System;
using System.Collections;
using System.Collections.Generic;
using Stuart;
using UnityEngine;

public class WinTarget : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var invent = other.GetComponent<Inventory>();
        if (invent == null) return;
        Debug.Log($"Winner is Player {invent.playerId}");
    }
}
