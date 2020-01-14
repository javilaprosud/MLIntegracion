using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLIntegracion.Conexion
{
    class Conexion
    {
        public string query_pedidos;
        public string query_recepcion; 

        public SqlConnection procesadorabd()
        {
            SqlConnection sql_conexion = new SqlConnection(@"Data Source=192.168.1.69;Initial Catalog=procesadorabd;user=sa;pwd=procesadora1");
            sql_conexion.Open();

            return sql_conexion;
        }

        public SqlConnection procesadora_analisis()
        {
            SqlConnection sql_conexion = new SqlConnection(@"Data Source=192.168.1.68;Initial Catalog=procesadora_analisis;user=sa;pwd=procesadora1");
            sql_conexion.Open();

            return sql_conexion;
        }

        public string pedidoquery()
        {
            query_pedidos = "select distinct Numero from OP_PENDIENTE_OPERATIVA_2020_v2 where CajasPendientes > 0 and LineaProducto = 'CHOCOLATES PREMIUM'"; 
            return query_pedidos; 
        }
    }
}
