using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace platformer
{
    public class PhysicsObject : GameObject
    {
        private Vector2 velocity;
        private bool isGrounded;

        public bool IsGrounded
        {
            get { return isGrounded; }
            set { isGrounded = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public PhysicsObject(Texture2D texture, Rectangle localBoundingRectangle)
            : base(texture, localBoundingRectangle)
        {
            isGrounded = false;
        }
    }
}
