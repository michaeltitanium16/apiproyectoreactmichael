using Aplicacion.ManejadorError;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Documentos
{
    public class ObtenerArchivo
    {
        public class Ejecuta : IRequest<ArchivoGenerico>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, ArchivoGenerico>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;

            }
            public async  Task<ArchivoGenerico> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
               var documento =  await  _context.Documento.FirstAsync(x => x.ObjetoReferencia == request.Id);

                if(documento != null)
                {
                    ArchivoGenerico archivoGenerico = new ArchivoGenerico
                    {
                        Data = Convert.ToBase64String(documento.Contenido),
                        Nombre = documento.Nombre,
                        Extension = documento.Extension
                    };

                    return archivoGenerico;
                }
                else
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new { mensaje = "No se encontro la imagen" });
                }
            }
        }
    }
}
