using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Comentarios
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid ComentarioId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _cursosOnlineContext;
            public Manejador(CursosOnlineContext cursosOnlineContext)
            {
                this._cursosOnlineContext = cursosOnlineContext;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
               var comentario =  await _cursosOnlineContext.Comentario.FindAsync(request.ComentarioId);
                if(comentario == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new { mensaje = "no se encontro el comentario" });
                }
                _cursosOnlineContext.Comentario.Remove(comentario);
                var resultado =  await _cursosOnlineContext.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se elimino ninguno comentario");
            }
        }
    }
}
