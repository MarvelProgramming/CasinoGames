using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class BlackjackDealer : IBlackjackDealer
    {
        public IList<ICard> Deck { get; }

        public IList<IGameChip> Chips { get; }

        public int CurrentBet { get; private set; }

        public IList<ICard> Cards { get; }

        public BlackjackDealer(IList<ICard> deck, IList<IGameChip> chips)
        {
            Deck = new List<ICard>(deck);
            Chips = chips;
            Cards = new List<ICard>();
        }

        public int GetHandValue()
        {
            int handValue = 0;
            List<ICard> cards = new List<ICard>(Cards);
            cards.Sort((firstCard, secondCard) => firstCard.Value - secondCard.Value);

            foreach (ICard card in cards)
            {
                handValue += card.Value;
            }

            foreach (ICard card in cards)
            {
                if (handValue > 21 && card.Value == 11)
                {
                    handValue -= 10;
                }
            }

            return handValue;
        }

        public void AddCard(ICard card)
        {
            Cards.Add(card);
        }

        public void Bust()
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

        public void IncreaseBet(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"{nameof(IncreaseBet)} expects a positive input, received {amount}!");
            }

            CurrentBet += amount;
        }

        public void DecreaseBet(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"{nameof(DecreaseBet)} expects a positive input, received {amount}!");
            }

            CurrentBet = Mathf.Max(CurrentBet - amount, 0);
        }

        public void RemoveAllCards()
        {
            Cards.Clear();
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

        public void Hit()
        {
            throw new NotImplementedException();
        }

        public void Double()
        {
            throw new NotImplementedException();
        }

        public void Stay()
        {
            throw new NotImplementedException();
        }
    }
}
