using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace CasinoGames.Core
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
