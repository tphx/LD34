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
using System.Collections.Generic;
using System.Linq;

namespace Tphx.Bicephalisnake.Gameplay
{
    class Snake
    {
        private SnakePiece head;
        private SnakePiece tail;
        private List<SnakePiece> bodyPieces = new List<SnakePiece>();
        private Texture2D texture;
        private double timeSinceLastMove;

        public Snake(ContentManager content, Vector2 position, double timeBetweenMoves)
        {
            this.texture = content.Load<Texture2D>("Textures\\Snake");
            this.TimeBetweenMoves = timeBetweenMoves;
            CreateSnake(position);
        }

        public bool MovingVertically { get; set; }

        public Vector2 NextHorizontalTurn { get; private set; }

        public Vector2 NextVerticalTurn { get; private set; }

        public double TimeBetweenMoves { get; set; }

        public SnakePiece Head
        {
            get
            {
                return this.head;
            }
        }

        public List<SnakePiece> BodyPieces
        {
            get
            {
                return this.bodyPieces.ToList();
            }
        }

        public SnakePiece Tail
        {
            get
            {
                return this.tail;
            }
        }

        public void Update(GameTime gameTime)
        {
            this.timeSinceLastMove += gameTime.ElapsedGameTime.TotalSeconds;

            if(this.timeSinceLastMove >= this.TimeBetweenMoves)
            {
                MoveSnake();
                this.timeSinceLastMove = 0.0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Head
            Vector2 position;

            // Body
            foreach(SnakePiece bodyPiece in this.bodyPieces)
            {
                position = new Vector2(bodyPiece.Position.X * 16.0f, bodyPiece.Position.Y * 16.0f);
                position.X += 8.0f;
                position.Y += 8.0f;
                spriteBatch.Draw(this.texture, position, new Rectangle(35, 1, 16, 16), Color.White,
                    MathHelper.ToRadians(bodyPiece.Rotation), new Vector2(8.0f, 8.0f), Vector2.One, 
                    SpriteEffects.None, 0.5f);
            }

            // Tail
            position = new Vector2(this.tail.Position.X * 16.0f, this.tail.Position.Y * 16.0f);
            position.X += 8.0f;
            position.Y += 8.0f;
            spriteBatch.Draw(this.texture, position, new Rectangle(18, 1, 16, 16), Color.White,
                MathHelper.ToRadians(this.tail.Rotation), new Vector2(8.0f, 8.0f), Vector2.One, 
                SpriteEffects.None, 0.5f);

            // Head
            position = new Vector2(this.head.Position.X * 16.0f, this.head.Position.Y * 16.0f);
            position.X += 8.0f;
            position.Y += 8.0f;
            spriteBatch.Draw(this.texture, position, new Rectangle(1, 1, 16, 16), Color.White,
                MathHelper.ToRadians(this.head.Rotation), new Vector2(8.0f, 8.0f), Vector2.One,
                SpriteEffects.None, 0.5f);
        }

        public void MoveSnake()
        {
            if (this.head.MovementDirection != Vector2.Zero)
            {
                SnakePiece originalHead = new SnakePiece(this.head.Position, this.head.Rotation,
                    this.head.MovementDirection);
                SnakePiece originalBodyEnd = GetBodyEnd();

                // Move the head and everything else just follows its leading piece.
                MoveHead();
                MoveBody(originalHead);
                this.tail = (originalBodyEnd == null) ? originalHead : originalBodyEnd;

                RotatePiece(this.head);
                RotatePiece(this.tail);
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

        public void TurnHorizontally()
        {
            this.head.MovementDirection = this.NextHorizontalTurn;
            this.MovingVertically = false;
            this.NextHorizontalTurn = new Vector2(
                new Random((int)System.DateTime.Now.Ticks).Next(0, 2) > 0 ? 1.0f : -1.0f,
                0.0f);
        }

        public void TurnVertically()
        {
            this.head.MovementDirection = this.NextVerticalTurn;
            this.MovingVertically = true;
            this.NextVerticalTurn = new Vector2(0.0f,
                new Random((int)System.DateTime.Now.Ticks).Next(0, 2) > 0 ? 1.0f : -1.0f);
        }

        public void Stop()
        {
            this.head.MovementDirection = Vector2.Zero;
        }

        private void MoveHead()
        {
            this.head.Position = new Vector2(
                (this.head.Position.X + (1.0f * this.head.MovementDirection.X)),
                (this.head.Position.Y + (1.0f * this.head.MovementDirection.Y)));

            if (this.head.Position.X < 0.0f)
            {
                this.head.Position = new Vector2(BicephalisnakeGameplay.BoardDimensions.X - 1.0f,
                    this.head.Position.Y);
            }

            if (this.head.Position.X >= BicephalisnakeGameplay.BoardDimensions.X)
            {
                this.head.Position = new Vector2(0.0f, this.head.Position.Y);
            }

            if (this.head.Position.Y < 0.0f)
            {
                this.head.Position = new Vector2(this.head.Position.X,
                    BicephalisnakeGameplay.BoardDimensions.Y - 1.0f);
            }

            if (this.head.Position.Y >= BicephalisnakeGameplay.BoardDimensions.Y)
            {
                this.head.Position = new Vector2(this.head.Position.X, 0.0f);
            }
        }

        private void CreateSnake(Vector2 position)
        {
            this.head = new SnakePiece(position, 0.0f, new Vector2(0.0f, -1.0f));

            // Seed the turns then start moving upwards for consistency.
            TurnHorizontally();
            TurnVertically();
            this.head.MovementDirection = new Vector2(0.0f, -1.0f);

            this.tail = new SnakePiece(new Vector2(position.X, position.Y + 1.0f), 0.0f, 
               Vector2.Zero);
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

        private void RotatePiece(SnakePiece piece)
        {   
            if (piece.MovementDirection.Y == 1.0f)
            {
                piece.Rotation = 180.0f;
            }
            else if (piece.MovementDirection.Y == -1.0f)
            {
                piece.Rotation = 0.0f;
            }
            else if (piece.MovementDirection.X == -1.0f)
            {
                piece.Rotation = 270.0f;
            }
            if (piece.MovementDirection.X == 1.0f)
            {
                piece.Rotation = 90.0f;
            }
        }
    }
}
