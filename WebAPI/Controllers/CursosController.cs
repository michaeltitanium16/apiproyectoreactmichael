using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aplicacion;
using MediatR;
using Dominio;
using Aplicacion.Cursos;
using Microsoft.AspNetCore.Authorization;
using Persistencia.Paginacion;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : MiControllerBaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<CursoDTO>>> Get()
        {
            return await Mediator.Send(new Consulta.ListaCursos());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDTO>> Detalle(Guid id)
        {
            return await Mediator.Send(new ConsultaId.CursoUnico { Id = id });
        }
            
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }
        
        [HttpPut("{id}")]
       public  async Task<ActionResult<Unit>> Editar(Guid id ,Editar.Ejecuta data)
        {
            data.CursoId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id )
        {
            return await Mediator.Send(new Eliminar.Ejecuta { Id = id});
        }

        [HttpPost("report")]
        public async Task<ActionResult<PaginacionModel>> Report(PaginacionCurso.Ejecuta data) 
        {
            return await Mediator.Send(data);
       
        }

    }
}