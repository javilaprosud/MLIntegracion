using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MLIntegracion.Controller
{
    class ingresoRecepcion
    {
        public void ingresarRecepcion()
        {
            Model.Integracion inte = new Model.Integracion { URL = "http://wikets.no-ip.info:82/Api/IngresarRecepcion" };

            Model.IngresarRecepcion rc = new Model.IngresarRecepcion();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(inte.URL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("APIKey", inte.APIKey);
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                DirectoryInfo di = new DirectoryInfo(rc.PATH);

                foreach (var fi in di.GetFiles())
                {
                    string archivo_origen = rc.PATH + "\\" + fi.Name;

                    using (StreamReader jsonStream = File.OpenText(archivo_origen))
                    {
                        rc.jsonIPedido = jsonStream.ReadToEnd();
                    }


                    streamWriter.Write(rc.jsonIPedido);
                    streamWriter.Flush();
                    string archivo_destino = rc.PATHProcesado + "\\" + fi.Name;

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
                    c.EjecutarLog("IGRS_R", "Recepcion enviada exitosamente a ML.", "PROCESADO", "R");

                   // Console.ReadKey();


                }
            }
            catch (Exception e)
            {
                Conexion.Conexion c = new Conexion.Conexion();
                c.EjecutarLog("IGRS_R", e.ToString(), "NO PROCESADO", "R");
                Console.ReadKey();
            }
        }
    }
}
