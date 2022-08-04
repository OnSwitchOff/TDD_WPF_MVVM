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
        public string City { get => GetValue<string>(); set => SetValue(value); }
        public string Street { get => GetValue<string>(); set => SetValue(value); }
        public string StreetNumber { get => GetValue<string>(); set => SetValue(value); }
    }
}
