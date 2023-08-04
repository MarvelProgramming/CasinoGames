using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class Chipstack : MonoBehaviour
    {
        [SerializeField]
        private int maxStackSize;

        public void SetChips(CasinoChipType type, int amount)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllChips()
        {
            throw new NotImplementedException();
        }

        public int GetChipCount()
        {
            throw new NotImplementedException();
        }
    }
}
