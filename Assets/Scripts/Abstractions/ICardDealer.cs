using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasinoGames.Core
{
    public interface ICardDealer
    {
        IList<ICard> Deck { get; }
        void ShuffleDeck();
        void DealCards();
        void DealCard(ICardHolder cardHolder);
        void ClearBoard();
    }
}
