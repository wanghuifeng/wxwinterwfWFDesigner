using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SnookerCore
{
    /// <summary>
    /// Represents a single game snapshot (i.e. the positions of each ball in a specific instant).
    /// </summary>
    [Serializable]
    public class Snapshot
    {
        #region private members
        int snapshotNumber = 0;
        List<BallPosition> ballPositionList = new List<BallPosition>();
        GameSound sound = GameSound.None;
        #endregion

        #region constructors
        public Snapshot() { }

        public Snapshot(int snapshotNumber, List<BallPosition> ballPositionList, GameSound sound)
        {
            this.snapshotNumber = snapshotNumber;
            this.ballPositionList = ballPositionList;
            this.sound = sound;
        }
        #endregion

        #region public members
        
        
        public int SnapshotNumber { get { return snapshotNumber; } set { snapshotNumber = value; } }

        
        public List<BallPosition> BallPositionList { get { return ballPositionList; } set { ballPositionList = value; } }

        
        public GameSound Sound { get { return sound; } set { sound = value; } }
        #endregion
    }
}
