using CasinoGames.Abstractions;
using UnityEngine;

namespace CasinoGames.Core.Poker
{
    public class PokerCommunityCardArea : MonoBehaviour
    {
        [SerializeField]
        private PhysicalCard[] communityCards;

        private int currentCard;

        public void AddCard(ICard card)
        {
            communityCards[currentCard].Setup(card.FrontImage, card.BackImage, FacingDirection.Front);
            communityCards[currentCard].gameObject.SetActive(true);
            currentCard++;
        }

        public void ClearCards()
        {
            foreach (PhysicalCard physicalCard in communityCards)
            {
                physicalCard.gameObject.SetActive(false);
            }

            currentCard = 0;
        }
    }
}
