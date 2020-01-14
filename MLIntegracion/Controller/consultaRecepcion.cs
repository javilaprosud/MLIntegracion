using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MLIntegracion.Controller
{
    class consultaRecepcion
    {
        public void consultaDocumentoRecepcion()
        {
            Model.Integracion inte = new Model.Integracion { URL = "http://wikets.no-ip.info:82/Api/ConsultarRecepcion" };

            Model.ConsultarRecepcion rc = new Model.ConsultarRecepcion();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(inte.URL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("APIKey", inte.APIKey);
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json_script = "";
                json_script = json_script + "{ \"IDKey\": \""+inte.IDKey+"\", \"Cliente\": \""+inte.Cliente+"\", \"Documento\":\""+rc.documento+"\"}";
                streamWriter.Write(json_script);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                Console.WriteLine(responseText);

            }
        }
    }
}
