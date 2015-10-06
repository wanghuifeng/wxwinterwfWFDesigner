using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snooker.Client.Core.Model;

namespace Snooker.Client.XNA.Controls
{
    public class XNARadioButton : Sprite
    {
        bool isChecked = false;
        string text = "";
        SpriteFont gameFont;
        float fontScale = 1f;

        public XNARadioButton(SpriteFont gameFont)
        {
            this.gameFont = gameFont;
        }

        public XNARadioButton(SpriteFont gameFont, bool isChecked, string text)
        {
            this.gameFont = gameFont;
            this.isChecked = isChecked;
            this.text = text;
        }

        public bool Checked { get { return isChecked; } set { isChecked = value; } }
        public string Text { get { return text; } set { text = value; } }

        public Vector2 DrawString(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            this.position = position;
            Color color;
            if (isChecked)
            {
                color = new Color(255, 255, 255, 255);
            }
            else
            {
                color = new Color(200, 200, 200, 255);
            }
            
            string checkString = "[ " + (isChecked ? "x" : "");
            Vector2 textPosition = position;
            spriteBatch.DrawString(gameFont, checkString, textPosition, color, 0, new Vector2(0, 0), new Vector2(scale, scale), SpriteEffects.None, 0f);
            textPosition += new Vector2(20, 0);
            spriteBatch.DrawString(gameFont, "] " + text, textPosition, color, 0, new Vector2(0, 0), new Vector2(scale, scale), SpriteEffects.None, 0f);
            Vector2 measureString = gameFont.MeasureString(text);
            textPosition += new Vector2(0, measureString.Y);
            textPosition -= new Vector2(20, 0);
            return textPosition;
        }

        public bool TestClick(Vector2 clickPosition)
        {
            bool ret = false;
            Vector2 measureString = gameFont.MeasureString(text);

            if (clickPosition.X >= position.X && clickPosition.X <= (position.X + measureString.X) &&
                clickPosition.Y >= position.Y && clickPosition.Y <= (position.Y + measureString.Y))
            {
                ret = true;
            }
            return ret;
        }
    }
}
