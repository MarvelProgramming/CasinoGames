using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core.Blackjack
{
    internal class BlackjackManager : MonoBehaviour, IGameManager
    {
        [SerializeField]
        private PlayerStatsUI playerStatsUI;
        [SerializeField]
        private GameOverUI gameOverUI;
        [SerializeField]
        private GameObject playerActionUI;
        [SerializeField]
        private GameObject playerBettingUI;
        [SerializeField]
        private List<CasinoCard> baseDeck;
        [SerializeField]
        private BlackjackDealer dealer;
        [SerializeField]
        private List<BlackjackPlayer> players;
        private int activePlayer;

        #region Unity

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            Begin();
        }

        #endregion

        public void Begin()
        {
            playerBettingUI.SetActive(true);
            players[0].Cash = 1000;
            UpdateUI();
        }

        public void Restart()
        {
            if (players[0].Cash == 0)
            {
                gameOverUI.SetConditionUI("You're All Out Of Money!\nResetting");
                gameOverUI.gameObject.SetActive(true);
                players[0].Cash = 1000;
                return;
            }

            players.ForEach(player =>
            {
                player.RemoveAllCards();
                player.ClearBet();
            });
            players.Select(player => player.GetComponent<PlayerArea>()).ToList().ForEach(playerArea => playerArea.RemoveAllCards());
            gameOverUI.gameObject.SetActive(false);
            playerActionUI.SetActive(false);
            playerBettingUI.SetActive(true);
            activePlayer = 0;
            Initialize();
            UpdateUI();
        }

        public void End()
        {
            gameOverUI.gameObject.SetActive(true);
            playerActionUI.SetActive(false);
            playerBettingUI.SetActive(false);
        }

        public void Initialize()
        {
            dealer.Deck = new List<ICard>(baseDeck);

            foreach (ICard card in dealer.Deck)
            {
                card.Facing = FacingDirection.Front;
            }
        }

        public void ActivePlayerHit()
        {
            IBlackjackPlayer player = players[activePlayer];
            dealer.DealCard(player);
            ICardDealer.OnDealtCard?.Invoke(activePlayer, player.Cards[player.Cards.Count - 1]);

            if (player.GetHandValue() > 21)
            {
                gameOverUI.SetConditionUI("Busted!\nYou lose!");
                Lose();
            }
        }

        public void ActivePlayerStay()
        {
            // Assuming player 0 is the actual user, instead of the dealer/a bot.
            if (activePlayer == 0)
            {
                playerActionUI.SetActive(false);
            }

            activePlayer++;

            if (players[activePlayer] == dealer)
            {
                DealerHitUntilWinOrBust();
            }
        }

        public void ActivePlayerDouble()
        {
            players[activePlayer].IncreaseBet(players[activePlayer].CurrentBet);
            UpdateUI();
            ActivePlayerStay();
        }

        public void ActivePlayerFinishedBetting()
        {
            playerBettingUI.SetActive(false);
            dealer.DealCards(players.Select(player => (ICardHolder)player).ToList());
            dealer.DealCards(players.Select(player => (ICardHolder)player).ToList());

            if (players[0].GetHandValue() == 21)
            {
                gameOverUI.SetConditionUI("Blackjack!\nYou Win!");
                Win();
                return;
            }

            playerActionUI.SetActive(true);
            UpdateUI();
        }

        public void ActivePlayerIncreaseBet(int amount)
        {
            players[activePlayer].IncreaseBet(amount);
            UpdateUI();
        }

        public void ActivePlayerDecreaseBet(int amount)
        {
            players[activePlayer].DecreaseBet(amount);
            UpdateUI();
        }

        public void ActivePlayerClearBet()
        {
            players[activePlayer].ClearBet();
            UpdateUI();
        }

        public void ActivePlayerAllIn()
        {
            IBlackjackPlayer player = players[activePlayer];
            player.ClearBet();
            player.IncreaseBet(player.Cash);
            UpdateUI();
        }

        private void Win()
        {
            players[0].Cash += players[0].CurrentBet;
            End();
        }

        private void Lose()
        {
            players[0].Cash -= players[0].CurrentBet;
            End();
        }

        private void DealerHitUntilWinOrBust()
        {
            ICard lastDealerCard = dealer.Cards[^1];
            lastDealerCard.Facing = FacingDirection.Front;
            ICard.OnCardChanged(activePlayer, dealer.Cards.Count - 1, lastDealerCard);

            while (dealer.GetHandValue() < 17)
            {
                dealer.DealCard(dealer);
                ICardDealer.OnDealtCard?.Invoke(activePlayer, dealer.Cards[dealer.Cards.Count - 1]);
            }

            int dealerHandValue = dealer.GetHandValue();

            if (dealerHandValue > 21 || dealerHandValue < players[0].GetHandValue())
            {
                gameOverUI.SetConditionUI($"Dealer busted.\nYou Win!");
                Win();
            }
            else if (dealerHandValue > players[0].GetHandValue())
            {
                gameOverUI.SetConditionUI($"Dealer reached {dealerHandValue}.\nYou Lose!");
                Lose();
            }
            else if (dealerHandValue == players[0].GetHandValue())
            {
                gameOverUI.SetConditionUI($"Draw!");
                End();
            }
        }

        private ICardHolder GetPlayerWithLargestHand()
        {
            ICardHolder playerWithLargestHand = null;

            foreach (ICardHolder player in players)
            {
                if (player == (ICardHolder)dealer)
                {
                    continue;
                }

                if (playerWithLargestHand == null || playerWithLargestHand.GetHandValue() < player.GetHandValue())
                {
                    playerWithLargestHand = player;
                }
            }

            return playerWithLargestHand;
        }

        private void UpdateUI()
        {
            playerStatsUI.SetCashUI(players[0].Cash);
            playerStatsUI.SetBetUI(players[0].CurrentBet);
        }
    }
}
