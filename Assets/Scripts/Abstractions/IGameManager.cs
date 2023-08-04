using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
{
    public interface IGameManager
    {
        void Begin();
        void Restart();
        void End();
        void Initialize();
    }
}
