using CRUDbanco.Datos.Intefaz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDbanco.Datos.Implementacion
{
    internal class DaoCliente : IDaoCliente
    {
        public List<TipoCuenta> ObtenerCuentas()
        {
            List<TipoCuenta> lst = new List<TipoCuenta>();
            string sp_nombre = "sp_consultarCeuntas";

            DataTable td = Helper.ObtenerInstancia().CargarCombo(sp_nombre);

            foreach(DataRow dr in td.Rows)
            {
                int id = Convert.ToInt32(dr["id_tipoCuenta"].ToString());
                string nombre = dr["nombre"].ToString();

                TipoCuenta aux = new TipoCuenta(id,nombre);
                lst.Add(aux);
            }
            return lst;
        }

        public int ObtenerProximo()
        {
            string sp_nombre = "ProximoCliente";
            string OutPut = "@next";
            return Helper.ObtenerInstancia().ObtenerProximo(sp_nombre, OutPut);
        }
    }
}
