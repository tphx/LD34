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

        public BicephalisnakeGameplay(ContentManager content)
            : base(content)
        {
            NewGame();
        }

        public static Vector2 BoardDimensions { get; private set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(snake != null)
            {
                snake.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
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
                    break;
                case GameplayState.GameOver:
                    break;
            }
        }

        private void NewGame()
        {
            BicephalisnakeGameplay.BoardDimensions = new Vector2(35.0f, 35.0f);

            Vector2 snakePosition = new Vector2((BicephalisnakeGameplay.BoardDimensions.X / 2),
                (BicephalisnakeGameplay.BoardDimensions.X / 2));
            snakePosition.X = (float)Math.Floor(snakePosition.X);
            snakePosition.Y = (float)Math.Floor(snakePosition.Y);
            snake = new Snake(this.content, snakePosition);
        }
    }
}
