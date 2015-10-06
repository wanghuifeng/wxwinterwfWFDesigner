using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snooker.Client.Core.Model
{
    public class PictureSprite : Sprite
    {
        int alpha = 255;
        public PictureSprite(Texture2D texture, Vector2 position, Vector2 size)
            : base()
        {
            Texture = texture;
            Position = position;
            Size = size;
        }

        public int Alpha { get { return alpha; } set { alpha = value; } }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(this.position.X, this.position.Y);
            Color color = new Color(255, 255, 255, (byte)MathHelper.Clamp(alpha, 0, 255));
            spriteBatch.Draw(Texture, position, null, color, 0f, new Vector2(0, 0), new Vector2(size.X / texture.Width, size.Y / texture.Height), SpriteEffects.None, 0f);
        }

        public void Draw(SpriteBatch spriteBatch, int alpha, float scale)
        {
            Vector2 position = new Vector2(this.position.X, this.position.Y);
            Color color = new Color(255, 255, 255, (byte)MathHelper.Clamp(alpha, 0, 255));
            spriteBatch.Draw(Texture, position, null, color, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
        }
    }
}
