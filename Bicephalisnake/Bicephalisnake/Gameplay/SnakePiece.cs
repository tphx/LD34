using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tphx.Bicephalisnake.Gameplay
{
    class SnakePiece
    {
        public SnakePiece(Vector2 position, float rotation, Vector2 movementDirection)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.MovementDirection = movementDirection;
        }

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }

        public Vector2 MovementDirection { get; set; }
    }
}
