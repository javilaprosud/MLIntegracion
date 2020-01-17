using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            try {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    Console.WriteLine(responseText);
                    if (responseText != "")
                    {
                        Results(responseText);
                        Conexion.Conexion c = new Conexion.Conexion();
                        c.EjecutarLog(pd.documento, "Documento(" + pd.documento + ") consultado exitosamente.", "PROCESADO", "P");
                    }
                    else
                    {
                        Conexion.Conexion c = new Conexion.Conexion();
                        c.EjecutarLog(pd.documento, "Documento(" + pd.documento + ") sin datos.", "SIN DATOS", "P");
                    }
                    Console.ReadKey();
                }
            }
            catch(Exception e)
            {
                Conexion.Conexion c = new Conexion.Conexion();
                c.EjecutarLog(pd.documento, e.ToString(), "NO PROCESADO", "P");
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

        public void Results(string json)
        {

            Conexion.Conexion con = new Conexion.Conexion();
            Model.ConsultarPedido icp = new Model.ConsultarPedido();
            dynamic data = JObject.Parse(json);
            icp.documento = data.Documento;
            var jsonLinq = JObject.Parse(json);
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    if (column.Value is JValue)
                    {

                        cleanRow.Add(column.Name, column.Value);
                    }
                }

                trgArray.Add(cleanRow);
            }

            DataTable dt = JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());

            foreach (DataRow row in dt.Rows)
            {
                icp.posicion = row[0].ToString();
                icp.SKU = row[1].ToString();
                icp.descripcion = row[2].ToString();
                icp.cantSolicitada = row[3].ToString();
                icp.cantidadPicking = row[4].ToString();
                icp.Lote = row[5].ToString();
                icp.fechaVencimiento = row[6].ToString();
                icp.aliasBodega = row[7].ToString();
                icp.fechaPicking = row[8].ToString();

                using (con.procesadorabd())
                {
                    SqlCommand cmd = new SqlCommand(con.insercion_ML(), con.procesadorabd());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@documento", SqlDbType.VarChar).Value = icp.documento;
                    cmd.Parameters.Add("@posicion", SqlDbType.VarChar).Value = icp.posicion;
                    cmd.Parameters.Add("@sku", SqlDbType.VarChar).Value = icp.SKU;
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = icp.descripcion;
                    cmd.Parameters.Add("@cantidad_sol", SqlDbType.VarChar).Value = icp.cantSolicitada;
                    cmd.Parameters.Add("@cantidad_pick", SqlDbType.VarChar).Value = icp.cantidadPicking;
                    cmd.Parameters.Add("@lote", SqlDbType.VarChar).Value = icp.Lote;
                    cmd.Parameters.Add("@fchven", SqlDbType.VarChar).Value = icp.fechaVencimiento;
                    cmd.Parameters.Add("@alias", SqlDbType.VarChar).Value = icp.aliasBodega;
                    cmd.Parameters.Add("@fchpicking", SqlDbType.VarChar).Value = icp.fechaPicking;
                    cmd.ExecuteNonQuery();
                }

            }
        }
    }
}
