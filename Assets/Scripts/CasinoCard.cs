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
        public string Name => throw new NotImplementedException();

        public int Value => throw new NotImplementedException();

        public FacingDirection Facing { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Sprite FrontImage => throw new NotImplementedException();

        public Sprite BackImage => throw new NotImplementedException();
    }
}
