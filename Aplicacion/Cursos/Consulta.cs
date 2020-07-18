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
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDTO>> { }

        public class Manejador : IRequestHandler<ListaCursos, List<CursoDTO>>
        {
            private readonly CursosOnlineContext context;
            private readonly IMapper _mapper;
            public Manejador(CursosOnlineContext _context,IMapper mapper)
            {
                this.context = _context;
                this._mapper = mapper;
            }

            public async Task<List<CursoDTO>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await context.Curso           
                    .Include(x=> x.PrecioPromocion)
                    .Include(x=> x.ComentarioLista)
                    .Include(x => x.InstructoresLink)
                    .ThenInclude(x=> x.Instructor)
                    .ToListAsync();

                var cursosDto = _mapper.Map<List<Curso>, List<CursoDTO>>(cursos);

                if (cursos is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { Mensaje = "No se pudo encontar el curso" });
                }

                return cursosDto;
            }
        }
    }
}
