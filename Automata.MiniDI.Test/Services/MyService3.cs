using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata.MiniDI.Test.Services
{
    public interface IMyServiceBase
    {
        string key { get; set; }
    }

    public interface IMyService3 : IMyServiceBase
    {

    }

    public interface IMyService4 : IMyServiceBase
    {

    }

    public class MyServiceBase : IMyServiceBase
    {
        public string key { get; set; }

        public MyServiceBase()
        {
            key = GetType().Name;
        }
    }

    public class MyService3 : MyServiceBase, IMyService3
    {
        public MyService3()
        {
            key = GetType().Name;
        }
    }

    public class MyService4 : MyServiceBase, IMyService4
    {
        public MyService4()
        {
            key = GetType().Name;
        }
    }
}
