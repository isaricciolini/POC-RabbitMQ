using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer
{
    public class Mensagem
    {
        public DateTime DataHora = DateTime.Now;
        public int NumeroPDV { get; set; }
        public string Operador { get; set; }
        public string Descricao { get; set; }
    }

    
}
