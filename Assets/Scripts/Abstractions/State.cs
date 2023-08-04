using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
{
    public abstract class State<T> where T : StateMachine<T>
    {
        public abstract void Enter(T sm);
        public abstract void Update(T sm);
        public abstract void Exit(T sm);
    }
}
