using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace Lab2.Models
{
    internal class Storage : BaseEntity, INotifyPropertyChanged
    {
        public string Location { get; set; }
        public int Capacity { get; set; }
        private double _freeSpace;
        public double FreeSpace 
        {
            get => _freeSpace;
            set => Set(ref _freeSpace, value);
        }

        public Storage()
        {
            Location = "";
            Capacity = 0;
            FreeSpace = 0;
        }

        public Storage(string location, int capacity, double freeSpace)
        {
            Location = location;
            Capacity = capacity;
            FreeSpace = freeSpace;
        }

        public Storage(int id, string location, int capacity, double freeSpace):this(location, capacity, freeSpace)
        {
            Id = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            var handlers = PropertyChanged;
            if (handlers != null)
            {
                var invocationList = handlers.GetInvocationList();
                var arg = new PropertyChangedEventArgs(PropertyName);
                foreach (var action in invocationList)
                {
                    if (action.Target is DispatcherObject dispatcher)
                    {
                        dispatcher.Dispatcher.Invoke(action, this, arg);
                    }
                    else
                    {
                        action.DynamicInvoke(this, arg);
                    }
                }
            }
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
    }
}
