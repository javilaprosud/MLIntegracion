using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLIntegracion
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller.ingresoPedido ip = new Controller.ingresoPedido();
            //ip.ingresarPedido();

            Controller.consultaPedidos cp = new Controller.consultaPedidos();
            cp.obtenerDocumento();
        }
    }
}
