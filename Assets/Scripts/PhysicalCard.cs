using CasinoGames.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CasinoGames.Core
{
    public class PhysicalCard : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer frontImage;
        [SerializeField]
        private SpriteRenderer backImage;

        public void Setup(Sprite frontSprite, Sprite backSprite, FacingDirection facingDirection)
        {
            frontImage.sprite = frontSprite;
            backImage.sprite = backSprite;
            SetFacingDirection(facingDirection);
        }

        public void SetFacingDirection(FacingDirection facingDirection)
        {
            transform.forward = facingDirection == FacingDirection.Front ? Vector3.up : Vector3.down;
        }
    }
}
