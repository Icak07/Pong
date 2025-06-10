using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        bool isGameOver;

        Texture2D ballTexture;
        Texture2D pixelTexture;
        Vector2 ballPosition;
        Vector2 ballSpeedVector;
        float ballSpeed;

        Vector2 pl1BatPosition;
        Vector2 pl2BatPosition;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                                       _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 400f;
            ballSpeedVector = new Vector2(1, -1);

            pl1BatPosition = new Vector2(30, _graphics.PreferredBackBufferHeight / 2 - 30);
            pl2BatPosition = new Vector2(_graphics.PreferredBackBufferWidth - 40, _graphics.PreferredBackBufferHeight / 2 - 30);

            isGameOver = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("ball");


            pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });
        }

        private void checkBallCollision()
        {
            Rectangle ballRect = new Rectangle(
                (int)(ballPosition.X - ballTexture.Width / 2),
                (int)(ballPosition.Y - ballTexture.Height / 2),
                ballTexture.Width, ballTexture.Height
            );

            Rectangle bat1Rect = new Rectangle((int)pl1BatPosition.X, (int)pl1BatPosition.Y, 10, 60);
            Rectangle bat2Rect = new Rectangle((int)pl2BatPosition.X, (int)pl2BatPosition.Y, 10, 60);


            if (ballPosition.X < 0 || ballPosition.X > _graphics.PreferredBackBufferWidth)
            {
                isGameOver = true;
            }


            if (ballPosition.Y < 0 || ballPosition.Y > _graphics.PreferredBackBufferHeight)
            {
                ballSpeedVector.Y *= -1;
            }


            if (ballRect.Intersects(bat1Rect) || ballRect.Intersects(bat2Rect))
            {
                ballSpeedVector.X *= -1;
            }
        }

        private void updateBallPosition(float updatedBallSpeed)
        {
            float ratio = this.ballSpeedVector.X / this.ballSpeedVector.Y;
            float deltaY = updatedBallSpeed / (float)Math.Sqrt(1 + ratio * ratio);
            float deltaX = Math.Abs(ratio * deltaY);

            ballPosition.X += (ballSpeedVector.X > 0) ? deltaX : -deltaX;
            ballPosition.Y += (ballSpeedVector.Y > 0) ? deltaY : -deltaY;
        }

        private void updateBatsPositions()
        {
            var kstate = Keyboard.GetState();
            float speed = 5f;


            if (kstate.IsKeyDown(Keys.W))
                pl1BatPosition.Y -= speed;
            if (kstate.IsKeyDown(Keys.S))
                pl1BatPosition.Y += speed;


            if (kstate.IsKeyDown(Keys.Up))
                pl2BatPosition.Y -= speed;
            if (kstate.IsKeyDown(Keys.Down))
                pl2BatPosition.Y += speed;


            pl1BatPosition.Y = Math.Clamp(pl1BatPosition.Y, 0, _graphics.PreferredBackBufferHeight - 60);
            pl2BatPosition.Y = Math.Clamp(pl2BatPosition.Y, 0, _graphics.PreferredBackBufferHeight - 60);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!isGameOver)
            {
                float updatedBallSpeed = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                checkBallCollision();
                updateBallPosition(updatedBallSpeed);
                updateBatsPositions();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();


            _spriteBatch.Draw(
                ballTexture,
                ballPosition,
                null,
                Color.White,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );


            _spriteBatch.Draw(pixelTexture, new Rectangle((int)pl1BatPosition.X, (int)pl1BatPosition.Y, 20, 90), Color.Black);
            _spriteBatch.Draw(pixelTexture, new Rectangle((int)pl2BatPosition.X, (int)pl2BatPosition.Y, 20, 90), Color.Blue);


            if (isGameOver)
            {

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
