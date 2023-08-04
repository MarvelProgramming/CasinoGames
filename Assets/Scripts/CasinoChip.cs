using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasinoGames.Core
{
    public class CasinoChip
    {
        public CasinoChipType type;

        public CasinoChip(CasinoChipType type)
        {
            this.type = type;
        }

        public int GetValue()
        {
            return (int)type;
        }
    }
}