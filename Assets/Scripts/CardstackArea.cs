using CasinoGames.Core.Blackjack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class CardstackArea : MonoBehaviour
    {
        public int Size { get; private set; }

        [SerializeField, Range(-1, 1)]
        private int placementDirection = 1;

        [SerializeField]
        private BlackjackPlayer player;

        [SerializeField]
        private GameObject physicalCardPrefab;

        [SerializeField]
        private GameObject basePlacementLocation;

        #region Unity

        private void Awake()
        {
            ICardHolder.OnGivenCard += HandleHolderGivenCard;
            ICardHolder.OnCardChanged += HandleHolderCardChanged;
            ICardHolder.OnAllCardsRemoved += HandleHolderCardsRemoved;
        }

        private void OnDestroy()
        {
            ICardHolder.OnGivenCard -= HandleHolderGivenCard;
            ICardHolder.OnCardChanged -= HandleHolderCardChanged;
            ICardHolder.OnAllCardsRemoved -= HandleHolderCardsRemoved;
        }

        #endregion

        public void AddCard(PhysicalCard card)
        {
            card.transform.SetParent(basePlacementLocation.transform, false);
            card.transform.localPosition += Vector3.right * placementDirection * 0.1f * Size;
            card.transform.position += Vector3.up * 0.0008f * Size;

            if (Size > 0)
            {
                basePlacementLocation.transform.localPosition -= Vector3.right * placementDirection * 0.1f * 0.5f;
            }

            Size++;
        }

        public void UpdateCard(int cardIndex, FacingDirection newDirection)
        {
            PhysicalCard cardToUpdate = basePlacementLocation.transform.GetChild(cardIndex).GetComponent<PhysicalCard>();
            cardToUpdate.SetFacingDirection(newDirection);
        }

        public void RemoveAllCards()
        {
            foreach (Transform physicalCardTransform in basePlacementLocation.transform)
            {
                Destroy(physicalCardTransform.gameObject);
            }

            Size = 0;
            basePlacementLocation.transform.localPosition = new Vector3(0, basePlacementLocation.transform.localPosition.y, basePlacementLocation.transform.localPosition.z);
        }

        private void HandleHolderGivenCard(ICardHolder holder)
        {
            if (holder is IPlayer targetPlayer && targetPlayer == (IPlayer)player)
            {
                GameObject physicalCardObject = Instantiate(physicalCardPrefab);
                PhysicalCard physicalCard = physicalCardObject.GetComponent<PhysicalCard>();
                ICard newCard = holder.Cards[^1];
                physicalCard.Setup(newCard.FrontImage, newCard.BackImage, newCard.Facing);
                AddCard(physicalCard);
            }
        }

        private void HandleHolderCardsRemoved(ICardHolder holder)
        {
            if (holder is IPlayer targetPlayer && targetPlayer == (IPlayer)player)
            {
                RemoveAllCards();
            }
        }

        private void HandleHolderCardChanged(ICardHolder holder, int cardIndex)
        {
            if (holder is IPlayer targetPlayer && targetPlayer == (IPlayer)player)
            {
                UpdateCard(cardIndex, holder.Cards[cardIndex].Facing);
            }
        }
    }
}
