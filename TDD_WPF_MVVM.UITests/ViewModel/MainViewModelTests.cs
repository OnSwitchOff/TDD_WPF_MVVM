using Moq;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD_Model;
using TDD_WPF_MVVM.Events;
using TDD_WPF_MVVM.ViewModel;
using Xunit;
using TDD_WPF_MVVM.UITests.Extensions;
using TDD_WPF_MVVM.Wrapper;

namespace TDD_WPF_MVVM.UITests.ViewModel
{
    public class MainViewModelTests
    {
        private Mock<INavigationViewModel> _navigationViewModelMock;
        private OpenFriendEditViewEvent _openFriendEditViewEvent;
        private Mock<IEventAggregator> _eventAggregator;
        private MainViewModel _viewModel;
        private List<Mock<IFriendEditViewModel>> _friendsEditViewModelMocks;
        private FriendDeletedEvent _friendDeletedEvent;

        public MainViewModelTests()
        {
            _friendDeletedEvent = new FriendDeletedEvent();
            _friendsEditViewModelMocks = new List<Mock<IFriendEditViewModel>>();
            _navigationViewModelMock = new Mock<INavigationViewModel>();
            _openFriendEditViewEvent = new OpenFriendEditViewEvent();
            _eventAggregator = new Mock<IEventAggregator>();
            _eventAggregator.Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>())
                .Returns(_openFriendEditViewEvent);
            _eventAggregator.Setup(ea => ea.GetEvent<FriendDeletedEvent>())
                 .Returns(_friendDeletedEvent);

            _viewModel = new MainViewModel(_navigationViewModelMock.Object,
                CreateFriendEditViewModel,_eventAggregator.Object);
        }

        private IFriendEditViewModel CreateFriendEditViewModel()
        {
            var friendEditViewModelMock = new Mock<IFriendEditViewModel>();
            friendEditViewModelMock.Setup(vm => vm.Load(It.IsAny<int>()))
                .Callback<int?>(friendId =>
                {
                    friendEditViewModelMock.Setup(vm => vm.Friend)
                    .Returns(new FriendWrapper(new Friend { Id = friendId.Value }));
                });
            _friendsEditViewModelMocks.Add(friendEditViewModelMock);
            return friendEditViewModelMock.Object;
        }

        [Fact]
        public void ShouldCallTheLoadNethodOfTheNavigationViewModel()
        {

            _viewModel.Load();
            _navigationViewModelMock.Verify(vm => vm.Load(), Times.Once);
        }

        [Fact]
        public void ShouldAddFriendEditViewModelAndLoadAndSelectIt()
        {
            const int friendId = 7;
            _openFriendEditViewEvent.Publish(friendId);
            Assert.Equal(1, _viewModel.FriendEditViewModels.Count);
            var friendEditVm = _viewModel.FriendEditViewModels.First();
            Assert.Equal(friendEditVm, _viewModel.SelectedFriendEditViewModel);
            _friendsEditViewModelMocks.First().Verify(vm => vm.Load(friendId), Times.Once);
        }

        [Fact]
        public void ShouldAddFriendsEditViewModelsOnlyOnce()
        {
            _openFriendEditViewEvent.Publish(5);
            _openFriendEditViewEvent.Publish(5);
            _openFriendEditViewEvent.Publish(6);
            _openFriendEditViewEvent.Publish(7);
            _openFriendEditViewEvent.Publish(7);

            Assert.Equal(3, _viewModel.FriendEditViewModels.Count);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForSelectedFriendEditViewModel()
        {

            var friendEditVmMock = new Mock<IFriendEditViewModel>();
            var fired = _viewModel.IsPropertyChangedFired(() =>
            {
                _viewModel.SelectedFriendEditViewModel = friendEditVmMock.Object;
            }, nameof(_viewModel.SelectedFriendEditViewModel));

            Assert.True(fired);
        }

        [Fact]
        public void ShouldRemoveFriendEditViewModelOnCloseFriendTabCommand()
        {
            _openFriendEditViewEvent.Publish(7);
            var friendEditVm = _viewModel.SelectedFriendEditViewModel;
            _viewModel.CloseFriendTabCommand.Execute(friendEditVm);
            Assert.Empty(_viewModel.FriendEditViewModels);
        }

        [Fact]
        public void ShouldAddFriendEditViewModelAndLoadItWithIdNullAndSelectIt()
        {
            _viewModel.AddFriendCommand.Execute(null);

            Assert.Equal(1, _viewModel.FriendEditViewModels.Count);
            var friendEditVm = _viewModel.FriendEditViewModels.First();
            Assert.Equal(friendEditVm, _viewModel.SelectedFriendEditViewModel);
            _friendsEditViewModelMocks.First().Verify(vm => vm.Load(null), Times.Once);
        }

        [Fact]
        public void ShouldRemoveFriendEditViewModelOnFriendDeletedEvent()
        {
            const int deleteFriendId = 7;
            _openFriendEditViewEvent.Publish(deleteFriendId);
            _openFriendEditViewEvent.Publish(8);
            _openFriendEditViewEvent.Publish(9);

            _friendDeletedEvent.Publish(deleteFriendId);
            Assert.Equal(2, _viewModel.FriendEditViewModels.Count);
            Assert.True(_viewModel.FriendEditViewModels.All(vm => vm.Friend.Id != deleteFriendId));
        }
    }
}
