using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLIntegracion.Model
{
    class ConsultarPedido
    {
        public string documento { get; set; }
        public string posicion { get; set; }
        public string SKU { get; set; }
        public string descripcion { get; set; }
        public string cantSolicitada { get; set; }
        public string cantidadPicking { get; set; }
        public string Lote { get; set; }
        public string fechaVencimiento { get; set; }
        public string aliasBodega { get; set; }
        public string fechaPicking { get; set; }

    }
}
