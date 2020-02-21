using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLIntegracion.Conexion
{
    class Conexion
    {

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

        public SqlConnection Prosud_BI_A()
        {
            SqlConnection sql_conexion = new SqlConnection(@"Data Source=192.168.1.68;Initial Catalog=PROSUD_BI;user=sa;pwd=procesadora1");
            sql_conexion.Open();

            return sql_conexion;
        }

        public string pedidoquery()
        {
            string query_pedidos;
            //query_pedidos = "select '353612' as numero ";
            query_pedidos = "select distinct Numero from Prosud_BI.dbo.OP_PENDIENTE_OPERATIVA_24M where Cod_Producto like '%LN%' and  LineaProducto = 'CHOCOLATES PREMIUM' and CajasPendientes > 0"; 
            return query_pedidos; 
        }

        public string insercion_ML()
        {
            string query_SP;
            query_SP ="SP_MLInsercion";
            return query_SP;
        }


        public string insercion_MLS()
        {
            string query_SP;
            query_SP = "SP_MLInsercionStock";
            return query_SP;
        }


        public string recepcionquery()
        {
            string query_recepcion;
            query_recepcion = "SELECT distinct PF_NRO FROM PROSUD_BI.dbo.TB_ABASTECIMIENTO_temp where PF_ProdCodigo like '%CHOC%' or PF_ProdCodigo like '%LINDT%' or PF_ProdCodigo like '%LN%'";
            return query_recepcion; 
        }
        public string insercionLog()
        {
            string query_SP_Log;
            query_SP_Log = "SP_InsercionLog";
            return query_SP_Log; 

        }
        public string insercion_ML_Rep()
        {
            string query_SP;
            query_SP = "SP_MLInsercion_Rep";
            return query_SP;
        }

        public void EjecutarLog (string documento, string info, string estado, string tipo)
        {
            using (procesadora_analisis())
            {
                SqlCommand cmd = new SqlCommand(insercionLog(), procesadora_analisis());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@documento", SqlDbType.VarChar).Value = documento;
                cmd.Parameters.Add("@info", SqlDbType.VarChar).Value = info;
                cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = estado;
                cmd.Parameters.Add("@tipo", SqlDbType.VarChar).Value = tipo;
                cmd.ExecuteNonQuery();
            }
        }

       public void SP_Abastecimiento()
        {
            SqlCommand cmd = new SqlCommand("SP_COMPRAS_DISPO_PF", procesadorabd());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@tiempo", SqlDbType.VarChar).Value = "1";
            cmd.Parameters.Add("@destino", SqlDbType.VarChar).Value = "T";
            cmd.Parameters.Add("@periodos", SqlDbType.VarChar).Value = "M";
            cmd.CommandTimeout = 0;
            cmd.ExecuteNonQuery();
        }

        public void SP_OPPendientes()
        {
            SqlCommand cmd = new SqlCommand("SP_OP_PENDIENTE_OPERATIVA_2", procesadorabd());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@tiempo", SqlDbType.VarChar).Value = "1";
            cmd.Parameters.Add("@agno", SqlDbType.VarChar).Value = "2020";
            cmd.Parameters.Add("@destino", SqlDbType.VarChar).Value = "T";
            cmd.CommandTimeout = 0; 
            cmd.ExecuteNonQuery();
        }
    }
}
