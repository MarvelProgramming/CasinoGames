using System.Collections;
using System.Collections.Generic;
using CasinoGames.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CasinoGames.Core.Tests
{
    public class GameBoardTests
    {
        private GameBoard gameBoard;

        [SetUp]
        public void Setup()
        {
            gameBoard = GameObject.Instantiate(Resources.Load<GameBoard>("GameBoard"));
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(gameBoard.gameObject);
        }

        [Test]
        public void GameBoard_Adds_Card_To_Correct_Player_Area_When_OnCardDealt_Event_Is_Invoked()
        {
            // Act
            ICardDealer.OnDealtCard(0, new CasinoCard(0, FacingDirection.Front, null, null));

            // Assert
            PlayerArea[] playerAreas = gameBoard.GetComponentsInChildren<PlayerArea>();
            int cardStackAreaSize = playerAreas[0].GetComponentInChildren<CardstackArea>().Size;
            Assert.AreEqual(cardStackAreaSize, 1, $"cardstackarea size is {cardStackAreaSize}, expected 1");
        }
    }
}
