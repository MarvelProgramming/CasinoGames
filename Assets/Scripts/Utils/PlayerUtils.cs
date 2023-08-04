using CasinoGames.Abstractions;
using UnityEngine;

namespace CasinoGames.Utils
{
    public static class PlayerUtils
    {
        public static int GetMaxBet()
        {
            int maxBet = int.MinValue;
            IPlayer.players.ForEach(player => maxBet = Mathf.Max(maxBet, player.CurrentBet));

            return maxBet;
        }
    }
}
