using CasinoGames.Abstractions.Poker;
using CasinoGames.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CasinoGames.Core
{
    public class PokerPlayerActionUI : PlayerActionUI
    {
        [SerializeField]
        private TMP_Text callButtonText;

        [SerializeField]
        private Button raiseButton;

        [SerializeField]
        private TMP_InputField raiseInput;

        public void HandleUpdateRaiseInput()
        {
            CancelInvoke(nameof(RaiseInputSanitation));
            Invoke(nameof(RaiseInputSanitation), 0.35f);
        }

        private void RaiseInputSanitation()
        {
            raiseInput.SetTextWithoutNotify(GetSanitizedRaiseInput().ToString());
            UpdateRaiseButtonInteractability();
        }

        public void Raise()
        {
            ((IPokerPlayer)IPlayer.UserPlayer).Raise(GetSanitizedRaiseInput());
        }

        protected override void HandlePlayerStateChanged(IPlayer player)
        {
            base.HandlePlayerStateChanged(player);

            if (actionArea.activeSelf)
            {
                callButtonText.text = $"Call ${PlayerUtils.GetMaxBet()}";
                raiseInput.SetTextWithoutNotify((PlayerUtils.GetMaxBet() - IPlayer.UserPlayer.CurrentBet).ToString());
                UpdateRaiseButtonInteractability();
            }
        }

        private int GetSanitizedRaiseInput()
        {
            string newRaiseAmountText = raiseInput.text;
            int maxBet = PlayerUtils.GetMaxBet();

            if (string.IsNullOrEmpty(newRaiseAmountText))
            {
                return maxBet - IPlayer.UserPlayer.CurrentBet;
            }

            int newRaiseAmount = int.Parse(newRaiseAmountText);

            if (newRaiseAmount + IPlayer.UserPlayer.CurrentBet > IPlayer.UserPlayer.Cash)
            {
                return IPlayer.UserPlayer.Cash - IPlayer.UserPlayer.CurrentBet;
            }
            else if (newRaiseAmount + IPlayer.UserPlayer.CurrentBet < maxBet)
            {
                return maxBet - IPlayer.UserPlayer.CurrentBet;
            }
            else
            {
                return newRaiseAmount;
            }
        }

        private void UpdateRaiseButtonInteractability()
        {
            raiseButton.interactable = IPlayer.UserPlayer.Cash - IPlayer.UserPlayer.CurrentBet > 0 && raiseInput.text != "0";
        }
    }
}
