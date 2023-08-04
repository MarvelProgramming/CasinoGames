using CasinoGames.Abstractions;
using UnityEngine;

namespace CasinoGames.Core
{
    public class PlayerActionUI : MonoBehaviour
    {
        [SerializeField]
        protected GameObject actionArea;

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

        protected virtual void HandlePlayerStateChanged(IPlayer player)
        {
            if (player != IPlayer.UserPlayer && player != IPlayer.ActivePlayer)
            {
                return;
            }

            actionArea.SetActive(player.CurrentState == PlayerState.TakingAction);
        }
    }
}
