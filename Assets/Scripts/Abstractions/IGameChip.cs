namespace CasinoGames.Abstractions
{
    public interface IGameChip
    {
        public CasinoChipType ChipType { get; }
        public int GetValue();
    }
}
