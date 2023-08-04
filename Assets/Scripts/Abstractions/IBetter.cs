using System;

namespace CasinoGames.Abstractions
{
    public interface IBetter
    {
        public static Action<IBetter> OnBetChanged;
        int CurrentBet { get; }
        void IncreaseBet(int amount);
        void DecreaseBet(int amount);
        void BetAllIn();
        void ClearBet();
    }
}
