using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public class CharacterWidthMapping : ISingleton<CharacterWidthMapping, CharacterWidthMapping.InstantiateArgs>
    {
        private CharacterWidthMapping(byte[] mapping)
        {
            ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
            ThrowHelper.ArrayLengthOutOfRange(65536, mapping, nameof(mapping));

            _mapping = mapping;
        }

        private static readonly Lock _slock = new();
        private readonly byte[] _mapping;

        public static bool IsInstanceLoaded => _Instance is not null;

        public static CharacterWidthMapping Instance => _Instance ?? throw new InvalidOperationException("实例未加载");
        private static CharacterWidthMapping? _Instance;

        public byte this[char ch] => _mapping[ch];

        public static CharacterWidthMapping LoadInstance(InstantiateArgs args)
        {
            ArgumentNullException.ThrowIfNull(args, nameof(args));

            lock (_slock)
            {
                if (_Instance is not null)
                    throw new InvalidOperationException("试图重复加载单例实例");

                _Instance = new(args.Mapping);
                return _Instance;
            }
        }

        public int GetWidth(char ch)
        {
            return _mapping[ch];
        }

        public int GetWidth(string? text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            int width = 0;
            foreach (char ch in text)
                width += _mapping[ch];

            return width;
        }

        public class InstantiateArgs(byte[] mapping) : Core.InstantiateArgs
        {
            public byte[] Mapping { get; } = mapping;
        }
    }
}
