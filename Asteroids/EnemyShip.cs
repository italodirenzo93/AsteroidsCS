using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class EnemyShip : Sprite
    {
        private float movementSpeed = 100f;
        private Queue<Vector2> waypoints = new Queue<Vector2>();
        private Vector2 currentWaypoint;
        private Random rand = new Random();

        public EnemyShip(Texture2D texture, Vector2 position, Rectangle initFrame)
            : base(texture, position, initFrame, Vector2.Zero, Color.White)
        {
            randomPath();
            currentWaypoint = waypoints.Dequeue();
        }

        public override void Update(GameTime gameTime)
        {
            if (!waypointReached())
            {
                Vector2 angle = currentWaypoint - this.Center;
                //this.RotateTo(angle);

                if (angle != Vector2.Zero)
                    angle.Normalize();

                this.Velocity = angle * movementSpeed;
            }
            else
            {
                if (waypoints.Count > 0)
                    currentWaypoint = waypoints.Dequeue();
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var point in waypoints)
            {
                spriteBatch.DrawShadowedString(FontManager.ArDestine16, "X", point, Color.White);
            }

            base.Draw(spriteBatch);
        }

        private void randomPath()
        {
            float x, y;

            for (int i = 0; i < 10; ++i)
            {
                x = (float)rand.Next(0, AsteroidsGame.WINDOW_WIDTH);
                y = (float)rand.Next(0, AsteroidsGame.WINDOW_HEIGHT);
                waypoints.Enqueue(new Vector2(x, y));
            }
        }

        private bool waypointReached()
        {
            if (Vector2.Distance(this.Center, currentWaypoint) < (float)(this.SourceRect.Width / 2))
                return true;
            else
                return false;
        }
    }
}
