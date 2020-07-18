using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Documentos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoController : MiControllerBaseController
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> SubirArchivo(SubirArchivo.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArchivoGenerico>> ObtenerDocumento(Guid id)
        {
            return await Mediator.Send(new ObtenerArchivo.Ejecuta { Id = id });
        }
    }
}
