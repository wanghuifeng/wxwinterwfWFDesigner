using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SnookerCore
{
    /// <summary>
    /// Represents the data for a whole snooker shot information, such as ball movements, sounds, score and so on, that can be
    /// broadcasted so that the other players can see each other's actions.
    /// This class is one of the Data Contracts (hence DataContract attribute) 
    /// exposed by the service that can be understood by the client.
    /// </summary>

    [DataContract]
    public class Shot
    {
        #region private members
        int teamId = 0;
        List<Snapshot> snapshotList = new List<Snapshot>();
        int currentTeamScore = 0;
        int otherTeamScore = 0;
        bool hasFinishedTurn = false;
        bool gameOver = false;
        #endregion

        #region constructors
        public Shot() { }

        public Shot(int teamId, List<Snapshot> snapshotList, int currentTeamScore, int otherTeamScore, bool hasFinishedTurn, bool gameOver)
        {
            this.teamId = teamId;
            this.snapshotList = snapshotList;
            this.currentTeamScore = currentTeamScore;
            this.otherTeamScore = otherTeamScore;
            this.hasFinishedTurn = hasFinishedTurn;
            this.gameOver = gameOver;
        }
        #endregion

        #region public members

        [DataMember]
        public int TeamId { get { return teamId; } set { teamId = value; } }

        [DataMember]
        public List<Snapshot> SnapshotList { get { return snapshotList; } set { snapshotList = value; } }

        [DataMember]
        public int CurrentTeamScore { get { return currentTeamScore; } set { currentTeamScore = value; } }

        [DataMember]
        public int OtherTeamScore { get { return otherTeamScore; } set { otherTeamScore = value; } }

        [DataMember]
        public bool HasFinishedTurn { get { return hasFinishedTurn; } set { hasFinishedTurn = value; } }

        [DataMember]
        public bool GameOver { get { return gameOver; } set { gameOver = value; } }
        #endregion
    }
}
