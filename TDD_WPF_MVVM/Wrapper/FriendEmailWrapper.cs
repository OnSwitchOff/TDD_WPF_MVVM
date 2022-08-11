using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int IdOriginal => GetOriginalValue<int>(nameof(Id));
        public bool IdIsChanged => GetIsChanged(nameof(Id));

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage ="Email is not valid email address")]
        public string Email { get => GetValue<string>(); set => SetValue(value); }
        public string EmailOriginal => GetOriginalValue<string>(nameof(Email));
        public bool EmailIsChanged => GetIsChanged(nameof(Email));
        public string Comment { get => GetValue<string>(); set => SetValue(value); }
        public string CommentOriginal => GetOriginalValue<string>(nameof(Comment));
        public bool CommentIsChanged => GetIsChanged(nameof(Comment));
    }
}
