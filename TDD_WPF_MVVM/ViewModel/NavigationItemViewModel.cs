using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDD_WPF_MVVM.Command;
using TDD_WPF_MVVM.Events;

namespace TDD_WPF_MVVM.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;
        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            OpenFriendEditViewCommand = new DelegateCommand(OnFriendEditViewExecute);
            _eventAggregator = eventAggregator;
        }

        private void OnFriendEditViewExecute(object? obj)
        {
            _eventAggregator
                .GetEvent<OpenFriendEditViewEvent>()
                    .Publish(Id);
        }

        public int Id { get; private set; }
        public ICommand OpenFriendEditViewCommand { get; private set; }
        public string DisplayMember 
        { 
            get => _displayMember; 
            set 
            { 
                _displayMember = value;
                OnPropertyChanged();
            } 
        }

        private IEventAggregator _eventAggregator;
    }
}
