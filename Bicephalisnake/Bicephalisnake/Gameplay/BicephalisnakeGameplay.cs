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
            CountDown,
            Playing,
            GameOver
        };

        private GameplayState gameplayState = GameplayState.Uninitialized;
        private Snake snake;
        private double timeSinceLastInput;
        private double inputCooldown = 0.02;

        public BicephalisnakeGameplay(ContentManager content)
            : base(content)
        {
            NewGame();
        }

        public static Vector2 BoardDimensions { get; private set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(this.snake != null)
            {
                this.snake.Draw(spriteBatch);
            }
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
                case GameplayState.CountDown:
                    break;
                case GameplayState.Playing:
                    UpdateGameplay(gameTime);
                    break;
                case GameplayState.GameOver:
                    break;
            }
        }

        private void UpdateGameplay(GameTime gameTime)
        {
            if (this.timeSinceLastInput >= this.inputCooldown)
            {
                KeyboardState keyboardState = Keyboard.GetState();

                if(keyboardState.IsKeyDown(Keys.Left) && this.snake.MovingVertically)
                {
                    float turnDirection = new Random((int)System.DateTime.Now.Ticks).Next(2) > 0 ? 1.0f : -1.0f;
                    this.snake.TurnSnake(new Vector2(turnDirection, 0.0f));
                    this.timeSinceLastInput = 0.0f;
                }
                else if (keyboardState.IsKeyDown(Keys.Down) && !this.snake.MovingVertically)
                {
                    float turnDirection = new Random((int)System.DateTime.Now.Ticks).Next(2) > 0 ? 1.0f : -1.0f;
                    this.snake.TurnSnake(new Vector2(0.0f, turnDirection));
                    this.timeSinceLastInput = 0.0f;
                }
            }

            this.snake.Update(gameTime);
        }

        private void NewGame()
        {
            BicephalisnakeGameplay.BoardDimensions = new Vector2(35.0f, 35.0f);

            Vector2 snakePosition = new Vector2((BicephalisnakeGameplay.BoardDimensions.X / 2),
                (BicephalisnakeGameplay.BoardDimensions.X / 2));
            snakePosition.X = (float)Math.Floor(snakePosition.X);
            snakePosition.Y = (float)Math.Floor(snakePosition.Y);

            this.snake = new Snake(this.content, snakePosition, 1.0);
            this.gameplayState = GameplayState.Playing;
        }
    }
}
