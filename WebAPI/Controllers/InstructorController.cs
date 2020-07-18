using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Aplicacion.Instructores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : MiControllerBaseController
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructores()
        {
            return await Mediator.Send(new Aplicacion.Instructores.Consulta.Lista());
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Aplicacion.Instructores.Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Actualizar(Guid id,Aplicacion.Instructores.Editar.Ejecuta data)
        {
            data.InstructorId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar (Guid id)
        {
            return await Mediator.Send(new Aplicacion.Instructores.Eliminar.Ejecuta { Id = id });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorModel>> ObtenerPorId(Guid id)
        {
            return await Mediator.Send(new Aplicacion.Instructores.ConsultaId.Ejecuta { Id = id });
        }
    }
}