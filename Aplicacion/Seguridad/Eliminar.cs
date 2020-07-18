using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {        
            public string Nombre { get; set; }
        }
    }
    public class ValidaEjecuta : AbstractValidator<Eliminar.Ejecuta>
    {
        public ValidaEjecuta()
        {
            RuleFor(x => x.Nombre).NotEmpty();
        }
    }

    public class Manejador : IRequestHandler<Eliminar.Ejecuta>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public Manejador(RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
        }
        public async Task<Unit> Handle(Eliminar.Ejecuta request, CancellationToken cancellationToken)
        {
          var role = await  this._roleManager.FindByNameAsync(request.Nombre);

            if(role != null)
            {
              var resultado = await  this._roleManager.DeleteAsync(role);

                if (resultado.Succeeded)
                {
                    return Unit.Value;
                }
            }

            throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, "No se encontro el rol a eliminar");
        }
    }
}
