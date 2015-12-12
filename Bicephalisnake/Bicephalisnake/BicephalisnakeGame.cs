using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tphx.Bicephalisnake.Gameplay;

namespace Tphx.Bicephalisnake
{
    public class Game1 : Game
    {
        private enum GameState
        {
            Uninitialized,
            Playing
        };

        private GameState gameState = GameState.Uninitialized;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameComponent activeComponent;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 560,
                PreferredBackBufferHeight = 660
            };

            this.Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            ChangeGameState(GameState.Playing);
        }

        protected override void UnloadContent()
        {
            this.activeComponent.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            this.activeComponent.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this.spriteBatch.Begin();
            this.activeComponent.Draw(this.spriteBatch);
            this.spriteBatch.End();
            base.Draw(gameTime);
        }

        private void ChangeGameState(GameState newGameState)
        {
            if(newGameState != this.gameState)
            {
                if(this.activeComponent != null)
                {
                    this.activeComponent.Dispose();
                }

                switch(newGameState)
                {
                    case GameState.Uninitialized:
                        this.activeComponent = null;
                        break;
                    case GameState.Playing:
                        this.activeComponent = new BicephalisnakeGameplay(this.Content);
                        break;
                }
            }
        }
    }
}
