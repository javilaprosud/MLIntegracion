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
            foreach (string arg in args)
            {
                if (arg == "IP")
                {
                    Controller.ingresoPedido ip = new Controller.ingresoPedido();
                    ip.ingresarPedido();
                }
                if (arg == "IR")
                {
                    Controller.ingresoRecepcion ir = new Controller.ingresoRecepcion();
                    ir.ingresarRecepcion();
                }
                if (arg == "CP")
                {
                    Controller.consultaPedidos cp = new Controller.consultaPedidos();
                    cp.obtenerDocumento();
                }
                if (arg == "CR")
                {
                    Controller.consultaRecepcion cr = new Controller.consultaRecepcion();
                    cr.obtenerDocumento();
                }
            }

            //Controller.ingresoRecepcion ir = new Controller.ingresoRecepcion();
            //ir.ingresarRecepcion();
        }
    }
}
