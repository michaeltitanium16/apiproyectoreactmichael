using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : MiControllerBaseController
    {
        [HttpPost("crear")]
        public async Task<ActionResult<Unit>> Crear(RolNuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpDelete("eliminar")]
        public async Task<ActionResult<Unit>> Eliminar(Eliminar.Ejecuta data)
        {
            return await Mediator.Send(data);
        }
        [HttpGet("lista")]
        public async Task<ActionResult<List<IdentityRole>>> ListaRoles()
        {
            return await Mediator.Send(new RolLista.Ejecuta());
        }



        [HttpPost("agregarrolusuario")]
        public async Task<ActionResult<Unit>> AgregarRoleUsuario(UsuarioRolAgregar.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpDelete("eliminarrolusuario")]
        public async Task<ActionResult<Unit>> EliminarRolUsuario(UsuarioRolEliminar.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<List<string>>> ListaRolesUsuario(string  username)
        {
           return await Mediator.Send(new UsuarioRolBuscar.Ejecuta { Username = username});

        }
    }




}