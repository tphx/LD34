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
        private double timeSinceLastMove;

        public Snake(ContentManager content, Vector2 position, double timeBetweenMoves)
        {
            this.texture = content.Load<Texture2D>("Textures\\TestTextures");
            this.TimeBetweenMoves = timeBetweenMoves;
            CreateSnake(position);
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
            spriteBatch.Draw(this.texture, (this.head.Position * 16.0f), new Rectangle(1, 1, 16, 16), Color.White,
                this.head.Rotation, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.5f);

            foreach(SnakePiece bodyPiece in this.bodyPieces)
            {
                spriteBatch.Draw(this.texture, (bodyPiece.Position * 16.0f), new Rectangle(35, 1, 16, 16), Color.White,
                    bodyPiece.Rotation, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.5f);
            }

            spriteBatch.Draw(this.texture, (this.tail.Position * 16.0f), new Rectangle(18, 1, 16, 16), Color.White,
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

        public bool MovingVertically { get; set; }

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

        public Vector2 NextHorizontalTurn { get; private set; }

        public Vector2 NextVerticalTurn { get; private set; }

        public double TimeBetweenMoves { get; set; }

        public Vector2 HeadMovementDirection
        {
            get
            {
                return this.head.MovementDirection;
            }

            set
            {
                this.head.MovementDirection = value;
            }
        }

        private void MoveHead()
        {
            this.head.Position = new Vector2(
                (this.head.Position.X + (1.0f * this.head.MovementDirection.X)),
                (this.head.Position.Y + (1.0f * this.head.MovementDirection.Y)));

            if (this.head.Position.X < 0.0f)
            {
                this.head.Position = new Vector2(BicephalisnakeGameplay.BoardDimensions.X - 1,
                    this.head.Position.Y);
            }

            if (this.head.Position.X > BicephalisnakeGameplay.BoardDimensions.X)
            {
                this.head.Position = new Vector2(0.0f, this.head.Position.Y);
            }

            if (this.head.Position.Y < 0.0f)
            {
                this.head.Position = new Vector2(this.head.Position.X,
                    BicephalisnakeGameplay.BoardDimensions.Y - 1);
            }

            if (this.head.Position.Y > BicephalisnakeGameplay.BoardDimensions.Y)
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
    }
}
