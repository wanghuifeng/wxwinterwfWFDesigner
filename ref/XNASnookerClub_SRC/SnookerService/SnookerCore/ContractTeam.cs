using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SnookerCore
{
    /// <summary>
    /// Represents a single snooker team participating in the game.
    /// This class is one of the Data Contracts (hence DataContract attribute) 
    /// exposed by the service that can be understood by the client.
    /// </summary>
    [DataContract]
    public class ContractTeam
    {
        #region private members
        int id = 0;
        int ballOnIndex = -1;
        int points = 0;
        List<int> foulList = new List<int>();
        int strength = 0;
        bool justSwapped = true;
        int shotCount = 0;
        List<ContractPerson> players = new List<ContractPerson>();
        int currentPlayerIndex = 0;
        #endregion

        #region constructor
        public ContractTeam() { }

        public ContractTeam(int id)
        {
            this.id = id;
        }
        #endregion constructor

        #region properties
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [DataMember]
        public int BallOnIndex
        {
            get { return ballOnIndex; }
            set { ballOnIndex = value; }
        }
        [DataMember]
        public int Points
        {
            get { return points; }
            set { points = value; }
        }
        [DataMember]
        public List<int> FoulList
        {
            get { return foulList; }
            set { foulList = value; }
        }
        [DataMember]
        public int Strength
        {
            get { return strength; }
            set { 
                strength = value; 
            }
        }
        [DataMember]
        public bool JustSwapped
        {
            get { return justSwapped; }
            set { justSwapped = value; }
        }
        [DataMember]
        public int ShotCount
        {
            get { return shotCount; }
            set { shotCount = value; }
        }
        [DataMember]
        public List<ContractPerson> Players { get { return players; } set { players = value; } }
        
        public ContractPerson CurrentPlayer { get { return players[currentPlayerIndex]; } set { players[currentPlayerIndex] = value; } }

        public void NextPlayer()
        {
            currentPlayerIndex++;

            if (currentPlayerIndex >= players.Count)
                currentPlayerIndex = 0;
        }

        #endregion properties
    }
}
