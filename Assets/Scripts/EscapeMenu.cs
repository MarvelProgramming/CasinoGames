using UnityEngine;

namespace CasinoGames.Core
{
    public class EscapeMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject wrapper;

        #region Unity

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Toggle();
            }
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
        }

        #endregion

        public void Toggle()
        {
            wrapper.SetActive(!wrapper.activeSelf);
            Time.timeScale = wrapper.activeSelf ? 0f : 1f;
        }
    }
}
