using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Windows;
using SnookerCore;

namespace Snooker.Client.Core
{
    #region Public enums/event args
    /// <summary>
    /// Enumerates the different types of call backs handled by the client app
    /// </summary>
    public enum CallBackType { ReceivePlay, UserEnter, UserLeave, WaitForUserShot };
    
    /// <summary>
    /// Proxy event args
    /// </summary>
    public class ProxyEventArgs : EventArgs
    {
        /// <summary>
        /// Array containing a list of snooker teams
        /// </summary>
        public ContractTeam[] teamList;
    }

    /// <summary>
    /// Proxy callback event args
    /// </summary>
    public class ProxyCallBackEventArgs : EventArgs
    {
        public CallBackType callBackType;
        public ContractTeam team;
        public ContractPerson person;
        public Shot shot;
    }

    #endregion
    #region SnookerServiceAgent class

    public sealed class SnookerServiceAgent : ISnookerCallback
    {
        #region Instance Fields
        private static SnookerServiceAgent singleton = null;
        private static readonly object singletonLock = new object();
        private SnookerClient proxy;
        private ContractPerson myPerson;
        private delegate void HandleDelegate(ContractPerson[] list);
        private delegate void HandleErrorDelegate();
        //main proxy event
        public delegate void ProxyEventHandler(object sender, ProxyEventArgs e);
        public event ProxyEventHandler ProxyEvent;
        //callback proxy event
        public delegate void ProxyCallBackEventHandler(object sender, ProxyCallBackEventArgs e);
        public event ProxyCallBackEventHandler ProxyCallBackEvent;
        #endregion
        #region Ctor
        /// <summary>
        /// Blank constructor
        /// </summary>
        private SnookerServiceAgent() 
        { 

        }
        #endregion
        #region Public Methods
        #region ISnookerCallback implementation


        public void ReceivePlay(ContractTeam team, ContractPerson person, Shot shot)
        {
            ProxyCallBackEventArgs e = new ProxyCallBackEventArgs();
            e.callBackType = CallBackType.ReceivePlay;
            e.team = team;
            e.person = person;
            e.shot = shot;
            OnProxyCallBackEvent(e);
        }

        public void UserEnter(ContractTeam team, ContractPerson person)
        {
            UserEnterLeave(team, person, CallBackType.UserEnter);
        }

        public void UserLeave(ContractTeam team, ContractPerson person)
        {
            UserEnterLeave(team, person, CallBackType.UserLeave);
        }

        private void UserEnterLeave(ContractTeam team, ContractPerson person, CallBackType callbackType)
        {
            ProxyCallBackEventArgs e = new ProxyCallBackEventArgs();
            e.team = team;
            e.person = person;
            e.callBackType = callbackType;
            OnProxyCallBackEvent(e);
        }

        private void WaitForUserShot(ContractTeam team, ContractPerson person, CallBackType callbackType)
        {
            ProxyCallBackEventArgs e = new ProxyCallBackEventArgs();
            e.team = team;
            e.person = person;
            e.callBackType = callbackType;
            OnProxyCallBackEvent(e);
        }

        #endregion
 
        public void Connect(ContractPerson person)
        {
            InstanceContext site = new InstanceContext(this);
            proxy = new SnookerClient(site);
            IAsyncResult iar = proxy.BeginJoin(person, new AsyncCallback(OnEndJoin), null);
        }

        public void Play(ContractTeam team, ContractPerson person, Shot shot)
        {
            proxy.Play(team, person, shot);
        }

        private void OnEndJoin(IAsyncResult iar)
        {
            try
            {
                ContractTeam[] list = proxy.EndJoin(iar);
                HandleEndJoin(list);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void OnEndPlay(IAsyncResult iar)
        {
        }

        private void HandleEndJoin(ContractTeam[] list)
        {

            if (list == null)
            {
                ExitSnookerGame();
            }
            else
            {
                ProxyEventArgs e = new ProxyEventArgs();
                e.teamList = list;
                OnProxyEvent(e);
            }
        }

        protected void OnProxyCallBackEvent(ProxyCallBackEventArgs e)
        {
            if (ProxyCallBackEvent != null)
            {
                ProxyCallBackEvent(this, e);
            }
        }

        protected void OnProxyEvent(ProxyEventArgs e)
        {
            if (ProxyEvent != null)
            {
                ProxyEvent(this, e);
            }
        }

        public static SnookerServiceAgent GetInstance()
        {
            lock (singletonLock)
            {
                if (singleton == null)
                {
                    singleton = new SnookerServiceAgent();
                }
                return singleton;
            }
        }

        public void ExitSnookerGame()
        {
            try
            {
                proxy.Leave();
            }
            catch { }
            finally
            {
                AbortProxy();
            }
        }

        public void AbortProxy()
        {
            if (proxy != null)
            {
                proxy.Abort();
                proxy.Close();
                proxy = null;
            }
        }
        #endregion

        #region ISnookerCallback Members


        public IAsyncResult BeginReceive(ContractPerson sender, string message, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReceive(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public void Receive(SnookerCore.ContractPerson sender, string message)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReceivePlay(ContractTeam team, ContractPerson person, Shot shot, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReceivePlay(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginUserEnter(ContractTeam team, ContractPerson person, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndUserEnter(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginUserLeave(ContractTeam team, ContractPerson person, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndUserLeave(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISnookerCallback Members


        public IAsyncResult BeginReceiveScore(ContractTeam team, ContractPerson person, int wonPoints, int lostPoints, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReceiveScore(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://tempuri.org/ISnooker/ReceiveScore")]
        public void ReceiveScore(SnookerCore.ContractTeam team, SnookerCore.ContractPerson person, int wonPoints, int lostPoints)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISnookerCallback Members


        public void WaitForUserShot(ContractTeam team, ContractPerson person)
        {
            WaitForUserShot(team, person, CallBackType.WaitForUserShot);
        }

        public IAsyncResult BeginWaitForUserShot(ContractTeam team, ContractPerson person, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndWaitForUserShot(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    #endregion
}
