using TMPro;
using UnityEngine;

namespace CasinoGames.Core.Poker
{
    public class PokerPotUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text potText;

        public void UpdatePotText(int pot)
        {
            potText.text = $"Current Pot: ${pot}";
        }
    }
}
