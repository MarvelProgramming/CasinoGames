using CasinoGames.Abstractions;
using TMPro;
using UnityEngine;

namespace CasinoGames.Core
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject areaWrapper;

        [SerializeField]
        private TMP_Text conditionText;

        #region Unity

        private void Awake()
        {
            IPlayer.OnPlayerWin += HandlePlayerGameOverCondition;
            IPlayer.OnPlayerLose += HandlePlayerGameOverCondition;
            IPlayer.OnPlayerDraw += HandlePlayerGameOverCondition;
        }

        private void OnDestroy()
        {
            IPlayer.OnPlayerWin -= HandlePlayerGameOverCondition;
            IPlayer.OnPlayerLose -= HandlePlayerGameOverCondition;
            IPlayer.OnPlayerDraw -= HandlePlayerGameOverCondition;
        }

        #endregion

        public void SetConditionUI(string condition)
        {
            conditionText.text = condition;
        }

        private void HandlePlayerGameOverCondition(IPlayer player, string reason)
        {
            if (player == IPlayer.UserPlayer)
            {
                areaWrapper.SetActive(true);
                SetConditionUI(reason);
            }
        }
    }
}
