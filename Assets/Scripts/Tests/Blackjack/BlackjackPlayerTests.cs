using CasinoGames.Abstractions;
using CasinoGames.Core.Blackjack;
using CasinoGames.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CasinoGames.Core.Tests
{
    public class BlackjackPlayerTests
    {
        private BlackjackPlayer player;

        [SetUp]
        public void SetupPlayer()
        {
            player = GameObjectUtils.CreateComponent<BlackjackPlayer>("Player");
        }

        [TearDown]
        public void TearDownPlayer()
        {
            GameObject.Destroy(player.gameObject);
        }

        [Test]
        public void AddCard_Adds_Card_To_End_Of_Cards_Collection()
        {
            // Assemble
            var card = new CasinoCard(0, FacingDirection.Front, null, null);

            // Act
            player.GiveCard(card);

            // Assert
            ICard lastDealerCard = player.Cards[player.Cards.Count - 1];
            Assert.AreEqual(lastDealerCard, card, $"Expected player's last card {lastDealerCard.Value} to be {card.Value}!");
        }

        [Test]
        public void RemoveAllCards_Results_In_Empty_Cards_Collection()
        {
            // Assemble
            var hand = new List<ICard>()
            {
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
            };

            // Act
            foreach (ICard card in hand)
            {
                player.GiveCard(card);
            }

            player.RemoveAllCards();

            // Assert
            Assert.AreEqual(player.Cards.Count, 0, $"player has {player.Cards.Count} cards, expected 0");
        }

        [Test]
        public void IncreaseBet_Positively_Changes_Bet_By_Specified_Amount()
        {
            // Act
            player.IncreaseBet(1);
            player.IncreaseBet(2);
            player.IncreaseBet(3);
            player.IncreaseBet(10);
            player.IncreaseBet(12);

            // Assert
            Assert.AreEqual(player.CurrentBet, 28, $"player bet amount is {player.CurrentBet}, expected 28");
        }

        [Test]
        public void IncreaseBet_Throws_With_Negative_Input()
        {
            // Assert
            Assert.Throws(typeof(ArgumentException), () => player.IncreaseBet(-1), $"Expected {nameof(BlackjackPlayer.IncreaseBet)} with negative input to throw exception");
        }

        [Test]
        public void DecreaseBet_Negatively_Changes_Bet_By_Specified_Amount()
        {
            // Act
            player.IncreaseBet(1);
            player.IncreaseBet(2);
            player.IncreaseBet(3);
            player.IncreaseBet(10);
            player.IncreaseBet(12);
            player.DecreaseBet(28);

            // Assert
            Assert.AreEqual(player.CurrentBet, 0, $"player bet amount is {player.CurrentBet}, expected 0");
        }

        [Test]
        public void DecreaseBet_Throws_With_Negative_Input()
        {
            // Assert
            Assert.Throws(typeof(ArgumentException), () => player.DecreaseBet(-1), $"Expected {nameof(BlackjackPlayer.DecreaseBet)} with negative input to throw exception");
        }

        [Test]
        public void DecreaseBet_Does_Not_Result_In_Negative_Bets()
        {
            // Act
            player.IncreaseBet(1);
            player.IncreaseBet(2);
            player.IncreaseBet(3);
            player.IncreaseBet(10);
            player.IncreaseBet(12);
            player.DecreaseBet(100);

            // Assert
            Assert.GreaterOrEqual(player.CurrentBet, 0);
        }

        #region HandValueTests

        [Test]
        public void TotalHandValue_Is_11_When_Hand_Is_Ace()
        {
            // Act
            player.GiveCard(new CasinoCard(11, FacingDirection.Front, null, null));

            // Assert
            Assert.AreEqual(player.GetHandValue(), 11);
        }

        [Test]
        public void TotalHandValue_Is_21_When_Hand_Is_Ace_Ten()
        {
            // Assemble
            var hand = new List<ICard>()
            {
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(10, FacingDirection.Front, null, null),
            };

            // Act
            foreach (ICard card in hand)
            {
                player.GiveCard(card);
            }

            // Assert
            Assert.AreEqual(player.GetHandValue(), 21);
        }

        [Test]
        public void TotalHandValue_Is_23_When_Hand_Is_Ace_Two_Ten_Ten()
        {
            // Assemble
            var hand = new List<ICard>()
            {
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(2, FacingDirection.Front, null, null),
                new CasinoCard(10, FacingDirection.Front, null, null),
                new CasinoCard(10, FacingDirection.Front, null, null),
            };

            // Act
            foreach (ICard card in hand)
            {
                player.GiveCard(card);
            }

            // Assert
            Assert.AreEqual(player.GetHandValue(), 23);
        }

        [Test]
        public void TotalHandValue_Is_17_When_Hand_Is_5_6_Ace_Ace_4()
        {
            // Assemble
            var hand = new List<ICard>()
            {
                new CasinoCard(5, FacingDirection.Front, null, null),
                new CasinoCard(6, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(4, FacingDirection.Front, null, null),
            };

            // Act
            foreach (ICard card in hand)
            {
                player.GiveCard(card);
            }

            // Assert
            Assert.AreEqual(player.GetHandValue(), 17);
        }

        [Test]
        public void TotalHandValue_Is_21_When_Hand_Is_21_Aces()
        {
            // Assemble
            var hand = new List<ICard>()
            {
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
                new CasinoCard(11, FacingDirection.Front, null, null),
            };

            // Act
            foreach (ICard card in hand)
            {
                player.GiveCard(card);
            }

            // Assert
            Assert.AreEqual(player.GetHandValue(), 21);
        }

        #endregion
    }
}
