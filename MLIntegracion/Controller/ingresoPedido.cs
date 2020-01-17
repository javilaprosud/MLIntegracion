using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MLIntegracion.Controller
{
    class ingresoPedido
    {

        public void ingresarPedido()
        {
            Model.Integracion inte = new Model.Integracion { URL = "http://wikets.no-ip.info:82/Api/IngresarPedido" };

            Model.IngresarPedido pd = new Model.IngresarPedido();
 
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(inte.URL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("APIKey", inte.APIKey);
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                DirectoryInfo di = new DirectoryInfo(pd.PATH);

                foreach (var fi in di.GetFiles())
                {
                    string archivo_origen = pd.PATH + "\\" + fi.Name;

                    using (StreamReader jsonStream = File.OpenText(archivo_origen))
                    {
                        pd.jsonIPedido = jsonStream.ReadToEnd();
                    }


                    streamWriter.Write(pd.jsonIPedido);
                    streamWriter.Flush();
                    string archivo_destino = pd.PATHProcesado + "\\" + fi.Name;

                    System.IO.File.Move(archivo_origen, archivo_destino);

                }
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    Console.WriteLine(responseText);
                    Conexion.Conexion c = new Conexion.Conexion();
                    c.EjecutarLog("IGRS_P", "Pedido enviado exitosamente a ML.", "PROCESADO", "P");

                   // Console.ReadKey();


                }
            }catch(Exception e)
            {
                Conexion.Conexion c = new Conexion.Conexion();
                c.EjecutarLog("IGRS_P", e.ToString(), "NO PROCESADO", "P");
                Console.ReadKey();
            }
        }
    }
}
