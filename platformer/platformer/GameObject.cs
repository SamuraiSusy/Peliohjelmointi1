using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace platformer
{
    public class GameObject
    {
        protected Vector2 origin;
        protected Texture2D texture;
        
        private Vector2 position;
        private Rectangle localBoundingRectangle;

        public Rectangle BoundingRectangle
        {
            get
            {
                int x = (int)Math.Round(position.X - origin.X) + localBoundingRectangle.X;
                int y = (int)Math.Round(position.Y - origin.Y) + localBoundingRectangle.Y;

                return new Rectangle(x, y, localBoundingRectangle.Width, localBoundingRectangle.Height);
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public GameObject(Texture2D texture, Rectangle localBoundingRectangle)
        {
            this.texture = texture;
            this.localBoundingRectangle = localBoundingRectangle;
        }
    }
}
