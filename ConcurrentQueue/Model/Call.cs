using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentQueue.Model
{
    internal class Call
    {
        public Call()
        {
            Id = Guid.NewGuid();
            DurationSec = new Random().Next(5, 10);
        }

        public Guid Id { get; set; }
        public int DurationSec { get; set; }
    }
}
