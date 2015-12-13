using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tphx.Bicephalisnake.Gameplay
{
    class Powerup
    {
        public Powerup(Vector2 position, PowerupManager.PowerupType powerupType)
        {
            this.Position = position;
            this.PowerupType = powerupType;
        }

        public Vector2 Position { get; set; }

        public PowerupManager.PowerupType PowerupType { get; set; }
    }
}
