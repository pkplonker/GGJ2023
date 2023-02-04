using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatRecorder
{
    public static int winner = 0;
    public static void StartGame(){
    
    }
    public static void StopGame(int winnerId)
    {
        winner = winnerId;
    }
}
