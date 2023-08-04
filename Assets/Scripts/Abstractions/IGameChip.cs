using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
{
    public interface IGameChip
    {
        public CasinoChipType ChipType { get; }
        public int GetValue();
    }
}
