using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using SnookerCore;

namespace SnookerService
{
    #region ISnooker interface
    /// <summary>
    /// This interface provides the methods that may be used in order to clients
    /// to carry out specific actions in the snoooker game. This interface
    /// expects the clients that implement this interface to be able also support
    /// a callback of type <see cref="ISnookerCallback">ISnookerCallback</see>
    /// 
    /// There are methods for
    /// 
    /// Say : send a globally broadcasted message
    /// Play : send a list of ball positions
    /// Join : join the game
    /// Leave : leave the game
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ISnookerCallback))]
    interface ISnooker
    {
        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void Play(ContractTeam team, ContractPerson name, Shot shot);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        ContractTeam[] Join(ContractPerson name);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = true)]
        void Leave();
    }
    #endregion

    #region ISnookerCallback interface
    /// <summary>
    /// This interface provides 4 methods that may be used in order to carry 
    /// out a callback to the client. The methods are 1 way (back to the client).
    /// 
    /// There are methods for 
    /// 
    /// Receive : receive a globally broadcasted message
    /// ReceivePlay : receive a list of ball positions
    /// UserEnter : recieve notification a new user has entered the game
    /// UserLeave : recieve notification a existing user has left the game
    /// </summary>
    interface ISnookerCallback
    {
        [OperationContract(IsOneWay = true)]
        void Receive(ContractPerson sender, string message);

        [OperationContract(IsOneWay = true)]
        void ReceivePlay(ContractTeam team, ContractPerson person, Shot shot);

        [OperationContract(IsOneWay = true)]
        void UserEnter(ContractTeam team, ContractPerson person);

        [OperationContract(IsOneWay = true)]
        void UserLeave(ContractTeam team, ContractPerson person);

        [OperationContract(IsOneWay = true)]
        void WaitForUserShot(ContractTeam team, ContractPerson person);
    }
    #endregion

    #region Public enums/event args
    /// <summary>
    /// A simple enumeration for dealing with the snooker message types
    /// </summary>
    public enum MessageType { Receive, UserEnter, UserLeave, ReceivePlay, WaitForUserShot};

    /// <summary>
    /// This class is used when carrying out any of the 4 snooker callback actions
    /// such as Receive, ReceivePlay, UserEnter, UserLeave <see cref="ISnookerCallback">
    /// ISnookerCallback</see> for more details
    /// </summary>
    public class SnookerEventArgs : EventArgs
    {
        public MessageType msgType;
        public ContractTeam team;
        public ContractPerson person;
        public string message;
        public Shot shot;
    }
    #endregion

    #region SnookerService
    /// <summary>
    /// This class provides the service that is used by all clients. This class
    /// uses the bindings as specified in the App.Config
    /// 
    /// This class also implements the <see cref="ISnooker">ISnooker</see> interface in order
    /// to facilitate a common snooker interface for all snooker clients
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SnookerService : ISnooker
    {
        #region Instance fields
        //thread sync lock object
        private static Object syncObj = new Object();
        //callback interface for clients
        ISnookerCallback callback = null;
        //delegate used for BroadcastEvent
        public delegate void SnookerEventHandler(object sender, SnookerEventArgs e);
        public static event SnookerEventHandler SnookerEvent;
        private SnookerEventHandler myEventHandler = null;
        //holds a list of players, and a delegate to allow the BroadcastEvent to work
        //out which player delegate to invoke
        static Dictionary<ContractPerson, SnookerEventHandler> players = new Dictionary<ContractPerson, SnookerEventHandler>();
        static ContractTeam team1 = new ContractTeam(1);
        static ContractTeam team2 = new ContractTeam(2);
        static Dictionary<ContractTeam, SnookerEventHandler> teams = new Dictionary<ContractTeam, SnookerEventHandler>();
        //current person 
        private ContractPerson person;
        private ContractTeam team;
        #endregion

        #region constructor
        public SnookerService()
        {
            //create a new SnookerEventHandler delegate, pointing to the MyEventHandler() method
            myEventHandler = new SnookerEventHandler(MyEventHandler);
            if (teams.Count == 0)
            {
                teams.Add(team1, myEventHandler);
                teams.Add(team2, myEventHandler);
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Searches the intenal list of players for a particular person, and returns
        /// true if the person could be found
        /// </summary>
        /// <param name="name">the name of the <see cref="Common.Person">Person</see> to find</param>
        /// <returns>True if the <see cref="Common.Person">Person</see> was found in the
        /// internal list of players</returns>
        private bool checkIfPersonExists(string name)
        {
            foreach (ContractPerson p in players.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Searches the intenal list of players for a particular person, and returns
        /// the individual players SnookerEventHandler delegate in order that it can be
        /// invoked
        /// </summary>
        /// <param name="name">the name of the <see cref="Common.Person">Person</see> to find</param>
        /// <returns>The True SnookerEventHandler delegate for the <see cref="Common.Person">Person</see> who matched
        /// the name input parameter</returns>
        private SnookerEventHandler getPersonHandler(string name)
        {
            foreach (ContractPerson p in players.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    SnookerEventHandler playWith = null;
                    players.TryGetValue(p, out playWith);
                    return playWith;
                }
            }
            return null;
        }

        /// <summary>
        /// Searches the intenal list of players for a particular person, and returns
        /// the actual <see cref="Common.Person">Person</see> whos name matches
        /// the name input parameter
        /// </summary>
        /// <param name="name">the name of the <see cref="Common.Person">Person</see> to find</param>
        /// <returns>The actual <see cref="Common.Person">Person</see> whos name matches
        /// the name input parameter</returns>
        private ContractPerson getPerson(string name)
        {
            foreach (ContractPerson p in players.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return p;
                }
            }
            return null;
        }
        #endregion

        #region ISnooker implementation

        /// <summary>
        /// Takes a <see cref="Common.Person">Person</see> and allows them
        /// to join the snooker game, if there is not already a player with
        /// the same name
        /// </summary>
        /// <param name="person"><see cref="Common.Person">Person</see> joining</param>
        /// <returns>An array of <see cref="Common.Person">Person</see> objects</returns>
        public ContractTeam[] Join(ContractPerson person)
        {
            bool personAddedToPlayers = false;

            //create a new SnookerEventHandler delegate, pointing to the MyEventHandler() method
            myEventHandler = new SnookerEventHandler(MyEventHandler);

            //carry out a critical section that checks to see if the new player
            //name is already in use, if its not allow the new player to be
            //added to the list of players, using the person as the key, and the
            //SnookerEventHandler delegate as the value, for later invocation
            lock (syncObj)
            {
                if (!checkIfPersonExists(person.Name) && person != null)
                {
                    if (team1.Players.Count < 2 || team2.Players.Count < 2)
                    {
                        this.person = person;
                        players.Add(person, MyEventHandler);

                        if (team1.Players.Count == team2.Players.Count)
                        {
                            team = team1;
                            team.Players.Add(person);
                        }
                        else
                        {
                            team = team2;
                            team.Players.Add(person);
                        }
                        Console.WriteLine(string.Format("Player {0} joined Team {1}.", person.Name, team.Id));

                        personAddedToPlayers = true;
                    }                    
                }
            }

            //if the new player could be successfully added, get a callback instance
            //create a new message, and broadcast it to all other players, and then 
            //return the list of al players such that connected clients may show a
            //list of all the players
            if (personAddedToPlayers)
            {
                callback = OperationContext.Current.GetCallbackChannel<ISnookerCallback>();
                SnookerEventArgs e = new SnookerEventArgs();
                e.msgType = MessageType.UserEnter;
                e.team = team;
                e.person = this.person;
                BroadcastMessage(e);

                if (team1.Players.Count == 1 && team2.Players.Count == 1)
                {
                    e = new SnookerEventArgs();
                    e.msgType = MessageType.WaitForUserShot;
                    e.team = team1;
                    e.person = team1.Players[0];
                    BroadcastMessage(e);

                    Console.WriteLine(string.Format("Teams information has been broadcasted."));
                }

                //add this newly joined players SnookerEventHandler delegate, to the global
                //multicast delegate for invocation
                SnookerEvent += myEventHandler;

                ContractPerson[] playersList = new ContractPerson[players.Count];
                ContractTeam[] teamsList = new ContractTeam[teams.Count];
                //carry out a critical section that copy all players to a new list
                lock (syncObj)
                {
                    players.Keys.CopyTo(playersList, 0);
                    teams.Keys.CopyTo(teamsList, 0);
                }
                return teamsList;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Broadcasts the input msg parameter to all connected players
        /// by using the BroadcastMessage() method
        /// </summary>
        /// <param name="msg">The message to broadcast to all players</param>
        public void Say(string msg)
        {
            SnookerEventArgs e = new SnookerEventArgs();
            e.msgType = MessageType.Receive;
            e.person = this.person;
            e.message = msg;
            BroadcastMessage(e);
        }

        /// <summary>
        /// Broadcasts the list of ball positions to all the <see cref="Common.Person">
        /// Person</see> whos name matches the to input parameter
        /// by looking up the person from the internal list of players
        /// and invoking their SnookerEventHandler delegate asynchronously.
        /// Where the MyEventHandler() method is called at the start of the
        /// asynch call, and the EndAsync() method at the end of the asynch call
        /// </summary>
        /// <param name="to">The persons name to send the message to</param>
        /// <param name="msg">The message to broadcast to all players</param>
        public void Play(ContractTeam team, ContractPerson person, Shot shot)
        {
            Console.WriteLine(string.Format("Receiving shot information from team {0}, player {1}, with {2} snapshots.", team.Id, person.Name, shot.SnapshotList.Count));
            SnookerEventArgs e = new SnookerEventArgs();
            e.msgType = MessageType.ReceivePlay;
            e.team = team;
            e.person = person;
            e.shot = shot;
            BroadcastMessage(e);
            Console.WriteLine(string.Format("Shot information has been broadcasted"));
        }

        /// <summary>
        /// A request has been made by a client to leave the snooker game,
        /// so remove the <see cref="Common.Person">Person </see>from
        /// the internal list of players, and unwire the players
        /// delegate from the multicast delegate, so that it no longer
        /// gets invokes by globally broadcasted methods
        /// </summary>
        public void Leave()
        {
            if (this.person == null)
                return;

            //get the players SnookerEventHandler delegate
            SnookerEventHandler playerToRemove = getPersonHandler(this.person.Name);

            Console.WriteLine(string.Format("Player {0} from team {1} has just left the game", person.Name, team.Id));

            //carry out a critical section, that removes the player from the
            //internal list of players
            lock (syncObj)
            {
                players.Remove(this.person);
            }

            //unwire the players delegate from the multicast delegate, so that 
            //it no longer gets invokes by globally broadcasted methods
            SnookerEvent -= playerToRemove;
            SnookerEventArgs e = new SnookerEventArgs();
            e.msgType = MessageType.UserLeave;
            e.team = this.team;
            e.person = this.person;
            this.person = null;
            //broadcast this leave message to all other remaining connected
            //players
            BroadcastMessage(e);
            Console.WriteLine(string.Format("User Leave information has been broadcasted"));

            teams.Clear();
            players.Clear();
        }
        #endregion

        #region private methods

        /// <summary>
        /// This method is called when ever one of the players
        /// SnookerEventHandler delegates is invoked. When this method
        /// is called it will examine the events SnookerEventArgs to see
        /// what type of message is being broadcast, and will then
        /// call the correspoding method on the clients callback interface
        /// </summary>
        /// <param name="sender">the sender, which is not used</param>
        /// <param name="e">The SnookerEventArgs</param>
        private void MyEventHandler(object sender, SnookerEventArgs e)
        {
            try
            {
                switch (e.msgType)
                {
                    case MessageType.Receive:
                        callback.Receive(e.person, e.message);
                        break;
                    case MessageType.ReceivePlay:
                        callback.ReceivePlay(e.team, e.person, e.shot);
                        break;
                    case MessageType.UserEnter:
                        callback.UserEnter(e.team, e.person);
                        break;
                    case MessageType.UserLeave:
                        callback.UserLeave(e.team, e.person);
                        break;
                    case MessageType.WaitForUserShot:
                        callback.WaitForUserShot(e.team, e.person);
                        break;
                }
            }
            catch (Exception ex)
            {
                Leave();
            }
        }

        /// <summary>
        ///loop through all connected players and invoke their 
        ///SnookerEventHandler delegate asynchronously, which will firstly call
        ///the MyEventHandler() method and will allow a asynch callback to call
        ///the EndAsync() method on completion of the initial call
        /// </summary>
        /// <param name="e">The SnookerEventArgs to use to send to all connected players</param>
        private void BroadcastMessage(SnookerEventArgs e)
        {

            SnookerEventHandler temp = SnookerEvent;

            //loop through all connected players and invoke their 
            //SnookerEventHandler delegate asynchronously, which will firstly call
            //the MyEventHandler() method and will allow a asynch callback to call
            //the EndAsync() method on completion of the initial call
            if (temp != null)
            {
                foreach (SnookerEventHandler handler in temp.GetInvocationList())
                {
                    handler.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);
                }
            }
        }


        /// <summary>
        /// Is called as a callback from the asynchronous call, so simply get the
        /// delegate and do an EndInvoke on it, to signal the asynchronous call is
        /// now completed
        /// </summary>
        /// <param name="ar">The asnch result</param>
        private void EndAsync(IAsyncResult ar)
        {
            SnookerEventHandler d = null;

            try
            {
                //get the standard System.Runtime.Remoting.Messaging.AsyncResult,and then
                //cast it to the correct delegate type, and do an end invoke
                System.Runtime.Remoting.Messaging.AsyncResult asres = (System.Runtime.Remoting.Messaging.AsyncResult)ar;
                d = ((SnookerEventHandler)asres.AsyncDelegate);
                d.EndInvoke(ar);
            }
            catch
            {
                SnookerEvent -= d;
            }
        }
        #endregion
    }
    #endregion
}

