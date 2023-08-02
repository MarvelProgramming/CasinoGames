using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasinoGames.Core
{
    public interface IPlayer : ICardHolder, ICashHolder, IBetter
    {
        public static Action<IPlayer, string> OnPlayerWin;

        public static Action<IPlayer, string> OnPlayerLose;

        public static Action<IPlayer, string> OnPlayerDraw;

        public static Action<IPlayer> OnActivePlayerChanged;

        public static Action<IPlayer> OnStateChanged;

        public static List<IPlayer> players = new List<IPlayer>();

        private static int _playerTurn;

        public static int PlayerTurn
        {
            get => _playerTurn;
            set
            {
                _playerTurn = Mathf.Max(value, 0) % players.Count;
                OnActivePlayerChanged?.Invoke(ActivePlayer);
            }
        }

        public static IPlayer ActivePlayer => players[PlayerTurn];

        public static IPlayer UserPlayer;

        string Nickname { get; }

        /// <summary>
        /// The order in which this player should take their turn.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Whether this instance is the real player and not a bot.
        /// </summary>
        bool IsUser { get; }

        PlayerState CurrentState { get; set; }

        void Win(string reason = "");

        void Lose(string reason = "");

        void Draw(string reason = "");

    }
}
