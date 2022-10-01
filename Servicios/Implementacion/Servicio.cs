using CRUDbanco.Datos.Implementacion;
using CRUDbanco.Datos.Intefaz;
using CRUDbanco.Servicios.Interfaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDbanco.Servicios.Implementacion
{
    internal class Servicio : IServicio
    {
        private IDaoCliente dao;

        public Servicio()
        {
            dao = new DaoCliente();
        }

        public List<TipoCuenta> ObtenerCuentas()
        {
            return dao.ObtenerCuentas();
        }

        public int ObtenerProximo()
        {
           return dao.ObtenerProximo();
        }
    }
}
