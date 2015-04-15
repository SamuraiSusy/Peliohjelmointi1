using System;

namespace platformer
{
    public class Tile
    {
        public const int Size = 80;

        private bool isPassable;

        public bool IsPassable
        {
            get { return isPassable; }
        }

        public Tile()
        {
            isPassable = true;
        }

        public Tile(bool isPassable)
        {
            this.isPassable = isPassable;
        }
    }
}
