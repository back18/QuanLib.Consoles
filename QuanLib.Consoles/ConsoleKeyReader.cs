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
        public ConsoleKeyReader(ILoggerGetter? loggerGetter = null) : base(loggerGetter)
        {
            ReadTimeSpan = 1;
            KeyEventHandler = new();

            KeyRead += OnKeyRead;
            KeyNotAvailable += OnKeyNotAvailable;
        }

        public int ReadTimeSpan { get; set; }

        public KeyEventHandler KeyEventHandler { get; }

        public event EventHandler<ConsoleKeyReader, ConsoleKeyInfoEventArgs> KeyRead;

        public event EventHandler<ConsoleKeyReader, EventArgs> KeyNotAvailable;

        protected virtual void OnKeyRead(ConsoleKeyReader sender, ConsoleKeyInfoEventArgs e)
        {
            KeyEventHandler.Invoke(KeyInfo.From(e.ConsoleKeyInfo));
        }

        protected virtual void OnKeyNotAvailable(ConsoleKeyReader sender, EventArgs e)
        {

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

                    KeyNotAvailable.Invoke(this, EventArgs.Empty);
                    Thread.Sleep(ReadTimeSpan);
                }

                KeyRead.Invoke(this, new(keyInfo));
            }
        }

        protected override void OnStarted(IRunnable sender, EventArgs e)
        {
            if (Console.CursorLeft != 0)
                Console.WriteLine();
        }
    }
}
