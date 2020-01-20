using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLIntegracion.Model
{
    class IngresarPedido
    {
        public string PATH { get; set; }
        public string jsonIPedido { get; set; }
        public string PATHProcesado { get; set; }

        public IngresarPedido()
        {
            this.PATH = @"\\172.16.200.5\MLIntegracion\Pedidos"; 
            this.PATHProcesado = @"\\172.16.200.5\MLIntegracion\Pedidos\Procesados";
        }
    }
}
