using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CasinoGames.Core.Tests
{
    public class BlackjackDealerTests
    {
        [Test]
        public void AddCard_Adds_Card_To_End_Of_Cards_Collection()
        {
            #region Assemble

            var deck = new List<ICard>();
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);
            var card = new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null);

            #endregion

            #region Act

            dealer.AddCard(card);

            #endregion

            #region Assert

            ICard lastDealerCard = dealer.Cards[dealer.Cards.Count - 1];
            Assert.AreEqual(lastDealerCard, card, $"Expected dealer's last card {lastDealerCard.Name} to be {card.Name}!");

            #endregion
        }

        [Test]
        public void RemoveAllCards_Results_In_Empty_Cards_Collection()
        {
            #region Assemble

            var deck = new List<ICard>();
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);
            var hand = new List<ICard>()
            {
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
            };

            #endregion

            #region Act

            foreach (ICard card in hand)
            {
                dealer.AddCard(card);
            }

            dealer.RemoveAllCards();

            #endregion

            #region Assert

            Assert.AreEqual(dealer.Cards.Count, 0, $"dealer has {dealer.Cards.Count} cards, expected 0");

            #endregion
        }

        [Test]
        public void ShuffleDeck_Results_In_At_Least_Fifty_Percent_Of_Cards_Shifting_Positions()
        {
            #region Assemble

            var deck = new List<ICard>()
            {
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null),
            };
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);

            #endregion

            #region Act

            dealer.ShuffleDeck();

            #endregion

            #region Assert

            float ratioOfCardsThatChangedPosition = 0;
            int numberOfCardsThatChangedPosition = 0;

            for(int i = 0; i < dealer.Deck.Count; i++)
            {
                ICard dealerCard = dealer.Deck[i];
                ICard testCard = deck[i];
                numberOfCardsThatChangedPosition += dealerCard != testCard ? 1 : 0;
            }

            ratioOfCardsThatChangedPosition = deck.Count / numberOfCardsThatChangedPosition;

            Assert.GreaterOrEqual(ratioOfCardsThatChangedPosition, 0.5f, $"Only {(int)(ratioOfCardsThatChangedPosition * 100)}% of cards changed positions, expected >= 50%");

            #endregion
        }

        [Test]
        public void IncreaseBet_Positively_Changes_Bet_By_Specified_Amount()
        {
            #region Assemble

            var deck = new List<ICard>();
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);

            #endregion

            #region Act

            dealer.IncreaseBet(1);
            dealer.IncreaseBet(2);
            dealer.IncreaseBet(3);
            dealer.IncreaseBet(10);
            dealer.IncreaseBet(12);

            #endregion

            #region Assert

            Assert.AreEqual(dealer.CurrentBet, 28, $"dealer bet amount is {dealer.CurrentBet}, expected 28");

            #endregion
        }

        [Test]
        public void IncreaseBet_Throws_With_Negative_Input()
        {
            #region Assemble

            var deck = new List<ICard>();
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);

            #endregion

            #region Assert

            Assert.Throws(typeof(ArgumentException), () => dealer.IncreaseBet(-1), $"Expected {nameof(BlackjackDealer.IncreaseBet)} with negative input to throw exception");

            #endregion
        }

        [Test]
        public void DecreaseBet_Negatively_Changes_Bet_By_Specified_Amount()
        {
            #region Assemble

            var deck = new List<ICard>();
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);

            #endregion

            #region Act

            dealer.IncreaseBet(1);
            dealer.IncreaseBet(2);
            dealer.IncreaseBet(3);
            dealer.IncreaseBet(10);
            dealer.IncreaseBet(12);
            dealer.DecreaseBet(28);

            #endregion

            #region Assert

            Assert.AreEqual(dealer.CurrentBet, 0, $"dealer bet amount is {dealer.CurrentBet}, expected 0");

            #endregion
        }

        [Test]
        public void DecreaseBet_Throws_With_Negative_Input()
        {
            #region Assemble

            var deck = new List<ICard>();
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);

            #endregion

            #region Assert

            Assert.Throws(typeof(ArgumentException), () => dealer.DecreaseBet(-1), $"Expected {nameof(BlackjackDealer.DecreaseBet)} with negative input to throw exception");

            #endregion
        }

        [Test]
        public void DecreaseBet_Does_Not_Result_In_Negative_Bets()
        {
            #region Assemble

            var deck = new List<ICard>();
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);

            #endregion

            #region Act

            dealer.IncreaseBet(1);
            dealer.IncreaseBet(2);
            dealer.IncreaseBet(3);
            dealer.IncreaseBet(10);
            dealer.IncreaseBet(12);
            dealer.DecreaseBet(100);

            #endregion

            #region Assert

            Assert.GreaterOrEqual(dealer.CurrentBet, 0);

            #endregion
        }

        #region HandValueTests

        [Test]
        public void TotalHandValue_Is_11_When_Hand_Is_Ace()
        {
            #region Assemble

            var deck = new List<ICard>();
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);

            #endregion

            #region Act

            dealer.AddCard(new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null));

            #endregion

            #region Assert

            Assert.AreEqual(dealer.GetHandValue(), 11);

            #endregion
        }

        [Test]
        public void TotalHandValue_Is_21_When_Hand_Is_Ace_Ten()
        {
            #region Assemble

            var deck = new List<ICard>();
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);
            var hand = new List<ICard>()
            {
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 10, FacingDirection.Front, null, null),
            };

            #endregion

            #region Act

            foreach (ICard card in hand)
            {
                dealer.AddCard(card);
            }

            #endregion

            #region Assert

            Assert.AreEqual(dealer.GetHandValue(), 21);

            #endregion
        }

        [Test]
        public void TotalHandValue_Is_23_When_Hand_Is_Ace_Two_Ten_Ten()
        {
            #region Assemble

            var deck = new List<ICard>();
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);
            var hand = new List<ICard>()
            {
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 2, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 10, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 10, FacingDirection.Front, null, null),
            };

            #endregion

            #region Act

            foreach (ICard card in hand)
            {
                dealer.AddCard(card);
            }

            #endregion

            #region Assert

            Assert.AreEqual(dealer.GetHandValue(), 23);

            #endregion
        }

        [Test]
        public void TotalHandValue_Is_17_When_Hand_Is_5_6_Ace_Ace_4()
        {
            #region Assemble

            var deck = new List<ICard>();
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);
            var hand = new List<ICard>()
            {
                new CasinoCard(string.Empty, 5, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 6, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 4, FacingDirection.Front, null, null),
            };

            #endregion

            #region Act

            foreach (ICard card in hand)
            {
                dealer.AddCard(card);
            }

            #endregion

            #region Assert

            Assert.AreEqual(dealer.GetHandValue(), 17);

            #endregion
        }

        [Test]
        public void TotalHandValue_Is_21_When_Hand_Is_21_Aces()
        {
            #region Assemble

            var deck = new List<ICard>();
            var chips = new List<IGameChip>();
            var dealer = new BlackjackDealer(deck, chips);
            var hand = new List<ICard>()
            {
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
            };

            #endregion

            #region Act

            foreach (ICard card in hand)
            {
                dealer.AddCard(card);
            }

            #endregion

            #region Assert

            Assert.AreEqual(dealer.GetHandValue(), 21);

            #endregion
        }

        #endregion


    }
}
