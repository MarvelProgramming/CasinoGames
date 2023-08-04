using CasinoGames.Abstractions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CasinoGames.Core.Blackjack
{
    public class PlayerBlackjackBettingUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject areaWrapper;

        [SerializeField]
        private BetOption dealButton;

        [SerializeField]
        private BetOption[] betOptions;

        [Serializable]
        public class BetOption
        {
            [field: SerializeField]
            public int Value { get; private set; }

            [field: SerializeField]
            public Button ChipButton { get; private set; }

            [field: SerializeField]
            public Image ChipImage { get; private set; }

            [field: SerializeField]
            public TMP_Text ChipText { get; private set; }

            private Color originalChipTextColor;

            public BetOption()
            {
                if (ChipText != null)
                {
                    originalChipTextColor = ChipText.color;
                }
            }

            public void SetEnabled(bool enabled)
            {
                ChipButton.enabled = enabled;
                float alpha = enabled ? 1 : 0.4f;

                if (ChipImage != null)
                {
                    ChipImage.color = new Color(1, 1, 1, alpha);
                }

                if (ChipText != null)
                {
                    ChipText.color = new Color(originalChipTextColor.r, originalChipTextColor.g, originalChipTextColor.b, alpha);
                }
            }
        }

        #region Unity

        private void Awake()
        {

            IBetter.OnBetChanged += HandlePlayerBetChanged;
            IPlayer.OnStateChanged += HandlePlayerStateChanged;
        }

        private void Start()
        {
            HandlePlayerBetChanged(IPlayer.UserPlayer);
            HandlePlayerStateChanged(IPlayer.UserPlayer);
        }

        private void OnDestroy()
        {
            IBetter.OnBetChanged -= HandlePlayerBetChanged;
            IPlayer.OnStateChanged -= HandlePlayerStateChanged;
        }

        #endregion

        private void HandlePlayerBetChanged(IBetter better)
        {
            if (better is IPlayer player && player == IPlayer.UserPlayer)
            {
                int remainingCashToBet = player.Cash - player.CurrentBet;

                foreach (BetOption option in betOptions)
                {
                    option.SetEnabled(remainingCashToBet >= option.Value);
                }

                dealButton.SetEnabled(player.CurrentBet > 0);
            }
        }

        private void HandlePlayerStateChanged(IPlayer player)
        {
            if (player == IPlayer.UserPlayer)
            {
                areaWrapper.SetActive(player.CurrentState == PlayerState.Betting);
            }
        }
    }
}
