using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentQueue.Model
{
    internal class Agent
    {
        public string Name { get; set; }
        public bool IsFree { get; set; }

        public Agent(string name)
        {
            Name = name;
            IsFree = true;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
