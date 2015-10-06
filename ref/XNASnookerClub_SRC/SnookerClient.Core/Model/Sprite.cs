using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snooker.Client.Core.Model
{
    public abstract class Sprite
    {
        protected Texture2D texture;
        protected Vector2 position = new Vector2(0, 0);
        protected Vector2 size;

        public Texture2D Texture { get { return texture; } set { texture = value; } } // Sprite texture, read-only property
        public Vector2 Position { get { return position; } set { position = value; } } // Sprite position on screen
        public Vector2 Size { get { return size; } set { size = value; } } // Sprite size in pixels
        
        public Sprite() { }

        public Sprite(Texture2D texture, Vector2 position, Vector2 size)
        {
            Texture = texture;
            Position = position;
            Size = size;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Draw(spriteBatch, new Vector2(0, 0), 255);
        }

        public virtual void Draw(SpriteBatch spriteBatch, int alpha)
        {
            Draw(spriteBatch, new Vector2(0, 0), alpha);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset, int alpha)
        {
            Vector2 position = new Vector2(this.position.X + offset.X, this.position.Y + offset.Y);
            Color color = new Color(255, 255, 255, (byte)MathHelper.Clamp(alpha, 0, 255));
            spriteBatch.Draw(texture, position, null, color, 0f, new Vector2(0, 0), (1.0f / 1.0f), SpriteEffects.None, 0f);
        }
    }
}

