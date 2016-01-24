using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class AsteroidsGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static GameStates GameState;
        public static ControlStyles ControlStyle;

        public const int WINDOW_WIDTH = 800;
        public const int WINDOW_HEIGHT = 600;

        Texture2D background;
        Texture2D shipSpriteSheet;
        Texture2D shotSprite;

        // temp
        EnemyShip enemy;

        public AsteroidsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            this.graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            this.graphics.ApplyChanges();

            this.IsMouseVisible = true;

            GameState = GameStates.Playing;
            ControlStyle = ControlStyles.KBM_RH;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = this.Content.Load<Texture2D>("Backgrounds/space2");
            shipSpriteSheet = this.Content.Load<Texture2D>("Sprites/ship");
            shotSprite = this.Content.Load<Texture2D>("Sprites/lasers");

            FontManager.LoadFonts(this.Content);
            SoundEffectManager.LoadSounds(this.Content);
            MusicManager.LoadMusic(this.Content);

            Player.Initialize(
                shipSpriteSheet,
                shotSprite,
                new Vector2(
                    (this.Window.ClientBounds.Width / 2) - 22.5f,
                    (this.Window.ClientBounds.Height / 2) - 20f),
                new Rectangle(0, 0, 45, 40),
                Vector2.Zero);

            enemy = new EnemyShip(shipSpriteSheet, Vector2.Zero, new Rectangle(0, 0, 45, 40));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            switch (GameState)
            {
                case GameStates.TitleScreen:
                    {

                    }
                    break;

                case GameStates.Playing:
                    {
                        if (ControlStyle == ControlStyles.KBM_RH || ControlStyle == ControlStyles.KBM_LH)
                        {
                            if (keyState.IsKeyDown(Keys.Escape))
                                GameState = GameStates.Paused;
                        }
                        else if (ControlStyle == ControlStyles.GamePad)
                        {
                            if (gamepadState.IsButtonDown(Buttons.Start))
                                GameState = GameStates.Paused;
                        }

                        if (!this.IsActive) { GameState = GameStates.Paused; }

                        Player.Update(gameTime);
                        enemy.Update(gameTime);
                    }
                    break;

                case GameStates.Paused:
                    {
                        if (ControlStyle == ControlStyles.KBM_RH || ControlStyle == ControlStyles.KBM_LH)
                        {
                            if (keyState.IsKeyDown(Keys.Escape))
                                GameState = GameStates.Playing;
                        }
                        else if (ControlStyle == ControlStyles.GamePad)
                        {
                            if (gamepadState.IsButtonDown(Buttons.Start))
                                GameState = GameStates.Playing;
                        }
                    }
                    break;

                case GameStates.GameOver:
                    {

                    }
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

            if (GameState == GameStates.Playing || GameState == GameStates.Paused)
            {
                Player.Draw(spriteBatch);
                enemy.Draw(spriteBatch);
            }

            if (GameState == GameStates.Paused)
            {
                spriteBatch.DrawShadowedString(
                    FontManager.ArDestine22,
                    "PAUSED",
                    new Vector2(WINDOW_WIDTH / 2 - 50f, WINDOW_HEIGHT / 2 - 15f),
                    Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
