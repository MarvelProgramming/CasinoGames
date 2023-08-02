using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
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
