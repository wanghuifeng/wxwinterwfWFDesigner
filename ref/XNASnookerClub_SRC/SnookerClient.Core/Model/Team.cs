using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Drawing;
using System.Text;
using SnookerCore;

namespace Snooker.Client.Core.Model
{
    public class Team
    {
        #region attributes
        int id = 0;
        Ball ballOn = null;
        int points = 0;
        List<int> foulList = new List<int>();
        int strength = 50;
        bool justSwapped = true;
        int shotCount = 0;
        List<Player> players = new List<Player>();
        int currentPlayerIndex = 0;
        Vector2 testPosition = new Vector2(0, 0);
        int testStrenght = 0;

        int attempts = 0;
        int attemptsToWin = 0;
        int attemptsNotToLose = 0;
        int attemptsOfDespair = 0;
        bool bestShotSelected = false;
        public TestShot bestShot = new TestShot(0, 1000, new Vector2(0, 0), 0);
        public TestShot lastShot = new TestShot(0, 1000, new Vector2(0, 0), 0);
        bool isRotatingCue = false;
        float currentCueAngle;
        float finalCueAngle;

        #endregion attributes

        #region constructor
        public Team() { }

        public Team(int id)
        {
            this.id = id;
        }
        #endregion constructor

        #region properties
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public Ball BallOn
        {
            get { return ballOn; }
            set { ballOn = value;

            if (value == null)
                value = value;
            }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public List<int> FoulList
        {
            get { return foulList; }
            set { foulList = value; }
        }

        public int Strength
        {
            get { return strength; }
            set { 
                strength = value; 
            }
        }

        public bool JustSwapped
        {
            get { return justSwapped; }
            set { justSwapped = value; }
        }

        public int ShotCount
        {
            get { return shotCount; }
            set { shotCount = value; }
        }

        public List<Player> Players { get { return players; } set { players = value; } }

        public Player CurrentPlayer
        { 
            get { 

                if (currentPlayerIndex > players.Count - 1)
                    return null; 
                else
                    return players[currentPlayerIndex]; 
            } 
            set { 
                players[currentPlayerIndex] = value; 
            }
        }

        public void NextPlayer()
        {
            currentPlayerIndex++;

            if (currentPlayerIndex >= players.Count)
                currentPlayerIndex = 0;
        }

        public Vector2 TestPosition
        {
            get { return testPosition; }
            set { testPosition = value; }
        }

        public int TestStrength
        {
            get { return testStrenght; }
            set { testStrenght = value; }
        }

        public int Attempts { get { return attempts; } set { attempts = value; } }
        public int AttemptsToWin { get { return attemptsToWin; } set { attemptsToWin = value; } }
        public int AttemptsNotToLose { get { return attemptsNotToLose; } set { attemptsNotToLose = value; } }
        public int AttemptsOfDespair { get { return attemptsOfDespair; } set { attemptsOfDespair = value; } }
        public bool BestShotSelected { get { return bestShotSelected; } set { bestShotSelected = value; } }
        public TestShot BestShot { get { return bestShot; } set { bestShot = value; } }
        public TestShot LastShot { get { return lastShot; } set { lastShot = value; } }
        public bool IsRotatingCue { get { return isRotatingCue; } set { isRotatingCue = value; } }
        public float CurrentCueAngle { get { return currentCueAngle; } set { currentCueAngle = value; } }
        public float FinalCueAngle { get { return finalCueAngle; } set { finalCueAngle = value; } }

        #endregion properties
    }
}
