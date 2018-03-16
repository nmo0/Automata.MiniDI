using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata.MiniDI.Test.Services
{
    public interface IGenericService<T1>
    {
        string GetName();
    }

    public class Model1
    {

    }
    public class Model2
    {

    }

    public class GenericService1 : IGenericService<Model1>
    {
        public string GetName()
        {
            return "Service1";
        }
    }


    public class GenericService2 : IGenericService<Model2>
    {
        public string GetName()
        {
            return "Service2";
        }
    }
}
