using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLIntegracion.Model
{
    class IngresarRecepcion
    {
        public string PATH { get; set; }
        public string jsonIPedido { get; set; }
        public string PATHProcesado { get; set; }

        public IngresarRecepcion()
        {
            this.PATH = @"\\172.16.200.5\MLIntegracion\Recepcion";
            this.PATHProcesado = @"\\172.16.200.5\MLIntegracion\Recepcion\Procesados";
        }
    }
}
