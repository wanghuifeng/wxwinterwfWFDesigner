using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SnookerCore
{
    #region ContractPerson class
    /// <summary>
    /// Represents a snooker player participating in one of the teams of the game.
    /// This class is one of the Data Contracts (hence DataContract attribute) 
    /// exposed by the service that can be understood by the client.
    /// </summary>
    [DataContract]
    public class ContractPerson : INotifyPropertyChanged
    {
        #region private members
        private int index;
        private byte[] imageByteArray;
        private string name;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion private members

        #region constructors

        public ContractPerson()
        {
        }
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="imageURL">Image url for this person</param>
        /// <param name="name">The player's name</param>
        public ContractPerson(byte[] imageByteArray, string name)
        {
            this.imageByteArray = imageByteArray;
            this.name = name;
        }
        #endregion constructors

        #region public members

        [DataMember]
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Index");
            }
        }

        /// <summary>
        /// The player's image byte array
        /// </summary>
        [DataMember]
        public byte[] ImageByteArray
        {
            get { return imageByteArray; }
            set
            {
                imageByteArray = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("imageByteArray");
            }
        }

        /// <summary>
        /// The player Name
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Name");
            }
        }
        #endregion

        #region OnPropertyChanged
        /// <summary>
        /// Notifies the parent bindings (if any) that a property
        /// value changed and that the binding needs updating
        /// </summary>
        /// <param name="propValue">The property which changed</param>
        protected void OnPropertyChanged(string propValue)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propValue));
            }
        }
        #endregion
    }
    #endregion
}
