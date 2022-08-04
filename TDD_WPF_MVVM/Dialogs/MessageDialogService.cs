using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TDD_WPF_MVVM.Dialogs
{
    public class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowYesNoDialog(string title, string message)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo)
                == MessageBoxResult.Yes
                ? MessageDialogResult.Yes
                : MessageDialogResult.No;
        }
    }
}
