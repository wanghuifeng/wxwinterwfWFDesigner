using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snooker.Client.Core.Model
{
    public class CueSprite : Sprite
    {
        float rotationAngle = 0f;
        float shadowRotationAngle = 0f;
        Vector2 origin = new Vector2(-24, 14);
        Vector2 target = new Vector2(0, 0);
        Vector2 newTarget = new Vector2(0, 0);
        Texture2D alphaTexture;
        Texture2D shadowTexture;
        public CueSprite(Texture2D texture, Texture2D alphaTexture, Texture2D shadowTexture, Vector2 position, Vector2 size)
        {
            this.alphaTexture = alphaTexture;
            ShadowTexture = shadowTexture;
            Texture = texture;
            Position = position;
            Size = size;
        }

        public void DrawShadow(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 shadowPosition = new Vector2(this.position.X + offset.X, this.position.Y + offset.Y);
            Color color = new Color(255, 255, 255, 64);
            spriteBatch.Draw(shadowTexture, shadowPosition, null, color, shadowRotationAngle, origin, (1.0f / 3.0f), SpriteEffects.None, 0f);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 position = new Vector2(this.position.X + offset.X, this.position.Y + offset.Y);

            Color color = new Color(255, 255, 255, 64);
            spriteBatch.Draw(alphaTexture, position, null, color, rotationAngle, origin, (1.0f / 3.0f), SpriteEffects.None, 0f);
            
            color = new Color(255, 255, 255, 255);
            spriteBatch.Draw(texture, position, null, color, rotationAngle, origin, (1.0f / 3.0f), SpriteEffects.None, 0f);
        }

        public float RotationAngle
        {
            get { return rotationAngle; }
            set { rotationAngle = value; }
        }

        public float ShadowRotationAngle
        {
            get { return shadowRotationAngle; }
            set { shadowRotationAngle = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Vector2 Target
        {
            get { return target; }
            set { target = value; }
        }

        public Vector2 NewTarget
        {
            get { return newTarget; }
            set { newTarget = value; }
        }

        public Texture2D ShadowTexture { get { return shadowTexture; } set { shadowTexture = value; } }
    }
}
