namespace Battlestations
{
    using Gamelogic.Grids;
    using UnityEngine;
    using System.Collections;

    public class ShipCell : SpriteCell
    {
        public bool IsAccessible;

        public bool HasTopWall;
        public bool HasBottomWall;
        public bool HasLeftWall;
        public bool HasRightWall;
    }
}