using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata.MiniDI.Test.Services
{
    public class FileService : IFileService
    {
        public Guid guid { get; set; }

        public FileService()
        {
            guid = Guid.NewGuid();
        }

        public string Read(string fileName)
        {
            return fileName;
        }
    }

    public interface IFileService
    {
        Guid guid { get; set; }
        string Read(string fileName);
    }
}
