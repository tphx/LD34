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
using System;

namespace Tphx.Bicephalisnake.Gameplay
{
    class FoodManager
    {
        private Texture2D foodTexture;

        public FoodManager(ContentManager content)
        {
            this.foodTexture = content.Load<Texture2D>("Textures\\Food");
            SpawnFood();
        }

        public Vector2 FoodPosition { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.foodTexture, (this.FoodPosition * 16.0f), Color.Gray);
        }

        public void EatFood()
        {
            SpawnFood();
        }

        private void SpawnFood()
        {
            Random rand = new Random((int)System.DateTime.Now.Ticks);
            this.FoodPosition = new Vector2(rand.Next(0, 17), rand.Next(0, 17));
        }
    }
}
