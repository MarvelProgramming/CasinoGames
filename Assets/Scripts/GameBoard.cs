using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField]
        private IEnumerable<PlayerArea> playerAreas;

        public void UpdatePlayerCards(IEnumerable<ICard> cards)
        {
            throw new NotImplementedException();
        }
    }
}
