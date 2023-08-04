using System;
using System.Collections;
using UnityEngine;

namespace CasinoGames.Abstractions.Poker
{
    public interface IPokerPlayer : IPlayer
    {
        public static Action<IPokerPlayer> OnPlayerFinishTurn;

        public static Action<IPokerPlayer, int> OnPlayerRaiseBet;

        public static Action<IPokerPlayer> OnPlayerCall;

        public static Action<IPokerPlayer> OnPlayerFold;

        private static int _dealerButtonLocation;

        /// <summary>
        /// The index of the player which currently "holds" the dealer button.
        /// </summary>
        public static int DealerButtonLocation
        {
            get => _dealerButtonLocation;
            set
            {
                if (value < 0)
                {
                    _dealerButtonLocation = players.Count - 1;
                }
                else
                {
                    _dealerButtonLocation = value % Mathf.Max(players.Count, 1);
                }
            }
        }

        public static IPokerPlayer DealerButtonHolder => DealerButtonLocation < players.Count ? (IPokerPlayer)players[DealerButtonLocation] : null;

        public static IPokerPlayer SmallBlindBetter => (DealerButtonLocation + 1) % Mathf.Max(players.Count, 1) < players.Count ? (IPokerPlayer)players[(DealerButtonLocation + 1) % Mathf.Max(players.Count, 1)] : null;

        public static IPokerPlayer BigBlindBetter => (DealerButtonLocation + 2) % Mathf.Max(players.Count, 1) < players.Count ? (IPokerPlayer)players[(DealerButtonLocation + 2) % Mathf.Max(players.Count, 1)] : null;

        public bool HasMadeBlindBet { get; }

        void Raise(int amount);

        void Call();

        void Fold();

        IEnumerator TakeTurn(PokerGameState gamestate, int maxBet);
    }
}
