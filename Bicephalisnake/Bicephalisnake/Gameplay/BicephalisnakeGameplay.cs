using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

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
        private Texture2D snakeTexture;
        private Texture2D levelTexture;

        public BicephalisnakeGameplay(ContentManager content)
            : base(content)
        {
            LoadTextures();
            this.gameplayState = GameplayState.NewGame;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            switch(this.gameplayState)
            {
                case GameplayState.NewGame:
                    break;
                case GameplayState.CountDown:
                    break;
                case GameplayState.Playing:
                    break;
                case GameplayState.GameOver:
                    break;
            }
        }

        private void LoadTextures()
        {
            this.snakeTexture = this.content.Load<Texture2D>("Textures\\TestTextures");
            this.levelTexture = this.content.Load<Texture2D>("Textures\\TestGround");
        }
    }
}
