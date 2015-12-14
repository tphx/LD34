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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tphx.Bicephalisnake
{
    class MainMenu : GameComponent
    {
        private Texture2D texture;
        private double timeSinceLastInput;

        public MainMenu(ContentManager content)
            : base(content)
        {
            this.texture = this.content.Load<Texture2D>("Textures\\MainMenu");
        }

        public bool StartGame { get; private set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, Vector2.Zero, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            this.timeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;

            if (this.timeSinceLastInput >= 2.0)
            {
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.Down))
                {
                    this.StartGame = true;
                }
            }
        }
    }
}
