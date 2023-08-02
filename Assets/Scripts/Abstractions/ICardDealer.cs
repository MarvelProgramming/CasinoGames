using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasinoGames.Core
{
    public interface ICardDealer
    {
        public static Action<int, ICard> OnDealtCard { get; set; }
        IList<ICard> Deck { get; set; }
        void ShuffleDeck();
        IEnumerator DealCards(IList<ICardHolder> cardHolders);
        void DealCard(ICardHolder cardHolder);
        void ClearBoard(IList<ICardHolder> cardHolders);
    }
}
