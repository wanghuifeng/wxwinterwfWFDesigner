using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using SnookerCore;

namespace Snooker.Client.Core.Model
{
    public class TableBorder : Sprite
    {
        #region attributes
        //int x = 0;
        //int y = 0;
        //int width = 0;
        //int height = 0;
        ForcedDirection direction = ForcedDirection.None;
        public static string message;
        IBallObserver observer;
        #endregion attributes

        #region constructor
        public TableBorder(IBallObserver observer, int x, int y, int width, int height, ForcedDirection direction)
        {
            this.observer = observer;
            this.position = new Vector2(x, y);
            this.size = new Vector2(width, height);
            //this.x = x;
            //this.y = y;
            //this.width = width;
            //this.height = height;
            this.direction = direction;
        }
        #endregion constructor

        #region properties
        //public int X
        //{
        //    get { return x; }
        //    set { x = value; }
        //}
        //public int Y
        //{
        //    get { return y; }
        //    set { y = value; }
        //}
        //public int Width
        //{
        //    get { return width; }
        //    set { width = value; }
        //}
        //public int Height
        //{
        //    get { return height; }
        //    set { height = value; }
        //}
        #endregion properties

        #region functions
        public RectangleCollision Colliding(Ball ball)
        {
            RectangleCollision collision = RectangleCollision.None;

            //if (ball.X > 600 && x == 577)
            //{
            //    int i = 0;
            //}

            double mediumX = (ball.LastX + ball.X) / 2.0;
            double mediumY = (ball.LastY + ball.Y) / 2.0;

            if (!ball.IsBallInPocket)
            {
                //if (position.X < 288 && (ball.X - Ball.Radius < position.X + size.X) && (ball.Y >= position.Y && ball.Y <= position.Y + size.Y) && (ball.TranslateVelocity.X + ball.VSpinVelocity.X < 0.0d) && (ball.LastX > position.X + size.X))
                //{
                //    collision = RectangleCollision.Right;
                //}
                //else if (position.X > 288 && (ball.X + Ball.Radius > position.X) && (ball.Y >= position.Y && ball.Y <= position.Y + size.Y) && (ball.TranslateVelocity.X + ball.VSpinVelocity.X > 0.0d) && (ball.LastX < position.X))
                //{
                //    collision = RectangleCollision.Left;
                //}

                //if (position.Y < 161 && (ball.Y - Ball.Radius < position.Y + size.Y) && (ball.X >= position.X && ball.X - Ball.Radius <= position.X + size.X) && (ball.TranslateVelocity.Y + ball.VSpinVelocity.Y < 0.0d) && (ball.LastY > position.Y) && (ball.LastX < position.X + size.X))
                //{
                //    collision = RectangleCollision.Bottom;
                //}
                //else if (position.Y > 161 && (ball.Y + Ball.Radius > position.Y) && (ball.X >= position.X && ball.X <= position.X + size.X) && (ball.TranslateVelocity.Y + ball.VSpinVelocity.Y > 0.0d) && (ball.LastY < position.Y) && (ball.LastY < position.Y) && (ball.LastX < position.X + size.X))
                //{
                //    collision = RectangleCollision.Top;
                //}

                bool insideWidth = (ball.X > position.X) && (ball.X < position.X + size.X);
                bool insideHeight = (ball.Y > position.Y) && (ball.Y < position.Y + size.Y);

                //bool insideWidth = true;
                //bool insideHeight = true;

                if (position.X > 288 && (ball.X + Ball.Radius > position.X) && (ball.X - Ball.Radius < position.X + size.X) && insideHeight && ((ball.TranslateVelocity + ball.VSpinVelocity).X > 0))// && (ball.LastX < position.X))
                {
                    collision = RectangleCollision.Left;
                }
                else if (position.X < 288 && (ball.X - Ball.Radius < position.X + size.X) && (ball.X + Ball.Radius > position.X) && insideHeight && ((ball.TranslateVelocity + ball.VSpinVelocity).X < 0)) // && (ball.LastX > position.X))
                {
                    collision = RectangleCollision.Right;
                }

                if (position.Y > 161 && (ball.Y + Ball.Radius > position.Y) && (ball.Y - Ball.Radius < position.Y + size.Y) && insideWidth && ((ball.TranslateVelocity + ball.VSpinVelocity).Y > 0)) // && (ball.LastY < position.Y))
                {
                    collision = RectangleCollision.Top;
                }
                else if (position.Y < 161 && (ball.Y - Ball.Radius < position.Y + size.Y) && (ball.Y + Ball.Radius > position.Y) && insideWidth && ((ball.TranslateVelocity + ball.VSpinVelocity).Y < 0)) // && (ball.LastY > position.Y))
                {
                    collision = RectangleCollision.Bottom;
                }
            }

            return collision;
        }

        public void ResolveCollision(Ball ball, RectangleCollision collision)
        {
            //double xSound = (float)(ball.Position.X - 300.0) / 300.0;
            observer.Hit(GameSound.Bank01);

            float absorption = 0.9f;

            if (this.direction == ForcedDirection.None)
            {
                switch (collision)
                {
                    case RectangleCollision.Right:
                    case RectangleCollision.Left:
                        if (Math.Sign(ball.TranslateVelocity.X) == Math.Sign(ball.VSpinVelocity.X) && ball.VSpinVelocity.X > 0.0)
                        {
                            ball.TranslateVelocity += new Vector2(ball.VSpinVelocity.X, 0);
                            ball.VSpinVelocity = new Vector2(0, ball.VSpinVelocity.Y);
                        }
                        ball.TranslateVelocity *= new Vector2(-1.0f * absorption, 1);
                        break;
                    case RectangleCollision.Bottom:
                    case RectangleCollision.Top:
                        if (Math.Sign(ball.TranslateVelocity.Y) == Math.Sign(ball.VSpinVelocity.Y) && ball.VSpinVelocity.Y > 0.0)
                        {
                            ball.TranslateVelocity += new Vector2(0, ball.VSpinVelocity.Y);
                            ball.VSpinVelocity = new Vector2(ball.VSpinVelocity.X, 0);
                        }
                        ball.TranslateVelocity *= new Vector2(1, -1.0f * absorption);
                        break;
                }
            }
            else
            {
                Vector2 pos = new Vector2(position.X + size.X / 2, position.Y + size.Y / 2);

                switch (this.direction)
                {
                    case ForcedDirection.Up:
                        ball.TranslateVelocity *= new Vector2(1, -0.5f);
                        ball.TranslateVelocity = new Vector2(ball.TranslateVelocity.Y * -0.5f, ball.TranslateVelocity.Y);
                        break;
                    case ForcedDirection.Down:
                        ball.TranslateVelocity *= new Vector2(1, -0.5f);
                        ball.TranslateVelocity = new Vector2(ball.TranslateVelocity.X, ball.TranslateVelocity.Y * -0.5f);
                        break;
                }

                return;
                // get the mtd
                Vector2 delta = (pos - (ball.Position));
                float d = delta.Length();
                // minimum translation distance to push balls apart after intersecting
                Vector2 mtd = delta * ((float)(((size.X * 2) - d) / d));

                // resolve intersection --
                // inverse mass quantities
                float im1 = 0.5f;
                float im2 = 0.5f;

                // push-pull them apart based off their mass
                ball.Position = ball.Position - (mtd * (im2 / (im1 + im2)));

                // impact speed
                Vector2 v = ball.TranslateVelocity * (-1.0f);
                Vector2 mtdNormalize = new Vector2(mtd.X, mtd.Y);
                mtdNormalize.Normalize();
                float vn = Vector2.Dot(v, mtdNormalize);
                //float vn = 1;

                // sphere intersecting but moving away from each other already
                if (vn > 0.0f)
                    return;

                // collision impulse
                float i = Math.Abs((float)((-(1.0f + 0.1) * vn) / (im1 + im2)));
                Vector2 impulse = mtd * (1);

                switch (this.direction)
                {
                    case ForcedDirection.Up:
                        ball.TranslateVelocity = ball.TranslateVelocity + (new Vector2((float)0, (float)vn));
                        break;
                    case ForcedDirection.Down:
                        ball.TranslateVelocity = ball.TranslateVelocity - (new Vector2((float)0, (float)vn));
                        break;
                    case ForcedDirection.Left:
                        ball.TranslateVelocity = ball.TranslateVelocity - (new Vector2((float)vn, (float)0));
                        break;
                    case ForcedDirection.Right:
                        ball.TranslateVelocity = ball.TranslateVelocity + (new Vector2((float)vn, (float)0));
                        break;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("TableBorder({0}, {1}, {2}, {3})", position.X, position.Y, position.X + size.X, position.Y + size.Y);
        }

        //Vector2 ClosestPointOnRectangle(Ball ball, out bool inside)
        //{
        //    float deltaX = ball.Position.X - (x + width) / 2.0f;
        //    float deltaY = ball.Position.Y - (y + height) / 2.0f;

        //    bool inx = Math.Abs(deltaX) < (width / 2.0f);
        //    bool iny = Math.Abs(deltaY) < (height / 2.0f);
        //    inside  = (inx && iny);

        //    Vector2 h = new Vector2(deltaX, deltaY);
        //    if (!inx || inside)
        //        h.X = width * Math.Sign(deltaX);

        //    if (!iny || inside)
        //        h.Y = height * Math.Sign(deltaY);

        //    return new Vector2((x + width) / 2.0f, (y + height) / 2.0f) + h;
        //}

        //// test collision of a ball [m_position, m_velocity, m_radius] against 
        //// a axis aligned box [centre, halfsize].
        //// --------------------------------------------------
        //// This algo is iterative, and will work in a way like GJK works
        //// 1) let {p} = {m_position}.
        //// 2) find closest point {pcoll} on box to {p}.
        //// 3) if that point within radius distance, then we have a collision
        //// 4) else, compute the plane normal, {normal} = [{p} - {pcoll}].normalised().
        //// 5) find time {t} of collision with the plane [{pcoll}, {normal}].
        //// 6) if {t} > {tmax}, or point {m_position} back-facing the plane, no collision, we missed.
        //// 7) move {p} to time of collision : {p} = {m_position} + {m_velocity} * {t}.
        //// 8) go to 2).
        //public bool Collided(Ball ball, float tcoll, Vector2 pcoll, float tmax)
        //{
        //    // search time
        //    float t = 0.0f;

        //    // search position. 
        //    // we will advance the ball forward
        //    // and refine the collision plane
        //    Vector2 p = ball.Position;

        //    // radius, plus some extra to terminate the collision iteration.
        //    float r2 = (((float)Ball.Radius + 0.01f) * ((float)Ball.Radius + 0.01f));
        	
        //    while (true)
        //    {
        //        // find the closest point on box from our search position.
        //        bool inside;
        //        pcoll = ClosestPointOnRectangle(ball, out inside);
        		
        //        // position inside the box.  
        //        // extreme case when the ball gets inside a brick.
        //        if(inside)
        //        {
        //            tcoll = t;
        //            return true;
        //        }

        //        // check to see if that point can be our 
        //        // final collison point
        //        Vector2 delta = (p - pcoll);
        		
        //        // see if the point is inside our radius (plus extra).
        //        float d2 = Vector2.Dot(delta, delta);
        		
        //        // yup, so we have collision
        //        if(d2 < r2)
        //        {
        //            // we have an intersection.
        //            tcoll = t;
        //            return true;
        //        }

        //        // compute the normal of the plane of collision
        //        Vector2 n = delta;
        //        n /= Vector2.Normalize(d2);

        //        // find time of collision with that plane
        //        float denom = (m_velocity * n);
        //        float numer = ((m_position - pcoll).dotProduct(n));
        		
        //        // condition when the search becomes invalid
        //        // (the ball misses the object).
        //        if(denom > -1.0E-8f || numer < 0.0f) 
        //            return false;
        			
        //        // find time of collision between 
        //        // ball and the new collision plane
        //        t = ((float)Ball.Radius - numer) / denom;
        		
        //        // intersection with that plane if invalid
        //        if(t > tmax || t < 0.0f) 
        //            return false;

        //        // move search position forward.
        //        p = ball.Position + m_velocity * t;
        //    }
        //}

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Vector2 position = new Vector2(this.position.X + offset.X, this.position.Y + offset.Y);
            Color color = new Color(255, 255, 255, 255);
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, (int)size.X, (int)size.Y), color, 0f, new Vector2(0, 0), (1.0f / 1.0f), SpriteEffects.None, 0f);
        }

        #endregion functions
    }

    public enum RectangleCollision
    {
        None,
        Top,
        Bottom,
        Left,
        Right,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public enum ForcedDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
}
