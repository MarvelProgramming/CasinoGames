using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
{
    public interface IBlackjackPlayer : ICardHolder, IChipHolder
    {
        int CurrentBet { get; }
        void IncreaseBet(int amount);
        void DecreaseBet(int amount);
        void Bust();
        void Hit();
        void Stay();
        void Double();
    }
}
