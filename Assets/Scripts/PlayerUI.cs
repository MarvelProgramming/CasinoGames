using CasinoGames.Abstractions;
using TMPro;
using UnityEngine;

namespace CasinoGames.Core
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeReference]
        protected Player player;

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private TMP_Text nicknameText;

        [SerializeField]
        private TMP_Text stateText;

        #region Unity

        protected virtual void Awake()
        {
            IPlayer.OnPlayerWin += HandlePlayerWin;
            IPlayer.OnPlayerLose += HandlePlayerLose;
            IPlayer.OnPlayerDraw += HandlePlayerDraw;
            IPlayer.OnActivePlayerChanged += HandleActivePlayerChanged;
            IGameManager.OnRestart += HandleGameRestart;
        }

        private void Start()
        {
            nicknameText.text = player.Nickname;
        }

        protected virtual void OnDestroy()
        {
            IPlayer.OnPlayerWin -= HandlePlayerWin;
            IPlayer.OnPlayerLose -= HandlePlayerLose;
            IPlayer.OnPlayerDraw -= HandlePlayerDraw;
            IGameManager.OnRestart -= HandleGameRestart;
            IPlayer.OnActivePlayerChanged -= HandleActivePlayerChanged;
        }

        #endregion

        private void HandlePlayerWin(IPlayer targetPlayer, string winReason = "Win")
        {
            if (targetPlayer != (IPlayer)player)
            {
                return;
            }

            SetState(winReason);
        }

        private void HandlePlayerLose(IPlayer targetPlayer, string loseReason = "Lose")
        {
            if (targetPlayer != (IPlayer)player)
            {
                return;
            }

            SetState(loseReason);
        }

        private void HandlePlayerDraw(IPlayer targetPlayer, string drawReason = "Draw")
        {
            if (targetPlayer != (IPlayer)player)
            {
                return;
            }

            SetState(drawReason);
        }

        private void HandleGameRestart()
        {
            canvasGroup.alpha = 1;
            stateText.gameObject.SetActive(false);
        }

        private void SetState(string newState)
        {
            stateText.text = newState;
            stateText.gameObject.SetActive(true);
            canvasGroup.alpha = 0.05f;
        }

        protected virtual void HandleActivePlayerChanged(IPlayer targetPlayer)
        {
            nicknameText.text = (IPlayer)player == IPlayer.ActivePlayer ? $"<color=\"yellow\">{player.Nickname}" : player.Nickname;
        }
    }
}
