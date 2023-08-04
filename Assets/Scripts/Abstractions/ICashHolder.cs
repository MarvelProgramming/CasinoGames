using System;

namespace CasinoGames.Abstractions
{
    public interface ICashHolder
    {
        public static Action<ICashHolder> OnCashChanged;
        int Cash { get; set; }
    }
}
