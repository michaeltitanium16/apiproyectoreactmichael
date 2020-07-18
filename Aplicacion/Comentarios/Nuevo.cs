using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Comentarios
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Alumno { get; set; }
            public int Puntaje { get; set; }
            public string Comentario { get; set; }
            public Guid CursoId { get; set; }
        }

        public class Validador : AbstractValidator<Ejecuta>
        {
            public Validador()
            {
                RuleFor(x => x.Alumno).NotEmpty();
                RuleFor(x => x.Puntaje).NotEmpty();
                RuleFor(x => x.Comentario).NotEmpty();
                RuleFor(x => x.CursoId).NotEmpty();

            }
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
                var comentario = new Comentario
                {
                    ComentarioId = new Guid(),
                    Alumno = request.Alumno,
                    ComentarioTexto = request.Comentario,
                    Puntaje = request.Puntaje,
                    CursoId = request.CursoId,
                    FechaCreacion = DateTime.UtcNow
                };

               _cursosOnlineContext.Comentario.Add(comentario);
               var resultado = await  _cursosOnlineContext.SaveChangesAsync();
                if(resultado > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo guardar el comentario");
            }
        }
    }
}
