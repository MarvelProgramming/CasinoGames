using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CasinoGames.Core.Tests
{
    public class BlackjackPlayerTests
    {
        [Test]
        public void AddCard_Adds_Card_To_End_Of_Cards_Collection()
        {
            #region Assemble

            var player = new BlackjackPlayer();
            var card = new CasinoCard(string.Empty, 0, FacingDirection.Front, null, null);

            #endregion

            #region Act

            player.AddCard(card);

            #endregion

            #region Assert

            ICard lastDealerCard = player.Cards[player.Cards.Count - 1];
            Assert.AreEqual(lastDealerCard, card, $"Expected player's last card {lastDealerCard.Name} to be {card.Name}!");

            #endregion
        }

        [Test]
        public void RemoveAllCards_Results_In_Empty_Cards_Collection()
        {
            #region Assemble

            var player = new BlackjackPlayer();
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
                player.AddCard(card);
            }

            player.RemoveAllCards();

            #endregion

            #region Assert

            Assert.AreEqual(player.Cards.Count, 0, $"player has {player.Cards.Count} cards, expected 0");

            #endregion
        }

        [Test]
        public void IncreaseBet_Positively_Changes_Bet_By_Specified_Amount()
        {
            #region Assemble

            var player = new BlackjackPlayer();

            #endregion

            #region Act

            player.IncreaseBet(1);
            player.IncreaseBet(2);
            player.IncreaseBet(3);
            player.IncreaseBet(10);
            player.IncreaseBet(12);

            #endregion

            #region Assert

            Assert.AreEqual(player.CurrentBet, 28, $"player bet amount is {player.CurrentBet}, expected 28");

            #endregion
        }

        [Test]
        public void IncreaseBet_Throws_With_Negative_Input()
        {
            #region Assemble

            var player = new BlackjackPlayer();

            #endregion

            #region Assert

            Assert.Throws(typeof(ArgumentException), () => player.IncreaseBet(-1), $"Expected {nameof(BlackjackPlayer.IncreaseBet)} with negative input to throw exception");

            #endregion
        }

        [Test]
        public void DecreaseBet_Negatively_Changes_Bet_By_Specified_Amount()
        {
            #region Assemble

            var player = new BlackjackPlayer();

            #endregion

            #region Act

            player.IncreaseBet(1);
            player.IncreaseBet(2);
            player.IncreaseBet(3);
            player.IncreaseBet(10);
            player.IncreaseBet(12);
            player.DecreaseBet(28);

            #endregion

            #region Assert

            Assert.AreEqual(player.CurrentBet, 0, $"player bet amount is {player.CurrentBet}, expected 0");

            #endregion
        }

        [Test]
        public void DecreaseBet_Throws_With_Negative_Input()
        {
            #region Assemble

            var player = new BlackjackPlayer();

            #endregion

            #region Assert

            Assert.Throws(typeof(ArgumentException), () => player.DecreaseBet(-1), $"Expected {nameof(BlackjackPlayer.DecreaseBet)} with negative input to throw exception");

            #endregion
        }

        [Test]
        public void DecreaseBet_Does_Not_Result_In_Negative_Bets()
        {
            #region Assemble

            var player = new BlackjackPlayer();

            #endregion

            #region Act

            player.IncreaseBet(1);
            player.IncreaseBet(2);
            player.IncreaseBet(3);
            player.IncreaseBet(10);
            player.IncreaseBet(12);
            player.DecreaseBet(100);

            #endregion

            #region Assert

            Assert.GreaterOrEqual(player.CurrentBet, 0);

            #endregion
        }

        [Test]
        public void GetChipOfType_Returns_Correct_Chipstack_Based_On_Type()
        {
            #region Assemble

            var player = new BlackjackPlayer();

            #endregion

            #region Act


            /*player.Chipstacks.Add()*/

            #endregion

            #region Assert

            Assert.GreaterOrEqual(player.CurrentBet, 0);

            #endregion
        }

        #region HandValueTests

        [Test]
        public void TotalHandValue_Is_11_When_Hand_Is_Ace()
        {
            #region Assemble

            var player = new BlackjackPlayer();

            #endregion

            #region Act

            player.AddCard(new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null));

            #endregion

            #region Assert

            Assert.AreEqual(player.GetHandValue(), 11);

            #endregion
        }

        [Test]
        public void TotalHandValue_Is_21_When_Hand_Is_Ace_Ten()
        {
            #region Assemble

            var player = new BlackjackPlayer();
            var hand = new List<ICard>()
            {
                new CasinoCard(string.Empty, 11, FacingDirection.Front, null, null),
                new CasinoCard(string.Empty, 10, FacingDirection.Front, null, null),
            };

            #endregion

            #region Act

            foreach (ICard card in hand)
            {
                player.AddCard(card);
            }

            #endregion

            #region Assert

            Assert.AreEqual(player.GetHandValue(), 21);

            #endregion
        }

        [Test]
        public void TotalHandValue_Is_23_When_Hand_Is_Ace_Two_Ten_Ten()
        {
            #region Assemble

            var player = new BlackjackPlayer();
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
                player.AddCard(card);
            }

            #endregion

            #region Assert

            Assert.AreEqual(player.GetHandValue(), 23);

            #endregion
        }

        [Test]
        public void TotalHandValue_Is_17_When_Hand_Is_5_6_Ace_Ace_4()
        {
            #region Assemble

            var player = new BlackjackPlayer();
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
                player.AddCard(card);
            }

            #endregion

            #region Assert

            Assert.AreEqual(player.GetHandValue(), 17);

            #endregion
        }

        [Test]
        public void TotalHandValue_Is_21_When_Hand_Is_21_Aces()
        {
            #region Assemble

            var player = new BlackjackPlayer();
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
                player.AddCard(card);
            }

            #endregion

            #region Assert

            Assert.AreEqual(player.GetHandValue(), 21);

            #endregion
        }

        #endregion
    }
}
