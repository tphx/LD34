using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tphx.Bicephalisnake.Gameplay
{
    struct DirectionalArrow
    {
        public DirectionalArrow(float rotation, Color color)
        {
            this.Rotation = rotation;
            this.Color = color;
        }

        public float Rotation { get; set; }

        public Color Color { get; set; }
    }
}
