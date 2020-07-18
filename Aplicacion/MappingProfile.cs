using Aplicacion.Cursos;
using AutoMapper;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aplicacion
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<Curso, CursoDTO>()
                .ForMember(x => x.Instructores, y => y.MapFrom(z => z.InstructoresLink.Select(a => a.Instructor).ToList()))
                .ForMember(x => x.Comentarios, y => y.MapFrom(z => z.ComentarioLista))
                .ForMember(x => x.Precio, y => y.MapFrom(z => z.PrecioPromocion));

            CreateMap<CursoInstructor, CursoInstructorDTO>();
            CreateMap<Dominio.Instructor, InstructorDTO>();
            CreateMap<Comentario, ComentarioDTO>();
            CreateMap<Precio, PrecioDTO>();
        }
    }
}
