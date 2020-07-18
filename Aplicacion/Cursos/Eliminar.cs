using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }

        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext context;
            public Manejador(CursosOnlineContext _context)
            {
                this.context = _context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var cursoInstructores = context.CursoInstructor.Where(x => x.CursoId == request.Id).ToList();

                foreach(var instructor in cursoInstructores)
                {
                    context.CursoInstructor.Remove(instructor);
                }

                var comentarios = context.Comentario.Where(x => x.CursoId == request.Id).ToList();

                foreach(var cmt in comentarios)
                {
                    context.Comentario.Remove(cmt);
                }

                var precioDB = context.Precio.FirstOrDefault(x => x.CursoId == request.Id);
                if(precioDB != null)
                {
                    context.Precio.Remove(precioDB);
                }

                var cursoEncontrado = await context.Curso.FindAsync(request.Id);

                

                if (cursoEncontrado is null)
                {
                    //throw new Exception("No se pudo encontrar el curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { Mensaje = "No se encontro el curso" });

                }

                context.Curso.Remove(cursoEncontrado);

                var valor = await context.SaveChangesAsync();

                if (valor > 0)
                    return Unit.Value;
                //throw new Exception("No se pudo encontrar el curso");
                throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { Mensaje = "No se pudo eliminar el curso" });
            }
        }
    }
}
