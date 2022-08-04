using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TDD_Model;
using TDD_WPF_MVVM.ViewModel;

namespace TDD_WPF_MVVM.Wrapper
{
    public class FriendWrapper : ModelWrapper<Friend>
    {
        private bool _isChanged;

        public FriendWrapper(Friend friend) : base(friend)
        {
            InitializeComplexProperties(friend);
        }

        private void InitializeComplexProperties(Friend friend)
        {
            if (friend.Address is null)
            {
                throw new ArgumentException("Address cant be null","Address");
            }
            Address = new AddressWrapper(friend.Address);
        }

        public bool IsChanged
        {
            get { return _isChanged; }
            private set 
            { 
                _isChanged = value;
                OnPropertyChanged();
            }
        }

        public void AcceptChanges()
        {
            IsChanged = false;
        }

        public int Id
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int FriendgroupId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string FirstName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string LastName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
           
        public DateTime? Birthday
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public bool IsDeveloper
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public AddressWrapper Address { get; private set; }

        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName != nameof(IsChanged))
            {
                IsChanged = true;
            }
        }
    }
}
