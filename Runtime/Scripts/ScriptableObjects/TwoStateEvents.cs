using System;

namespace Alteracia.Patterns.ScriptableObjects
{
    public interface ITwoStateEvent<T>
    {
        bool? IsSecondary { get; }
        T LastPrimary { get; }
        T LastSecondary { get; }
        Action<T> OnPrimaryEvent { get; set; }
        Action<T> OnSecondaryEvent { get; set; }
    }

    /// <summary>
    /// Events of two state
    /// </summary>
    public abstract class TwoStateSubscribableEvents<T> : NestedScriptableObject, ITwoStateEvent<T>, ISubscribableEvent
    {
        [NonSerialized] private bool? _state = null;
        public bool? IsSecondary => _state;

        [NonSerialized] private T _lastPrimary;
        public T LastPrimary => _lastPrimary;
        
        [NonSerialized] private T _lastSecondary;
        public T LastSecondary => _lastSecondary;

        [NonSerialized] private Action<T> _onPrimaryEvent;
        public Action<T> OnPrimaryEvent
        {
            get => _onPrimaryEvent;
            set => _onPrimaryEvent = value;
        }
        
        [NonSerialized] private Action<T> _onSecondaryEvent;
        public Action<T> OnSecondaryEvent
        {
            get => _onSecondaryEvent;
            set => _onSecondaryEvent = value;
        }
        
        private object _temporalLast;
        public object TemporalLast
        {
            get => _temporalLast;
            set => _temporalLast = value;
        }
        
        private object _temporalSecondaryLast;
        public object TemporalSecondaryLast
        {
            get => _temporalSecondaryLast;
            set => _temporalSecondaryLast = value;
        }
        
        protected TwoStateSubscribableEvents()
        {
            _onPrimaryEvent += obj =>
            {
                _lastPrimary = obj;
                _state = true;
            };
            _onSecondaryEvent += obj =>
            {
                _lastSecondary = obj;
                _state = false;
            };
        }

        public bool Equals(ISubscribableEvent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            TwoStateSubscribableEvents<T> otherSubscribableEvent = other as TwoStateSubscribableEvents<T>;
            return otherSubscribableEvent && 
                   String.Equals(this.name, otherSubscribableEvent.name, StringComparison.CurrentCultureIgnoreCase);
        }

        public void SubscribeTo(ISubscribableEvent other)
        {
            TwoStateSubscribableEvents<T> otherObjectSubscribableEvent = (TwoStateSubscribableEvents<T>)other;
            this.OnPrimaryEvent += passed =>
            {
                if (passed == null || passed.Equals(other.TemporalLast))
                {
                    _temporalLast = null;
                    other.TemporalLast = null;
                    return;
                }

                _temporalLast = passed;
                otherObjectSubscribableEvent.OnPrimaryEvent?.Invoke(passed);
            };
            this.OnSecondaryEvent += passed =>
            {
                if (passed == null || passed.Equals(otherObjectSubscribableEvent.TemporalSecondaryLast))
                {
                    _temporalSecondaryLast = null;
                    otherObjectSubscribableEvent.TemporalSecondaryLast = null;
                    return;
                }

                _temporalSecondaryLast = passed;
                otherObjectSubscribableEvent.OnSecondaryEvent?.Invoke(passed);
            };
        }
    }
}