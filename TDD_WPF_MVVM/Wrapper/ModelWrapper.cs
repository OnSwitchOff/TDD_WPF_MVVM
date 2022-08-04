using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TDD_WPF_MVVM.ViewModel;

namespace TDD_WPF_MVVM.Wrapper
{
    public class ModelWrapper<T> : ViewModelBase
    { 
        public ModelWrapper(T model)
        {
            if (model is null)
            {
                throw new ArgumentNullException("model");
            }
            Model = model;
        }
        public T Model { get; private set; }

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            var propertiInfo = Model!.GetType().GetProperty(propertyName);
            return (TValue)propertiInfo!.GetValue(Model)!;
            
        }
        protected void SetValue<TValue>(TValue value, [CallerMemberName] string propertyName = null)
        {
            var propertiInfo = Model!.GetType().GetProperty(propertyName);
            var currentValue = propertiInfo!.GetValue(Model);
            if (!Equals(currentValue, value))
            {
                propertiInfo.SetValue(Model, value);
                OnPropertyChanged(propertyName);
            }
        }
    }
}
