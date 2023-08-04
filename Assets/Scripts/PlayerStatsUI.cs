using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace CasinoGames.Core
{
    public class PlayerStatsUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text playerCashText;
        [SerializeField]
        private TMP_Text playerBetText;

        #region Unity

        private void Awake()
        {
            ICashHolder.OnCashChanged += HandleCashChanged;
            IBetter.OnBetChanged += HandleBetChanged;
        }

        private void Start()
        {
            SetCashUI(IPlayer.UserPlayer.Cash);
            SetBetUI(IPlayer.UserPlayer.CurrentBet);
        }

        private void OnDestroy()
        {
            ICashHolder.OnCashChanged -= HandleCashChanged;
            IBetter.OnBetChanged -= HandleBetChanged;
        }

        #endregion

        public void SetCashUI(int cash)
        {
            playerCashText.text = $"Cash: ${cash}";
        }

        public void SetBetUI(int bet)
        {
            playerBetText.text = $"Bet: ${bet}";
        }

        private void HandleCashChanged(ICashHolder cashHolder)
        {
            if (cashHolder is IPlayer player && player != IPlayer.UserPlayer)
            {
                return;
            }

            SetCashUI(cashHolder.Cash);
        }

        private void HandleBetChanged(IBetter better)
        {
            if (better is IPlayer player && player != IPlayer.UserPlayer)
            {
                return;
            }

            SetBetUI(better.CurrentBet);
        }
    }
}
