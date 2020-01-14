using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace MLIntegracion.Controller
{
    class consultaPedidos
    {

        public void consultaDocumento(string nro)
        {
            Model.Integracion inte = new Model.Integracion { URL = "http://wikets.no-ip.info:82/Api/ConsultarPedido" };
            Model.ConsultarPedido pd = new Model.ConsultarPedido();
            pd.documento = nro;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(inte.URL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("APIKey", inte.APIKey);
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json_script = "";
                json_script = json_script + "{ \"IDKey\": \"" + inte.IDKey + "\", \"Cliente\": \"" + inte.Cliente + "\", \"Documento\":\"" + pd.documento + "\"}";
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

        public void obtenerDocumento()
        {
            Conexion.Conexion conn = new Conexion.Conexion();
            DataTable dt = new DataTable();
            using (conn.procesadorabd())
            {
                SqlCommand cmd = new SqlCommand(conn.pedidoquery(), conn.procesadorabd());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        consultaDocumento(row[dc].ToString()); 
                    }
                }

            }
        }
    }
}
