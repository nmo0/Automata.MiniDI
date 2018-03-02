using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata.MiniDI.Test.Services
{
    public class OtherService
    {
        public Guid guid { get; set; }

        public OtherService()
        {
            guid = Guid.NewGuid();
        }
    }
}
