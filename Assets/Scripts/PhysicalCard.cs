using CasinoGames.Abstractions;
using UnityEngine;
using UnityEngine.UI;

namespace CasinoGames.Core
{
    public class PhysicalCard : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        private Material matCopy;

        private void Awake()
        {
            matCopy = Instantiate(image.material);
            image.material = matCopy;
        }

        public void Setup(Sprite frontSprite, Sprite backSprite, FacingDirection facingDirection)
        {
            image.material.SetTexture("_FrontTex", frontSprite.texture);
            image.material.SetTexture("_BackTex", backSprite.texture);
            SetFacingDirection(facingDirection);
        }

        public void SetFacingDirection(FacingDirection facingDirection)
        {
            image.transform.rotation = Quaternion.AngleAxis(facingDirection == FacingDirection.Front ? 0 : 180, Vector3.up);
        }
    }
}
