using Microsoft.Xna.Framework;

namespace Tphx.Bicephalisnake.Gameplay
{
    struct DirectionalArrow
    {
        public DirectionalArrow(float rotation, Color color)
        {
            this.Rotation = rotation;
            this.Color = color;
        }

        public float Rotation { get; private set; }

        public Color Color { get; private set; }
    }
}
