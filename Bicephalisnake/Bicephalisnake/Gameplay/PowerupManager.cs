using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tphx.Bicephalisnake.Gameplay
{
    class PowerupManager
    {
        public enum PowerupType
        {
            SpeedReduction,
            BonusPoints
        }
        private Texture2D powerupTexture;
        private double timeSinceLastPowerup;

        public PowerupManager(ContentManager content)
        {
            this.powerupTexture = content.Load<Texture2D>("Textures\\Powerup");
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastPowerup += gameTime.ElapsedGameTime.TotalSeconds;

            if (this.ActivePowerup == null && timeSinceLastPowerup >= 20.0 && 
                new Random((int)System.DateTime.Now.Ticks).Next(0, 100) >= 96)
            {
                SpawnPowerup();
                Console.WriteLine("Spawning Powerup");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.ActivePowerup != null)
            {
                spriteBatch.Draw(this.powerupTexture, (this.ActivePowerup.Position * 16.0f), 
                    GetPowerupColor(this.ActivePowerup.PowerupType));
            }
        }

        public void EatPowerup()
        {
            timeSinceLastPowerup = 0.0;
            this.ActivePowerup = null;
        }

        public Powerup ActivePowerup { get; private set; }

        private void SpawnPowerup()
        {
            Random rand = new Random((int)System.DateTime.Now.Ticks);
            this.ActivePowerup = new Powerup(new Vector2(rand.Next(0, 17), rand.Next(0, 17)), 
                (PowerupType)new Random((int)System.DateTime.Now.Ticks).Next(0, 2));
        }

        private Color GetPowerupColor(PowerupType powerupType)
        {
            switch(powerupType)
            {
                case PowerupType.BonusPoints:
                    return Color.Blue;
                case PowerupType.SpeedReduction:
                    return Color.Yellow;
                default:
                    return Color.White;
            }

        }
    }
}
