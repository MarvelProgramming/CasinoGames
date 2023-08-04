using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasinoGames.Abstractions.Poker
{
    public enum PokerGameState
    {
        None = 0,
        PreFlop = 1,
        Flop = 2,
        Turn = 4,
        River = 8,
        Showdown = 16,
        Betting = 32
    }
}
