using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLIntegracion.Model
{
    class ConsultarRecepcion
    {
        public string documento { get; set; }
        public string posicion { get; set; }
        public string SKU { get; set; }
        public string descripcion { get; set; }
        public string cantidadDespachada { get; set; }
        public string cantidadRecepcion { get; set; }
        public string lote { get; set;}
        public string fechaVencieminto { get; set; }
        public string aliasBodega { get; set; }
        public string fechaRecepcion { get; set; }
        public string tipoDocRecepcion { get; set; }
        public string numDocRecepcion { get; set; }
   
    }
}
