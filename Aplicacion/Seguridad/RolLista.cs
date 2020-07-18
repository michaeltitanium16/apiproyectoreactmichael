using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class RolLista
    {
        public class Ejecuta : IRequest<List<IdentityRole>>
        {

        }
        public class Manejador : IRequestHandler<Ejecuta, List<IdentityRole>>
        {
            private readonly CursosOnlineContext _cursosOnlineContext;
            public Manejador(CursosOnlineContext cursosOnlineContext)
            {
                this._cursosOnlineContext = cursosOnlineContext;
            }

            public  async Task<List<IdentityRole>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                 var listaRoles =  await this._cursosOnlineContext.Roles.ToListAsync();

                return listaRoles;
            }
        }
    }
}
