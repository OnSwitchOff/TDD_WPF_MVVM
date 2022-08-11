using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD_WPF_MVVM.Wrapper
{
    public interface IValidatableTrackingObject: IRevertibleChangeTracking, INotifyPropertyChanged
    {
        bool IsValid { get; }
    }
}
