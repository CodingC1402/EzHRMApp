using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.Helper
{
    public class ListenerEvent<T>
    {
        private List<Action<T>> _actions = new List<Action<T>>();
        public void AddListener(Action<T> action)
        {
            _actions.Add(action);
        }

        public void RemoveListener(Action<T> action)
        {
            _actions.Remove(action);
        }

        public void Invoke(T value)
        {
            foreach (Action<T> action in _actions)
            {
                action.Invoke(value);
            }
        }
    }
}
