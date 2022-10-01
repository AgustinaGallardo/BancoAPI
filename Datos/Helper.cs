using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace CRUDbanco.Datos
{
    internal class Helper
    {
        private static Helper instancia;
        SqlConnection cnn = new SqlConnection(Properties.Resources.cnnCRUDbanco);



        public static Helper ObtenerInstancia()
        {
            if (instancia == null)
                instancia = new Helper();
            return instancia;
        }

        public int ObtenerProximo(string sp_nombre,string OutPut)
        {
            cnn.Open();
            SqlCommand cmdNext = new SqlCommand();
            cmdNext.CommandText = sp_nombre;
            cmdNext.CommandType = CommandType.StoredProcedure;
            cmdNext.Connection = cnn;

            SqlParameter pOutPut = new SqlParameter();
            pOutPut.ParameterName= OutPut;
            pOutPut.Direction = ParameterDirection.Output;
            pOutPut.DbType = DbType.Int32;

            cmdNext.Parameters.Add(pOutPut);
           
            
            cmdNext.ExecuteNonQuery();
            cnn.Close();

            return (int)pOutPut.Value;
        }

        public DataTable CargarCombo(string sp_nombre)
        {
            DataTable dt = new DataTable();
            cnn.Open();
            SqlCommand cmdCombo = new SqlCommand();
            cmdCombo.Connection=cnn;
            cmdCombo.CommandText = sp_nombre;
            cmdCombo.CommandType= CommandType.StoredProcedure;

            dt.Load(cmdCombo.ExecuteReader());
            cnn.Close();

                return dt;  
        }

        public bool ConfirmarCliente(Clientes oCliente)
        {
            bool ok = true;
            SqlTransaction t = null;

            try
            {
                
                SqlCommand cmdMaestro = new SqlCommand();
                cnn.Open();
                t = cnn.BeginTransaction();
                


                cmdMaestro.Connection=cnn;
                cmdMaestro.Transaction=t;
                cmdMaestro.CommandType= CommandType.StoredProcedure;
                cmdMaestro.CommandText="insertCliente";

                cmdMaestro.Parameters.AddWithValue("@apellido", oCliente.Apellido);
                cmdMaestro.Parameters.AddWithValue("@nombre", oCliente.Nombre);
                cmdMaestro.Parameters.AddWithValue("@dni", oCliente.Dni);

                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName="@cod_cliente";
                parameter.Direction= ParameterDirection.Output;
                parameter.DbType= DbType.Int32;

                cmdMaestro.Parameters.Add(parameter);

                cmdMaestro.ExecuteNonQuery();

                int cod_Cliente = (int)parameter.Value;

                foreach (Cuenta item in oCliente.Cuentas)
                {
                    SqlCommand cmdDetalle = new SqlCommand();
                    cmdDetalle.Connection=cnn;
                    cmdDetalle.CommandText="insertCuenta";
                    cmdDetalle.CommandType= CommandType.StoredProcedure;
                    cmdDetalle.Transaction=t;

                    cmdDetalle.Parameters.AddWithValue("@cbu", item.Cbu);
                    cmdDetalle.Parameters.AddWithValue("@id_tipoCuenta", item.tipoCuenta.Id);
                    cmdDetalle.Parameters.AddWithValue("@saldo", item.Saldo);
                    cmdDetalle.Parameters.AddWithValue("@ultimoMovimiento", item.UltimoMovimiento);
                    cmdDetalle.Parameters.AddWithValue("@cod_cliente", cod_Cliente);

                    cmdDetalle.ExecuteNonQuery();
                }
            
                t.Commit();

            }
            catch (Exception )
            {
                if(t != null)
                    t.Rollback();
                return ok = false;  

            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open) 
                cnn.Close();
            }

            return ok;
        }

    }
}
