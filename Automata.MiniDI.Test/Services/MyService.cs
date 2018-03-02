using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata.MiniDI.Test.Services
{
    public class MyService :IMyService
    {
        private MyService()
        {

        }

        public MyService(IFileService service)
        {

        }
    }

    public interface IMyService
    {
    }
}
