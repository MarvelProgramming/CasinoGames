using CasinoGames.Abstractions;

namespace CasinoGames.Core
{
    public class CasinoChip : IGameChip
    {
        public CasinoChipType ChipType { get; private set; }

        public CasinoChip(CasinoChipType type)
        {
            ChipType = type;
        }

        public int GetValue()
        {
            return (int)ChipType;
        }
    }
}