using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snooker.Client.Core.Model
{
    public class TestShot
    {
        int wonPoints = 0;
        int lostPoints = 0;
        Vector2 position = new Vector2(0, 0);
        int strength = 0;

        public TestShot(int wonPoints, int lostPoints, Vector2 position, int strength)
        {
            this.wonPoints = wonPoints;
            this.lostPoints = lostPoints;
            this.position = position;
            this.strength = strength;
        }

        public int WonPoints { get { return wonPoints; } set { wonPoints = value; } }
        public int LostPoints { get { return lostPoints; } set { lostPoints = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public int Strength { get { return strength; } set { strength = value; } }
    }
}
