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
