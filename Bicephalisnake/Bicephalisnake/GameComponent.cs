using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tphx.Bicephalisnake
{
    abstract class GameComponent : IDisposable
    {
        protected ContentManager content;
        protected bool disposed = false;

        public GameComponent(ContentManager content)
        {
            this.content = new ContentManager(content.ServiceProvider, content.RootDirectory);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        private void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    this.content.Unload();
                    this.content.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}
