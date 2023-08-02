using CasinoGames.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class PlayerArea : MonoBehaviour
    {
        [SerializeField]
        private GameObject physicalCardPrefab;
        [SerializeField]
        private CardstackArea cardstack;

        public void AddCard(ICard card)
        {
            GameObject physicalCardObject = Instantiate(physicalCardPrefab);
            PhysicalCard physicalCard = physicalCardObject.GetComponent<PhysicalCard>();
            cardstack.AddCard(physicalCard);
            physicalCard.Setup(card.FrontImage, card.BackImage, card.Facing);
        }

        public void UpdateCard(int cardIndex, ICard cardDetails)
        {
            cardstack.UpdateCard(cardIndex, cardDetails.Facing);
        }

        public void RemoveAllCards()
        {
            cardstack.RemoveAllCards();
        }
    }
}
