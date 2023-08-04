using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
{
    public interface ICardHolder
    {
        IList<ICard> Cards { get; }
        int GetHandValue();
        void AddCard(ICard card);
        void RemoveAllCards();
    }
}
