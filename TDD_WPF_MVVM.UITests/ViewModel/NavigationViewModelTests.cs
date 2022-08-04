using Moq;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD_Model;
using TDD_WPF_MVVM.DataProvider;
using TDD_WPF_MVVM.Events;
using TDD_WPF_MVVM.ViewModel;
using Xunit;

namespace TDD_WPF_MVVM.UITests.ViewModel
{
    public class NavigationViewModelTests
    {
        private NavigationViewModel _viewModel;
        private FriendSaveEvent _friendSaveEvent;
        private FriendDeletedEvent _friendDeletedEvent;
        public NavigationViewModelTests()
        {
            _friendSaveEvent = new FriendSaveEvent();
            _friendDeletedEvent = new FriendDeletedEvent();

            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(ea => ea.GetEvent<FriendSaveEvent>())
                .Returns(_friendSaveEvent);
            eventAggregatorMock.Setup(ea => ea.GetEvent<FriendDeletedEvent>())
                .Returns(_friendDeletedEvent);

            var navigationDataProviderMock = new Mock<INavigationDataProvider>();
            navigationDataProviderMock.Setup(dp => dp.GetAllFriends())
                .Returns(new List<LookupItem>
                {
                    new LookupItem { Id = 1, DisplayMember = "Julia" },
                    new LookupItem { Id = 2, DisplayMember = "Thomas" }
                });
            _viewModel = new NavigationViewModel(navigationDataProviderMock.Object, eventAggregatorMock.Object);
        }

        [Fact]
        public void ShouldLoadFriends()
        {  
            _viewModel.Load();
            Assert.Equal(2, _viewModel.Friends.Count);

            var friend = _viewModel.Friends.SingleOrDefault(f => f.Id == 1);
            Assert.NotNull(friend);
            Assert.Equal("Julia", friend.DisplayMember);

            friend = _viewModel.Friends.SingleOrDefault(f => f.Id == 2);
            Assert.NotNull(friend);
            Assert.Equal("Thomas", friend.DisplayMember);
        }

        [Fact]
        public void ShouldLoadFriendsOnlyOnce()
        {
            _viewModel.Load();
            _viewModel.Load();
            Assert.Equal(2, _viewModel.Friends.Count);
        }

        [Fact]
        public void ShouldUpdateNavigationItemWhenFriendIsSaved()
        {
            _viewModel.Load();
            var navigationItem = _viewModel.Friends.First();
            var friendId = navigationItem.Id;
            _friendSaveEvent.Publish(
                new Friend
                {
                    Id = friendId,
                    FirstName = "Anna",
                    LastName = "Hubber"
                });
            Assert.Equal("Anna Hubber", navigationItem.DisplayMember);
        }
        [Fact]
        public void ShouldAddNavigationItemWhenAddedFriendIsSaved()
        {
            _viewModel.Load();

            const int newFriendId = 97;

            _friendSaveEvent.Publish(
                new Friend
                {
                    Id = newFriendId,
                    FirstName = "Anna",
                    LastName = "Hubber"
                });
            Assert.Equal(3, _viewModel.Friends.Count);

            var addedItem = _viewModel.Friends.SingleOrDefault(f => f.Id == newFriendId);
            Assert.NotNull(addedItem);
            Assert.Equal("Anna Hubber", addedItem.DisplayMember);
        }

        [Fact]
        public void ShouldRemoveNavigationItemWhenFriendIsDeleted()
        {
            _viewModel.Load();

            var deletedFriendId = _viewModel.Friends.First().Id;

            _friendDeletedEvent.Publish(deletedFriendId);

            Assert.Single(_viewModel.Friends);
            Assert.NotEqual(deletedFriendId, _viewModel.Friends.Single().Id);
        }
    }
}
 