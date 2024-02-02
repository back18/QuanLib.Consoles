using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public readonly struct KeyInfo(ConsoleModifiers modifiers, ConsoleKey key) : IEquatable<KeyInfo>
    {
        public ConsoleModifiers Modifiers { get; } = modifiers;

        public ConsoleKey Key { get; } = key;

        public static KeyInfo From(ConsoleKeyInfo consoleKeyInfo)
        {
            return new(consoleKeyInfo.Modifiers, consoleKeyInfo.Key);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Modifiers);
        }

        public bool Equals(KeyInfo other)
        {
            return this == other;
        }

        public override bool Equals(object? obj)
        {
            return obj is KeyInfo keyInfo && Equals(keyInfo);
        }

        public override string ToString()
        {
            List<string> items = [];
            if (Modifiers.HasFlag(ConsoleModifiers.Control))
                items.Add(ConsoleModifiers.Control.ToString());
            if (Modifiers.HasFlag(ConsoleModifiers.Shift))
                items.Add(ConsoleModifiers.Shift.ToString());
            if (Modifiers.HasFlag(ConsoleModifiers.Alt))
                items.Add(ConsoleModifiers.Alt.ToString());
            items.Add(Key.ToString());
            return string.Join('+', items);
        }

        public static bool operator ==(KeyInfo left, KeyInfo right)
        {
            return left.Modifiers == right.Modifiers && left.Key == right.Key;
        }

        public static bool operator !=(KeyInfo left, KeyInfo right)
        {
            return !(left == right);
        }
    }
}
