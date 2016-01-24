using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class ShotManager
    {
        private float shotSpeed;
        private List<Sprite> activeShots = new List<Sprite>();
        private int collisionRadius;

        private Texture2D shotTexture;
        private readonly Rectangle screenBounds = new Rectangle(0, 0, AsteroidsGame.WINDOW_WIDTH, AsteroidsGame.WINDOW_HEIGHT);

        public float ShotSpeed
        {
            get { return shotSpeed; }
            set { shotSpeed = value; }
        }

        public ShotManager(
            Texture2D texture,
            float shotSpeed,
            int collisionRadius,
            int numShots)
        {
            this.shotTexture = texture;
            this.shotSpeed = shotSpeed;
            this.collisionRadius = collisionRadius;
        }

        public void FireShot(Vector2 location, Vector2 velocity, float rotation)
        {
            // Find first available shot
            Sprite shot = new Sprite(
                shotTexture,
                location,
                new Rectangle(0, 0, 15, 13),
                velocity,
                Color.White);

            shot.Velocity *= shotSpeed;
            shot.Rotation = rotation;
            shot.CollisionRadius = collisionRadius;
            activeShots.Add(shot);
        }

        public void FireShot(Vector2 location, Vector2 velocity)
        {
            FireShot(location, velocity, 0f);
        }

        public void Update(GameTime gameTime)
        {
            for (int index = activeShots.Count - 1; index >= 0; --index)
            {
                activeShots[index].Update(gameTime);
                if (!screenBounds.Intersects(activeShots[index].BoundingBoxRect))
                {
                    activeShots.RemoveAt(index);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            activeShots.ForEach(shot => shot.Draw(spriteBatch));
        }
    }
}
