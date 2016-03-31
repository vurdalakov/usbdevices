namespace Vurdalakov
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows.Threading;

    // Implements a thread-safe ObservableCollection.
    public class ThreadSafeObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        private readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        public ThreadSafeObservableCollection()
        {
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (T item in e.NewItems)
                {
                    item.PropertyChanged += OnItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (T item in e.OldItems)
                {
                    item.PropertyChanged -= OnItemPropertyChanged;
                }
            }
            
            this.InvokeIfRequired(() => base.OnCollectionChanged(e));
        }

        public event PropertyChangedEventHandler ItemPropertyChanged;
        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.ItemPropertyChanged != null)
            {
                this.InvokeIfRequired(() => this.ItemPropertyChanged(this, e));
            }
        }
    
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.InvokeIfRequired(() => base.OnPropertyChanged(e));
        }

        private void InvokeIfRequired(Action action)
        {
            if (this.dispatcher.Thread != Thread.CurrentThread)
            {
                this.dispatcher.Invoke(DispatcherPriority.DataBind, action);
            }
            else
            {
                action();
            }
        }
        public void AddRange(IEnumerable<T> list)
        {
            this.AddRange(list, false, true);
        }

        public void ClearAndAddRange(IEnumerable<T> list)
        {
            this.AddRange(list, true, true);
        }

        private void AddRange(IEnumerable<T> list, Boolean clearFirst, Boolean invokeIfRequired)
        {
            if (invokeIfRequired)
            {
                this.InvokeIfRequired(() => this.AddRange(list, clearFirst, false));
            }
            else
            {
                this.CheckReentrancy(); // is it really needed?

                if (clearFirst)
                {
                    this.Items.Clear();
                }

                foreach (T item in list)
                {
                    this.Items.Add(item);
                }

                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            }
        }

        public new void Add(T item)
        {
            this.InvokeIfRequired(() => base.Add(item));
        }

        public new void Clear()
        {
            this.InvokeIfRequired(() => base.Clear());
        }

        public new void Insert(int index, T item)
        {
            this.InvokeIfRequired(() => base.Insert(index, item));
        }

        public new void RemoveAt(int index)
        {
            this.InvokeIfRequired(() => base.RemoveAt(index));
        }

        public new bool Remove(T item)
        {
            bool result = false;
            this.InvokeIfRequired(() => result = base.Remove(item));
            return result;
        }

        public new int IndexOf(T item)
        {
            int result = 0;
            this.InvokeIfRequired(() => result = base.IndexOf(item));
            return result;
        }

        public new bool Contains(T item)
        {
            bool result = false;
            this.InvokeIfRequired(() => result = base.Contains(item));
            return result;
        }

        public new void SetItem(int index, T item)
        {
            this.InvokeIfRequired(() => base.SetItem(index, item));
        }

        public new T this[int index]
        {
            get
            {
                T result = default(T);
                this.InvokeIfRequired(() => result = base[index]);
                return result;
            }
            set
            {
                this.InvokeIfRequired(() => base[index] = value);
            }
        }

    }
}
