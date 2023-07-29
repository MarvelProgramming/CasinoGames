using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class BlackjackDealer : BlackjackPlayer, IBlackjackDealer
    {
        public IList<ICard> Deck { get; }

        public BlackjackDealer(IList<ICard> deck)
        {
            Deck = new List<ICard>(deck);
        }

        public override void Bust()
        {
            throw new NotImplementedException();
        }

        public void ClearBoard()
        {
            throw new NotImplementedException();
        }

        public void DealCard(ICardHolder cardHolder)
        {
            throw new NotImplementedException();
        }

        public void DealCards()
        {
            throw new NotImplementedException();
        }

        public void ShuffleDeck()
        {
            // This doesn't ~guarantee~ that at least 50% of the cards will be in different positions.
            // But it's much more likely to be the case than not.
            const int timesToShuffle = 30;

            for(int i = 0; i < timesToShuffle; i++)
            {
                int firstCardIndex = UnityEngine.Random.Range(0, Deck.Count);
                int secondCardIndex = UnityEngine.Random.Range(0, Deck.Count);

                // Avoid attempting to swap the same card with itself.
                secondCardIndex = secondCardIndex == firstCardIndex ? (secondCardIndex + 1) % Deck.Count : secondCardIndex;

                ICard firstCard = Deck[firstCardIndex];
                ICard secondCard = Deck[secondCardIndex];
                Deck[firstCardIndex] = secondCard;
                Deck[secondCardIndex] = firstCard;
            }
        }

        public override void Hit()
        {
            throw new NotImplementedException();
        }

        public IChipstack GetChipOfType(CasinoChipType type)
        {
            throw new NotImplementedException();
        }
    }
}
