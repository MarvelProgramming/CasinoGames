using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core.Blackjack
{
    public class BlackjackPlayer : MonoBehaviour, IBlackjackPlayer
    {
        [field: SerializeField]
        public string Nickname { get; private set; }

        [field: SerializeField]
        public int Order { get; private set; }

        private int _currentBet;
        public int CurrentBet
        {
            get => _currentBet;
            set
            {
                if (_currentBet != value)
                {
                    _currentBet = value;
                    IBetter.OnBetChanged?.Invoke(this);
                }
            }
        }

        private int _cash = 1000;
        public int Cash
        {
            get => _cash;
            set
            {
                if (_cash != value)
                {
                    _cash = value;
                    ICashHolder.OnCashChanged?.Invoke(this);
                }
            }
        }

        public IList<ICard> Cards { get; private set; } = new List<ICard>();

        [field: SerializeField]
        public bool IsUser { get; private set; }

        private PlayerState _currentState = PlayerState.Betting;
        public PlayerState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    IPlayer.OnStateChanged(this);
                }
            }
        }

        protected const float cardPlacementDelay = 0.25f;

        #region Unity

        protected virtual void Awake()
        {
            if (IsUser)
            {
                if (IPlayer.UserPlayer != null)
                {
                    throw new Exception("More than one \"User\" player exists in the scene! There should only ever be one");
                }
                else
                {
                    IPlayer.UserPlayer = this;
                }
            }

            IPlayer.players.Add(this);
            IPlayer.OnActivePlayerChanged += HandleActivePlayerChanged;
            IGameManager.OnBegin += HandleGameBegin;
            IGameManager.OnRestart += HandleGameRestart;
            IGameManager.OnEnd += HandleGameEnd;
        }

        private void OnDestroy()
        {
            IPlayer.OnActivePlayerChanged -= HandleActivePlayerChanged;
            IGameManager.OnBegin -= HandleGameBegin;
            IGameManager.OnRestart -= HandleGameRestart;
            IGameManager.OnEnd -= HandleGameEnd;
        }

        #endregion

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

        public void GiveCard(ICard card)
        {
            Cards.Add(card);
            ICardHolder.OnGivenCard?.Invoke(this);
        }

        public void DecreaseBet(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"{nameof(DecreaseBet)} expects a positive input, received {amount}!");
            }

            CurrentBet = Mathf.Max(CurrentBet - amount, 0);
        }

        public virtual void Hit()
        {
            IBlackjackPlayer.OnHit?.Invoke(this);
        }

        public void Stay()
        {
            CurrentState = PlayerState.Waiting;
            IBlackjackPlayer.OnStay?.Invoke(this);
        }

        public void Double()
        {
            if (Cash >= CurrentBet * 2)
            {
                CurrentBet *= 2;
                IBlackjackPlayer.OnDouble?.Invoke(this);
            }
        }

        public void IncreaseBet(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"{nameof(IncreaseBet)} expects a positive input, received {amount}!");
            }

            CurrentBet = Mathf.Min(CurrentBet + amount, Cash);
        }

        public void ClearBet()
        {
            CurrentBet = 0;
        }

        public void RemoveAllCards()
        {
            Cards.Clear();
            ICardHolder.OnAllCardsRemoved?.Invoke(this);
        }

        public void BetAllIn()
        {
            CurrentBet = Cash;
        }

        /// <summary>
        /// Wrapper function for setting player state from unity event. See <see cref="PlayerState"/> for more info.
        /// </summary>
        public void SetState(int newState)
        {
            CurrentState = (PlayerState)newState;
        }

        public void Win(string reason = "")
        {
            Cash += CurrentBet;
            IPlayer.OnPlayerWin?.Invoke(this, reason);
            CurrentState = PlayerState.Spectating;
        }

        public void Lose(string reason = "")
        {
            Cash -= CurrentBet;
            IPlayer.OnPlayerLose?.Invoke(this, reason);
            CurrentState = PlayerState.Spectating;
        }

        public void Draw(string reason = "")
        {
            IPlayer.OnPlayerDraw?.Invoke(this, reason);
            CurrentState = PlayerState.Spectating;
        }

        protected virtual void HandleActivePlayerChanged(IPlayer newActivePlayer)
        {
            if (newActivePlayer == (IPlayer)this)
            {
                if (!IsUser)
                {
                    StartCoroutine(DelayedHitUntil17());
                }
                else
                {
                    CurrentState = PlayerState.TakingAction;
                }
            }
        }

        protected virtual IEnumerator DelayedHitUntil17(bool stayOnceFinished = true, float cardPlacementDelayOverride = cardPlacementDelay)
        {
            yield return new WaitForSeconds(1);

            do
            {
                Hit();
                yield return new WaitForSeconds(cardPlacementDelayOverride);
            } while (GetHandValue() < 17);

            if (stayOnceFinished)
            {
                Stay();
            }
        }

        private void HandleGameBegin()
        {
            if (!IsUser)
            {
                CurrentBet = UnityEngine.Random.Range(0, Cash);
                CurrentState = PlayerState.Waiting;
            }
        }

        private void HandleGameRestart()
        {
            CurrentBet = 0;
            CurrentState = PlayerState.Betting;
            RemoveAllCards();
        }

        private void HandleGameEnd()
        {
            
        }

        public void UpdateCard(int cardIndex, FacingDirection newFacingDirection)
        {
            Cards[cardIndex].Facing = newFacingDirection;
            ICardHolder.OnCardChanged?.Invoke(this, cardIndex);
        }
    }
}
