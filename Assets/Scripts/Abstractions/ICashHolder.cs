using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
{
    public interface ICashHolder
    {
        public static Action<ICashHolder> OnCashChanged;
        int Cash { get; set; }
    }
}
