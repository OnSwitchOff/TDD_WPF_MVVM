using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD_Model;
using TDD_WPF_MVVM.UITests.Extensions;
using TDD_WPF_MVVM.Wrapper;
using Xunit;

namespace TDD_WPF_MVVM.UITests.Wrapper
{

    public class FriendWrapperTests
    {
        private FriendEmail _friendEmail;
        private Friend _friend;
  
        public FriendWrapperTests()
        {
            _friendEmail = new FriendEmail { Id = 2, Email = "email2" };
            _friend = new Friend
            {
                FirstName = "Thijs",
                LastName = "LastIce",
                Address = new Address(),
                Emails = new List<FriendEmail>
                {
                    new FriendEmail { Id = 1, Email ="email1"},
                    _friendEmail
                }
            };
        }

        [Fact]
        public void ShouldContainFriendInFriendProperty()
        {
            var wrapper = new FriendWrapper(_friend);
            Assert.Equal(_friend, wrapper.Model);
        }

        [Fact]
        public void ShouldThrowArgumentNullExeptionIfFriendIsNull()
        {
            Assert.Throws<ArgumentNullException>("model", () => new FriendWrapper(null));
        }

        [Fact]
        public void ShouldGetValueOfUnderlyingAddressProperty()
        {
            var wrapper = new FriendWrapper(_friend);
            Assert.Equal(wrapper.FirstName, _friend.FirstName);
        }

        [Fact]
        public void ShouldSetValueOfUnderlyingModelProperty()
        {
            var wrapper = new FriendWrapper(_friend);
            wrapper.FirstName = "Julia";
            Assert.Equal("Julia", _friend.FirstName);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventOnFirstNamePropertyChanged()
        {
            var wrapper = new FriendWrapper(_friend);
            var fired = wrapper.IsPropertyChangedFired(
                () => wrapper.FirstName = "Julia",
                nameof(wrapper.FirstName));
            Assert.True(fired);
        }


        [Fact]
        public void ShouldNotRaisePropertyChangedEventIfFirstNameIsSetToSameValue()
        {
            var wrapper = new FriendWrapper(_friend);
            var fired = wrapper.IsPropertyChangedFired(
                () => wrapper.FirstName = wrapper.FirstName,
                nameof(wrapper.FirstName));
            Assert.False(fired);
        }

        [Fact]
        public void ShouldInitializeAddress()
        {
            var wrapper = new FriendWrapper(_friend);
            Assert.NotNull(wrapper.Address);
            Assert.Equal(_friend.Address, wrapper.Address.Model);
        }

        [Fact]
        public void ShoulThrowArgumentExeptionIfAddressIsNull()
        {
            _friend.Address = null;    
            Assert.Throws<ArgumentException>("Address",() => new FriendWrapper(_friend));
        }

        [Fact]
        public void ShoulThrowArgumentExeptionIfEmailsIsNull()
        {
            _friend.Emails = null;
            Assert.Throws<ArgumentException>("Emails", () => new FriendWrapper(_friend));
        }

        [Fact]
        public void ShouldInitializeEmails()
        {
            var wrapper = new FriendWrapper(_friend);
            Assert.NotNull(wrapper.Emails);
            CheckIfModelEmailsCollectiomIsInSync(wrapper);
        }

        [Fact]
        public void ShouldBeInSyncAfterAddingEmail()
        {
            _friend.Emails.Remove(_friendEmail);
            var wrapper = new FriendWrapper(_friend);
            wrapper.Emails.Add(new FriendEmailWrapper(_friendEmail));
            CheckIfModelEmailsCollectiomIsInSync(wrapper);
        }

        [Fact]
        public void ShouldBeInSyncAfterRemovingEmail()
        {
            var wrapper = new FriendWrapper(_friend);
            var emailToRemove = wrapper.Emails.Single(we => we.Model == _friendEmail);
            wrapper.Emails.Remove(emailToRemove);
            CheckIfModelEmailsCollectiomIsInSync(wrapper);
        }

        private void CheckIfModelEmailsCollectiomIsInSync(FriendWrapper wrapper)
        {
            Assert.Equal(_friend.Emails.Count, wrapper.Emails.Count);
            Assert.True(_friend.Emails.All(e => wrapper.Emails.Any(we => we.Model == e)));
        }
    }
}
