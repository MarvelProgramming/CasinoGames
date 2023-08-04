using CasinoGames.Abstractions;
using System;
using UnityEngine;

namespace CasinoGames.Core
{
    [Serializable]
    public class CasinoCard : ICard
    {
        [field: SerializeField]
        public int Value { get; private set; }

        [field: SerializeField]
        public FacingDirection Facing { get; set; }

        [field: SerializeField]
        public Sprite FrontImage { get; private set; }

        [field: SerializeField]
        public Sprite BackImage { get; private set; }

        [field: SerializeField]
        public CardSuit Suit { get; private set; }

        public CasinoCard(int value, FacingDirection facing, Sprite frontImage, Sprite backImage, CardSuit suit = CardSuit.Hearts)
        {
            Value = value;
            Facing = facing;
            FrontImage = frontImage;
            BackImage = backImage;
            Suit = suit;
        }
    }
}
