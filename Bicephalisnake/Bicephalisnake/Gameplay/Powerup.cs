using Microsoft.Xna.Framework;

namespace Tphx.Bicephalisnake.Gameplay
{
    class Powerup
    {
        public Powerup(Vector2 position, PowerupManager.PowerupType powerupType)
        {
            this.Position = position;
            this.PowerupType = powerupType;
        }

        public Vector2 Position { get; private set; }

        public PowerupManager.PowerupType PowerupType { get; private set; }
    }
}
