using CasinoGames.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class Chipstack : IChipstack
    {
        public CasinoChipType StackType => baseChip.ChipType;

        public int Size { get; private set; }

        private IGameChip baseChip;

        public Chipstack(IGameChip baseChip, int size)
        {
            this.baseChip = baseChip;
            Size = size;
        }

        public void DecreaseSize(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"{nameof(DecreaseSize)} expects a positive input, received {amount}");
            }

            Size = Mathf.Max(Size - amount, 0);
        }

        public int GetTotalValue()
        {
            throw new NotImplementedException();
        }

        public void SetStack(IGameChip chip, int size)
        {
            throw new NotImplementedException();
        }
    }
}
