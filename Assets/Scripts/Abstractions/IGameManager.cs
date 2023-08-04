using System;

namespace CasinoGames.Abstractions
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
