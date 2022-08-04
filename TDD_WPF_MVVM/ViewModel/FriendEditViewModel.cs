using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDD_Model;
using TDD_WPF_MVVM.Command;
using TDD_WPF_MVVM.DataProvider;
using TDD_WPF_MVVM.Dialogs;
using TDD_WPF_MVVM.Events;
using TDD_WPF_MVVM.Wrapper;

namespace TDD_WPF_MVVM.ViewModel
{
    public interface IFriendEditViewModel
    {
        void Load(int? friendId);
        FriendWrapper Friend { get; }
    }
    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
    {
        private IFriendDataProvider _dataProvider;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;
        private FriendWrapper _friend;

        public FriendEditViewModel(IFriendDataProvider dataProvider,IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _dataProvider = dataProvider;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute, OnDeleteCanExecute);
        }

      

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public FriendWrapper Friend 
        {
            get => _friend;
            private set 
            {
                _friend = value;
                OnPropertyChanged();
            }
        }        

        public void Load(int? friendId)
        {
            var friend = friendId.HasValue
                ? _dataProvider.GetFriendById(friendId.Value)
                : new Friend();
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += Friend_PropertyChanged;

            InvalidateComands();
        }

        private void Friend_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateComands();
        }

        private void InvalidateComands()
        {
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)DeleteCommand).RaiseCanExecuteChanged();
        }

        private void OnSaveExecute(object? obj)
        {
            _dataProvider.SaveFriend(Friend.Model);
            Friend.AcceptChanges();
            _eventAggregator.GetEvent<FriendSaveEvent>().Publish(Friend.Model);
        }

        private bool OnSaveCanExecute(object? obj)
        {
            return Friend is not null && Friend.IsChanged;
        }

     
        private void OnDeleteExecute(object? obj)
        {
            var result = _messageDialogService.ShowYesNoDialog("Delete friend", $"Do you realy want to delete the friend '{Friend.FirstName} {Friend.LastName}'");
            if (result == MessageDialogResult.Yes)
            {
                _dataProvider.DeleteFriend(Friend.Id);
                _eventAggregator.GetEvent<FriendDeletedEvent>().Publish(Friend.Id);
            }
        }

        private bool OnDeleteCanExecute(object? obj)
        {
            return Friend is not null && Friend.Id > 0;
        }

    }
}
