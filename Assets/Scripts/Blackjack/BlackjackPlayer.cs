using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core.Blackjack
{
    public class BlackjackPlayer : Player, IBlackjackPlayer
    {
        #region Unity

        protected override void Awake()
        {
            base.Awake();
            _currentState = PlayerState.Betting;
            IPlayer.OnActivePlayerChanged += HandleActivePlayerChanged;
            IGameManager.OnBegin += HandleGameBegin;
            IGameManager.OnRestart += HandleGameRestart;
            IGameManager.OnEnd += HandleGameEnd;
        }

        protected virtual void OnDestroy()
        {
            IPlayer.OnActivePlayerChanged -= HandleActivePlayerChanged;
            IGameManager.OnBegin -= HandleGameBegin;
            IGameManager.OnRestart -= HandleGameRestart;
            IGameManager.OnEnd -= HandleGameEnd;
        }

        #endregion

        public override int GetHandValue()
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

        public override void Win(string reason = "")
        {
            Cash += CurrentBet;
            base.Win(reason);
        }

        public override void Lose(string reason = "")
        {
            Cash -= CurrentBet;
            base.Lose(reason);
        }

        public override void Draw(string reason = "")
        {
            CurrentState = PlayerState.Spectating;
            base.Draw(reason);
        }

        /// <summary>
        /// Wrapper function for setting player state from unity event. See <see cref="PlayerState"/> for more info.
        /// </summary>
        public void SetState(int newState)
        {
            CurrentState = (PlayerState)newState;
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

            while (GetHandValue() < 17)
            {
                Hit();
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1f));
            };

            if (stayOnceFinished && CurrentState != PlayerState.Spectating)
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
            throw new NotImplementedException();
        }
    }
}
