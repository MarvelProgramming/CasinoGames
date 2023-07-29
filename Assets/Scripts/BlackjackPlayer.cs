using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class BlackjackPlayer : IBlackjackPlayer
    {
        public int CurrentBet { get; private set; }

        public IList<ICard> Cards { get; private set; }

        public IList<IChipstack> Chipstacks { get; private set; }

        public BlackjackPlayer()
        {
            Cards = new List<ICard>();
            Chipstacks = new List<IChipstack>();
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

        public virtual void Bust()
        {
            throw new NotImplementedException();
        }

        public void DecreaseBet(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"{nameof(DecreaseBet)} expects a positive input, received {amount}!");
            }

            CurrentBet = Mathf.Max(CurrentBet - amount, 0);
        }

        public void Double()
        {
            throw new NotImplementedException();
        }

        public virtual void Hit()
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

        public void RemoveAllCards()
        {
            Cards.Clear();
        }

        public void Stay()
        {
            throw new NotImplementedException();
        }

        public IChipstack GetChipOfType(CasinoChipType type)
        {
            throw new NotImplementedException();
        }
    }
}
