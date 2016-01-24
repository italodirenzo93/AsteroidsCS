using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    public static class Player
    {
        private static Sprite shipSprite;
        private static float playerSpeed = 180.0f;

        private static ShotManager shotManager;
        private static float shotSpeed = 460.0f;
        private static float shotTimer = 0.0f;
        private static float minShotTimer = 0.2f;

        public static void Initialize(
            Texture2D shipTexture,
            Texture2D shotTexture,
            Vector2 position,
            Rectangle initFrame,
            Vector2 velocity)
        {
            shipSprite = new Sprite(shipTexture, position, initFrame, velocity, Color.White);
            shipSprite.AddFrame(new Rectangle(45, 0, 45, 40));
            shotManager = new ShotManager(shotTexture, shotSpeed, 1, 10);
        }

        public static void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            GamePadState padState = GamePad.GetState(PlayerIndex.One);

            switch (AsteroidsGame.ControlStyle)
            {
                case ControlStyles.KBM_RH:
                    {
                        shipSprite.Velocity = Vector2.Zero;

                        // Movement
                        if (keyState.IsKeyDown(Keys.W))
                            shipSprite.Velocity += new Vector2(0, -1);
                        if (keyState.IsKeyDown(Keys.S))
                            shipSprite.Velocity += new Vector2(0, 1);
                        if (keyState.IsKeyDown(Keys.A))
                            shipSprite.Velocity += new Vector2(-1, 0);
                        if (keyState.IsKeyDown(Keys.D))
                            shipSprite.Velocity += new Vector2(1, 0);

                        shipSprite.Velocity.Normalize();
                        shipSprite.Velocity *= playerSpeed;

                        fireShotsMouse(mouseState, elapsed);

                        pointToMouseCursor(mouseState);
                    }
                    break;

                case ControlStyles.KBM_LH:
                    {
                        shipSprite.Velocity = Vector2.Zero;

                        // Movement
                        if (keyState.IsKeyDown(Keys.O))
                            shipSprite.Velocity += new Vector2(0, -1);
                        if (keyState.IsKeyDown(Keys.L))
                            shipSprite.Velocity += new Vector2(0, 1);
                        if (keyState.IsKeyDown(Keys.K))
                            shipSprite.Velocity += new Vector2(-1, 0);
                        if (keyState.IsKeyDown(Keys.OemSemicolon))
                            shipSprite.Velocity += new Vector2(1, 0);

                        shipSprite.Velocity.Normalize();
                        shipSprite.Velocity *= playerSpeed;

                        fireShotsMouse(mouseState, elapsed);

                        pointToMouseCursor(mouseState);
                    }
                    break;

                case ControlStyles.GamePad:
                    {
                        shipSprite.Velocity = Vector2.Zero;

                        shipSprite.Velocity = new Vector2(
                            padState.ThumbSticks.Left.X,
                            -padState.ThumbSticks.Left.Y);

                        shipSprite.Velocity.Normalize();
                        shipSprite.Velocity *= playerSpeed;

                        fireShotsGamePad(padState, elapsed);

                        shipSprite.RotateTo(new Vector2(
                            padState.ThumbSticks.Right.X,
                            padState.ThumbSticks.Right.Y) - shipSprite.Position);
                    }
                    break;
            }

            imposeMovementLimits();
            shipSprite.Update(gameTime);
            shotManager.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            shotManager.Draw(spriteBatch);
            shipSprite.Draw(spriteBatch);
        }

        public static void Kill()
        {
            shipSprite.Expired = true;
        }

        // helpers
        private static void pointToMouseCursor(MouseState mouseState)
        {
            shipSprite.RotateTo(new Vector2(mouseState.X, mouseState.Y) - shipSprite.Position);
            shipSprite.Rotation += (MathHelper.Pi * 0.5f); // to point the nose of the ship toward the cursor
        }

        private static void imposeMovementLimits()
        {
            if (shipSprite.Position.X <= 0)
                shipSprite.Position = new Vector2(0, shipSprite.Position.Y);
            if (shipSprite.Position.X >= (AsteroidsGame.WINDOW_WIDTH - shipSprite.FrameWidth))
                shipSprite.Position = new Vector2((AsteroidsGame.WINDOW_WIDTH - shipSprite.FrameWidth), shipSprite.Position.Y);
            if (shipSprite.Position.Y <= 0)
                shipSprite.Position = new Vector2(shipSprite.Position.X, 0);
            if (shipSprite.Position.Y >= (AsteroidsGame.WINDOW_HEIGHT - shipSprite.FrameHeight))
                shipSprite.Position = new Vector2(shipSprite.Position.X, (AsteroidsGame.WINDOW_HEIGHT - shipSprite.FrameHeight));
        }

        private static void fireShotsMouse(MouseState mouseState, float elapsed)
        {
            ButtonState button = (AsteroidsGame.ControlStyle == ControlStyles.KBM_LH) ? mouseState.RightButton
                : mouseState.LeftButton;

            if (button == ButtonState.Pressed)
            {
                if (shotTimer >= minShotTimer)
                {
                    Vector2 angle = mouseState.Position.ToVector2() - shipSprite.Position;

                    if (angle != Vector2.Zero)
                        angle.Normalize();

                    shotManager.FireShot(shipSprite.Center, angle, shipSprite.Rotation);
                    SoundEffectManager.GetSound("shipLaser").Play(0.5f, 0f, 0f);
                    shotTimer = 0.0f;
                }
            }
            shotTimer += elapsed;
        }

        private static void fireShotsGamePad(GamePadState padState, float elapsed)
        {
            if (padState.ThumbSticks.Right != Vector2.Zero)
            {
                if (shotTimer >= minShotTimer)
                {
                    Vector2 angle = padState.ThumbSticks.Right - shipSprite.Position;

                    if (angle != Vector2.Zero)
                        angle.Normalize();

                    shotManager.FireShot(shipSprite.Center, angle, shipSprite.Rotation);
                    SoundEffectManager.GetSound("shipLaser").Play(0.5f, 0f, 0f);
                    shotTimer = 0f;
                }
            }
            shotTimer += elapsed;
        }
    }
}
