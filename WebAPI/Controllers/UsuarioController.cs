using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : MiControllerBaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Registrar.Ejecuta data)
        {
            return await Mediator.Send(data);
        }
        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario()
        {
            return await Mediator.Send(new UsuarioActual.Ejecuta());
        }

        [HttpPut]
        public async Task<ActionResult<UsuarioData>> Actualizar(UsuarioActualizar.Ejecuta data)
        {
            return await Mediator.Send(data);
        }
    }
}
