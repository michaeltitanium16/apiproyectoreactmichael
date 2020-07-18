using Aplicacion.ManejadorError;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDTO> {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<CursoUnico, CursoDTO>
        {
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Manejador(CursosOnlineContext context,IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<CursoDTO> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso
                     .Include(x => x.PrecioPromocion)
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.InstructoresLink)
                    .ThenInclude(x => x.Instructor)
                    .FirstOrDefaultAsync(x => x.CursoId == request.Id);

                var cursosDto = _mapper.Map<Curso, CursoDTO>(curso);

                if (cursosDto is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { Mensaje = "No se pudo encontar el curso" });
                }

                return cursosDto;
            }
        }
    }
}
