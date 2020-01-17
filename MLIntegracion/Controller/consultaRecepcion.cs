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
    class consultaRecepcion
    {
        public void consultaDocumentoRecepcion(string nro)
        {
            Model.Integracion inte = new Model.Integracion { URL = "http://wikets.no-ip.info:82/Api/ConsultarRecepcion" };

            Model.ConsultarRecepcion rc = new Model.ConsultarRecepcion();
            rc.documento = nro;
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
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                   
                    var responseText = streamReader.ReadToEnd();
                    Console.WriteLine(responseText);
                    if (responseText != "")
                    {
                        Results(responseText);
                        Conexion.Conexion c = new Conexion.Conexion();
                        c.EjecutarLog(rc.documento, "Documento(" + rc.documento + ") consultado exitosamente.", "PROCESADO", "R");
                    }
                    else
                    {
                        Conexion.Conexion c = new Conexion.Conexion();
                        c.EjecutarLog(rc.documento, "Documento(" + rc.documento + ") sin datos.", "SIN DATOS", "R");
                    }
                  //  Console.ReadKey();
                }
            }
            catch(Exception e)
            {
                Conexion.Conexion c = new Conexion.Conexion();
                c.EjecutarLog(rc.documento, e.ToString(), "NO PROCESADO", "R");

            }

        }
        public void obtenerDocumento()
        {
            Conexion.Conexion conn = new Conexion.Conexion();
            conn.SP_Abastecimiento();
            DataTable dt = new DataTable();
            using (conn.procesadorabd())
            {
                SqlCommand cmd = new SqlCommand(conn.recepcionquery(), conn.procesadorabd());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        consultaDocumentoRecepcion(row[dc].ToString());
                    }
                }

            }
        }

        public void Results(string json)
        {

            Conexion.Conexion con = new Conexion.Conexion();
            Model.ConsultarRecepcion icp = new Model.ConsultarRecepcion();
            dynamic data = JObject.Parse(json);
            icp.documento = data.Documento;
            icp.tipoDocRecepcion = data.TipoDocRecepcion;
            icp.numDocRecepcion = data.NumDocRecepcion; 
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
                icp.cantidadDespachada = row[3].ToString();
                icp.cantidadRecepcion = row[4].ToString();
                icp.lote = row[5].ToString();
                icp.fechaVencieminto = row[6].ToString();
                icp.aliasBodega = row[7].ToString();
                icp.fechaRecepcion = row[8].ToString();

                using (con.procesadorabd())
                {
                    SqlCommand cmd = new SqlCommand(con.insercion_ML_Rep(), con.procesadorabd());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@documento", SqlDbType.VarChar).Value = icp.documento;
                    cmd.Parameters.Add("@tipodoc", SqlDbType.VarChar).Value = icp.tipoDocRecepcion;
                    cmd.Parameters.Add("@numdoc", SqlDbType.VarChar).Value = icp.numDocRecepcion;
                    cmd.Parameters.Add("@posicion", SqlDbType.VarChar).Value = icp.posicion;
                    cmd.Parameters.Add("@sku", SqlDbType.VarChar).Value = icp.SKU;
                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = icp.descripcion;
                    cmd.Parameters.Add("@cantidad_des", SqlDbType.VarChar).Value = icp.cantidadDespachada;
                    cmd.Parameters.Add("@cantidad_rec", SqlDbType.VarChar).Value = icp.cantidadRecepcion;
                    cmd.Parameters.Add("@lote", SqlDbType.VarChar).Value = icp.lote;
                    cmd.Parameters.Add("@fchven", SqlDbType.VarChar).Value = icp.fechaVencieminto;
                    cmd.Parameters.Add("@alias", SqlDbType.VarChar).Value = icp.aliasBodega;
                    cmd.Parameters.Add("@fchrecepcion", SqlDbType.VarChar).Value = icp.fechaRecepcion;
                    cmd.ExecuteNonQuery();
                }

            }
        }
    }
}
