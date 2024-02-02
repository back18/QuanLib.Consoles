using QuanLib.Consoles.Events;
using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public class ConsoleKeyReader : RunnableBase
    {
        public ConsoleKeyReader(ILogbuilder? logbuilder = null) : base(logbuilder)
        {
            KeyRead += OnKeyRead;
            KeyEventHandler = new();
        }

        public KeyEventHandler KeyEventHandler { get; }

        public event EventHandler<ConsoleKeyReader, ConsoleKeyInfoEventArgs> KeyRead;

        protected virtual void OnKeyRead(ConsoleKeyReader sender, ConsoleKeyInfoEventArgs e)
        {
            KeyEventHandler.Invoke(KeyInfo.From(e.ConsoleKeyInfo));
        }

        protected override void Run()
        {
            while (IsRunning)
            {
                ConsoleKeyInfo keyInfo;
                while (true)
                {
                    if (!IsRunning)
                    {
                        return;
                    }

                    if (Console.KeyAvailable)
                    {
                        keyInfo = Console.ReadKey(true);
                        break;
                    }

                    Thread.Sleep(1);
                }

                KeyRead.Invoke(this, new(keyInfo));
            }
        }
    }
}
