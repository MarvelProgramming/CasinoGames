using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
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
