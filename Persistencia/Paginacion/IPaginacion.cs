using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Paginacion
{
    public interface IPaginacion
    {
        Task<PaginacionModel> devolverPaginacion(
            string storeProcedure,
            int numeroPagina,
            int cantidadElementos,
            IDictionary<string,object> parametrosFilto,
            string ordenamientoColumna
            );
    }
}
