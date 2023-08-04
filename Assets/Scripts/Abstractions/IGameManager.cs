using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
{
    public interface IGameManager
    {
        public static Action OnBegin;
        public static Action OnRestart;
        public static Action OnEnd;
        void Begin();
        void Restart();
        void End();
        void Initialize();
    }
}
