using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
{
    public class BlackjackPlayer : IBlackjackPlayer
    {
        public IEnumerable<IGameChip> Chips => throw new NotImplementedException();

        public int CurrentBet => throw new NotImplementedException();

        public IEnumerable<ICard> Cards => throw new NotImplementedException();

        public int TotalCardValue => throw new NotImplementedException();

        public void AddCard(ICard card)
        {
            throw new NotImplementedException();
        }

        public void Bust()
        {
            throw new NotImplementedException();
        }

        public void DecreaseBet(int amount)
        {
            throw new NotImplementedException();
        }

        public void Double()
        {
            throw new NotImplementedException();
        }

        public void Hit()
        {
            throw new NotImplementedException();
        }

        public void IncreaseBet(int amount)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllCards()
        {
            throw new NotImplementedException();
        }

        public void Stay()
        {
            throw new NotImplementedException();
        }
    }
}
