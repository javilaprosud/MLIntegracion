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
            this.PATH = @"C:\MLIntegracion\Recepcion";
            this.PATHProcesado = @"C:\MLIntegracion\Recepcion\Procesados";
        }
    }
}
