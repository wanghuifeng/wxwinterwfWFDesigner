using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snooker.Client.Core.Model
{
    public class StrengthSprite : Sprite
    {
        int strenght = 0;
        public StrengthSprite(Texture2D texture, Vector2 position, Vector2 size, int strenght)
        {
            Texture = texture;
            Position = position;
            Size = size;
            this.strenght = strenght;
        }

        public int Strenght
        {
            get
            {
                return strenght;
            }
            set
            {
                strenght = value;
            }
        }

        //public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        //{
        //    spriteBatch.Draw(texture, new Vector2(position.X + offset.X, position.Y + offset.Y), null, new Color(255, 255, 255, 255), 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
        //}

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 position = new Vector2(this.position.X + offset.X, this.position.Y + offset.Y);
            Color color = new Color(255, 255, 255, 255);
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, (int)size.X, (int)size.Y), color, 0f, new Vector2(0, 0), (1.0f / 1.0f), SpriteEffects.None, 0f);
        }

    }
}
