using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class CasinoCard : ICard
    {
        public string Name { get; private set; }

        public int Value { get; private set; }

        public FacingDirection Facing { get; set; }

        public Sprite FrontImage { get; private set; }

        public Sprite BackImage { get; private set; }

        public CasinoCard(string name, int value, FacingDirection facing, Sprite frontImage, Sprite backImage)
        {
            Name = name;
            Value = value;
            Facing = facing;
            FrontImage = frontImage;
            BackImage = backImage;
        }
    }
}
