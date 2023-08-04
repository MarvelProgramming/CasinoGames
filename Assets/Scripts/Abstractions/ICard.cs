using System;
using UnityEngine;

namespace CasinoGames.Abstractions
{
    public interface ICard
    {
        public static Action<int, int, ICard> OnCardChanged;
        int Value { get; }
        FacingDirection Facing { get; set; }
        Sprite FrontImage { get; }
        Sprite BackImage { get; }
        CardSuit Suit { get; }
    }
}
