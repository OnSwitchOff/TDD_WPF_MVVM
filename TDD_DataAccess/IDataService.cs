using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD_Model;

namespace TDD_DataAccess
{
    public interface IDataService: IDisposable
    {
        Friend GetFriendById(int friendId);
        void SaveFriend(Friend friend);
        void DeleteFriend(int friendId);
        IEnumerable<LookupItem> GetAllFriends();
    }
}
