using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class ConsultaId
    {
        public class Ejecuta : IRequest<InstructorModel>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta,InstructorModel>
        {
            private readonly IInstructor _instructorRepositorio;
            public Manejador(IInstructor instructorRepositorio)
            {
                this._instructorRepositorio = instructorRepositorio;
            }

            public async Task<InstructorModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
               var instructor = await this._instructorRepositorio.ObtenerPorId(request.Id);

                if(instructor == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, "No se encontro el instructor");
                }

                return instructor;
            }
        }
    }
}
