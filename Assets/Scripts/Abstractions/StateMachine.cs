using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public abstract class StateMachine<T> : MonoBehaviour where T : StateMachine<T>
    {
        public State<T> CurrentState { get; private set; }
        private T _this;

        public StateMachine()
        {
            _this = (T)this;
        }

        public void SetState(State<T> newState)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit(_this);
            }

            CurrentState = newState;
            CurrentState.Enter(_this);
        }

        public void Update()
        {
            CurrentState.Update(_this);
        }
    }
}
