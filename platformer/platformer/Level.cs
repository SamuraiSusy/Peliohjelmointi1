using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace platformer
{
    public class Level
    {
        private const float viewMargin = 0.3f;

        private Tile[,] tiles;
        private List<Collectible> collectibles;
        private ContentManager contentManager;
        private Texture2D tileTexture;
        private Texture2D playerTexture;
        private Texture2D debugTexture;
        private Player player;
        private PhysicsManager physicsManager;
        private float viewPosition; // kameran liikuttaminen

        public Level(Tile[,] tiles, List<Collectible> collectibles, ContentManager contentManager)
        {
            this.tiles = tiles;
            this.collectibles = collectibles;
            this.contentManager = contentManager;
            tileTexture = contentManager.Load<Texture2D>("textures/tile");
            playerTexture = contentManager.Load<Texture2D>("textures/player");
            debugTexture = contentManager.Load<Texture2D>("textures/debugTexture");
            physicsManager = new PhysicsManager(this);
            player = new Player(playerTexture, debugTexture);
            player.Position = new Vector2(300.0f, 200.0f);
            physicsManager.AddPhysicsObject(player);
            viewPosition = 0f;
        }

        public bool IsTilePassable(int x, int y)
        {
            if (x < 0 || x >= tiles.GetLength(1) || y >= tiles.GetLength(0))
            {
                return false;
            }
            else if (y < 0)
                return true;
            else
                return tiles[y, x].IsPassable;
        }

        public void Update(float elapsedTime)
        {
            player.Update(elapsedTime);
            physicsManager.Update(elapsedTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CalculateViewPosition(spriteBatch.GraphicsDevice.Viewport);
            Matrix viewTransform = Matrix.CreateTranslation(-viewPosition, 0f, 0f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, viewTransform); // oletusarvot + matriisi viimisenä
            DrawTiles(spriteBatch);
            DrawCollectibles(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();
        }

        // kamera
        private void CalculateViewPosition(Viewport viewPort)
        {
            float marginWidth = viewMargin * viewPort.Width;
            float leftMargin = viewPosition + marginWidth;
            float rightMargin = viewPosition + viewPort.Width - marginWidth;
            float viewMovement = 0f;

            if (player.Position.X < leftMargin)
                viewMovement = player.Position.X - leftMargin;
            else if (player.Position.X > rightMargin)
                viewMovement = player.Position.X - rightMargin;

            float maxViewPosition = tiles.GetLength(1) * Tile.Size - viewPort.Width;
            viewPosition = MathHelper.Clamp(viewPosition + viewMovement, 0f, maxViewPosition);
        }

        private void DrawTiles(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle(0, 0, Tile.Size, Tile.Size);
            int leftBound = (int)Math.Floor(viewPosition / Tile.Size);
            int rightBound = leftBound + (int)Math.Ceiling((float)spriteBatch.GraphicsDevice.Viewport.Width / Tile.Size);
            rightBound = Math.Min(rightBound, tiles.GetLength(1) - 1);

            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = leftBound; x <= rightBound; x++)
                {
                    if (!tiles[y, x].IsPassable)
                    {
                        destinationRectangle.X = x * Tile.Size;
                        destinationRectangle.Y = y * Tile.Size;
                        spriteBatch.Draw(tileTexture, destinationRectangle, Color.White);
                    }
                }
            }
        }

        private void DrawCollectibles(SpriteBatch spriteBatch)
        {
            foreach (Collectible collectible in collectibles)
                collectible.Draw(spriteBatch);
        }
    }
}
