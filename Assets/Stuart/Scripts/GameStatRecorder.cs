using System.Collections;
using System.Collections.Generic;
using Stuart;
using UnityEngine;

namespace Stuart
{
	public static class GameStatRecorder
	{
		public static int winner { get; private set; } = 0;
		public static List<Inventory> inventories { get; private set; }

		public static void StartGame()
		{
			winner = 0;
		}

		public static void StopGame(int winnerId, List<Inventory> invents)
		{
			inventories = invents;
			winner = winnerId;
		}
	}
}