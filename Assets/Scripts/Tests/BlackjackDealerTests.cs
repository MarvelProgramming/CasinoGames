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
            var dealer = new BlackjackDealer(deck);

            #endregion

            #region Act

            dealer.ShuffleDeck();

            #endregion
            #region Assert
            int numberOfCardsThatChangedPosition = 0;

            for (int i = 0; i < dealer.Deck.Count; i++)
            {
                ICard dealerCard = dealer.Deck[i];
                ICard testCard = deck[i];
                numberOfCardsThatChangedPosition += dealerCard != testCard ? 1 : 0;
            }

            float ratioOfCardsThatChangedPosition = deck.Count / numberOfCardsThatChangedPosition;

            Assert.GreaterOrEqual(ratioOfCardsThatChangedPosition, 0.5f, $"Only {(int)(ratioOfCardsThatChangedPosition * 100)}% of cards changed positions, expected >= 50%");

            #endregion
        }
    }
}
