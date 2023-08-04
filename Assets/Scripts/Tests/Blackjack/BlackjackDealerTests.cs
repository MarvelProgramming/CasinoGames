using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CasinoGames.Core.Blackjack;
using CasinoGames.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CasinoGames.Core.Tests
{
    public class BlackjackDealerTests
    {
        private BlackjackDealer dealer;
        private List<ICard> deck;

        [SetUp]
        public void SetupPlayer()
        {
            dealer = GameObjectUtils.CreateComponent<BlackjackDealer>("Blackjack Dealer");
            deck = new List<ICard>()
            {
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
                new CasinoCard(0, FacingDirection.Front, null, null),
            };
        }

        [TearDown]
        public void TearDownPlayer()
        {
            GameObject.Destroy(dealer.gameObject);
        }

        [Test]
        public void ShuffleDeck_Results_In_At_Least_Fifty_Percent_Of_Cards_Shifting_Positions()
        {
            // Act
            dealer.Deck = new List<ICard>(deck);
            dealer.ShuffleDeck();

            // Assert
            int numberOfCardsThatChangedPosition = 0;

            for (int i = 0; i < dealer.Deck.Count; i++)
            {
                ICard dealerCard = dealer.Deck[i];
                ICard testCard = deck[i];
                numberOfCardsThatChangedPosition += dealerCard != testCard ? 1 : 0;
            }

            float ratioOfCardsThatChangedPosition = deck.Count / numberOfCardsThatChangedPosition;

            Assert.GreaterOrEqual(ratioOfCardsThatChangedPosition, 0.5f, $"Only {(int)(ratioOfCardsThatChangedPosition * 100)}% of cards changed positions, expected >= 50%");
        }

        [Test]
        public void DealCards_Gives_One_Card_To_Each_Player()
        {
            // Arrange
            List<ICardHolder> cardHolders = new List<ICardHolder>();

            for (int i = 0; i < 3; i++)
            {
                ICardHolder cardHolder = GameObjectUtils.CreateComponent<BlackjackPlayer>("Card Holder");
                cardHolders.Add(cardHolder);
            }

            cardHolders.Add(dealer);

            // Act
            dealer.Deck = deck;
            dealer.DealCards(cardHolders);

            // Assert
            for (int i = 0; i < cardHolders.Count; i++)
            {
                ICardHolder cardHolder = cardHolders[i];
                Assert.AreEqual(cardHolder.Cards.Count, 1, $"card holder[{i}] has {cardHolder.Cards.Count} cards, expected 1");
            }
        }

        [Test]
        public void DealCard_Removes_One_Card_From_Deck()
        {
            // Act
            dealer.Deck = new List<ICard>(deck);
            dealer.DealCard(dealer);

            // Assert
            Assert.AreEqual(dealer.Deck.Count, deck.Count - 1, $"dealer deck has {dealer.Deck.Count} cards, expected {deck.Count - 1}");
        }

        [Test]
        public void ClearBoard_Removes_All_Cards_From_All_Player()
        {
            // Arrange
            List<ICardHolder> cardHolders = new List<ICardHolder>();

            for (int i = 0; i < 3; i++)
            {
                ICardHolder cardHolder = GameObjectUtils.CreateComponent<BlackjackPlayer>("Card Holder");
                cardHolders.Add(cardHolder);
            }

            cardHolders.Add(dealer);

            // Act
            dealer.Deck = deck;
            dealer.DealCards(cardHolders);
            dealer.ClearBoard(cardHolders);

            // Assert
            for (int i = 0; i < cardHolders.Count; i++)
            {
                ICardHolder cardHolder = cardHolders[i];
                Assert.AreEqual(cardHolder.Cards.Count, 0, $"card holder[{i}] has {cardHolder.Cards.Count} cards, expected 0");
            }
        }
    }
}
