using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core
{
    public interface ICardHolder
    {
        public static Action<ICardHolder> OnGivenCard;
        public static Action<ICardHolder, int> OnCardChanged;
        public static Action<ICardHolder> OnAllCardsRemoved;
        IList<ICard> Cards { get; }
        int GetHandValue();
        void GiveCard(ICard card);
        void UpdateCard(int cardIndex, FacingDirection newFacingDirection);
        void RemoveAllCards();
    }
}
