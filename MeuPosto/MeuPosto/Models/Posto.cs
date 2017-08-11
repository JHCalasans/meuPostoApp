using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace MeuPosto.Models
{
    public class Posto
    {
        public Int32 codigo { get; set; }

        public string nome { get; set; }

        public string cep { get; set; }

        public string logradouro { get; set; }

        public string bairro { get; set; }

        public string cnpj { get; set; }

        public float gasolinaComum { get; set; }

        public float diesel { get; set; }

        public float alcool { get; set; }

        public float gasolinaAditivada { get; set; }

        public float gnv { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public Distribuidora distribuidora { get; set; }

        public String distanciaDoPonto { get; set; }

        public double distancia { get; set; }

        public bool debito { get; set; }

        public bool credito { get; set; }

        public bool dinheiro { get; set; }

        public String TiposPagamentos
        {
            get
            {
                String retorno = "Tipos de Pagamentos:";
                if (dinheiro)
                {
                    retorno += " Dinheiro";
                    if (debito)
                        retorno += ", Débito";
                    
                    if (credito)
                        retorno += ", Crédito";
                }
                else if (debito)
                {
                    retorno += " Débito";
                    if (credito)
                        retorno += ", Crédito";
                }
                else if (credito)
                    retorno += " Crédito";
                else
                    retorno += " Não Disponível";

                return retorno;
            }
            set { }
        } 



    }
}
