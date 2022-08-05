using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD_Model;

namespace TDD_WPF_MVVM.Wrapper
{
    public class FriendEmailWrapper : ModelWrapper<FriendEmail>
    {
        public FriendEmailWrapper(FriendEmail model): base(model)
        {

        }

        public int Id { get => GetValue<int>(); set => SetValue(value); }
        public string Email { get => GetValue<string>(); set => SetValue(value); }
        public string Comment { get => GetValue<string>(); set => SetValue(value); }
    }
}
