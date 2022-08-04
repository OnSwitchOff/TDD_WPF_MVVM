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
using TDD_WPF_MVVM.UITests.Extensions;
using Xunit;

namespace TDD_WPF_MVVM.UITests.ViewModel
{
    public class NavigationItemViewModelTests
    {
        const int _friendId = 7;
        private NavigationItemViewModel _viewModel;
        private Mock<IEventAggregator> _eventAggregatorMock;

        public NavigationItemViewModelTests()
        {
            _eventAggregatorMock = new Mock<IEventAggregator>();
            _viewModel = new NavigationItemViewModel(_friendId, "Thomas", _eventAggregatorMock.Object);
        }

        [Fact]
        public void ShouldPublishOpenFriendEditViewEvent()
        {        
            var eventMock = new Mock<OpenFriendEditViewEvent>();
            _eventAggregatorMock
             .Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>())
             .Returns(eventMock.Object);

            _viewModel.OpenFriendEditViewCommand.Execute(null);

            eventMock.Verify(e => e.Publish(_friendId), Times.Once);
        }


        [Fact]
        public void ShouldRaisedPropertyChangedEventForDisplayMember()
        {
            var fired =_viewModel.IsPropertyChangedFired(()=>
            {
                _viewModel.DisplayMember = "Changed";
            },nameof(_viewModel.DisplayMember));

            Assert.True(fired);
        }
    }
}
