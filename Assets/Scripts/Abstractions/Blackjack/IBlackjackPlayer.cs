using System;

namespace CasinoGames.Abstractions.Blackjack
{
    public interface IBlackjackPlayer : IPlayer
    {
        public static Action<IBlackjackPlayer> OnHit;
        public static Action<IBlackjackPlayer> OnStay;
        public static Action<IBlackjackPlayer> OnDouble;
        void Hit();
        void Stay();
        void Double();
    }
}
