﻿using CasinoGames.Abstractions;
using CasinoGames.Abstractions.Blackjack;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CasinoGames.Core.Blackjack
{
    [RequireComponent(typeof(AudioSource))]
    public class BlackjackDealer : BlackjackPlayer, IBlackjackDealer, IGameManager
    {
        public IList<ICard> Deck { get; set; }

        [SerializeField]
        private List<CasinoCard> baseDeck;

        [SerializeField]
        private int numberOfDecksToUse;

        [SerializeField]
        private AudioClip dealCardSound;

        private AudioSource audioSource;

        private bool dealtCards;

        #region Unity

        protected override void Awake()
        {
            base.Awake();
            IBlackjackPlayer.OnHit += HandlePlayerHit;
            IBlackjackPlayer.OnStay += HandlePlayerStay;
            IBlackjackPlayer.OnDouble += HandlePlayerDouble;
            IPlayer.OnStateChanged += HandlePlayerStateChanged;
            Initialize();
        }

        private void Start()
        {
            Begin();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            IBlackjackPlayer.OnHit -= HandlePlayerHit;
            IBlackjackPlayer.OnStay -= HandlePlayerStay;
            IBlackjackPlayer.OnDouble -= HandlePlayerDouble;
            IPlayer.OnStateChanged -= HandlePlayerStateChanged;
            End();
        }

        #endregion

        public void ClearBoard(IList<ICardHolder> cardHolders)
        {
            foreach (ICardHolder cardHolder in cardHolders)
            {
                cardHolder.RemoveAllCards();
            }
        }

        public void DealCard(ICardHolder cardHolder)
        {
            ICard dealtCard = GetRandomDeckCard();
            dealtCard.Facing = FacingDirection.Front;
            cardHolder.GiveCard(dealtCard);

            if (cardHolder == (ICardHolder)this)
            {
                if (cardHolder.Cards.Count == 2)
                {
                    cardHolder.UpdateCard(1, FacingDirection.Back);
                }
            }

            Deck.Remove(dealtCard);
            PlayDealCardSound();
        }

        public IEnumerator DealCards(IList<ICardHolder> cardHolders)
        {
            ShuffleDeck();

            for (int i = 0; i < cardHolders.Count; i++)
            {
                ICardHolder cardHolder = cardHolders[i];
                DealCard(cardHolder);

                yield return new WaitForSeconds(cardPlacementDelay);
            }
        }

        public void ShuffleDeck()
        {
            // This doesn't ~guarantee~ that at least 50% of the cards will be in different positions.
            // But it's much more likely to be the case than not.
            const int timesToShuffle = 30;

            for (int i = 0; i < timesToShuffle; i++)
            {
                int firstCardIndex = Random.Range(0, Deck.Count);
                int secondCardIndex = Random.Range(0, Deck.Count);

                // Avoid attempting to swap the same card with itself.
                secondCardIndex = secondCardIndex == firstCardIndex ? (secondCardIndex + 1) % Deck.Count : secondCardIndex;

                ICard firstCard = Deck[firstCardIndex];
                ICard secondCard = Deck[secondCardIndex];
                Deck[firstCardIndex] = secondCard;
                Deck[secondCardIndex] = firstCard;
            }
        }

        private ICard GetRandomDeckCard()
        {
            return Deck[Random.Range(0, Deck.Count)];
        }

        public void Begin()
        {
            IPlayer.players.Sort((firstPlayer, secondPlayer) => firstPlayer.Order - secondPlayer.Order);
            CurrentState = PlayerState.Waiting;
            IGameManager.OnBegin?.Invoke();
        }

        public void Restart()
        {
            Initialize();
            IGameManager.OnRestart?.Invoke();
            CurrentState = PlayerState.Waiting;
            dealtCards = false;
            Begin();
        }

        public void End()
        {
            IPlayer.players.Clear();
            IPlayer.UserPlayer = null;
            IPlayer.PlayerTurn = 0;
            IGameManager.OnEnd?.Invoke();
        }

        public void Initialize()
        {
            audioSource = GetComponent<AudioSource>();
            List<ICard> newDeck = new List<ICard>();

            for (int i = 0; i < numberOfDecksToUse; i++)
            {
                newDeck.AddRange(baseDeck);
            }

            Deck = newDeck;
        }

        private void HandlePlayerHit(IBlackjackPlayer player)
        {
            if (player != IPlayer.ActivePlayer)
            {
                return;
            }

            DealCard(player);

            if (player.GetHandValue() > 21)
            {
                player.Lose("Bust!");

                if (player != (IBlackjackPlayer)this)
                {
                    IPlayer.PlayerTurn++;
                }
            }
        }

        private void HandlePlayerStay(IBlackjackPlayer player)
        {
            if (player != IPlayer.ActivePlayer)
            {
                return;
            }

            if (player.GetHandValue() == 21)
            {
                player.Win("Blackjack!");
            }

            IPlayer.PlayerTurn++;
        }

        private void HandlePlayerDouble(IBlackjackPlayer player)
        {
            if (player != IPlayer.ActivePlayer)
            {
                return;
            }

            DealCard(player);

            if (player.GetHandValue() > 21)
            {
                player.Lose("Bust!");
            }

            IPlayer.PlayerTurn++;
        }

        private void HandlePlayerStateChanged(IPlayer player)
        {
            if (!dealtCards && AllPlayersWaiting())
            {
                StartCoroutine(DealTwoCardsToEachPlayer());
            }
        }

        protected override void HandleActivePlayerChanged(IPlayer newActivePlayer)
        {
            if (newActivePlayer == (IPlayer)this)
            {
                StartCoroutine(DelayedHitUntil17(false));
            }
        }

        private IEnumerator DealTwoCardsToEachPlayer()
        {
            yield return new WaitForSeconds(1);
            yield return DealCards(new List<ICardHolder>(IPlayer.players));
            yield return new WaitForSeconds(1);
            yield return DealCards(new List<ICardHolder>(IPlayer.players));
            yield return new WaitForSeconds(cardPlacementDelay);
            dealtCards = true;
            IPlayer.PlayerTurn = 0;
        }

        protected override IEnumerator DelayedHitUntil17(bool stayOnceFinished = true, float cardPlacementDelayOverride = cardPlacementDelay)
        {
            yield return new WaitForSeconds(1);
            UpdateCard(1, FacingDirection.Front);
            PlayDealCardSound();
            yield return base.DelayedHitUntil17(stayOnceFinished, 1);

            int dealerHandValue = GetHandValue();

            foreach (IPlayer player in IPlayer.players)
            {
                if (player == (IPlayer)this || player.CurrentState == PlayerState.Spectating)
                {
                    continue;
                }

                int playerHandValue = player.GetHandValue();

                if (dealerHandValue > 21 || dealerHandValue < player.GetHandValue())
                {
                    player.Win("Win!");
                }
                else if (dealerHandValue > playerHandValue)
                {
                    player.Lose($"Lose!");
                }
                else if (dealerHandValue == playerHandValue)
                {
                    player.Draw("Draw!");
                }

                yield return new WaitForSeconds(cardPlacementDelayOverride);
            }

            yield return new WaitForSeconds(2);

            Restart();
        }

        private void PlayDealCardSound()
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(dealCardSound);
        }

        private bool AllPlayersWaiting()
        {
            return IPlayer.players.All(player => player.CurrentState == PlayerState.Waiting);
        }
    }
}
