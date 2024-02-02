using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles.Events
{
    public class ConsoleKeyInfoEventArgs : EventArgs
    {
        public ConsoleKeyInfoEventArgs(ConsoleKeyInfo consoleKeyInfo)
        {
            ConsoleKeyInfo = consoleKeyInfo;
        }

        public ConsoleKeyInfo ConsoleKeyInfo { get; }
    }
}
