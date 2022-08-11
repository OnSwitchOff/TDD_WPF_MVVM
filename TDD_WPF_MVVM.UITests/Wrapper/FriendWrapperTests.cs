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
            _friendEmail = new FriendEmail { Id = 2, Email = "email@2" };
            _friend = new Friend
            {
                FirstName = "Thijs",
                LastName = "LastIce",
                Address = new Address { City = "Mariupol"},
                Emails = new List<FriendEmail>
                {
                    new FriendEmail { Id = 1, Email ="email@1"},
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

        [Fact]
        private void ShouldStoreOriginalValue()
        {
            var wrapper = new FriendWrapper(_friend);
            Assert.Equal("Thijs", wrapper.FirstNameOriginal);

            wrapper.FirstName = "Julia";
            Assert.Equal("Thijs", wrapper.FirstNameOriginal);
        }

        [Fact]
        private void ShouldSetIsChangedValue()
        {
            var wrapper = new FriendWrapper(_friend);
            Assert.False(wrapper.FirstNameIsChanged);
            Assert.False(wrapper.IsChanged);

            wrapper.FirstName = "Julia";
            Assert.True(wrapper.FirstNameIsChanged);
            Assert.True(wrapper.IsChanged);

            wrapper.FirstName = "Thijs";
            Assert.False(wrapper.FirstNameIsChanged);
            Assert.False(wrapper.IsChanged);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForFirstNameIsChanged()
        {
            var wrapper = new FriendWrapper(_friend);
            var fired = wrapper.IsPropertyChangedFired(
                () => wrapper.FirstName = "Julia",
                nameof(wrapper.FirstNameIsChanged));
            Assert.True(fired);
        }
        [Fact]
        public void ShouldRaisePropertyChangedEventForIsChanged()
        {
            var wrapper = new FriendWrapper(_friend);
            var fired = wrapper.IsPropertyChangedFired(
                () => wrapper.FirstName = "Julia",
                nameof(wrapper.IsChanged));
            Assert.True(fired);
        }


        [Fact]
        public void ShouldAcceptChanges()
        {
            var wrapper = new FriendWrapper(_friend);
            wrapper.FirstName = "Julia";
            Assert.Equal("Julia", wrapper.FirstName);
            Assert.Equal("Thijs", wrapper.FirstNameOriginal);
            Assert.True(wrapper.FirstNameIsChanged);
            Assert.True(wrapper.IsChanged);
            wrapper.AcceptChanges();

            Assert.Equal("Julia", wrapper.FirstName);
            Assert.Equal("Julia", wrapper.FirstNameOriginal);
            Assert.False(wrapper.FirstNameIsChanged);
            Assert.False(wrapper.IsChanged);
        }


        [Fact]
        public void ShouldRejectedChanges()
        {
            var wrapper = new FriendWrapper(_friend);
            wrapper.FirstName = "Julia";
            Assert.Equal("Julia", wrapper.FirstName);
            Assert.Equal("Thijs", wrapper.FirstNameOriginal);
            Assert.True(wrapper.FirstNameIsChanged);
            Assert.True(wrapper.IsChanged);
            wrapper.RejectChanges();

            Assert.Equal("Thijs", wrapper.FirstName);
            Assert.Equal("Thijs", wrapper.FirstNameOriginal);
            Assert.False(wrapper.FirstNameIsChanged);
            Assert.False(wrapper.IsChanged);
        }

        [Fact]
        public void ShouldSetIsChangedOfFriendWrapper()
        {
            var wrapper = new FriendWrapper(_friend);
            wrapper.Address.City = "Salt Lake City";     
            Assert.True(wrapper.IsChanged);

            wrapper.Address.City = "Mariupol";
            Assert.False(wrapper.IsChanged);
        }


        [Fact]
        public void ShouldRaisePropertyChangedEventForIsCHangedPropertyOfFriendWrapp()
        {
            var wrapper = new FriendWrapper(_friend);
            var fired = wrapper.IsPropertyChangedFired(
                () => wrapper.Address.City = "Salt Lake City",
                nameof(wrapper.IsChanged));
            Assert.True(fired);
        }


        [Fact]
        public void ShouldAcceptChangesForAddressChanged()
        {
            var wrapper = new FriendWrapper(_friend);
            wrapper.Address.City = "Salt Lake City";
            Assert.Equal("Mariupol", wrapper.Address.CityOriginal);

            wrapper.AcceptChanges();

            Assert.Equal("Salt Lake City", wrapper.Address.City);
            Assert.Equal("Salt Lake City", wrapper.Address.CityOriginal);
            Assert.False(wrapper.IsChanged);
        }


        [Fact]
        public void ShouldRejectedChangesForAddressChanged()
        {
            var wrapper = new FriendWrapper(_friend);
            wrapper.Address.City = "Salt Lake City";
            Assert.Equal("Mariupol", wrapper.Address.CityOriginal);

            wrapper.RejectChanges();

            Assert.Equal("Mariupol", wrapper.Address.City);
            Assert.Equal("Mariupol", wrapper.Address.CityOriginal);
            Assert.False(wrapper.IsChanged);
        }

        [Fact]
        public void ShouldSetIsChangedOfFriendWrapperWhenEmailsCollectionChanged()
        {
            var wrapper = new FriendWrapper(_friend);
            var removedEmail = wrapper.Emails.First();
            wrapper.Emails.Remove(removedEmail);
            Assert.True(wrapper.IsChanged);
        }


        [Fact]
        public void ShouldRaisePropertyChangedEventForIsCHangedPropertyOfFriendWrappWhenEmailsCollectionChanged()
        {
            var wrapper = new FriendWrapper(_friend);
            var removedEmail = wrapper.Emails.First();
            var fired = wrapper.IsPropertyChangedFired(
                () => wrapper.Emails.Remove(removedEmail),
                nameof(wrapper.IsChanged));
            Assert.True(fired);
        }

        [Fact]
        public void ShouldAcceptChangesForEmailsChanged()
        {
            var wrapper = new FriendWrapper(_friend);
            var modEmail = wrapper.Emails.First();
            modEmail.Email = "email@33";
            Assert.Equal("email@1", modEmail.EmailOriginal);
            Assert.True(wrapper.IsChanged);

            wrapper.AcceptChanges();

            Assert.Equal("email@33", modEmail.Email);
            Assert.Equal("email@33", modEmail.EmailOriginal);
            Assert.False(wrapper.IsChanged);
        }


        [Fact]
        public void ShouldRejectedChangesForEmailsChanged()
        {
            var wrapper = new FriendWrapper(_friend);
            var modEmail = wrapper.Emails.First();
            modEmail.Email = "email@33";
            Assert.Equal("email@1", modEmail.EmailOriginal);
            Assert.True(wrapper.IsChanged);

            wrapper.RejectChanges();

            Assert.Equal("email@1", modEmail.Email);
            Assert.Equal("email@1", modEmail.EmailOriginal);
            Assert.False(wrapper.IsChanged);
        }

        [Fact]
        public void ShouldBeInSyncAfterClearingEmail()
        {
            var wrapper = new FriendWrapper(_friend);
            wrapper.Emails.Clear();
            CheckIfModelEmailsCollectiomIsInSync(wrapper);
        }




        [Fact]
        public void ShouldReturnValidationErrorIfFirstNameIsEmpty()
        {
            var wrapper = new FriendWrapper(_friend);
            Assert.False(wrapper.HasErrors);

            wrapper.FirstName = "";
            Assert.True(wrapper.HasErrors);

            var errors = wrapper.GetErrors(nameof(wrapper.FirstName)).Cast<string>();
            Assert.Single(errors);
            Assert.Equal("Firstname is required", errors.First());

            wrapper.FirstName = "Julia";
            Assert.False(wrapper.HasErrors);
        }

        [Fact]
        public void ShouldRaiseErrorsChangedEventWhenFirstNameSetToEmpty()
        {
            var fired = false;

            var wrapper = new FriendWrapper(_friend);
            wrapper.ErrorsChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(wrapper.FirstName))
                {
                    fired = true;
                }
            };

            wrapper.FirstName = "";
            Assert.True(fired);

            fired = false;
            wrapper.FirstName = "Julia";
            Assert.True(fired);
        }


        [Fact]
        public void ShouldSetIsValid()
        {
            var wrapper = new FriendWrapper(_friend);
            Assert.True(wrapper.IsValid);

            wrapper.FirstName = "";
            Assert.False(wrapper.IsValid);

            wrapper.FirstName = "Julia";
            Assert.True(wrapper.IsValid);
        }

        [Fact]
        public void ShouldRaiseEPropertyChangedEventForIsValid()
        {
            var wrapper = new FriendWrapper(_friend);
            var fired = wrapper.IsPropertyChangedFired(
                () => wrapper.FirstName = "",
                nameof(wrapper.IsValid));

            fired = wrapper.IsPropertyChangedFired(
                () => wrapper.FirstName = "Julia",
                nameof(wrapper.IsValid));
            Assert.True(fired);
        }



        [Fact]
        public void ShouldSetErrorsAndIsValidAfterInitialization()
        {
            _friend.FirstName = "";
            var wrapper = new FriendWrapper(_friend);

            Assert.False(wrapper.IsValid);
            Assert.True(wrapper.HasErrors);

            var errors = wrapper.GetErrors(nameof(wrapper.FirstName)).Cast<string>();
            Assert.Single(errors);
            Assert.Equal("Firstname is required", errors.First());
        }

        [Fact]
        public void ShouldRefreshErrorsAndIsValidWhenRejectingChanges()
        {
            var wrapper = new FriendWrapper(_friend);
            Assert.True(wrapper.IsValid);
            Assert.False(wrapper.HasErrors);

            wrapper.FirstName = "";
            Assert.False(wrapper.IsValid);
            Assert.True(wrapper.HasErrors);

            wrapper.RejectChanges();

            Assert.True(wrapper.IsValid);
            Assert.False(wrapper.HasErrors);
        }


        [Fact]
        public void ShouldSetIsValidOfRoot()
        {
            var wrapper = new FriendWrapper(_friend);
            Assert.True(wrapper.IsValid);

            wrapper.Address.City = "";
            Assert.False(wrapper.IsValid);

            wrapper.Address.City = "Salt Lake City";
            Assert.True(wrapper.IsValid);
        }

        [Fact]
        public void ShouldSetIsValidOfRootAfterInitialization()
        {
            _friend.Address.City = "";
            var wrapper = new FriendWrapper(_friend);
            Assert.False(wrapper.IsValid);

            wrapper.Address.City = "Salt Lake City";
            Assert.True(wrapper.IsValid);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForIsValidOfRoot()
        {
            var wrapper = new FriendWrapper(_friend);

            var fired = wrapper.IsPropertyChangedFired(
                () => wrapper.Address.City = "",
                nameof(wrapper.IsValid));
            Assert.True(fired);
        }

        [Fact]
        public void ShouldSetIsValidOfRootForEmail()
        {
            var wrapper = new FriendWrapper(_friend);
            Assert.True(wrapper.IsValid);

            wrapper.Emails.First().Email = "";
            Assert.False(wrapper.IsValid);

            wrapper.Emails.First().Email = "thomas@bobs.ua";
            Assert.True(wrapper.IsValid);
        }

        [Fact]
        public void ShouldSetIsValidOfRootAfterInitializationForEmail()
        {
            _friend.Emails.First().Email = "";
            var wrapper = new FriendWrapper(_friend);
            Assert.False(wrapper.IsValid);

            wrapper.Emails.First().Email = "thomas@bobs.ua";
            Assert.True(wrapper.IsValid);
        }

        [Fact]
        public void ShouldSetIsValidOfRootAfterRemovingInvalidItem()
        {
            _friend.Emails.First().Email = "";
            var wrapper = new FriendWrapper(_friend);
            Assert.False(wrapper.IsValid);


            wrapper.Emails.Remove(wrapper.Emails.First());
            Assert.True(wrapper.IsValid);
        }

        [Fact]
        public void ShouldSetIsValidOfRootAfterAddingInvalidItem()
        {
            var emailToAdd = new FriendEmailWrapper(new FriendEmail());
            var wrapper = new FriendWrapper(_friend);
            Assert.True(wrapper.IsValid);
            wrapper.Emails.Add(emailToAdd);
            Assert.False(wrapper.IsValid);
            emailToAdd.Email = "thomas@bobs.ua";
            Assert.True(wrapper.IsValid);
        }


        [Fact]
        public void ShouldRaisePropertyChangedEventForIsValidOfRootForEmail()
        {
            var wrapper = new FriendWrapper(_friend);

            var fired = wrapper.IsPropertyChangedFired(
                () => wrapper.Emails.First().Email = "",
                nameof(wrapper.IsValid));
            Assert.True(fired);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForIsValidOfRootAfterRemovingEmail()
        {

            var wrapper = new FriendWrapper(_friend);
            var emailToRemove = wrapper.Emails.First();
            var fired = wrapper.IsPropertyChangedFired(
                () => wrapper.Emails.Remove(emailToRemove),
                nameof(wrapper.IsValid));
            Assert.True(fired);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForIsValidOfRootAfterAddingEmail()
        {
            var emailToAdd = new FriendEmailWrapper(new FriendEmail());
            var wrapper = new FriendWrapper(_friend);

            var fired = wrapper.IsPropertyChangedFired(
                () => wrapper.Emails.Add(emailToAdd),
                nameof(wrapper.IsValid));
            Assert.True(fired);
        }


        [Fact]
        public void ShouldHaveErrorsAndNotBeValidWhenIsDeveloperIsTrueAndNoEmailExists()
        {
            var expectedError = "A developer must have an email-address";
            _friend.IsDeveloper = false;
            var wrapper = new FriendWrapper(_friend);
            wrapper.Emails.Clear();
            Assert.False(wrapper.IsDeveloper);
            Assert.True(wrapper.IsValid);

            wrapper.IsDeveloper = true;
            Assert.False(wrapper.IsValid);

            var emailsErrors = wrapper.GetErrors(nameof(wrapper.Emails)).Cast<string>().ToList();
            Assert.Single(emailsErrors);
            Assert.Equal(expectedError, emailsErrors[0]);

            var isDeveloperErrors = wrapper.GetErrors(nameof(wrapper.IsDeveloper)).Cast<string>().ToList();
            Assert.Single(isDeveloperErrors);
            Assert.Equal(expectedError, isDeveloperErrors[0]);
        }

        [Fact]
        public void ShouldBeValidAgaineWhenIsDeveloperIsSetBackToFalse()
        {
            var expectedError = "A developer must have an email-address";
            _friend.IsDeveloper = false;
            var wrapper = new FriendWrapper(_friend);
            wrapper.Emails.Clear();
            Assert.False(wrapper.IsDeveloper);
            Assert.True(wrapper.IsValid);

            wrapper.IsDeveloper = true;
            Assert.False(wrapper.IsValid);

            wrapper.IsDeveloper = false;
            Assert.True(wrapper.IsValid);
        }

        [Fact]
        public void ShouldBeValidAgainWhenEmailIsAdded()
        {
            _friend.IsDeveloper = false;
            var wrapper = new FriendWrapper(_friend);
            var emailToAdd = wrapper.Emails.First();
            wrapper.Emails.Clear();
            Assert.False(wrapper.IsDeveloper);
            Assert.True(wrapper.IsValid);

            wrapper.IsDeveloper = true;
            Assert.False(wrapper.IsValid);

            wrapper.Emails.Add(emailToAdd);
            Assert.True(wrapper.IsValid);
        }


        [Fact]
        public void ShouldInitWithoutProblems()
        {
            _friend.IsDeveloper = true;
            var wrapper = new FriendWrapper(_friend);
            Assert.True(wrapper.IsValid);
        }
    }
}

