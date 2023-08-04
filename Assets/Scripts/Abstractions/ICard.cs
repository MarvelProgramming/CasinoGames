using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public interface ICard
    {
        string Name { get; }
        int Value { get; }
        FacingDirection Facing { get; set; }
        Sprite FrontImage { get; }
        Sprite BackImage { get; }
    }
}
