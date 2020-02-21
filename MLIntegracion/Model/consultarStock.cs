using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLIntegracion.Model
{
    class consultarStock
    {
        public string AliasBodega { get; set; }
        public string IdBodega { get; set; }
        public string Articulo { get; set; }
        public string SKU { get; set; }
        public string Descripcion { get; set; }
        public string IdEstado { get; set; }
        public string Estado { get; set; }
        public string Lote { get; set; }
        public string FechaVencimiento { get; set; }
        public string FechaElaboracion { get; set; }

        public string IdUnidad { get; set; }
        public string Unidad { get; set; }

        public string Maquilado { get; set; }

        public string Cantidad { get; set; }
    }
}
