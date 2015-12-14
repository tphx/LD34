using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tphx.Bicephalisnake.Gameplay;

namespace Tphx.Bicephalisnake
{
    public class Game1 : Game
    {
        private enum GameState
        {
            Uninitialized,
            MainMenu,
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

            ChangeGameState(GameState.MainMenu);
        }

        protected override void UnloadContent()
        {
            this.activeComponent.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            this.activeComponent.Update(gameTime);

            switch(this.gameState)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.Playing:
                    UpdateGameplay(gameTime);
                    break;
            }

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
                this.gameState = newGameState;

                if(this.activeComponent != null)
                {
                    this.activeComponent.Dispose();
                    this.activeComponent = null;
                }

                switch(newGameState)
                {
                    case GameState.Uninitialized:
                        this.activeComponent = null;
                        break;
                    case GameState.MainMenu:
                        this.activeComponent = new MainMenu(this.Content);
                        break;
                    case GameState.Playing:
                        this.activeComponent = new BicephalisnakeGameplay(this.Content);
                        break;
                }
            }
        }

        private void UpdateMainMenu(GameTime gameTime)
        {
            MainMenu mainMenu = this.activeComponent as MainMenu;

            if (mainMenu.StartGame)
            {
                ChangeGameState(GameState.Playing);
            }
        }

        private void UpdateGameplay(GameTime gameTime)
        {
            BicephalisnakeGameplay gameplay = this.activeComponent as BicephalisnakeGameplay;

            if (gameplay.ReturnToMainMenu)
            {
                ChangeGameState(GameState.MainMenu);
            }
        }
    }
}
