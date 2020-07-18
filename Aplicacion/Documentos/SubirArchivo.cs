using Dominio;
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
    public class SubirArchivo 
    {
        public class Ejecuta : IRequest
        {
            public Guid? ObjetoReferencia { get; set; }
            public string Data { get; set; }
            public string Extension { get; set; }
            public string Nombre { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;

            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var documento = await _context.Documento.FirstOrDefaultAsync(x => x.ObjetoReferencia == request.ObjetoReferencia);

                if(documento == null)
                {
                    var doc = new Documento
                    {
                        Contenido = Convert.FromBase64String(request.Data),
                        Nombre = request.Nombre,
                        Extension = request.Extension,
                        ObjetoReferencia = request.ObjetoReferencia ?? Guid.Empty,
                        FechaCreacion = DateTime.UtcNow,
                        DocumentoId = new Guid()
                    };

                   var resultado = _context.Documento.Add(doc);
                }
                else
                {
                    documento.Contenido = Convert.FromBase64String(request.Data);
                    documento.Nombre = request.Nombre;
                    documento.Extension = request.Extension;
                    documento.FechaCreacion = DateTime.UtcNow;

                }

                var finalResultado = await _context.SaveChangesAsync();

                if(finalResultado > 0)
                {
                    return Unit.Value;
                }
                else
                {
                    throw new Exception("No se pudo guardar los cambios");
                }
            }
        }
    }
}
