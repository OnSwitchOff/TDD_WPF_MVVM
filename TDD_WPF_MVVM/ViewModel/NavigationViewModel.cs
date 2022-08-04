using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD_Model;
using TDD_WPF_MVVM.DataProvider;
using TDD_WPF_MVVM.Events;

namespace TDD_WPF_MVVM.ViewModel
{
    public interface INavigationViewModel
    {
        void Load();
    }
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private INavigationDataProvider _dataProvider;
        private IEventAggregator _eventAggregator;

        public NavigationViewModel(INavigationDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            Friends = new ObservableCollection<NavigationItemViewModel>();
            _dataProvider = dataProvider;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<FriendSaveEvent>().Subscribe(OnFriendSaved);
            _eventAggregator.GetEvent<FriendDeletedEvent>().Subscribe(OnFriendDeleted);
        }      

        private void OnFriendSaved(Friend friend)
        {
            var displayMember = $"{friend.FirstName} {friend.LastName}";
            var navigationItem = Friends.SingleOrDefault(ni => ni.Id == friend.Id);
            if (navigationItem is not null)
            {
                navigationItem.DisplayMember = displayMember;
            }
            else
            {
                navigationItem = new NavigationItemViewModel(friend.Id, displayMember, _eventAggregator);
                Friends.Add(navigationItem);
            }
        }

        private void OnFriendDeleted(int friendId)
        {
            var navigationItem = Friends.Single(ni => ni.Id == friendId);
            Friends.Remove(navigationItem);
        }

        public void Load() 
        {
            Friends.Clear();
            foreach (var friend in _dataProvider.GetAllFriends())
            {
                Friends.Add(new NavigationItemViewModel(friend.Id,friend.DisplayMember,_eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; private set; }

    }
}
