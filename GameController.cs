using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace OrbtNN
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameController : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D pixel;
        GameManager manager;
        SpriteFont font;

        int width, height;
        public GameController()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = width = 1280;
            graphics.PreferredBackBufferHeight = height = 720;
            width /= 2;
            height /= 2;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            SpriteFactory.Content = Content;
            font = Content.Load<SpriteFont>("Font/PixelFont");
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            manager = new GameManager(this, graphics.PreferredBackBufferWidth - 200, graphics.PreferredBackBufferHeight);
            // TODO: use this.Content to load your game content here
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            manager.Update(gameTime);
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Space)) manager.Press();
            else manager.Release();
            manager.Debug(!keyboard.NumLock);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(23, 2, 45));
            spriteBatch.Begin();
            manager.Draw();
            // Test(gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public void DrawString(Vector2 position, string text, Color color, Align align = Align.CORNER)
        {
            if (align == Align.CENTER)
            {
                position = Vector2.Add(position, -Vector2.Multiply(font.MeasureString(text), 0.5f));
            }
            spriteBatch.DrawString(font, text, position, color);
        }
        public void DrawLine(Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle rect = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 vector = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(vector, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            spriteBatch.Draw(pixel, rect, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
        public void DrawVector(Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            DrawLine(begin, end, color, width);
            double angle = Math.Atan2(end.Y - begin.Y, end.X - begin.X);
            double delta = 160 * Math.PI / 180;
            Vector2 extra = new Vector2(10 * (float)Math.Cos(angle), 10 * (float)Math.Sin(angle ));
            DrawLine(end, Vector2.Add(end, Rotate(extra, delta)), color, width);
            DrawLine(end, Vector2.Add(end, Rotate(extra, -delta)), color, width);
        }
        public void DrawTexture(Texture2D texture, Vector2 position, float opacity = 1f, Align align = Align.CENTER)
        {
            if (align == Align.CENTER)
            {
                position = Vector2.Add(position, new Vector2(-texture.Width / 2, -texture.Height / 2));
            }
            spriteBatch.Draw(texture, position, Color.White * opacity);
        }
        public void DrawSprite(Sprite sprite, Vector2 position, float opacity = 1f, Align align = Align.CENTER)
        {
            Texture2D texture = sprite.Current;
            DrawTexture(texture, position, opacity, align);
        }
        public static Vector2 Rotate(Vector2 vector, double angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new Vector2(cos * vector.X - sin * vector.Y, sin * vector.X + cos * vector.Y);
        }
    }
}
