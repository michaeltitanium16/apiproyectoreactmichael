using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ExportarDocumento : MiControllerBaseController
    {
        [HttpGet]
        public async Task<ActionResult<Stream>> GetTask()
        {
            return await Mediator.Send(new ExportPDF.Consulta());
        }
    }
}
