using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField]
        private List<PlayerArea> playerAreas;

        #region Unity

        private void Awake()
        {
            ICardDealer.OnDealtCard += HandlePlayerDealtCard;
            ICard.OnCardChanged += HandlePlayerCardChanged;
        }

        private void OnDestroy()
        {
            ICardDealer.OnDealtCard -= HandlePlayerDealtCard;
            ICard.OnCardChanged -= HandlePlayerCardChanged;
        }

        #endregion

        public void HandlePlayerDealtCard(int playerIndex, ICard dealtCard)
        {
            /*PlayerArea playerArea = GetPlayerArea(playerIndex);
            playerArea.AddCard(dealtCard);*/
        }

        public void HandlePlayerCardChanged(int playerIndex, int cardIndex, ICard changedCard)
        {
            /*PlayerArea playerArea = GetPlayerArea(playerIndex);
            playerArea.UpdateCard(cardIndex, changedCard);*/
        }

        private PlayerArea GetPlayerArea(int playerIndex)
        {
            return playerAreas.Count == 0 ? null : playerAreas[playerIndex];
        }
    }
}
