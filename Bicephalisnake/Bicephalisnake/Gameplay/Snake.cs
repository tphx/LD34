using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tphx.Bicephalisnake.Gameplay
{
    class Snake
    {
        private SnakePiece head;
        private SnakePiece tail;
        private List<SnakePiece> bodyPieces = new List<SnakePiece>();
        private Texture2D texture;

        public Snake(ContentManager content, Vector2 position)
        {
            this.texture = content.Load<Texture2D>("Textures\\TestTextures");
            CreateSnake(position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, (this.head.Position * 16.0f), new Rectangle(1, 1, 16, 16), Color.White,
                this.head.Rotation, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.5f);

            foreach(SnakePiece bodyPiece in this.bodyPieces)
            {
                spriteBatch.Draw(texture, (bodyPiece.Position * 16.0f), new Rectangle(35, 1, 16, 16), Color.White,
                    bodyPiece.Rotation, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.5f);
            }

            spriteBatch.Draw(texture, (this.tail.Position * 16.0f), new Rectangle(18, 1, 16, 16), Color.White,
                this.tail.Rotation, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.5f);
        }

        public void MoveSnake()
        {
            SnakePiece originalHead = new SnakePiece(this.head.Position, this.head.Rotation,
                this.head.MovementDirection);
            SnakePiece originalBodyEnd = GetBodyEnd();

            // Move the head and everything else just follows its leading piece.
            MoveHead();
            MoveBody(originalHead);
            this.tail = (originalBodyEnd == null) ? originalHead : originalBodyEnd;
        }

        private void MoveHead()
        {
            this.head.Position = new Vector2(
                (this.head.Position.X + (1.0f * this.head.MovementDirection.X)),
                (this.head.Position.Y + (1.0f * this.head.MovementDirection.Y)));
            
            if(this.head.Position.X < 0.0f)
            {
                this.head.Position = new Vector2(BicephalisnakeGameplay.BoardDimensions.X, 
                    this.head.Position.Y);
            }

            if (this.head.Position.X > BicephalisnakeGameplay.BoardDimensions.X)
            {
                this.head.Position = new Vector2(0.0f, this.head.Position.Y);
            }

            if (this.head.Position.Y < 0.0f)
            {
                this.head.Position = new Vector2(this.head.Position.X,
                    BicephalisnakeGameplay.BoardDimensions.Y);
            }

            if (this.head.Position.Y > BicephalisnakeGameplay.BoardDimensions.Y)
            {
                this.head.Position = new Vector2(this.head.Position.X, 0.0f);
            }
        }

        public void SpawnSnakePiece()
        {
            // The new piece is the current tail. 
            this.bodyPieces.Add(new SnakePiece(this.tail.Position, this.tail.Rotation, 
                this.tail.MovementDirection));

            // The tail is pushed back in the opposite direction that it's moving.
            this.tail.Position = new Vector2(
                (this.tail.Position.X + (-1.0f * this.tail.MovementDirection.X)),
                (this.tail.Position.Y + (-1.0f * this.tail.MovementDirection.Y)));
        }

        public void TurnSnake(Vector2 direction)
        {
            // The snake can only turn 90 degrees. We need to check to make sure the direction is valid before
            // we actually turn. If it isn't we can ignore the turn.

            // Horizonal turn if the head isn't moving horizontally.
            if((direction.X != 0.0f && direction.Y == 0.0f) && (this.head.MovementDirection.X == 0.0f))
            {
                this.head.MovementDirection = direction;
            }
            // Vertical turn if the head isn't moving vertically.
            else if ((direction.Y != 0.0f && direction.X == 0.0f) && (this.head.MovementDirection.Y == 0.0f))
            {
                this.head.MovementDirection = direction;
            }
        }

        private void CreateSnake(Vector2 position)
        {
            // Snake starts facing upwards.
            this.head = new SnakePiece(position, 0.0f, new Vector2(0.0f, -1.0f));
            this.tail = new SnakePiece(new Vector2(position.X, position.Y + 1.0f), 0.0f, 
                new Vector2(0.0f, -1.0f));
        }

        private void MoveBody(SnakePiece originalHead)
        {
            int currentIndex = 0;

            SnakePiece lastPiece = null;

            foreach (SnakePiece bodyPiece in this.bodyPieces.ToList())
            {

                if (currentIndex == 0)
                {
                    lastPiece = bodyPiece;
                    this.bodyPieces[currentIndex] = originalHead;
                }
                else
                {
                    this.bodyPieces[currentIndex] = lastPiece;
                    lastPiece = bodyPiece;
                }

                currentIndex++;
            }
        }

        private SnakePiece GetBodyEnd()
        {
            if (this.bodyPieces.Count > 0)
            {
                int bodyEndIndex = this.bodyPieces.Count - 1;
                return new SnakePiece(this.bodyPieces[bodyEndIndex].Position, this.bodyPieces[bodyEndIndex].Rotation,
                this.bodyPieces[bodyEndIndex].MovementDirection);
            }
            else
            {
                return null;
            }
        }
    }
}
