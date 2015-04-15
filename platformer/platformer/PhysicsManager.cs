using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace platformer
{
    class PhysicsManager
    {
        private const float gravityAcceleration = 3000f;
        private const float maxVerticalSpeed = 5000f;

        private Level level;
        private List<PhysicsObject> physicsObjects;

        public PhysicsManager() { }

        public PhysicsManager(Level level)
        {
            this.level = level;
            physicsObjects = new List<PhysicsObject>();
        }

        public void AddPhysicsObject(PhysicsObject physicsObject)
        {
            physicsObjects.Add(physicsObject);
        }

        public void Update(float elapsedTime)
        { 
            foreach(PhysicsObject physicsObject in physicsObjects)
            {
                ApplyGravity(elapsedTime, physicsObject);
                physicsObject.Position += physicsObject.Velocity * elapsedTime;
                ResolveCollision(physicsObject);
            }
        }

        private void ApplyGravity(float elapsedTime, PhysicsObject physicsObject)
        {
            Vector2 velocity = physicsObject.Velocity;
            velocity.Y += gravityAcceleration * elapsedTime;
            velocity.Y = MathHelper.Clamp(velocity.Y, -maxVerticalSpeed, maxVerticalSpeed);
            physicsObject.Velocity = velocity;
        }

        private void ResolveCollision(PhysicsObject physicObject)
        {
            Rectangle boundingRectangle = physicObject.BoundingRectangle;
            int leftTile = (int)Math.Floor((float)boundingRectangle.Left / Tile.Size);
            int rightTile = (int)Math.Floor((float)boundingRectangle.Right / Tile.Size);
            int topTile = (int)Math.Floor((float)boundingRectangle.Top / Tile.Size);
            int bottomTile = (int)Math.Floor((float)boundingRectangle.Bottom / Tile.Size);

            for(int y = topTile; y <= bottomTile; y++)
            {
                for(int x = leftTile; x <= rightTile; x++)
                {
                    if(!level.IsTilePassable(x, y))
                    {
                        Rectangle tileBounds = new Rectangle(x * Tile.Size, y * Tile.Size, Tile.Size, Tile.Size);
                        Vector2 intersectionDepth = CalculateIntersectionDepth(boundingRectangle, tileBounds);

                        if(intersectionDepth != Vector2.Zero)
                        {
                            if(Math.Abs(intersectionDepth.Y) < Math.Abs(intersectionDepth.X))
                            {
                                AdjustPosition(physicObject, 0, intersectionDepth.Y);
                                physicObject.Velocity = new Vector2(physicObject.Velocity.X, 0);

                                if (intersectionDepth.Y < 0)
                                    physicObject.IsGrounded = true;
                            }
                            else if(intersectionDepth.X != 0)
                            {
                                AdjustPosition(physicObject, intersectionDepth.X, 0);
                                physicObject.Velocity = new Vector2(0, physicObject.Velocity.Y);
                            }

                            boundingRectangle = physicObject.BoundingRectangle;
                        }
                    }
                }
            }
        }

        private Vector2 CalculateIntersectionDepth(Rectangle boundingRectangleA, Rectangle boundingRectangleB)
        {
            float halfWidthA = 0.5f * boundingRectangleA.Width;
            float halfHeightA = 0.5f * boundingRectangleA.Height;
            float halfWidthB = 0.5f * boundingRectangleB.Width;
            float halfHeightB = 0.5f * boundingRectangleB.Height;

            Vector2 centerA = new Vector2(boundingRectangleA.Left + halfWidthA, boundingRectangleA.Top + halfHeightA);
            Vector2 centerB = new Vector2(boundingRectangleB.Left + halfWidthB, boundingRectangleB.Top + halfHeightB);

            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            Vector2 depth = Vector2.Zero;

            if(Math.Abs(distanceX) < minDistanceX ||
                Math.Abs(distanceY) < minDistanceY)
            {
                if (distanceX > 0)
                    depth.X = minDistanceX - distanceX;
                else
                    depth.X = -minDistanceX - distanceX;

                if (distanceY > 0)
                    depth.Y = minDistanceY - distanceY;
                else
                    depth.Y = -minDistanceY - distanceY;
            }

            return depth;
        }

        private void AdjustPosition(PhysicsObject physicsObject, float offsetX, float offsetY)
        {
            Vector2 position = physicsObject.Position;
            position.X = (float)Math.Round(position.X + offsetX);
            position.Y = (float)Math.Round(position.Y + offsetY);
            physicsObject.Position = position;
        }
    }
}