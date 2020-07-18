using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
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
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructores { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; }
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
               var cursoEncontrado = await context.Curso.FindAsync(request.CursoId);

                if (cursoEncontrado is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { Mensaje = "No se pudo editar el curso" });
                }               
              
                cursoEncontrado.Titulo =request.Titulo ?? cursoEncontrado.Titulo;
                cursoEncontrado.Descripcion = request.Descripcion ?? cursoEncontrado.Descripcion;
                cursoEncontrado.FechaPublicacion = request.FechaPublicacion ?? cursoEncontrado.FechaPublicacion;
                cursoEncontrado.FechaCreacion = DateTime.UtcNow;


                /*Actualizar precio curso*/

                var precioEntidad = context.Precio.FirstOrDefault(x => x.CursoId == request.CursoId);

                if(precioEntidad != null)
                {
                    if (request.Precio != null)
                    {
                        precioEntidad.PrecioActual = request.Precio.Value;
                    }
                    if (request.Promocion != null)
                    {
                        precioEntidad.Promocion = request.Promocion.Value;
                    }
                }
                else
                {
                    var precioNuevo = new Precio
                    {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio.Value,
                        Promocion = request.Promocion.Value,
                        CursoId = cursoEncontrado.CursoId

                    };
                    await context.Precio.AddAsync(precioNuevo);
                }

                if (request.ListaInstructores != null)
                {
                    if (request.ListaInstructores.Count > 0)
                    {
                        /*Eliminar los instructores actuales*/
                        var instructoresBD = context.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();

                        foreach (var instructor in instructoresBD)
                        {
                            context.CursoInstructor.Remove(instructor);
                        }
                        /*Fin de eliminar*/

                        /*Agregar instructores*/
                        foreach (var ids in request.ListaInstructores)
                        {
                            var nuevoInstructor = new CursoInstructor
                            {
                                CursoId = request.CursoId,
                                InstructorId = ids
                            };
                            context.CursoInstructor.Add(nuevoInstructor);
                        }
                        /*fin de agregar */
                    }
                }

                var valor = await context.SaveChangesAsync();
                if (valor > 0)
                    return Unit.Value;

                throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { Mensaje = "No se pudo editar el curso" });

            }
        }
    }
}
