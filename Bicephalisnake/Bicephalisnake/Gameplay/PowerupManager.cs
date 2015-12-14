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
