using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public FriendWrapper(Friend model) : base(model)
        {
            InitializeComplexProperties(model);
            InitializeCollectionProperties(model);
        }

        private void InitializeCollectionProperties(Friend friend)
        {
            if (friend.Emails is null)
            {
                throw new ArgumentException("Emails cant be null", "Emails");
            }
            Emails = new ChangeTrackingCollection<FriendEmailWrapper>(friend.Emails.Select(e => new FriendEmailWrapper(e)));

            RegisterCollection(Emails, friend.Emails);
        }



        private void InitializeComplexProperties(Friend friend)
        {
            if (friend.Address is null)
            {
                throw new ArgumentException("Address cant be null", "Address");
            }
            Address = new AddressWrapper(friend.Address);

            RegisterComplex(Address);
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
        public int FriendgroupIdIOriginal => GetOriginalValue<int>(nameof(FriendgroupId));
        public bool FriendgroupIdIsChanged => GetIsChanged(nameof(FriendgroupId));

        public string FirstName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string FirstNameOriginal => GetOriginalValue<string>(nameof(FirstName));
        public bool FirstNameIsChanged => GetIsChanged(nameof(FirstName));

        public string LastName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string LastNameOriginal => GetOriginalValue<string>(nameof(LastName));
        public bool LastNameIsChanged => GetIsChanged(nameof(LastName));

        public DateTime? Birthday
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }
        public DateTime? BirthdayOriginal => GetOriginalValue<DateTime?>(nameof(Birthday));
        public bool BirthdayIsChanged => GetIsChanged(nameof(Birthday));

        public bool IsDeveloper
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool IsDeveloperOriginal => GetOriginalValue<bool>(nameof(IsDeveloper));
        public bool IsDeveloperIsChanged => GetIsChanged(nameof(IsDeveloper));

        public AddressWrapper Address { get; private set; }

        public ChangeTrackingCollection<FriendEmailWrapper> Emails { get; private set; }

        //protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        //{
        //    base.OnPropertyChanged(propertyName);
        //    if (propertyName != nameof(IsChanged))
        //    {
        //        //IsChanged = true;
        //    }
        //}
    }
}
