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
            snake = new Snake(this.content, new Vector2(17.0f, 17.0f));
        }
    }
}
