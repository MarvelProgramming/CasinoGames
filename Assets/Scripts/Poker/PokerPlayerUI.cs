using CasinoGames.Abstractions;
using CasinoGames.Abstractions.Poker;
using TMPro;
using UnityEngine;

namespace CasinoGames.Core.Poker
{
    public class PokerPlayerUI : PlayerUI
    {
        [SerializeField]
        private GameObject dealerButton;

        [SerializeField]
        private TMP_Text betText;

        #region Unity

        protected override void Awake()
        {
            base.Awake();
            IBetter.OnBetChanged += HandlePlayerBetChanged;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            IBetter.OnBetChanged -= HandlePlayerBetChanged;
        }

        #endregion

        protected override void HandleActivePlayerChanged(IPlayer targetPlayer)
        {
            base.HandleActivePlayerChanged(targetPlayer);
            dealerButton.SetActive((IPokerPlayer)player == IPokerPlayer.DealerButtonHolder);
        }

        private void HandlePlayerBetChanged(IBetter better)
        {
            if (better != (IBetter)player)
            {
                return;
            }

            betText.text = $"${better.CurrentBet}";
        }
    }
}
