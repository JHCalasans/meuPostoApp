using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuPosto.Models
{
    public class Distribuidora
    {
        public int codigo { get; set; }


        public String descricao { get; set; }


        public byte[] logo { get; set; }


        public bool ativo { get; set; }
    }
}
