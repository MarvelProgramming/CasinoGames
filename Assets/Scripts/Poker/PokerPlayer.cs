using CasinoGames.Abstractions;
using CasinoGames.Abstractions.Poker;
using System;
using System.Collections;
using UnityEngine;

namespace CasinoGames.Core.Poker
{
    public class PokerPlayer : Player, IPokerPlayer
    {
        public bool HasMadeBlindBet { get; private set; }

        #region Unity

        protected override void Awake()
        {
            base.Awake();
            _currentState = PlayerState.Betting;
            IPlayer.OnActivePlayerChanged += HandleActivePlayerChanged;
            IGameManager.OnBegin += HandleGameBegin;
            IGameManager.OnRestart += HandleGameRestart;
            IGameManager.OnEnd += HandleGameEnd;
            IPokerPlayer.OnPlayerRaiseBet += HandlePlayerRaiseBet;
        }

        private void OnDestroy()
        {
            IPlayer.OnActivePlayerChanged -= HandleActivePlayerChanged;
            IGameManager.OnBegin -= HandleGameBegin;
            IGameManager.OnRestart -= HandleGameRestart;
            IGameManager.OnEnd -= HandleGameEnd;
            IPokerPlayer.OnPlayerRaiseBet -= HandlePlayerRaiseBet;
        }

        #endregion

        public void Call()
        {
            IPokerPlayer.OnPlayerCall?.Invoke(this);
            CurrentState = PlayerState.Waiting;
        }

        public void Fold()
        {
            IPokerPlayer.OnPlayerFold?.Invoke(this);
            CurrentState = PlayerState.Spectating;
        }

        public void Raise(int amount)
        {
            IPokerPlayer.OnPlayerRaiseBet?.Invoke(this, amount);
            CurrentState = PlayerState.Waiting;
        }

        public override int GetHandValue()
        {
            throw new NotImplementedException();
        }

        private void HandleActivePlayerChanged(IPlayer newActivePlayer)
        {
            /*if (newActivePlayer == (IPlayer)this)
            {
                if (!IsUser)
                {
                    if ((IPokerPlayer.SmallBlindBetter == (IPokerPlayer)this || IPokerPlayer.BigBlindBetter == (IPokerPlayer)this) && !hasMadeBlindBet)
                    {
                        if (IPokerPlayer.SmallBlindBetter == (IPokerPlayer)this)
                        {
                            Raise(2);
                        }
                        else if (IPokerPlayer.BigBlindBetter == (IPokerPlayer)this)
                        {
                            Raise(5);
                        }

                        hasMadeBlindBet = true;
                    }
                    else
                    {
                        float choice = UnityEngine.Random.Range(0f, 1f);

                        if (choice >= 0.8f)
                        {
                            int newBet = Mathf.Max(UnityEngine.Random.Range(1, 20), activeRaiseToRespondTo ? raiseBet : 0);

                            if (newBet <= Cash)
                            {
                                Raise(newBet);
                            }
                            else
                            {
                                Fold();
                            }
                        }
                        else if (choice >= 0.4f)
                        {
                            Call();
                        }
                        else
                        {
                            Fold();
                        }

                        activeRaiseToRespondTo = false;
                    }
                }
                else
                {
                    CurrentState = PlayerState.TakingAction;
                }
            }*/
        }

        private void HandleGameBegin()
        {

        }

        private void HandleGameRestart()
        {
            CurrentBet = 0;
            HasMadeBlindBet = false;
            CurrentState = PlayerState.Betting;
            RemoveAllCards();
        }

        private void HandleGameEnd()
        {
            throw new NotImplementedException();
        }

        private void HandlePlayerRaiseBet(IPokerPlayer player, int newMaxBet)
        {
            /*if (player == (IPokerPlayer)this)
            {
                return;
            }

            raiseBet = newMaxBet;
            activeRaiseToRespondTo = true;
            CurrentState = PlayerState.Betting;*/
        }

        public IEnumerator TakeTurn(PokerGameState gamestate, int maxBet)
        {
            if (CurrentState == PlayerState.Spectating)
            {
                yield break;
            }

            if (gamestate == PokerGameState.Betting)
            {
                yield return new WaitForSeconds(1);

                if (IsBlindBetter() && !HasMadeBlindBet)
                {
                    Call();
                    HasMadeBlindBet = true;
                }
                else if (!IsUser)
                {
                    float choice = UnityEngine.Random.Range(0f, 1f);

                    if (choice >= 0.8f)
                    {
                        int newBet = Mathf.Max(UnityEngine.Random.Range(1, 20), maxBet);

                        if (newBet <= Cash)
                        {
                            Raise(newBet - CurrentBet);
                        }
                        else
                        {
                            Fold();
                        }
                    }
                    else if (choice >= 0.2f)
                    {
                        Call();
                    }
                    else
                    {
                        Fold();
                    }
                }
                else
                {
                    CurrentState = PlayerState.TakingAction;
                }
            }

            yield return new WaitUntil(() => CurrentState != PlayerState.TakingAction);
        }

        private bool IsBlindBetter()
        {
            return IPokerPlayer.SmallBlindBetter == (IPokerPlayer)this || IPokerPlayer.BigBlindBetter == (IPokerPlayer)this;
        }
    }
}
