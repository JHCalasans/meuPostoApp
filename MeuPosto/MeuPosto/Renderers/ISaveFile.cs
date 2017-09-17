using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuPosto.Renderers
{
    public interface ISaveFile
    {
        Task<string> SaveFiles(string filename, byte[] bytes);
    }
}
