using CasinoGames.Abstractions;
using CasinoGames.Abstractions.Poker;
using CasinoGames.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CasinoGames.Core.Poker
{
    [RequireComponent(typeof(AudioSource))]
    public class PokerGameManager : MonoBehaviour, ICardDealer, IGameManager
    {
        public IList<ICard> Deck { get; set; }

        [SerializeField]
        private List<CasinoCard> baseDeck;

        private List<ICard> board = new List<ICard>();

        [SerializeField]
        private int minBet;

        [SerializeField]
        private PokerCommunityCardArea communityCardArea;

        [SerializeField]
        private PokerPotUI potUI;

        [SerializeField]
        private AudioClip dealCardSound;

        private int _currentPot;
        private int CurrentPot
        {
            get => _currentPot;
            set
            {
                _currentPot = value;
                potUI.UpdatePotText(_currentPot);
            }
        }

        private AudioSource audioSource;

        private PokerGameState currentState;

        private PokerGameState finishedStates;

        #region Unity

        private void Awake()
        {
            IPlayer.OnStateChanged += HandlePlayerStateChanged;
            IPokerPlayer.OnPlayerRaiseBet += HandlePlayerRaisedBet;
            IPokerPlayer.OnPlayerCall += HandlePlayerCalledBet;
            IPokerPlayer.OnPlayerFold += HandlePlayerFolded;
            SceneManager.sceneUnloaded += HandleSceneChanged;
            Initialize();
        }

        private void Start()
        {
            IPlayer.players.Sort((firstPlayer, secondPlayer) => firstPlayer.Order - secondPlayer.Order);
            Begin();
        }

        private void OnDestroy()
        {
            IPlayer.OnStateChanged -= HandlePlayerStateChanged;
            IPokerPlayer.OnPlayerRaiseBet -= HandlePlayerRaisedBet;
            IPokerPlayer.OnPlayerCall -= HandlePlayerCalledBet;
            IPokerPlayer.OnPlayerFold -= HandlePlayerFolded;
        }

        #endregion

        public void Begin()
        {
            finishedStates = PokerGameState.None;
            ExecuteNextGameState();
        }

        public void Restart()
        {
            IPokerPlayer.DealerButtonLocation++;
            communityCardArea.ClearCards();
            board.Clear();
            finishedStates = PokerGameState.None;
            currentState = PokerGameState.None;
            CurrentPot = 0;
            IPlayer.PlayerTurn = -1;
            Initialize();
            IGameManager.OnRestart?.Invoke();
            ExecuteNextGameState();
        }

        public void End()
        {
            IPlayer.players.Clear();
            IPlayer.UserPlayer = null;
            IPlayer.PlayerTurn = -1;
            IGameManager.OnEnd?.Invoke();
        }

        public void Initialize()
        {
            audioSource = GetComponent<AudioSource>();
            Deck = new List<ICard>(baseDeck);

            foreach (ICard card in Deck)
            {
                card.Facing = FacingDirection.Back;
            }
        }

        public void ShuffleDeck()
        {
            const int timesToShuffle = 30;

            for (int i = 0; i < timesToShuffle; i++)
            {
                int firstCardIndex = UnityEngine.Random.Range(0, Deck.Count);
                int secondCardIndex = UnityEngine.Random.Range(0, Deck.Count);

                // Avoid attempting to swap the same card with itself.
                secondCardIndex = secondCardIndex == firstCardIndex ? (secondCardIndex + 1) % Deck.Count : secondCardIndex;

                ICard firstCard = Deck[firstCardIndex];
                ICard secondCard = Deck[secondCardIndex];
                Deck[firstCardIndex] = secondCard;
                Deck[secondCardIndex] = firstCard;
            }
        }

        public IEnumerator DealCards(IList<ICardHolder> cardHolders)
        {
            ShuffleDeck();

            for (int i = 0; i < cardHolders.Count; i++)
            {
                ICardHolder cardHolder = cardHolders[i];
                DealCard(cardHolder);
                yield return new WaitForSeconds(0.25f);
            }
        }

        public void DealCard(ICardHolder cardHolder)
        {
            ICard dealtCard = GetRandomDeckCard();
            dealtCard.Facing = FacingDirection.Back;

            if (cardHolder == IPlayer.UserPlayer)
            {
                dealtCard.Facing = FacingDirection.Front;
            }

            cardHolder.GiveCard(dealtCard);
            Deck.Remove(dealtCard);
            PlayDealCardSound();
        }

        public void ClearBoard(IList<ICardHolder> cardHolders)
        {
            throw new NotImplementedException();
        }

        private void HandleSceneChanged(Scene _)
        {
            End();
            SceneManager.sceneUnloaded -= HandleSceneChanged;
        }

        private void HandlePlayerStateChanged(IPlayer player)
        {
            /*if (!dealtCards && AllPlayersWaiting())
            {
                StartCoroutine(DealCards());
            }
            else if (player.CurrentState == PlayerState.Waiting || player.CurrentState == PlayerState.Spectating)
            {
                IPlayer.PlayerTurn++;
            }*/
        }

        private void HandlePlayerRaisedBet(IPokerPlayer player, int raiseAmount)
        {
            player.IncreaseBet(raiseAmount);
            CurrentPot += raiseAmount;
            player.Cash -= raiseAmount;
        }

        private void HandlePlayerCalledBet(IPokerPlayer player)
        {
            if (IPokerPlayer.SmallBlindBetter == player && !player.HasMadeBlindBet)
            {
                int smallBlindBet = (int)(minBet * 0.5f);
                player.IncreaseBet(smallBlindBet);
                CurrentPot += smallBlindBet;
                player.Cash -= smallBlindBet;
            }
            else if (IPokerPlayer.BigBlindBetter == player && !player.HasMadeBlindBet)
            {
                player.IncreaseBet(minBet);
                CurrentPot += minBet;
                player.Cash -= minBet;
            }
            else
            {
                int maxBet = PlayerUtils.GetMaxBet();
                int playerRaiseAmount = maxBet - player.CurrentBet;

                if (playerRaiseAmount > 0)
                {
                    if (player.CurrentBet + playerRaiseAmount < player.Cash)
                    {
                        player.IncreaseBet(playerRaiseAmount);
                        CurrentPot += playerRaiseAmount;
                        player.Cash -= playerRaiseAmount;
                    }
                    else
                    {
                        player.IncreaseBet(player.Cash - player.CurrentBet);
                        CurrentPot += player.Cash - player.CurrentBet;
                        player.Cash -= player.Cash - player.CurrentBet;
                    }
                }
            }
        }

        private void HandlePlayerFolded(IPokerPlayer player)
        {
            player.Lose("Fold");
        }

        private void HandlePlayerFinishTurn(IPokerPlayer player)
        {
            /*ExecuteNextGameState();*/
        }

        private IEnumerator DealCards()
        {
            yield return new WaitForSeconds(1);
            yield return DealCards(new List<ICardHolder>(IPlayer.players));
            yield return new WaitForSeconds(1);
            yield return DealCards(new List<ICardHolder>(IPlayer.players));
            yield return new WaitForSeconds(1);

            IPlayer.PlayerTurn = (IPokerPlayer.DealerButtonLocation + 1) % Mathf.Max(IPlayer.players.Count, 1);
            ExecuteNextGameState();
        }

        private void PlayDealCardSound()
        {
            audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(dealCardSound);
        }

        private ICard GetRandomDeckCard()
        {
            return Deck[UnityEngine.Random.Range(0, Deck.Count)];
        }

        private void ExecuteNextGameState()
        {
            finishedStates |= currentState;
            currentState = GetNextGameState();

            switch (currentState)
            {
                case PokerGameState.PreFlop:
                    ExecutePreFlop();
                    break;
                case PokerGameState.Flop:
                    StartCoroutine(ExecuteFlop());
                    break;

                case PokerGameState.Turn:
                    StartCoroutine(ExecuteTurn());
                    break;

                case PokerGameState.River:
                    StartCoroutine(ExecuteRiver());
                    break;
                case PokerGameState.Showdown:
                    StartCoroutine(ExecuteShowdown());
                    break;
                case PokerGameState.Betting:
                    StartCoroutine(ExecuteBetting());
                    break;
            }
        }

        private void ExecutePreFlop()
        {
            StartCoroutine(DealCards());
        }

        private IEnumerator ExecuteFlop()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.25f);
                AddCardToBoard();
            }

            ExecuteNextGameState();
        }

        private IEnumerator ExecuteTurn()
        {
            yield return new WaitForSeconds(1);
            AddCardToBoard();
            ExecuteNextGameState();
        }

        private IEnumerator ExecuteRiver()
        {
            yield return new WaitForSeconds(1);
            AddCardToBoard();
            ExecuteNextGameState();
        }

        private IEnumerator ExecuteShowdown()
        {
            List<IPokerPlayer> remainingPlayers = GetRemainingPlayers();
            board.Sort((firstCard, secondCard) => firstCard.Value - secondCard.Value);

            // Reveal player hands.
            foreach (IPokerPlayer player in remainingPlayers)
            {
                // Player's hand is always visible (to the user), so it's okay to skip them.
                if (player == IPlayer.UserPlayer)
                {
                    continue;
                }

                for (int i = 0; i < player.Cards.Count; i++)
                {
                    player.UpdateCard(i, FacingDirection.Front);
                    PlayDealCardSound();
                    yield return new WaitForSeconds(0.25f);
                }

                yield return new WaitForSeconds(1);
            }

            int winningHandValue = int.MinValue;
            IPokerPlayer winningPlayer = null;
            int j = 0;

            // Determine which player has won, or if there's been a draw.
            while (j < remainingPlayers.Count)
            {
                IPokerPlayer player = remainingPlayers[j];
                int handValue = (int)GetPlayerHandType(player);
                bool shouldIncrementJ = true;

                if (handValue == (int)PokerHandType.HighCard)
                {
                    List<ICard> playerCards = player.Cards.ToList();
                    playerCards.Sort((firstCard, secondCard) => firstCard.Value - secondCard.Value);
                    handValue = playerCards[^1].Value;
                }

                if (winningHandValue < handValue)
                {
                    if (winningPlayer != null)
                    {
                        winningPlayer.Lose($"{(winningHandValue < 20 ? PokerHandType.HighCard : (PokerHandType)winningHandValue)}\nLose!");
                        remainingPlayers.Remove(winningPlayer);
                        shouldIncrementJ = false;
                    }

                    winningHandValue = handValue;
                    winningPlayer = player;
                }
                else if (winningHandValue > handValue)
                {
                    player.Lose($"{(handValue < 20 ? PokerHandType.HighCard : (PokerHandType)handValue)}\nLose!");
                    remainingPlayers.Remove(player);
                    shouldIncrementJ = false;
                }
                else
                {
                    List<ICard> winningPlayerCards = winningPlayer.Cards.ToList();
                    winningPlayerCards.Sort((firstCard, secondCard) => firstCard.Value - secondCard.Value);
                    List<ICard> currentPlayerCards = player.Cards.ToList();
                    currentPlayerCards.Sort((firstCard, secondCard) => firstCard.Value - secondCard.Value);

                    for (int k = 1; k < 2; k++)
                    {
                        if (winningPlayerCards[^k].Value > currentPlayerCards[^k].Value)
                        {
                            player.Lose(PokerHandType.HighCard.ToString());
                            remainingPlayers.Remove(player);
                            shouldIncrementJ = false;
                            break;
                        }
                    }
                }

                if (shouldIncrementJ)
                {
                    j++;
                }
            }

            // Split the pot between players in a draw, or pay the individual winner.
            if (remainingPlayers.Count > 1)
            {
                int potSplit = CurrentPot / remainingPlayers.Count;

                remainingPlayers.ForEach(player =>
                {
                    player.Cash += potSplit;
                    player.Win("Draw!");
                });
            }
            else
            {
                winningPlayer.Win($"{(winningHandValue < 20 ? PokerHandType.HighCard : (PokerHandType)winningHandValue)}\nWin!");
                winningPlayer.Cash += CurrentPot;
            }

            yield return new WaitForSeconds(2);

            Restart();
        }

        private void AddCardToBoard()
        {
            ICard randomCard = GetRandomDeckCard();
            communityCardArea.AddCard(randomCard);
            board.Add(randomCard);
            PlayDealCardSound();
        }

        private IEnumerator ExecuteBetting()
        {
            int activePlayers = GetNumberOfActivePlayers();
            int playersIterated = 0;

            do
            {
                yield return ((IPokerPlayer)IPlayer.ActivePlayer).TakeTurn(currentState, PlayerUtils.GetMaxBet());

                if (GetNumberOfActivePlayers() == 1)
                {
                    GetLastActivePlayer().Win("Win!");
                    yield return new WaitForSeconds(1);
                    Restart();
                    yield break;
                }

                MoveToNextActivePlayer();
                playersIterated++;
            } while (!AllPlayersFinishedBetting() || playersIterated < activePlayers);

            ExecuteNextGameState();
        }

        private PokerGameState GetNextGameState()
        {
            Array states = Enum.GetValues(typeof(PokerGameState));

            if ((int)currentState >= (int)PokerGameState.PreFlop && currentState != PokerGameState.Betting)
            {
                return PokerGameState.Betting;
            }

            foreach (PokerGameState state in states)
            {
                if (state != PokerGameState.None && !finishedStates.HasFlag(state))
                {
                    return state;
                }
            }

            return currentState;
        }

        private bool AllPlayersFinishedBetting()
        {
            int maxBet = PlayerUtils.GetMaxBet();

            return IPlayer.players.All(player => player.CurrentBet == maxBet || player.CurrentBet == player.Cash || player.CurrentState == PlayerState.Spectating);
        }

        private void MoveToNextActivePlayer()
        {
            do
            {
                IPlayer.PlayerTurn++;
            } while (IPlayer.ActivePlayer.CurrentState == PlayerState.Spectating);
        }

        private List<IPokerPlayer> GetRemainingPlayers()
        {
            return IPlayer.players.Where(player => player.CurrentState != PlayerState.Spectating).Select(player => (IPokerPlayer)player).ToList();
        }

        private int GetNumberOfActivePlayers()
        {
            return IPlayer.players.Sum((player) => player.CurrentState != PlayerState.Spectating ? 1 : 0);
        }

        private IPokerPlayer GetLastActivePlayer()
        {
            return (IPokerPlayer)IPlayer.players.Find(player => player.CurrentState != PlayerState.Spectating);
        }

        private PokerHandType GetPlayerHandType(IPlayer player)
        {
            if (PlayerHasTypeOfStraight(player, true, true))
            {
                return PokerHandType.StraightFlush;
            }

            if (PlayerHasNOfTheSameValueCard(player, 4, null, out var _))
            {
                return PokerHandType.FourOfAKind;
            }

            if (PlayerHasNOfTheSameValueCard(player, 3, null, out ICard foundCard) && PlayerHasNOfTheSameValueCard(player, 2, foundCard, out var _))
            {
                return PokerHandType.FullHouse;
            }

            if (PlayerHasTypeOfStraight(player, true, false))
            {
                return PokerHandType.Flush;
            }

            if (PlayerHasTypeOfStraight(player, false, true))
            {
                return PokerHandType.Straight;
            }

            if (PlayerHasNOfTheSameValueCard(player, 3, null, out var _))
            {
                return PokerHandType.ThreeOfAKind;
            }

            if (PlayerHasNOfTheSameValueCard(player, 2, null, out foundCard) && PlayerHasNOfTheSameValueCard(player, 2, foundCard, out var _))
            {
                return PokerHandType.TwoPairs;
            }

            if (PlayerHasNOfTheSameValueCard(player, 2, null, out var _))
            {
                return PokerHandType.Pair;
            }

            return PokerHandType.HighCard;
        }

        private bool PlayerHasTypeOfStraight(IPlayer player, bool sameSuit, bool sequential)
        {
            List<ICard> cards = new List<ICard>(board);
            cards.AddRange(player.Cards);
            cards.Sort((firstCard, secondCard) => firstCard.Value - secondCard.Value);
            List<ICard> aces = new List<ICard>();
            int i = 0;

            while (i < cards.Count)
            {
                // Assuming any card with a value of 11 is an ace.
                if (cards[i].Value == 11)
                {
                    aces.Add(cards[i]);
                    cards.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            ICard prevCard = cards[0];
            int cardsInSequence = 1;

            for (int j = 0; j < cards.Count; j++)
            {
                if (cards[j].Value == 2 || cards[j].Value == 13)
                {
                    ICard validAce = aces.Find(ace => !sameSuit || ace.Suit == cards[j].Suit);

                    if (validAce != null)
                    {
                        aces.Remove(validAce);
                        cardsInSequence++;
                        continue;
                    }
                }

                if (j > 0 && (!sequential || cards[j].Value - prevCard.Value == 1) && (!sameSuit || cards[j].Suit == prevCard.Suit))
                {
                    prevCard = cards[j];
                    cardsInSequence++;
                }
            }

            return cardsInSequence >= 5;
        }

        private bool PlayerHasNOfTheSameValueCard(IPlayer player, int n, ICard exlusionCard, out ICard foundCard)
        {
            foundCard = null;
            List<ICard> cards = new List<ICard>(board);
            cards.AddRange(player.Cards);
            Dictionary<int, int> cardTally = new Dictionary<int, int>();

            for (int i = 0; i < cards.Count; i++)
            {
                ICard card = cards[i];

                if (exlusionCard != null && card.Value == exlusionCard.Value)
                {
                    continue;
                }

                if (cardTally.ContainsKey(card.Value))
                {
                    cardTally[card.Value]++;

                    if (cardTally[card.Value] == n)
                    {
                        foundCard = card;
                        return true;
                    }
                }
                else
                {
                    cardTally.Add(card.Value, 1);
                }
            }

            return false;
        }
    }
}
