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
        private Texture2D hudTexture;
        private Texture2D levelTexture;
        private SpriteFont sousesFont;
        private int score;
        private Dictionary<Vector2, DirectionalArrow> directionalArrows = new Dictionary<Vector2, DirectionalArrow>()
        {
            { new Vector2(0.0f, -1.0f), new DirectionalArrow(0.0f, Color.Yellow) }, // Up
            { new Vector2(0.0f, 1.0f), new DirectionalArrow(0.0f, Color.Yellow) }, // Down
            { new Vector2(-1.0f, 0.0f), new DirectionalArrow(0.0f, Color.Yellow) }, // Left
            { new Vector2(1.0f, 0.0f), new DirectionalArrow(0.0f, Color.Yellow) }, // Right
        };

        public BicephalisnakeGameplay(ContentManager content)
            : base(content)
        {
            this.hudTexture = this.content.Load<Texture2D>("Textures\\HUD");
            this.levelTexture = this.content.Load<Texture2D>("Textures\\Level1");
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
            spriteBatch.Draw(this.hudTexture, new Vector2(40.0f, 580.0f), new Rectangle(561, 1, 61, 61), Color.White);
            spriteBatch.Draw(this.hudTexture, new Vector2(459.0f, 580.0f), new Rectangle(561, 1, 61, 61), Color.White);
            spriteBatch.DrawString(this.sousesFont, string.Format("Score: {0:00000000}", this.score), 
                new Vector2(197.0f, 600.0f), Color.Black);

            this.snake.Draw(spriteBatch);
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
            this.score = 0;
            this.gameplayState = GameplayState.Playing;
        }
    }
}
