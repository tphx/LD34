using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tphx.Bicephalisnake.Gameplay
{
    class FoodManager
    {
        private Texture2D foodTexture;
        private Vector2 foodPosition;

        public FoodManager(ContentManager content)
        {
            this.foodTexture = content.Load<Texture2D>("Textures\\Food");
            SpawnFood();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.foodTexture, (this.foodPosition * 16.0f), Color.Gray);
        }

        public void EatFood()
        {
            SpawnFood();
        }

        public Vector2 FoodPosition
        {
            get
            {
                return this.foodPosition;
            }
        }

        private void SpawnFood()
        {
            Random rand = new Random((int)System.DateTime.Now.Ticks);
            this.foodPosition = new Vector2(rand.Next(0, 17), rand.Next(0, 17));
        }
    }
}
