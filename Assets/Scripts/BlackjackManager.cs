using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    internal class BlackjackManager : MonoBehaviour, IGameManager
    {
        private IBlackjackDealer dealer;
        private IEnumerable<IBlackjackPlayer> players;
        private int activePlayer;

        #region Unity

        private void Start()
        {

        }

        #endregion

        public void Begin()
        {
            throw new NotImplementedException();
        }

        public void Restart()
        {
            throw new NotImplementedException();
        }

        public void End()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void ActivePlayerHit()
        {
            throw new NotImplementedException();
        }

        public void ActivePlayerStay()
        {
            throw new NotImplementedException();
        }

        public void ActivePlayerDouble()
        {
            throw new NotImplementedException();
        }

        public void ActivePlayerIncreaseBet()
        {
            throw new NotImplementedException();
        }

        public void ActivePlayerDecreaseBet()
        {
            throw new NotImplementedException();
        }
    }
}
