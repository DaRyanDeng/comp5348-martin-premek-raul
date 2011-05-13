//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace VideoStore.Business.Entities
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(Order))]
    [KnownType(typeof(LoginCredential))]
    [KnownType(typeof(Role))]
    public partial class User: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'Id' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        private int _id;
    
        [DataMember]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private string _name;
    
        [DataMember]
        public string Address
        {
            get { return _address; }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged("Address");
                }
            }
        }
        private string _address;
    
        [DataMember]
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged("Email");
                }
            }
        }
        private string _email;
    
        [DataMember]
        public byte[] Revision
        {
            get { return _revision; }
            set
            {
                if (_revision != value)
                {
                    ChangeTracker.RecordOriginalValue("Revision", _revision);
                    _revision = value;
                    OnPropertyChanged("Revision");
                }
            }
        }
        private byte[] _revision;
    
        [DataMember]
        public int BankAccountNumber
        {
            get { return _bankAccountNumber; }
            set
            {
                if (_bankAccountNumber != value)
                {
                    _bankAccountNumber = value;
                    OnPropertyChanged("BankAccountNumber");
                }
            }
        }
        private int _bankAccountNumber;

        #endregion
        #region Navigation Properties
    
        [DataMember]
        public TrackableCollection<Order> Orders
        {
            get
            {
                if (_orders == null)
                {
                    _orders = new TrackableCollection<Order>();
                    _orders.CollectionChanged += FixupOrders;
                }
                return _orders;
            }
            set
            {
                if (!ReferenceEquals(_orders, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_orders != null)
                    {
                        _orders.CollectionChanged -= FixupOrders;
                    }
                    _orders = value;
                    if (_orders != null)
                    {
                        _orders.CollectionChanged += FixupOrders;
                    }
                    OnNavigationPropertyChanged("Orders");
                }
            }
        }
        private TrackableCollection<Order> _orders;
    
        [DataMember]
        public LoginCredential LoginCredential
        {
            get { return _loginCredential; }
            set
            {
                if (!ReferenceEquals(_loginCredential, value))
                {
                    var previousValue = _loginCredential;
                    _loginCredential = value;
                    FixupLoginCredential(previousValue);
                    OnNavigationPropertyChanged("LoginCredential");
                }
            }
        }
        private LoginCredential _loginCredential;
    
        [DataMember]
        public TrackableCollection<Role> Roles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new TrackableCollection<Role>();
                    _roles.CollectionChanged += FixupRoles;
                }
                return _roles;
            }
            set
            {
                if (!ReferenceEquals(_roles, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_roles != null)
                    {
                        _roles.CollectionChanged -= FixupRoles;
                    }
                    _roles = value;
                    if (_roles != null)
                    {
                        _roles.CollectionChanged += FixupRoles;
                    }
                    OnNavigationPropertyChanged("Roles");
                }
            }
        }
        private TrackableCollection<Role> _roles;

        #endregion
        #region ChangeTracking
    
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
            {
                ChangeTracker.State = ObjectState.Modified;
            }
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        protected virtual void OnNavigationPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged{ add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
        private event PropertyChangedEventHandler _propertyChanged;
        private ObjectChangeTracker _changeTracker;
    
        [DataMember]
        public ObjectChangeTracker ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new ObjectChangeTracker();
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
                return _changeTracker;
            }
            set
            {
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
                }
                _changeTracker = value;
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
            }
        }
    
        private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                ClearNavigationProperties();
            }
        }
    
        protected bool IsDeserializing { get; private set; }
    
        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }
    
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            IsDeserializing = false;
            ChangeTracker.ChangeTrackingEnabled = true;
        }
    
        protected virtual void ClearNavigationProperties()
        {
            Orders.Clear();
            LoginCredential = null;
            FixupLoginCredentialKeys();
            Roles.Clear();
        }

        #endregion
        #region Association Fixup
    
        private void FixupLoginCredential(LoginCredential previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("LoginCredential")
                    && (ChangeTracker.OriginalValues["LoginCredential"] == LoginCredential))
                {
                    ChangeTracker.OriginalValues.Remove("LoginCredential");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("LoginCredential", previousValue);
                }
                if (LoginCredential != null && !LoginCredential.ChangeTracker.ChangeTrackingEnabled)
                {
                    LoginCredential.StartTracking();
                }
                FixupLoginCredentialKeys();
            }
        }
    
        private void FixupLoginCredentialKeys()
        {
            const string IdKeyName = "LoginCredential.Id";
    
            if(ChangeTracker.ExtendedProperties.ContainsKey(IdKeyName))
            {
                if(LoginCredential == null ||
                   !Equals(ChangeTracker.ExtendedProperties[IdKeyName], LoginCredential.Id))
                {
                    ChangeTracker.RecordOriginalValue(IdKeyName, ChangeTracker.ExtendedProperties[IdKeyName]);
                }
                ChangeTracker.ExtendedProperties.Remove(IdKeyName);
            }
        }
    
        private void FixupOrders(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (Order item in e.NewItems)
                {
                    item.Customer = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("Orders", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Order item in e.OldItems)
                {
                    if (ReferenceEquals(item.Customer, this))
                    {
                        item.Customer = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("Orders", item);
                    }
                }
            }
        }
    
        private void FixupRoles(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (Role item in e.NewItems)
                {
                    if (!item.User.Contains(this))
                    {
                        item.User.Add(this);
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("Roles", item);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Role item in e.OldItems)
                {
                    if (item.User.Contains(this))
                    {
                        item.User.Remove(this);
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("Roles", item);
                    }
                }
            }
        }

        #endregion
    }
}
