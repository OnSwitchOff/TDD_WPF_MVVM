using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD_DataAccess;
using TDD_Model;

namespace TDD_WPF_MVVM.DataProvider
{
    public class NavigationDataProvider : INavigationDataProvider
    {
        private Func<IDataService> _dataServiceCreator;

        public NavigationDataProvider(Func<IDataService> dataServiceCreator)
        {
            _dataServiceCreator = dataServiceCreator;
        }

        public IEnumerable<LookupItem> GetAllFriends()
        {
            using (var dataService = _dataServiceCreator())
            {
                return dataService.GetAllFriends();
            }

        }
    }
}
