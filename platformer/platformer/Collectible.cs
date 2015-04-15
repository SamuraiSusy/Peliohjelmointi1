using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace platformer
{
    class Collectible : GameObject
    {
        public Collectible(Texture2D texture)
            : base(texture, new Rectangle(0, 0, texture.Width, texture.Height))
        {
            origin = new Vector2(0.5f * texture.Width, 0.5f * texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f,
                origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
