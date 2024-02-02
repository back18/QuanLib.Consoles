using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public class KeyEventHandler
    {
        public KeyEventHandler()
        {
            _actions = [];
        }

        private readonly Dictionary<KeyInfo, List<Action>> _actions;

        public void Subscribe(KeyInfo key, Action value)
        {
            if (_actions.TryGetValue(key, out var actions))
                actions.Add(value);
            else
                _actions.Add(key, [value]);
        }

        public void Unsubscribe(KeyInfo key, Action value)
        {
            if (_actions.TryGetValue(key, out var actions))
            {
                actions.Remove(value);
                if (actions.Count == 0)
                    _actions.Remove(key);
            }
        }

        public bool Invoke(KeyInfo key)
        {
            if (_actions.TryGetValue(key, out var actions))
            {
                foreach (var action in actions)
                    action.Invoke();
                return true;
            }

            return false;
        }
    }
}
