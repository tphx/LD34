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
