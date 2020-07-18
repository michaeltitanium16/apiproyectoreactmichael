using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public Guid? CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal Precio { get; set; }
            public decimal Promocion { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
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
                Guid cursoID = Guid.NewGuid();

                if (request.CursoId.HasValue)
                {
                    cursoID = request.CursoId.Value;

                }

                Curso curso = new Curso
                {

                    CursoId = cursoID,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion.Value,
                    FechaCreacion = DateTime.UtcNow

                };

                if (request.ListaInstructor != null)
                {
                    foreach (var id in request.ListaInstructor)
                    {
                        var cursoInstructor = new CursoInstructor
                        {
                            CursoId = cursoID,
                            InstructorId = id
                        };

                        context.CursoInstructor.Add(cursoInstructor);
                    }
                }

                if(request.Precio != 0)
                {
                    var precioEntidad = new Precio();
                    precioEntidad.CursoId = cursoID;
                    precioEntidad.PrecioActual = request.Precio;
                    precioEntidad.Promocion = request.Promocion;
                    precioEntidad.PrecioId = Guid.NewGuid();

                    context.Precio.Add(precioEntidad);
                }



                context.Curso.Add(curso);
                var valor = await context.SaveChangesAsync();
                if(valor >0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}
