using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snooker.Client.Core.Model
{
    public class RotatorSprite : Sprite
    {
        float angle = 0f;
        float alpha = 255f;
        float alphaIncrease = -8;
        Vector2 axisPoint = new Vector2(0, 0);
        float scale = 1f;

        public RotatorSprite() { }

        public RotatorSprite(Texture2D texture, Vector2 position, Vector2 size, Vector2 axisPoint, float scale)
        {
            Texture = texture;
            Position = position;
            Size = size;
            this.axisPoint = axisPoint;
            this.scale = scale;
        }

        public float Angle { get { return angle; } set {angle = value;} }

        public float Alpha { get { return alpha; } set { alpha = value; } }

        public Vector2 AxisPoint { get { return axisPoint; } set { axisPoint = value; } }

        public float Scale { get { return scale; } set { scale = value; } }

        public void Fade()
        {
            alpha += alphaIncrease;
            if (alpha < 64)
            {
                alphaIncrease *= -1;
                alpha = 64;
            }
            if (alpha > 255)
            {
                alphaIncrease *= -1;
                alpha = 255;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 position = new Vector2(this.position.X + offset.X, this.position.Y + offset.Y);
            Color color = new Color(255, 255, 255, (byte)MathHelper.Clamp(alpha, 0, 255));
            spriteBatch.Draw(texture, position, null, color, angle, axisPoint, scale, SpriteEffects.None, 0f);
        }
    }
}

