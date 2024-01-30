using QuanLib.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public class CharacterWidthMapping : ISingleton<CharacterWidthMapping, CharacterWidthMapping.InstantiateArgs>
    {
        private const int LENGTH = 65536;

        private CharacterWidthMapping()
        {
            _items = [];
            for (int i = 0; i < LENGTH; i++)
            {
                char c = (char)i;
                Console.WriteLine();
                Console.Write(c);
                _items.Add(c, Console.CursorLeft);
            }

            Console.WriteLine();
        }

        private CharacterWidthMapping(byte[] cacheBytes)
        {
            ArgumentNullException.ThrowIfNull(cacheBytes, nameof(cacheBytes));
            if (cacheBytes.Length != LENGTH)
                throw new ArgumentException($"缓存文件尺寸不合法，应该为{LENGTH}字节");

            _items = [];
            for (int i = 0; i < LENGTH; i++)
            {
                char c = (char)i;
                _items.Add(c, cacheBytes[i]);
            }
        }

        private static readonly object _slock = new();

        public static bool IsInstanceLoaded => _Instance is not null;

        public static CharacterWidthMapping Instance => _Instance ?? throw new InvalidOperationException("实例未加载");
        private static CharacterWidthMapping? _Instance;

        private readonly Dictionary<char, int> _items;

        public int this[char c] => _items[c];

        public static CharacterWidthMapping LoadInstance(InstantiateArgs args)
        {
            ArgumentNullException.ThrowIfNull(args, nameof(args));

            lock (_slock)
            {
                if (_Instance is not null)
                    throw new InvalidOperationException("试图重复加载单例实例");

                if (args.CacheBytes is null)
                    _Instance = new();
                else
                    _Instance = new(args.CacheBytes);

                return _Instance;
            }
        }

        public int GetWidth(char c)
        {
            return _items[c];
        }

        public int GetWidth(string s)
        {
            ArgumentNullException.ThrowIfNull(s, nameof(s));

            int width = 0;
            foreach (char c in s)
                width += _items[c];

            return width;
        }

        public byte[] BuildCacheBytes()
        {
            byte[] cacheBytes = new byte[LENGTH];
            for (int i = 0; i < LENGTH; i++)
                cacheBytes[i] = (byte)_items[(char)i];
            return cacheBytes;
        }

        public class InstantiateArgs(byte[]? cacheBytes) : Core.InstantiateArgs
        {
            public byte[]? CacheBytes { get; } = cacheBytes;
        }
    }
}
