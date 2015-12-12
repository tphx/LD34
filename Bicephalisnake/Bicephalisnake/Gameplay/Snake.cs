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
        private SnakePiece snakeHead;
        private SnakePiece snakeTail;
        private List<SnakePiece> snakeBody = new List<SnakePiece>();
        private Texture2D texture;

        public Snake(ContentManager content, Vector2 position)
        {
            this.texture = content.Load<Texture2D>("Textures\\TestTextures");
            CreateSnake(position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, (this.snakeHead.Position * 16.0f), new Rectangle(1, 1, 16, 16), Color.White,
                this.snakeHead.Rotation, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.5f);

            foreach(SnakePiece thisPiece in this.snakeBody)
            {
                spriteBatch.Draw(texture, (thisPiece.Position * 16.0f), new Rectangle(35, 1, 16, 16), Color.White,
                    thisPiece.Rotation, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.5f);
            }

            spriteBatch.Draw(texture, (this.snakeTail.Position * 16.0f), new Rectangle(18, 1, 16, 16), Color.White,
                this.snakeTail.Rotation, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.5f);
        }

        public void SpawnSnakePiece()
        {
            // The new piece is the current tail. 
            this.snakeBody.Add(new SnakePiece(this.snakeTail.Position, this.snakeTail.Rotation, 
                this.snakeTail.MovementDirection));

            // The tail is pushed back in the opposite direction that it's moving.
            this.snakeTail.Position = new Vector2(
                (this.snakeTail.Position.X + (-1.0f * this.snakeTail.MovementDirection.X)),
                (this.snakeTail.Position.Y + (-1.0f * this.snakeTail.MovementDirection.Y)));
        }

        private void CreateSnake(Vector2 position)
        {
            // Snake starts facing upwards with the head in the middle of the screen.
            this.snakeHead = new SnakePiece(position, 0.0f, new Vector2(0.0f, -1.0f));
            this.snakeTail = new SnakePiece(new Vector2(position.X, position.Y + 1.0f), 0.0f, 
                new Vector2(0.0f, -1.0f));
     
        }
    }
}
