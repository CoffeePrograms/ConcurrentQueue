using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentQueue.Model
{
    internal interface ILogger
    {
        void Log(string message);
    }
}
