using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snooker.Client.Core.Model;

namespace Snooker.Client.XNA.Controls
{
    public class XNAButton : Sprite
    {
        string text = "";
        SpriteFont gameFont;
        float fontScale = 1f;
        int width;
        int height;

        public XNAButton(SpriteFont gameFont)
        {
            this.gameFont = gameFont;
        }

        public XNAButton(SpriteFont gameFont, string text)
        {
            this.gameFont = gameFont;
            this.text = text;
        }

        public string Text { get { return text; } set { text = value; } }
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }

        public void DrawButton(SpriteBatch spriteBatch, Vector2 position, int height, int width, float scale)
        {
            this.position = position;
            this.height = height;
            this.width = width;
            Color color = new Color(255, 255, 255, 255);

            Vector2 measureString = gameFont.MeasureString(text);
            Vector2 textPosition = new Vector2(position.X + (width - measureString.X) / 2f, (position.Y + (height - measureString.Y) / 2f));
            spriteBatch.DrawString(gameFont, text, textPosition, color, 0, new Vector2(0, 0), new Vector2(scale, scale), SpriteEffects.None, 0f);
        }

        public bool TestClick(Vector2 clickPosition)
        {
            bool ret = false;
            Vector2 measureString = gameFont.MeasureString(text);

            if (clickPosition.X >= position.X && clickPosition.X <= (position.X + width) &&
                clickPosition.Y >= position.Y && clickPosition.Y <= (position.Y + height))
            {
                ret = true;
            }
            return ret;
        }
    }
}
