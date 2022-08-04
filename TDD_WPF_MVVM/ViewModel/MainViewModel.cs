using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDD_Model;
using TDD_WPF_MVVM.Command;
using TDD_WPF_MVVM.DataProvider;
using TDD_WPF_MVVM.Events;

namespace TDD_WPF_MVVM.ViewModel
{
    public class MainViewModel:ViewModelBase
    {
        private IFriendEditViewModel? _selectedFriendEditViewModel;
        private Func<IFriendEditViewModel> _friendEditVmCreator;

        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IFriendEditViewModel> friendEditVmCreator,
            IEventAggregator eventAggregator)
        {
            NavigationViewModel = navigationViewModel;
            FriendEditViewModels = new ObservableCollection<IFriendEditViewModel>();
            _friendEditVmCreator = friendEditVmCreator;
            eventAggregator.GetEvent<OpenFriendEditViewEvent>().Subscribe(OnOpenFriendEditViewEvent);
            eventAggregator.GetEvent<FriendDeletedEvent>().Subscribe(OnFriendDeleted);
            CloseFriendTabCommand = new DelegateCommand(OnCloseFriendTabExecute);
            AddFriendCommand = new DelegateCommand(OnAddFriendExecute);

        }

        private void OnFriendDeleted(int friendId)
        {
            var friendEditVm = FriendEditViewModels.Single(vm => vm.Friend.Id == friendId);
            FriendEditViewModels.Remove(friendEditVm);
        }

        private void OnCloseFriendTabExecute(object? obj)
        {
            IFriendEditViewModel? friendEditVm = obj as IFriendEditViewModel;
            if (friendEditVm is not null)
            FriendEditViewModels.Remove(friendEditVm);
        }
        private void OnAddFriendExecute(object? obj)
        {
            SelectedFriendEditViewModel = CreateAndLoadFriendEditViewModel(null);
        }

        private IFriendEditViewModel CreateAndLoadFriendEditViewModel(int? friendId)
        {
            var friendEditVm = _friendEditVmCreator();
            FriendEditViewModels.Add(friendEditVm);
            friendEditVm.Load(friendId);
            return friendEditVm;
        }

        private void OnOpenFriendEditViewEvent(int friendId)
        {
            var friendEditVm = FriendEditViewModels.SingleOrDefault(vm => vm.Friend.Id == friendId);
            if (friendEditVm is null)
            {
                friendEditVm = CreateAndLoadFriendEditViewModel(friendId);
            }
            SelectedFriendEditViewModel = friendEditVm;
        }
        public ICommand AddFriendCommand { get; private set; }
        public ICommand CloseFriendTabCommand { get; private set; }
        public INavigationViewModel NavigationViewModel { get; private set; }
        public ObservableCollection<IFriendEditViewModel> FriendEditViewModels { get; private set; }
        public IFriendEditViewModel SelectedFriendEditViewModel
        {
            get => _selectedFriendEditViewModel;
            set
            {
                _selectedFriendEditViewModel = value;
                OnPropertyChanged();
            } 
        }

        public void Load()
        {
            NavigationViewModel.Load();
        }
    }
}
