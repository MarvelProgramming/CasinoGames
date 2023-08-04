using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class Cardstack : MonoBehaviour
    {
        [SerializeField]
        private IEnumerable<GameObject> placementLocations;
        private int size;

        public void AddCard()
        {
            throw new NotImplementedException();
        }

        public void RemoveAllCards()
        {
            throw new NotImplementedException();
        }
    }
}
