﻿using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Grado { get; set; }
        
        }

        public class EjecutaValida : AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Grado).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRespository;
            public Manejador(IInstructor instructorRepository)
            {
                _instructorRespository = instructorRepository;
            }
            public async  Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
              var resultado =await _instructorRespository.Nuevo(request.Nombre, request.Apellidos, request.Grado);

                if(resultado > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("no se pudo insertar el instructor");
            }
        }
    }
}
