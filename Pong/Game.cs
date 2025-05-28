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

        Texture2D ballTexture;
        Vector2 ballPosition;
        Vector2 ballSpeedVector;
        float ballSpeed;
        double remainderX;
        double remainderY;

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
            ballSpeed = 100f;

            //TODO: инициализирайте вектора на скоростта ballSpeedVector, за да зададете началната посока на движение
             Random random = new Random();
            float angle = (float)(random.NextDouble() * MathHelper.TwoPi);
            ballSpeedVector  = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * ballSpeed;

            


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("ball");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float updatedBallSpeed = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;


            ballPosition.X += ballSpeedVector.X * updatedBallSpeed / ballSpeed;
            ballPosition.Y += ballSpeedVector.Y * updatedBallSpeed / ballSpeed;

            //TODO: Изменете ballPosition.X и ballPosition.Y в зависимост от посоката на движение

            //TODO: Натрупайте остатъците получени от закръглянето в променливите remainderX и remainderY


            //TODO: Ако картинката напуска границите на екрана, това означава, че топката се е "ударила" в края на екрана и трябва да
            //промени посоката си на движение. За целта трябва да промените вектора ballSpeedVector.

             
            if (ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
            {
                
                ballSpeedVector.X = ballSpeedVector.X * -1;
            }
            else if (ballPosition.X < ballTexture.Width / 2)
            {
                ballSpeedVector.X = ballSpeedVector.X * -1;
                
            }

            if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
            {

                ballSpeedVector.Y = -ballSpeedVector.Y;
            }
            else if (ballPosition.Y < ballTexture.Height / 2)
            {
                ballSpeedVector.Y = ballSpeedVector.Y * -1;

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
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
