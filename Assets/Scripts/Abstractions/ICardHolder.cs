using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
{
    public interface ICardHolder
    {
        IEnumerable<ICard> Cards { get; }
        int TotalCardValue { get; }
        void AddCard(ICard card);
        void RemoveAllCards();
    }
}
