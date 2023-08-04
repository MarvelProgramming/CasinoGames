using System;

namespace CasinoGames.Abstractions
{
    [Serializable]
    public enum PlayerState
    {
        Waiting,
        Betting,
        TakingAction,
        Spectating
    }
}
