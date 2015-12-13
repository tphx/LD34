using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Tphx.Bicephalisnake.Gameplay
{
    class BicephalisnakeGameplay : GameComponent
    {
        private enum GameplayState
        {
            Uninitialized,
            NewGame,
            ReadyScreen,
            Countdown,
            Playing,
            GameOver
        };

        private GameplayState gameplayState = GameplayState.Uninitialized;
        private Snake snake;
        private double timeSinceLastInput;
        private Texture2D hudTexture;
        private Texture2D levelTexture;
        private Texture2D screenCover;
        private SpriteFont sousesFont;
        private int score;
        private double countDownTime = 4.0;
        private double countdownRemaining;
        private Dictionary<Vector2, DirectionalArrow> directionalArrows = new Dictionary<Vector2, DirectionalArrow>()
        {
            { new Vector2(0.0f, -1.0f), new DirectionalArrow(0.0f, Color.Yellow) }, // Up
            { new Vector2(0.0f, 1.0f), new DirectionalArrow(180.0f, Color.Green) }, // Down
            { new Vector2(-1.0f, 0.0f), new DirectionalArrow(270.0f, Color.Blue) }, // Left
            { new Vector2(1.0f, 0.0f), new DirectionalArrow(90.0f, Color.Red) }, // Right
        };

        public BicephalisnakeGameplay(ContentManager content)
            : base(content)
        {
            this.hudTexture = this.content.Load<Texture2D>("Textures\\HUD");
            this.levelTexture = this.content.Load<Texture2D>("Textures\\Level1");
            this.screenCover = this.content.Load<Texture2D>("Textures\\ScreenCover");
            this.sousesFont = this.content.Load<SpriteFont>("Fonts\\Souses");
        
            NewGame();
        }

        public static Vector2 BoardDimensions { get; private set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Level
            spriteBatch.Draw(this.levelTexture, Vector2.Zero, null, Color.White);

            // HUD
            spriteBatch.Draw(this.hudTexture, new Vector2(0.0f, levelTexture.Height), new Rectangle(0, 0, 560, 100), Color.White);
            spriteBatch.Draw(this.hudTexture, new Vector2(40.0f + 31.5f, 580.0f + 31.5f), new Rectangle(561, 1, 61, 61), 
                this.directionalArrows[this.snake.NextHorizontalTurn].Color, 
                MathHelper.ToRadians(this.directionalArrows[this.snake.NextHorizontalTurn].Rotation), new Vector2(31.5f, 31.5f), 
                Vector2.One, SpriteEffects.None, 0.6f);
            spriteBatch.Draw(this.hudTexture, new Vector2(459.0f + 31.5f, 580.0f + 31.5f), new Rectangle(561, 1, 61, 61),
                this.directionalArrows[this.snake.NextVerticalTurn].Color, 
                MathHelper.ToRadians(this.directionalArrows[this.snake.NextVerticalTurn].Rotation), new Vector2(31.5f, 31.5f), 
                Vector2.One, SpriteEffects.None, 0.6f);
            spriteBatch.DrawString(this.sousesFont, string.Format("Score: {0:00000000}", this.score), 
                new Vector2(197.0f, 600.0f), Color.Black);

            this.snake.Draw(spriteBatch);

            if(this.gameplayState == GameplayState.Countdown)
            {
                DrawCountdown(spriteBatch);
            }
            else if(this.gameplayState == GameplayState.GameOver)
            {
                DrawGameOver(spriteBatch);
            }
        }

        private void DrawCountdown(SpriteBatch spriteBatch)
        {
            string message;
            double countdownFloor = Math.Floor(this.countdownRemaining);

            if (countdownFloor < 1)
            {
                message = "Go!";
            }
            else
            {
                message = countdownFloor.ToString("0");
            }

            Vector2 messageDimensions = sousesFont.MeasureString(message);

            spriteBatch.Draw(screenCover, Vector2.Zero, Color.White);
            spriteBatch.DrawString(sousesFont, message, new Vector2((280.0f - (messageDimensions.X / 2)),
                (280.0f - (messageDimensions.X / 2))), Color.Red);
        }

        private void DrawGameOver(SpriteBatch spriteBatch)
        {
            string gameOverMessage = "Game Over!";
            string restartMessage = "Press Left or Down to restart.";
            string scoreMessage = "Final Score:";
            string scoreCountMessage = this.score.ToString();
            Vector2 gameOverMessageDimensions = sousesFont.MeasureString(gameOverMessage);
            Vector2 restartMessageDimensions = sousesFont.MeasureString(restartMessage);
            Vector2 scoreMessageDimensions = sousesFont.MeasureString(scoreMessage);
            Vector2 scoreCountMessageDimensions = sousesFont.MeasureString(scoreCountMessage);

            spriteBatch.Draw(screenCover, Vector2.Zero, Color.White);
            spriteBatch.DrawString(sousesFont, gameOverMessage, new Vector2((280.0f - (gameOverMessageDimensions.X / 2)),
                200.0f), Color.Red);
            spriteBatch.DrawString(sousesFont, restartMessage, new Vector2((280.0f - (restartMessageDimensions.X / 2)),
                250.0f), Color.Red);
            spriteBatch.DrawString(sousesFont, scoreMessage, new Vector2((280.0f - (scoreMessageDimensions.X / 2)),
                300.0f), Color.Red);
            spriteBatch.DrawString(sousesFont, scoreCountMessage, new Vector2((280.0f - (scoreCountMessageDimensions.X / 2)),
                350.0f), Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
            this.timeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;

            switch (this.gameplayState)
            {
                case GameplayState.NewGame:
                    NewGame();
                    break;
                case GameplayState.ReadyScreen:
                    break;
                case GameplayState.Countdown:
                    UpdateCountdown(gameTime);
                    break;
                case GameplayState.Playing:
                    UpdateGameplay(gameTime);
                    break;
                case GameplayState.GameOver:
                    UpdateGameOver(gameTime);
                    break;
            }
        }

        private void UpdateGameplay(GameTime gameTime)
        {
            if (this.timeSinceLastInput >= 0.2)
            {
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Left) && this.snake.MovingVertically)
                {
                    this.snake.TurnHorizontally();
                    this.timeSinceLastInput = 0.0f;
                }
                else if (keyboardState.IsKeyDown(Keys.Down) && !this.snake.MovingVertically)
                {
                    this.snake.TurnVertically();
                    this.timeSinceLastInput = 0.0f;
                }
            }

            this.snake.Update(gameTime);

            CheckCollisions();
        }

        private void NewGame()
        {
            BicephalisnakeGameplay.BoardDimensions = new Vector2(35.0f, 35.0f);
            Vector2 snakePosition = new Vector2((BicephalisnakeGameplay.BoardDimensions.X / 2),
                (BicephalisnakeGameplay.BoardDimensions.X / 2));
            snakePosition.X = (float)Math.Floor(snakePosition.X);
            snakePosition.Y = (float)Math.Floor(snakePosition.Y);

            this.snake = new Snake(this.content, snakePosition, 1.0);
            this.score = 0;
            this.countdownRemaining = this.countDownTime;
            this.gameplayState = GameplayState.Countdown;
        }

        private void UpdateCountdown(GameTime gameTime)
        {
            this.countdownRemaining -= gameTime.ElapsedGameTime.TotalSeconds;

            if(this.countdownRemaining <= 0.0f)
            {
                this.gameplayState = GameplayState.Playing;
            }
        }

        private void UpdateGameOver(GameTime gameTime)
        {
            if (this.timeSinceLastInput >= 1.0)
            {
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    this.timeSinceLastInput = 0.0;
                    NewGame();
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    this.timeSinceLastInput = 0.0;
                    NewGame();
                }
            }
        }

        private void CheckCollisions()
        {
            //Snake body.
            foreach(SnakePiece bodyPiece in this.snake.BodyPieces)
            {
                if(this.snake.Head.Position == bodyPiece.Position)
                {
                    this.gameplayState = GameplayState.GameOver;
                }
            }

            if (this.snake.Head.Position == this.snake.Tail.Position)
            {
                this.gameplayState = GameplayState.GameOver;
            }
        }
    }
}
