using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasinoGames.Core.Blackjack
{
    public class PlayerBlackjackActionUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject actionArea;

        #region Unity

        private void Awake()
        {
            IPlayer.OnStateChanged += HandlePlayerStateChanged;
        }

        private void Start()
        {
            HandlePlayerStateChanged(IPlayer.UserPlayer);
        }

        private void OnDestroy()
        {
            IPlayer.OnStateChanged -= HandlePlayerStateChanged;
        }

        #endregion

        private void HandlePlayerStateChanged(IPlayer player)
        {
            if (player != IPlayer.ActivePlayer)
            {
                return;
            }

            actionArea.SetActive(player.CurrentState == PlayerState.TakingAction);
        }
    }
}
