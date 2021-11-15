using System;

namespace Alteracia.Patterns.ScriptableObjects
{
    public interface ISubscribableEvent
    {
        object TemporalLast { get; set; }
        bool Equals(ISubscribableEvent other);
        void SubscribeTo(ISubscribableEvent other);
    }
    
    public interface IObjectEvent<T>
    {
        T Last { get; }
        Action<T> OnEvent { get; set; }
    }

    public abstract class ObjectSubscribableEvent<T> : NestedScriptableObject, IObjectEvent<T>, ISubscribableEvent
    {
        [NonSerialized] private Action<T> _onEvent;

        [NonSerialized] private T _last;
        public T Last => _last;

        private object _temporalLast;
        public object TemporalLast
        {
            get => _temporalLast;
            set => _temporalLast = value;
        }

        public void SubscribeTo(ISubscribableEvent other)
        {
            this.OnEvent += passed =>
            {
                if (passed == null || passed.Equals(other.TemporalLast))
                {
                    _temporalLast = null;
                    other.TemporalLast = null;
                    return;
                }

                _temporalLast = passed;
                ObjectSubscribableEvent<T> otherObjectSubscribableEvent = (ObjectSubscribableEvent<T>)other;
                otherObjectSubscribableEvent.OnEvent?.Invoke(passed);
            };
        }

        public Action<T> OnEvent
        {
            get => _onEvent;
            set => _onEvent = value;
        }

        protected ObjectSubscribableEvent()
        {
            _onEvent += obj => _last = obj;
        }
        
        public bool Equals(ISubscribableEvent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            ObjectSubscribableEvent<T> otherSubscribableEvent = other as ObjectSubscribableEvent<T>;
            return otherSubscribableEvent && String.Equals(this.name, otherSubscribableEvent.name, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}