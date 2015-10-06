using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Media;
using SnookerCore;

namespace Snooker.Client.Core.Model
{
    [Serializable]
    public class Ball: Sprite
    {
        #region attributes
        public static double Radius = 8;
        public static int CosBall45;
        bool isStill = true;
        string id;
        Vector2 initPosition = new Vector2(0, 0);
        double lastX;
        double lastY;
        //Vector2 position = new Vector2(0, 0);
        int width;
        int height;
        double rad = 0;
        Vector2 translateVelocity = new Vector2(0, 0);
        Vector2 vSpinVelocity = new Vector2(0, 0);
        Vector2 hSpinVelocity = new Vector2(0, 0);
        IBallObserver observer;
        SoundPlayer myPlayer;
        int points;
        bool isBallInPocket = false;
        static StreamReader reader;
        private Texture2D alphaTexture;
        private Texture2D shadowTexture;
        int alphaValue = 255;
        List<Vector2> lights = new List<Vector2>();
        Vector2 drawPosition = new Vector2(0, 0);
        #endregion attributes

        #region constructor

        public Ball(Texture2D texture, Texture2D alphaTexture, Texture2D shadowTexture, List<Vector2> lights, Vector2 position, Vector2 size, string id, IBallObserver observer, int points)
        {
            this.texture = texture;
            this.alphaTexture = alphaTexture;
            this.shadowTexture = shadowTexture;
            this.lights = lights;
            this.Position = position;
            this.size = size;
            CosBall45 = (int)(Math.Cos(Math.PI / 4) * Ball.Radius);
            this.id = id;
            this.observer = observer;
            width = 32;
            height = 32;
            initPosition = new Vector2(position.X, position.Y);
            Position = position;
            lastX = position.X;
            lastY = position.Y;
            this.points = points;
        }

        #endregion constructor

        #region properties

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool IsBallInPocket
        {
            get { return isBallInPocket; }
            set {
                if (isBallInPocket == false && value)
                {
                    if (value && id == "01")
                    {
                        isBallInPocket = false;
                        position.X = initPosition.X;
                        position.Y = initPosition.Y;
                        return;
                    }
                }

                isBallInPocket = value;
                if (!isBallInPocket)
                {
                    alphaValue = 255;
                }
            }
        }

        public int Points
        {
            get { return points; }
        }

        public float X
        {
            get { return position.X; }
            set
            {
                position.X = value;
                isStill = false;
            }
        }

        public float Y
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
                isStill = false;
            }
        }

        public double LastX
        {
            get { return lastX; }
            set { lastX = value; }
        }

        public double LastY
        {
            get { return lastY; }
            set { lastY = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public double Rad
        {
            get { return rad; }
            set { rad = value; }
        }

        private Vector2 Velocity
        {
            get { return translateVelocity; }
        }

        public Vector2 TranslateVelocity
        {
            get { return translateVelocity; }
            set { translateVelocity = value; }
        }

        public Vector2 VSpinVelocity
        {
            get { return vSpinVelocity; }
            set { vSpinVelocity = value; }
        }

        public Vector2 HSpinVelocity
        {
            get { return hSpinVelocity; }
            set { hSpinVelocity = value; }
        }

        public bool IsStill
        {
            get { return isStill; }
            set { isStill = value; }
        }

        public void ResetPosition()
        {
            translateVelocity = new Vector2(0, 0);
            isBallInPocket = false;
            position.X = initPosition.X;
            position.Y = initPosition.Y;
            lastX = position.X;
            lastY = position.Y;
        }

        public Vector2 InitPosition { get { return initPosition; } set { initPosition = value; } }

        public int AlphaValue { get { return alphaValue; } set { alphaValue = value; } }

        public Vector2 DrawPosition { get { return drawPosition; } set { drawPosition = value; } }

        #endregion properties

        #region functions

        public void DrawShadow(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (!this.isBallInPocket)
            {
                foreach (Vector2 light in lights)
                {
                    Vector2 position = new Vector2(this.drawPosition.X + offset.X, this.drawPosition.Y + offset.Y);

                    float dx = position.X - offset.X + 6 - light.X;
                    float dy = position.Y - offset.Y + 6 - light.Y;
                    float h = (float)Math.Sqrt(dx * dx + dy * dy);

                    int maxGray = 64;
                    int alpha = (int)(maxGray - Math.Abs(h / 660.0) * maxGray);
                    Color color = new Color(255, 255, 255, (byte)MathHelper.Clamp(alpha, 0, maxGray));

                    float scaleX = Math.Abs(dx / 450) < 1f ? 1f : Math.Abs(dx / 450);
                    float scaleY = Math.Abs(dx / 450) < 1f ? 1f : Math.Abs(dx / 450);
                    Vector2 scale = new Vector2(1.5f, 1.5f);

                    float rotationAngle = 0;

                    if (h == 0)
                    {
                        rotationAngle = 0;
                    }
                    else if (dx == 0 && dy == 0)
                    {
                        rotationAngle = 0;
                    }
                    else if (dx > 0 && dy > 0)
                    {
                        rotationAngle = (float)(MathHelper.Pi) + (float)Math.Acos(dx / h);
                    }
                    else if (dx > 0 && dy < 0)
                    {
                        rotationAngle = (float)(MathHelper.Pi) - (float)Math.Acos(dx / h);
                    }
                    else if (dx < 0 && dy > 0)
                    {
                        rotationAngle = (float)(MathHelper.Pi) + (float)Math.Acos(dx / h);
                    }
                    else if (dx < 0 && dy < 0)
                    {
                        rotationAngle = (float)(MathHelper.Pi) - (float)Math.Acos(dx / h);
                    }

                    rotationAngle = rotationAngle % (MathHelper.Pi * 2f);
                    

                    spriteBatch.Draw(shadowTexture, position, null, color, 0f, new Vector2(4f + 6f * (float)Math.Cos(rotationAngle), 4f + 6f * (float)Math.Sin(rotationAngle)), scale, SpriteEffects.None, 0f);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 position = new Vector2(this.drawPosition.X + offset.X, this.drawPosition.Y + offset.Y);

            //alphaTexture
            Color color = new Color(255, 255, 255, (byte)MathHelper.Clamp((alphaValue + 1) / 4, 0, 255));
            spriteBatch.Draw(alphaTexture, position, null, color, 0, new Vector2(0, 0), (1.0f / 1.0f), SpriteEffects.None, 0f);

            //texture
            color = new Color(255, 255, 255, (byte)MathHelper.Clamp(alphaValue, 0, 255));
            spriteBatch.Draw(Texture, position, null, color, 0, new Vector2(0, 0), (1.0f / 1.0f), SpriteEffects.None, 0f);
        }

        public void ResetPositionAt(float x, float y)
        {
            translateVelocity = new Vector2(0, 0);
            isBallInPocket = false;
            position.X = x;
            position.Y = y;
            lastX = x;
            lastY = y;
            alphaValue = 255;
        }

        public void SetPosition(float x, float y)
        {
            this.position.X = x;
            this.position.Y = y;

            lastX = x;
            LastY = y;
            isStill = false;
        }

        public bool Colliding(Ball ball)
        {
            if (!ball.isBallInPocket && !isBallInPocket)
            {
                float xd = (float)(position.X - ball.X);
                float yd = (float)(position.Y - ball.Y);

                float sumRadius = (float)((Ball.Radius + 1.0) * 2);
                float sqrRadius = sumRadius * sumRadius;

                float distSqr = (xd * xd) + (yd * yd);

                if (Math.Round(distSqr) < Math.Round(sqrRadius))
                {
                    return true;
                }
            }

            return false;
        }

        public void ResolveCollision(Ball ball)
        {
            // get the mtd
            Vector2 delta = (position - ball.position);
            float d = delta.Length();
            // minimum translation distance to push balls apart after intersecting
            Vector2 mtd = delta * ((float)(((Ball.Radius + 1.0 + Ball.Radius + 1.0) - d) / d));

            // resolve intersection --
            // inverse mass quantities
            float im1 = 1f;
            float im2 = 1f;

            // push-pull them apart based off their mass
            position = position + ((mtd * (im1 / (im1 + im2))));
            ball.position = ball.position - (mtd * (im2 / (im1 + im2)));

            // impact speed
            Vector2 v = (this.translateVelocity - (ball.translateVelocity));
            Vector2 mtdNormalize = new Vector2(mtd.X, mtd.Y);
            mtdNormalize.Normalize();
            float vn = Vector2.Dot(v, mtdNormalize);

            // sphere intersecting but moving away from each other already
            if (vn > 0.0f)
                return;

            // collision impulse
            //float i = Math.Abs((float)((-(1.0f + 0.1) * vn) / (im1 + im2)));
            Vector2 impulse = mtd * (1);

            int hitSoundIntensity = (int)(Math.Abs(impulse.X) + Math.Abs(impulse.Y));

            if (hitSoundIntensity > 5)
                hitSoundIntensity = 5;

            if (hitSoundIntensity < 1)
                hitSoundIntensity = 1;

            GameSound sound = GameSound.None;
            switch (hitSoundIntensity)
            {
                case 1:
                    sound = GameSound.Hit01;
                    break;
                case 2:
                    sound = GameSound.Hit02;
                    break;
                case 3:
                    sound = GameSound.Hit03;
                    break;
                case 4:
                    sound = GameSound.Hit04;
                    break;
                case 5:
                    sound = GameSound.Hit05;
                    break;
                case 6:
                    sound = GameSound.Hit06;
                    break;
            }

            observer.Hit(sound);

            // change in momentum
            this.translateVelocity = this.translateVelocity + (impulse * (im1));
            ball.translateVelocity = ball.translateVelocity - (impulse * (im2));
        }

        private void wavPlayer_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ((System.Media.SoundPlayer)sender).Play();
        }

        public override string ToString()
        {
            return string.Format("Ball({0}, {1})", (int)position.X, (int)position.Y);
        }

        public override bool Equals(object obj)
        {
            return ((Ball)obj).id.Equals(id);
        }

        #endregion functions
    }
}

