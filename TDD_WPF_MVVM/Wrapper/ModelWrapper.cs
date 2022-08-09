﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TDD_WPF_MVVM.ViewModel;

namespace TDD_WPF_MVVM.Wrapper
{
    public class ModelWrapper<T> : ViewModelBase, IRevertibleChangeTracking
    {
        private Dictionary<string, object?> _originalValues;
        private List<IRevertibleChangeTracking> _trackingObjects;
        public ModelWrapper(T model)
        {
            if (model is null)
            {
                throw new ArgumentNullException("model");
            }
            Model = model;
            _originalValues = new Dictionary<string, object?>();
            _trackingObjects = new List<IRevertibleChangeTracking>();
        }
        public T Model { get; private set; }

        public bool IsChanged
        {
            get { return _originalValues.Count > 0 || _trackingObjects.Any(t => t.IsChanged); }
        }

        public void AcceptChanges()
        {
            foreach (var trackingObject in _trackingObjects)
            {
                trackingObject.AcceptChanges();
            }
            _originalValues.Clear();
            OnPropertyChanged("");
            //OnPropertyChanged("IsChanged");
        }

        public void RejectChanges()
        {
            foreach (var originalValueEntry in _originalValues)
            {
                typeof(T).GetProperty(originalValueEntry.Key)!.SetValue(Model, originalValueEntry.Value);
                //SetValue(originalValueEntry.Value,originalValueEntry.Key);
            }
            foreach (var trackingObject in _trackingObjects)
            {
                trackingObject.RejectChanges();
            }
            _originalValues.Clear();
            OnPropertyChanged("");
            //OnPropertyChanged("IsChanged");
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
                OnPropertyChanged(propertyName);
                OnPropertyChanged(propertyName + "IsChanged");
            }          
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
            };
            RegisterTrackingObject(wrapperCollection);
        }

        protected void RegisterComplex<TModel>(ModelWrapper<TModel> wrapper)
        {
            RegisterTrackingObject(wrapper);
        }

        private void RegisterTrackingObject<TTrackingObject>(TTrackingObject trackingObject) where TTrackingObject: IRevertibleChangeTracking, INotifyPropertyChanged
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
        }
    }
}
