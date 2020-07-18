using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }

        }


        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepositorio;
            public Manejador(IInstructor instructorRepositorio)
            {
                this._instructorRepositorio = instructorRepositorio;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
               var resultados = await this._instructorRepositorio.Elimina(request.Id);
                if(resultados > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar al instructor");
            }
        }
    }
}
