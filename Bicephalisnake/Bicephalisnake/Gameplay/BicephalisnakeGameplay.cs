#region LICENSE
//Copyright(c) 2015 TPHX

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Tphx.Bicephalisnake.Gameplay
{
    class BicephalisnakeGameplay : GameComponent
    {
        private enum GameplayState
        {
            Uninitialized,
            Countdown,
            Playing,
            GameOver
        };

        private GameplayState gameplayState = GameplayState.Uninitialized;
        private Snake snake;
        private Texture2D hudTexture;
        private Texture2D levelTexture;
        private Texture2D screenCover;
        private SpriteFont sousesFont;
        private int score;
        private int scoreMulitiplier;
        private double timeSinceLastInput;
        private double countDownTime = 4.0;
        private double countdownRemaining;
        private FoodManager foodManager;
        private PowerupManager powerupManager;
        private Dictionary<string, SoundEffect> soundEffects;
        double lastCountdownFloor;
        private Dictionary<Vector2, DirectionalArrow> directionalArrows = new Dictionary<Vector2, DirectionalArrow>()
        {
            { new Vector2(0.0f, -1.0f), new DirectionalArrow(0.0f, Color.Yellow) }, // Up
            { new Vector2(0.0f, 1.0f), new DirectionalArrow(180.0f, Color.Red) }, // Down
            { new Vector2(-1.0f, 0.0f), new DirectionalArrow(270.0f, Color.Green) }, // Left
            { new Vector2(1.0f, 0.0f), new DirectionalArrow(90.0f, Color.Blue) }, // Right
        };

        public BicephalisnakeGameplay(ContentManager content)
            : base(content)
        {
            NewGame();
        }

        public static Vector2 BoardDimensions { get; private set; }

        public bool ReturnToMainMenu { get; private set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Level
            spriteBatch.Draw(this.levelTexture, Vector2.Zero, null, Color.White);

            // HUD
            spriteBatch.Draw(this.hudTexture, new Vector2(0.0f, this.levelTexture.Height), new Rectangle(0, 0, 560, 100), Color.White);
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

            this.foodManager.Draw(spriteBatch);
            this.powerupManager.Draw(spriteBatch);
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

        public override void Update(GameTime gameTime)
        {
            this.timeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;

            switch (this.gameplayState)
            {
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
            if (this.timeSinceLastInput >= this.snake.TimeBetweenMoves)
            {
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Left) && (this.snake.MovingVertically || 
                    this.snake.Head.MovementDirection == Vector2.Zero))
                {
                    this.snake.TurnHorizontally();
                    this.timeSinceLastInput = 0.0f;
                }
                else if (keyboardState.IsKeyDown(Keys.Down) && (!this.snake.MovingVertically ||
                    this.snake.Head.MovementDirection == Vector2.Zero))
                {
                    this.snake.TurnVertically();
                    this.timeSinceLastInput = 0.0f;
                }
            }

            this.snake.Update(gameTime);
            this.powerupManager.Update(gameTime);

            CheckCollisions();
        }

        private void NewGame()
        {
            BicephalisnakeGameplay.BoardDimensions = new Vector2(35.0f, 35.0f);
            Vector2 snakePosition = new Vector2((BicephalisnakeGameplay.BoardDimensions.X / 2),
                (BicephalisnakeGameplay.BoardDimensions.X / 2));
            snakePosition.X = (float)Math.Floor(snakePosition.X);
            snakePosition.Y = (float)Math.Floor(snakePosition.Y);

            this.content.Unload();
            LoadContent();
            this.snake = new Snake(this.content, snakePosition, 0.20);
            this.foodManager = new FoodManager(this.content);
            this.powerupManager = new PowerupManager(this.content);
            this.score = 0;
            this.scoreMulitiplier = 1;
            this.countdownRemaining = this.countDownTime;
            this.timeSinceLastInput = 0.0;
            this.gameplayState = GameplayState.Countdown;
        }

        private void UpdateCountdown(GameTime gameTime)
        {
            this.lastCountdownFloor = Math.Floor(this.countdownRemaining);
            this.countdownRemaining -= gameTime.ElapsedGameTime.TotalSeconds;
            double currentFloor = Math.Floor(this.countdownRemaining);

            if (this.countdownRemaining <= 0.0f)
            {
                this.gameplayState = GameplayState.Playing;
            }
            else if(this.lastCountdownFloor > currentFloor)
            {
                if (currentFloor > 0)
                {
                    this.soundEffects["Countdown"].Play();
                }
                else
                {
                    this.soundEffects["Start"].Play();
                }
            }
        }

        private void UpdateGameOver(GameTime gameTime)
        {
            if (this.timeSinceLastInput >= 2.0)
            {
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    NewGame();
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    this.ReturnToMainMenu = true;
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
                    this.timeSinceLastInput = 0.0;
                    this.soundEffects["GameOver"].Play();
                }
            }

            // Snake tail.
            if (this.snake.Head.Position == this.snake.Tail.Position)
            {
                this.gameplayState = GameplayState.GameOver;
                this.timeSinceLastInput = 0.0;
                this.soundEffects["GameOver"].Play();
            }

            // Food.
            if(this.snake.Head.Position == this.foodManager.FoodPosition)
            {
                this.foodManager.EatFood();
                this.snake.SpawnSnakePiece();
                this.snake.TimeBetweenMoves *= 0.95;
                this.score += (10 * this.scoreMulitiplier);
                this.scoreMulitiplier++;
                this.soundEffects["Food"].Play();
            }

            // Powerups.
            if (this.powerupManager.ActivePowerup != null && 
                this.snake.Head.Position == this.powerupManager.ActivePowerup.Position)
            {
                switch(this.powerupManager.ActivePowerup.PowerupType)
                {
                    case PowerupManager.PowerupType.BonusPoints:
                        this.score += 1000;
                        this.soundEffects["Score"].Play();
                        break;
                    case PowerupManager.PowerupType.SpeedReduction:
                        this.snake.TimeBetweenMoves *= 1.20;
                        if(this.snake.TimeBetweenMoves >= 0.20)
                        {
                            this.snake.TimeBetweenMoves = 0.20;
                        }
                        this.soundEffects["SpeedReduction"].Play();
                        break;
                    case PowerupManager.PowerupType.Stop:
                        this.snake.Stop();
                        this.timeSinceLastInput = 0.0;
                        this.soundEffects["Stop"].Play();
                        break;
                }

                this.powerupManager.EatPowerup();
            }
        }

        private void LoadContent()
        {
            this.hudTexture = this.content.Load<Texture2D>("Textures\\HUD");
            this.levelTexture = this.content.Load<Texture2D>("Textures\\Level1");
            this.screenCover = this.content.Load<Texture2D>("Textures\\ScreenCover");
            this.sousesFont = this.content.Load<SpriteFont>("Fonts\\Souses");

            this.soundEffects = new Dictionary<string, SoundEffect>()
            {
                { "Countdown", this.content.Load<SoundEffect>("Sounds\\Countdown") },
                { "Food", this.content.Load<SoundEffect>("Sounds\\Food") },
                { "Score", this.content.Load<SoundEffect>("Sounds\\Score") },
                { "SpeedReduction", this.content.Load<SoundEffect>("Sounds\\SpeedReduction") },
                { "Stop", this.content.Load<SoundEffect>("Sounds\\Stop") },
                { "GameOver", this.content.Load<SoundEffect>("Sounds\\GameOver") },
                { "Start", this.content.Load<SoundEffect>("Sounds\\Start") }
            };
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

            spriteBatch.Draw(this.screenCover, Vector2.Zero, Color.White);
            spriteBatch.DrawString(this.sousesFont, message, new Vector2((280.0f - (messageDimensions.X / 2)),
                (280.0f - (messageDimensions.X / 2))), Color.Red);
        }

        private void DrawGameOver(SpriteBatch spriteBatch)
        {
            string gameOverMessage = "Game Over!";
            string restartMessage = "Press the Left Arrow to restart.";
            string mainMenuMessage = "Press the Down Arrow to return to the main menu.";
            string scoreMessage = "Final Score:";
            string scoreCountMessage = this.score.ToString();
            Vector2 gameOverMessageDimensions = sousesFont.MeasureString(gameOverMessage);
            Vector2 restartMessageDimensions = sousesFont.MeasureString(restartMessage);
            Vector2 mainMenuMessageDimensions = sousesFont.MeasureString(mainMenuMessage);
            Vector2 scoreMessageDimensions = sousesFont.MeasureString(scoreMessage);
            Vector2 scoreCountMessageDimensions = sousesFont.MeasureString(scoreCountMessage);

            spriteBatch.Draw(screenCover, Vector2.Zero, Color.White);
            spriteBatch.DrawString(sousesFont, gameOverMessage, new Vector2((280.0f - (gameOverMessageDimensions.X / 2)),
                200.0f), Color.Red);
            spriteBatch.DrawString(sousesFont, restartMessage, new Vector2((280.0f - (restartMessageDimensions.X / 2)),
                250.0f), Color.Red);
            spriteBatch.DrawString(sousesFont, mainMenuMessage, new Vector2((280.0f - (mainMenuMessageDimensions.X / 2)),
                300.0f), Color.Red);
            spriteBatch.DrawString(sousesFont, scoreMessage, new Vector2((280.0f - (scoreMessageDimensions.X / 2)),
                350.0f), Color.Red);
            spriteBatch.DrawString(sousesFont, scoreCountMessage, new Vector2((280.0f - (scoreCountMessageDimensions.X / 2)),
                400.0f), Color.Red);
        }
    }
}
