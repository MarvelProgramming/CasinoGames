using System.Collections;
using System.Collections.Generic;

namespace CasinoGames.Abstractions
{
    public interface ICardDealer
    {
        IList<ICard> Deck { get; set; }
        void ShuffleDeck();
        IEnumerator DealCards(IList<ICardHolder> cardHolders);
        void DealCard(ICardHolder cardHolder);
        void ClearBoard(IList<ICardHolder> cardHolders);
    }
}
