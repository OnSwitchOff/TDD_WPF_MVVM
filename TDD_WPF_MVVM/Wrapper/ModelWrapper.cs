using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TDD_WPF_MVVM.ViewModel;

namespace TDD_WPF_MVVM.Wrapper
{
    public class ModelWrapper<T> : NotifyDataErrorInfoBase, IValidatableTrackingObject, IValidatableObject
    {
        private Dictionary<string, object?> _originalValues;
        private List<IValidatableTrackingObject> _trackingObjects;
        public ModelWrapper(T model)
        {
            if (model is null)
            {
                throw new ArgumentNullException("model");
            }
            Model = model;
            _originalValues = new Dictionary<string, object?>();
            _trackingObjects = new List<IValidatableTrackingObject>();
            InitializeComplexProperties(model);
            InitializeCollectionProperties(model);
            Validate();
        }

        protected virtual void InitializeCollectionProperties(T model)
        {
 
        }

        protected virtual void InitializeComplexProperties(T model)
        {

        }

        public T Model { get; private set; }

        public bool IsChanged => _originalValues.Count > 0 || _trackingObjects.Any(t => t.IsChanged);
        public bool IsValid => !HasErrors && _trackingObjects.All(t => t.IsValid);

        public void AcceptChanges()
        {
            foreach (var trackingObject in _trackingObjects)
            {
                trackingObject.AcceptChanges();
            }
            _originalValues.Clear();
            OnPropertyChanged("");
        }

        public void RejectChanges()
        {
            foreach (var originalValueEntry in _originalValues)
            {
                typeof(T).GetProperty(originalValueEntry.Key)!.SetValue(Model, originalValueEntry.Value);
                //SetValue(originalValueEntry.Value,originalValueEntry.Key);
            }
            _originalValues.Clear();
            foreach (var trackingObject in _trackingObjects)
            {
                trackingObject.RejectChanges();
            }
            Validate();
            OnPropertyChanged("");
        }

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            var propertiInfo = Model!.GetType().GetProperty(propertyName);
            return (TValue)propertiInfo!.GetValue(Model)!;
            
        }
        protected void SetValue<TValue>(TValue newValue, [CallerMemberName] string propertyName = null)
        {
            var propertiInfo = Model!.GetType().GetProperty(propertyName);
            var currentValue = propertiInfo!.GetValue(Model);
            if (!Equals(currentValue, newValue))
            {
                UpdateOriginalValue(currentValue, newValue, propertyName);
                propertiInfo.SetValue(Model, newValue);
                Validate();
                OnPropertyChanged(propertyName);
                OnPropertyChanged(propertyName + "IsChanged");
            }          
        }

        private void Validate()
        {
            ClearErrors();
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);

            Validator.TryValidateObject(this, context, results,true);

            if (results.Any())
            {
                var propertyNames = results.SelectMany(r => r.MemberNames).Distinct().ToList();

                foreach (string propertyName in propertyNames)
                {
                    Errors[propertyName] = results
                        .Where(r => r.MemberNames.Contains(propertyName))
                        .Select(r => r.ErrorMessage!)
                        .Distinct().ToList();
                    OnErrorsChanged(propertyName);
                }              
            }
            OnPropertyChanged(nameof(IsValid));
        }      

        private void UpdateOriginalValue(object? currentValue, object? newValue, string propertyName)
        {
            if (!_originalValues.ContainsKey(propertyName))
            {
                _originalValues.Add(propertyName, currentValue);
            }
            else
            {
                if (Equals(_originalValues[propertyName], newValue))
                {
                    _originalValues.Remove(propertyName);
                }
            }
            OnPropertyChanged(nameof(IsChanged));
        }

        protected TValue GetOriginalValue<TValue>(string propertyName)
        {
            return GetIsChanged(propertyName)
                ? (TValue)_originalValues[propertyName]!
                : GetValue<TValue>(propertyName);
        }

        protected bool GetIsChanged(string propertyName)
        {
            return _originalValues.ContainsKey(propertyName);
        }

        protected void RegisterCollection<TWrapper, TModel>(ChangeTrackingCollection<TWrapper> wrapperCollection, List<TModel> modelCollection) where TWrapper : ModelWrapper<TModel>
        {
            wrapperCollection.CollectionChanged += (s, e) =>
            {
                modelCollection.Clear();
                modelCollection.AddRange(wrapperCollection.Select(w => w.Model));
                Validate();
            };
            RegisterTrackingObject(wrapperCollection);
        }

        protected void RegisterComplex<TModel>(ModelWrapper<TModel> wrapper)
        {
            RegisterTrackingObject(wrapper);
        }

        private void RegisterTrackingObject(IValidatableTrackingObject trackingObject)
        {
            if (!_trackingObjects.Contains(trackingObject))
            {
                _trackingObjects.Add(trackingObject);
                trackingObject.PropertyChanged += TrackingObjectPropertyChanged;
            }
        }

        private void TrackingObjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IsChanged))
            {
                OnPropertyChanged(nameof(IsChanged));
            }

            if (e.PropertyName == nameof(IsValid))
            {
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
