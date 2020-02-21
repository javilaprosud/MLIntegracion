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
    class consultaStock
    {

        public void consultaDocumento()
        {
            Model.Integracion inte = new Model.Integracion { URL = "http://190.153.223.174:82/Api/ConsultarStock" };
    
            
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(inte.URL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("APIKey", inte.APIKey);
            httpWebRequest.Method = "POST";


            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json_script = "";
                json_script = json_script + "{ \"IDKey\": \"" + inte.IDKey + "\", \"Cliente\": \"" + inte.Cliente + "\", \"SKU\": \"" + "" + "\", \"Lote\": \"" + "" + "\"}";
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
                          c.EjecutarLog("CSTOCK", "Documento consultado exitosamente.", "PROCESADO", "S");
                      }
                      else
                      {
                          Conexion.Conexion c = new Conexion.Conexion();
                          c.EjecutarLog("CSTOCK", "Documento sin datos.", "SIN DATOS", "S");
                      }
                      // Console.ReadKey();
                  }
              }
              catch (Exception e)
              {
                  Conexion.Conexion c = new Conexion.Conexion();
                  c.EjecutarLog("CSTOCK", e.ToString(), "NO PROCESADO", "S");
              }

        }




        public void Results(string json)
        {
            Conexion.Conexion con = new Conexion.Conexion();
            var obj = JsonConvert.DeserializeObject<List<Model.consultarStock>>(json);


            using (con.procesadorabd())
            {
                var eliminar = ("truncate table MLStockProcesado");
                SqlCommand cmd = new SqlCommand(eliminar, con.procesadorabd());
       
                cmd.ExecuteNonQuery();

            }


            foreach (Model.consultarStock icp in obj) {

                using (con.procesadorabd())
                {
                    
                    SqlCommand cmd = new SqlCommand(con.insercion_MLS(), con.procesadorabd());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AliasBodega", SqlDbType.VarChar).Value = icp.AliasBodega;
                    cmd.Parameters.Add("@IdBodega", SqlDbType.VarChar).Value = icp.IdBodega.ToString();
                    cmd.Parameters.Add("@Articulo", SqlDbType.VarChar).Value = icp.Articulo.ToString();
                    cmd.Parameters.Add("@SKU", SqlDbType.VarChar).Value = icp.SKU;
                    cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar).Value = icp.Descripcion;
                    cmd.Parameters.Add("@IdEstado", SqlDbType.VarChar).Value = icp.IdEstado;
                    cmd.Parameters.Add("@Estado", SqlDbType.VarChar).Value = icp.Estado;
                    cmd.Parameters.Add("@Lote", SqlDbType.VarChar).Value = icp.Lote;
                    cmd.Parameters.Add("@fchven", SqlDbType.VarChar).Value = icp.FechaVencimiento;
                    cmd.Parameters.Add("@fchela", SqlDbType.VarChar).Value = icp.FechaElaboracion;
                    cmd.Parameters.Add("@IdUnidad", SqlDbType.VarChar).Value = icp.IdUnidad;
                    cmd.Parameters.Add("@Unidad", SqlDbType.VarChar).Value = icp.Unidad;
                    cmd.Parameters.Add("@Maquilado", SqlDbType.VarChar).Value = icp.Maquilado.ToString();
                    cmd.Parameters.Add("@Cantidad", SqlDbType.VarChar).Value = icp.Cantidad.ToString();
                    cmd.ExecuteNonQuery();
                   
                }




            }

        }




    }
}
