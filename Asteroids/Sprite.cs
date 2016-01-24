using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class Sprite
    {
        public Texture2D Texture;

        private Vector2 position = Vector2.Zero;
        private Vector2 velocity = Vector2.Zero;
        private float rotation = 0.0f;

        private List<Rectangle> frames = new List<Rectangle>();
        private int currentFrame;
        private float frameTime = 0.1f;
        private float timeForCurrentFrame = 0.0f;

        public bool Expired = false;
        public bool Animate = true;
        public bool AnimateWhenStopped = true;

        public bool Collidable = true;
        public int CollisionRadius = 0;
        public int BoundingXPadding = 0;
        public int BoundingYPadding = 0;

        public Color TintColor { get; set; }

        public Sprite(
            Texture2D texture,
            Vector2 position,
            Rectangle initFrame,
            Vector2 velocity,
            Color tintColor)
        {
            this.Texture = texture;
            this.position = position;
            this.velocity = velocity;
            this.TintColor = tintColor;
            frames.Add(initFrame);
        }

        public int FrameWidth
        {
            get { return frames[0].Height; }
        }

        public int FrameHeight
        {
            get { return frames[0].Height; }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
            set
            {
                currentFrame = (int)MathHelper.Clamp(value, 0, frames.Count - 1);
            }
        }

        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }

        public Rectangle SourceRect
        {
            get { return frames[currentFrame]; }
        }

        public void AddFrame(Rectangle frameRect)
        {
            frames.Add(frameRect);
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Center
        {
            get
            {
                return Position + new Vector2(FrameWidth / 2, FrameHeight / 2);
            }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }

        public void RotateTo(Vector2 direction)
        {
            direction.Normalize();
            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        public Rectangle BoundingBoxRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + BoundingXPadding,
                    (int)position.Y + BoundingYPadding,
                    FrameWidth - (BoundingXPadding * 2),
                    FrameHeight - (BoundingYPadding * 2));
            }
        }

        public bool IsBoxColliding(Rectangle other)
        {
            return BoundingBoxRect.Intersects(other);
        }

        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
        {
            if (Vector2.Distance(Center, otherCenter) < (CollisionRadius + otherRadius))
                return true;
            else
                return false;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Animate)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                timeForCurrentFrame += elapsed;

                if (timeForCurrentFrame >= FrameTime)
                {
                    currentFrame = (currentFrame + 1) % frames.Count;
                    timeForCurrentFrame = 0.0f;
                }
                position += (velocity * elapsed);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Expired)
            {
                spriteBatch.Draw(
                    Texture,
                    Center,
                    SourceRect,
                    TintColor,
                    rotation,
                    new Vector2(FrameWidth / 2, FrameHeight / 2),
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
            }
        }
    }
}
