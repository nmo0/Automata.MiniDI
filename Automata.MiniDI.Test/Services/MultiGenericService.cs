using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata.MiniDI.Test.Services
{
    public interface IMultiGenericService<T1, T2>
    {
        string GetName();
    }

    public class Model3
    {

    }

    public class Model4
    {

    }

    public class MultiGenericService1 : IMultiGenericService<Model3, Model4>
    {
        public string GetName()
        {
            return "Multi Service1";
        }
    }


    public class MultiGenericService2 : IMultiGenericService<Model1, Model2>
    {
        public string GetName()
        {
            return "Multi Service2";
        }
    }
}
