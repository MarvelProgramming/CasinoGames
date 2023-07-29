using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
{
    public interface IChipstack
    {
        CasinoChipType StackType { get; }
        int Size { get; }
        void SetStack(IGameChip chip, int size);
        void DecreaseSize(int amount);
        int GetTotalValue();
    }
}
