using UnityEngine;
using UnityEngine.SceneManagement;

namespace CasinoGames.Core
{
    public class GameController : MonoBehaviour
    {
        public void LoadSceneByName(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
