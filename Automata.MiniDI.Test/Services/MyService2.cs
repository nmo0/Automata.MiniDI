using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata.MiniDI.Test.Services
{
    public class MyService2: IMyService2
    {
        private MyService2()
        {

        }

        public MyService2(IMyService service, OtherService other)
        {

        }
    }

    public interface IMyService2
    {

    }
}
