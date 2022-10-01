using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDbanco.Servicios.Interfaz
{
    internal interface IServicio
    {
        int ObtenerProximo();

        List<TipoCuenta> ObtenerCuentas();
    }
}
