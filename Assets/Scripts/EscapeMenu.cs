using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class EscapeMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject wrapper;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                wrapper.SetActive(!wrapper.activeSelf);
            }
        }
    }
}
