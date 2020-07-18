using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : MiControllerBaseController
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Aplicacion.Comentarios.Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await Mediator.Send(new Aplicacion.Comentarios.Eliminar.Ejecuta { ComentarioId = id });
        }
    }
}