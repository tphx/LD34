using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tphx.Bicephalisnake.Gameplay
{
    class PowerupManager
    {
        public enum PowerupType
        {
            SpeedReduction,
            BonusPoints,
            Stop
        }

        private Texture2D powerupTexture;
        private double timeSinceLastPowerup;
        private double timePowerupActive;
        private double timeSinceLastTrySpawn;

        public PowerupManager(ContentManager content)
        {
            this.powerupTexture = content.Load<Texture2D>("Textures\\Powerup");
        }

        public Powerup ActivePowerup { get; private set; }

        public void Update(GameTime gameTime)
        {
            this.timeSinceLastPowerup += gameTime.ElapsedGameTime.TotalSeconds;
            this.timeSinceLastTrySpawn += gameTime.ElapsedGameTime.TotalSeconds;

            if (this.ActivePowerup == null && this.timeSinceLastPowerup >= 15.0 && this.timeSinceLastTrySpawn >= 1.0)
            {
                this.timeSinceLastTrySpawn = 0.0;

                if (new Random((int)System.DateTime.Now.Ticks).Next(0, 100) < 10)
                {
                    SpawnPowerup();
                }
            }
            else if(this.ActivePowerup != null)
            {
                this.timePowerupActive += gameTime.ElapsedGameTime.TotalSeconds;

                if(this.timePowerupActive >= 10.0f)
                {
                    EatPowerup();
                }
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
            this.timeSinceLastPowerup = 0.0;
            this.ActivePowerup = null;
        }

        private void SpawnPowerup()
        {
            Random rand = new Random((int)System.DateTime.Now.Ticks);
            this.ActivePowerup = new Powerup(new Vector2(rand.Next(0, 17), rand.Next(0, 17)), 
                (PowerupType)rand.Next(0, 3));
            this.timePowerupActive = 0.0;
        }

        private Color GetPowerupColor(PowerupType powerupType)
        {
            switch(powerupType)
            {
                case PowerupType.BonusPoints:
                    return Color.Blue;
                case PowerupType.SpeedReduction:
                    return Color.Yellow;
                case PowerupType.Stop:
                    return Color.Red;
                default:
                    return Color.White;
            }

        }
    }
}
