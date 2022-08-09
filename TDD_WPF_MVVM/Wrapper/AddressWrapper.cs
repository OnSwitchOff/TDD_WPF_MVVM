using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD_Model;

namespace TDD_WPF_MVVM.Wrapper
{
    public class AddressWrapper : ModelWrapper<Address>
    {
        public AddressWrapper(Address model):base(model)    
        {

        }

        public int Id { get => GetValue<int>(); set => SetValue(value); }
        public int IdOriginal => GetOriginalValue<int>(nameof(Id));
        public bool IdIsChanged => GetIsChanged(nameof(Id));
        public string City { get => GetValue<string>(); set => SetValue(value); }
        public string CityOriginal => GetOriginalValue<string>(nameof(City));
        public bool CityIsChanged => GetIsChanged(nameof(City));
        public string Street { get => GetValue<string>(); set => SetValue(value); }
        public string StreetOriginal => GetOriginalValue<string>(nameof(Street));
        public bool StreetIsChanged => GetIsChanged(nameof(Street));
        public string StreetNumber { get => GetValue<string>(); set => SetValue(value); }
        public string StreetNumberOriginal => GetOriginalValue<string>(nameof(StreetNumber));
        public bool StreetNumberIsChanged => GetIsChanged(nameof(StreetNumber));
    }
}
