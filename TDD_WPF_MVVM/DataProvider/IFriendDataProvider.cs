using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD_Model;

namespace TDD_WPF_MVVM.DataProvider
{
    public interface IFriendDataProvider
    {
        Friend GetFriendById(int id);
        void SaveFriend(Friend friend);
        void DeleteFriend(int id);
    }
}
