using CasinoGames.Abstractions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CasinoGames.Core
{
    public abstract class Player : MonoBehaviour, IPlayer
    {
        public string Nickname { get; set; }

        public int Order { get; set; }

        [field: SerializeField]
        public bool IsUser { get; set; }

        protected PlayerState _currentState;
        public PlayerState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    IPlayer.OnStateChanged?.Invoke(this);
                }
            }
        }

        public IList<ICard> Cards { get; protected set; } = new List<ICard>();

        protected const float cardPlacementDelay = 0.25f;

        private int _cash = 1000;
        public int Cash
        {
            get => _cash;
            set
            {
                _cash = value;
                ICashHolder.OnCashChanged?.Invoke(this);
            }
        }

        private int _currentBet;
        public int CurrentBet
        {
            get => _currentBet;
            set
            {
                _currentBet = value;
                IBetter.OnBetChanged?.Invoke(this);
            }
        }

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

            Nickname = gameObject.name;
            Order = transform.GetSiblingIndex();
            IPlayer.players.Add(this);
        }

        #endregion

        public virtual void IncreaseBet(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"{nameof(IncreaseBet)} expects a positive input, received {amount}!");
            }

            CurrentBet = Mathf.Min(CurrentBet + amount, Cash);
        }

        public virtual void DecreaseBet(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"{nameof(DecreaseBet)} expects a positive input, received {amount}!");
            }

            CurrentBet = Mathf.Max(CurrentBet - amount, 0);
        }

        public virtual void ClearBet()
        {
            CurrentBet = 0;
        }

        public virtual void BetAllIn()
        {
            CurrentBet = Cash;
        }

        public virtual void Win(string reason = "")
        {
            IPlayer.OnPlayerWin?.Invoke(this, reason);
            CurrentState = PlayerState.Spectating;
        }

        public virtual void Lose(string reason = "")
        {
            IPlayer.OnPlayerLose?.Invoke(this, reason);
            CurrentState = PlayerState.Spectating;
        }

        public virtual void Draw(string reason = "")
        {
            IPlayer.OnPlayerDraw?.Invoke(this, reason);
        }

        public abstract int GetHandValue();

        public virtual void GiveCard(ICard card)
        {
            Cards.Add(card);
            ICardHolder.OnGivenCard?.Invoke(this);
        }

        public virtual void RemoveAllCards()
        {
            Cards.Clear();
            ICardHolder.OnAllCardsRemoved?.Invoke(this);
        }

        public virtual void UpdateCard(int cardIndex, FacingDirection newFacingDirection)
        {
            Cards[cardIndex].Facing = newFacingDirection;
            ICardHolder.OnCardChanged?.Invoke(this, cardIndex);
        }
    }
}
