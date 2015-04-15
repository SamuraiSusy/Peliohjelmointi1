using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace platformer
{
    public class Player : PhysicsObject
    {
        private const float movementAcceleration = 20000.0f;
        private const float maxMovementSpeed = 8000f;
        private const float frictionFactor = 0.6f;
        private const float jumpAcceleration = 78000f;
        private KeyboardState prevKeyboardState, curKeyboardState;
        private float direction;
        private Texture2D debugTexture;

        public Player(Texture2D texture, Texture2D debugTexture)
            : base(texture, new Rectangle(10, 0, 80, 120))
        {
            origin = new Vector2(0.5f * texture.Width, texture.Height);
            this.debugTexture = debugTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
            //spriteBatch.Draw(debugTexture, BoundingRectangle, Color.White * 0.5f);
        }

        public void Update(float elapsedTime)
        {
            prevKeyboardState = curKeyboardState;
            curKeyboardState = Keyboard.GetState();
            Vector2 velocity = Velocity;
            velocity.X += GetMovementDirection() * movementAcceleration * elapsedTime;
            velocity.X *= frictionFactor;
            velocity.X = MathHelper.Clamp(velocity.X, -maxMovementSpeed, maxMovementSpeed);

            if(ShouldJump())
            {
                velocity.Y -= jumpAcceleration * elapsedTime;
                IsGrounded = false;
            }

            Velocity = velocity;
        }

        private float GetMovementDirection()
        {
            direction = 0;

            curKeyboardState = Keyboard.GetState();

            if (curKeyboardState.IsKeyDown(Keys.Left))
            {
                direction -= 1;
            }
            if(curKeyboardState.IsKeyDown(Keys.Right))
            {
                direction += 1;
            }

            if(curKeyboardState.IsKeyDown(Keys.Left) &&
                curKeyboardState.IsKeyDown(Keys.Right))
            {
                direction = 0;
            }

            return direction;
        }

        private bool ShouldJump()
        {
            bool isJumpKeyPressed = curKeyboardState.IsKeyDown(Keys.Space);
            bool wasJumpKeyPressed = prevKeyboardState.IsKeyDown(Keys.Space);

            return (isJumpKeyPressed && !wasJumpKeyPressed && IsGrounded);

        }
    }
}
