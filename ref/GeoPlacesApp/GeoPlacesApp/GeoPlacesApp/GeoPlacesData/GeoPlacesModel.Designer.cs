//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3031
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Data.Objects.DataClasses.EdmSchemaAttribute()]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("GeoPlacesModel", "FK_Places_Users", "Users", global::System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(GeoPlacesModel.Users), "Places", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(GeoPlacesModel.Places))]

// Original file name:
// Generation date: 08/03/2009 17:09:54
namespace GeoPlacesModel
{
    
    /// <summary>
    /// There are no comments for GeoPlacesEntities in the schema.
    /// </summary>
    public partial class GeoPlacesEntities : global::System.Data.Objects.ObjectContext
    {
        /// <summary>
        /// Initializes a new GeoPlacesEntities object using the connection string found in the 'GeoPlacesEntities' section of the application configuration file.
        /// </summary>
        public GeoPlacesEntities() : 
                base("name=GeoPlacesEntities", "GeoPlacesEntities")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Initialize a new GeoPlacesEntities object.
        /// </summary>
        public GeoPlacesEntities(string connectionString) : 
                base(connectionString, "GeoPlacesEntities")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Initialize a new GeoPlacesEntities object.
        /// </summary>
        public GeoPlacesEntities(global::System.Data.EntityClient.EntityConnection connection) : 
                base(connection, "GeoPlacesEntities")
        {
            this.OnContextCreated();
        }
        partial void OnContextCreated();
        /// <summary>
        /// There are no comments for Places in the schema.
        /// </summary>
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public global::System.Data.Objects.ObjectQuery<Places> Places
        {
            get
            {
                if ((this._Places == null))
                {
                    this._Places = base.CreateQuery<Places>("[Places]");
                }
                return this._Places;
            }
        }
        private global::System.Data.Objects.ObjectQuery<Places> _Places;
        /// <summary>
        /// There are no comments for Users in the schema.
        /// </summary>
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public global::System.Data.Objects.ObjectQuery<Users> Users
        {
            get
            {
                if ((this._Users == null))
                {
                    this._Users = base.CreateQuery<Users>("[Users]");
                }
                return this._Users;
            }
        }
        private global::System.Data.Objects.ObjectQuery<Users> _Users;
        /// <summary>
        /// There are no comments for Places in the schema.
        /// </summary>
        public void AddToPlaces(Places places)
        {
            base.AddObject("Places", places);
        }
        /// <summary>
        /// There are no comments for Users in the schema.
        /// </summary>
        public void AddToUsers(Users users)
        {
            base.AddObject("Users", users);
        }
    }
    /// <summary>
    /// There are no comments for GeoPlacesModel.Places in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(
        NamespaceName="GeoPlacesModel", Name="Places")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    public partial class Places : global::System.Data.Objects.DataClasses.EntityObject
    {
        /// <summary>
        /// Create a new Places object.
        /// </summary>
        /// <param name="ID">Initial value of ID.</param>
        /// <param name="name">Initial value of Name.</param>
        /// <param name="description">Initial value of Description.</param>
        /// <param name="longitude">Initial value of Longitude.</param>
        /// <param name="latitude">Initial value of Latitude.</param>
        public static Places CreatePlaces(int ID, string name, 
            string description, double longitude, double latitude)
        {
            Places places = new Places();
            places.ID = ID;
            places.Name = name;
            places.Description = description;
            places.Longitude = longitude;
            places.Latitude = latitude;
            return places;
        }
        /// <summary>
        /// There are no comments for Property ID in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(
            EntityKeyProperty=true, IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public int ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this.OnIDChanging(value);
                this.ReportPropertyChanging("ID");
                this._ID = global::System.Data.Objects.DataClasses.
                    StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("ID");
                this.OnIDChanged();
            }
        }
        private int _ID;
        partial void OnIDChanging(int value);
        partial void OnIDChanged();
        /// <summary>
        /// There are no comments for Property Name in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this.ReportPropertyChanging("Name");
                this._Name = global::System.Data.Objects.DataClasses.
                    StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Name");
                this.OnNameChanged();
            }
        }
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// There are no comments for Property Description in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this.OnDescriptionChanging(value);
                this.ReportPropertyChanging("Description");
                this._Description = global::System.Data.Objects.DataClasses.
                    StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Description");
                this.OnDescriptionChanged();
            }
        }
        private string _Description;
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        /// <summary>
        /// There are no comments for Property Longitude in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.
            EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public double Longitude
        {
            get
            {
                return this._Longitude;
            }
            set
            {
                this.OnLongitudeChanging(value);
                this.ReportPropertyChanging("Longitude");
                this._Longitude = global::System.Data.Objects.DataClasses.
                    StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Longitude");
                this.OnLongitudeChanged();
            }
        }
        private double _Longitude;
        partial void OnLongitudeChanging(double value);
        partial void OnLongitudeChanged();
        /// <summary>
        /// There are no comments for Property Latitude in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public double Latitude
        {
            get
            {
                return this._Latitude;
            }
            set
            {
                this.OnLatitudeChanging(value);
                this.ReportPropertyChanging("Latitude");
                this._Latitude = global::System.Data.Objects.DataClasses.
                    StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Latitude");
                this.OnLatitudeChanged();
            }
        }
        private double _Latitude;
        partial void OnLatitudeChanging(double value);
        partial void OnLatitudeChanged();
        /// <summary>
        /// There are no comments for Users in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmRelationshipNavigationPropertyAttribute(
            "GeoPlacesModel", "FK_Places_Users", "Users")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public Users Users
        {
            get
            {
                return ((global::System.Data.Objects.DataClasses.IEntityWithRelationships)(this)).
                    RelationshipManager.GetRelatedReference<Users>(
                    "GeoPlacesModel.FK_Places_Users", "Users").Value;
            }
            set
            {
                ((global::System.Data.Objects.DataClasses.IEntityWithRelationships)(this)).
                    RelationshipManager.GetRelatedReference<Users>(
                    "GeoPlacesModel.FK_Places_Users", "Users").Value = value;
            }
        }
        /// <summary>
        /// There are no comments for Users in the schema.
        /// </summary>
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public global::System.Data.Objects.DataClasses.EntityReference<Users> UsersReference
        {
            get
            {
                return ((global::System.Data.Objects.DataClasses.IEntityWithRelationships)
                    (this)).RelationshipManager.GetRelatedReference<Users>(
                    "GeoPlacesModel.FK_Places_Users", "Users");
            }
            set
            {
                if ((value != null))
                {
                    ((global::System.Data.Objects.DataClasses.IEntityWithRelationships)
                        (this)).RelationshipManager.InitializeRelatedReference<Users>(
                        "GeoPlacesModel.FK_Places_Users", "Users", value);
                }
            }
        }
    }
    /// <summary>
    /// There are no comments for GeoPlacesModel.Users in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="GeoPlacesModel", Name="Users")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    public partial class Users : global::System.Data.Objects.DataClasses.EntityObject
    {
        /// <summary>
        /// Create a new Users object.
        /// </summary>
        /// <param name="ID">Initial value of ID.</param>
        /// <param name="name">Initial value of Name.</param>
        /// <param name="password">Initial value of Password.</param>
        public static Users CreateUsers(int ID, string name, string password)
        {
            Users users = new Users();
            users.ID = ID;
            users.Name = name;
            users.Password = password;
            return users;
        }
        /// <summary>
        /// There are no comments for Property ID in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public int ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this.OnIDChanging(value);
                this.ReportPropertyChanging("ID");
                this._ID = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("ID");
                this.OnIDChanged();
            }
        }
        private int _ID;
        partial void OnIDChanging(int value);
        partial void OnIDChanged();
        /// <summary>
        /// There are no comments for Property Name in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this.ReportPropertyChanging("Name");
                this._Name = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Name");
                this.OnNameChanged();
            }
        }
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// There are no comments for Property Password in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                this.OnPasswordChanging(value);
                this.ReportPropertyChanging("Password");
                this._Password = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Password");
                this.OnPasswordChanged();
            }
        }
        private string _Password;
        partial void OnPasswordChanging(string value);
        partial void OnPasswordChanged();
        /// <summary>
        /// There are no comments for Places in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmRelationshipNavigationPropertyAttribute("GeoPlacesModel", "FK_Places_Users", "Places")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public global::System.Data.Objects.DataClasses.EntityCollection<Places> Places
        {
            get
            {
                return ((global::System.Data.Objects.DataClasses.IEntityWithRelationships)(this)).RelationshipManager.GetRelatedCollection<Places>("GeoPlacesModel.FK_Places_Users", "Places");
            }
            set
            {
                if ((value != null))
                {
                    ((global::System.Data.Objects.DataClasses.IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedCollection<Places>("GeoPlacesModel.FK_Places_Users", "Places", value);
                }
            }
        }
    }
}