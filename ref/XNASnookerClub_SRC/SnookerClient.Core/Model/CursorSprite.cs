using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snooker.Client.Core.Model
{
    public class CursorSprite : Sprite
    {
        public CursorSprite(Texture2D texture, Vector2 position, Vector2 size)
            : base()
        {
            Texture = texture;
            Position = position;
            Size = size;
        }
    }
}
