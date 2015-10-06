using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SnookerCore
{
    /// <summary>
    /// Represents a ball position within a snapshot. It also determines whether the ball is potted.
    /// </summary>
    [Serializable]
    public class BallPosition
    {
        #region private members
        int snapshotNumber = 0;
        int ballIndex = 0;
        int x = 0;
        int y = 0;
        bool isBallInPocket = false;
        #endregion private members

        #region constructors
        public BallPosition() { }

        public BallPosition(int snapshotNumber, int ballIndex, int x, int y)
        {
            this.snapshotNumber = snapshotNumber;
            this.ballIndex = ballIndex;
            this.x = x;
            this.y = y;
        }
        #endregion constructors

        #region public members
        
        public int SnapshotNumber { get { return snapshotNumber; } set { snapshotNumber = value; } }
        
        public int BallIndex { get { return ballIndex; } set { ballIndex = value; } }
        
        public int X { get { return x; } set { x = value; } }
        
        public int Y { get { return y; } set { y = value; } }
        
        public bool IsBallInPocket { get { return isBallInPocket; } set { isBallInPocket = value; } }
        #endregion public members
    }
}
