using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
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
