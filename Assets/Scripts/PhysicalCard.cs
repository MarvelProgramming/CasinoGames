using CasinoGames.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class PhysicalCard : MonoBehaviour
    {
        private Sprite frontImage;
        private Sprite backImage;

        public void Setup(Sprite frontImage, Sprite backImage)
        {
            this.frontImage = frontImage;
            this.backImage = backImage;
        }

        public void SetFacingDirection(FacingDirection facingDirection)
        {
            throw new NotImplementedException();
        }
    }
}
