using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snooker.Client.Core.Model
{
    public class KeyboardSprite : Sprite
    {
        public KeyboardSprite(Texture2D texture, Vector2 position, Vector2 size)
            : base()
        {
            Texture = texture;
            Position = position;
            Size = size;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 position = new Vector2(this.position.X, this.position.Y);
            Color color = new Color(255, 255, 255, 255);
            spriteBatch.Draw(Texture, position, null, color, 0f, new Vector2(0, 0), new Vector2(1f, 1f), SpriteEffects.None, 0f);
        }
    }
}
