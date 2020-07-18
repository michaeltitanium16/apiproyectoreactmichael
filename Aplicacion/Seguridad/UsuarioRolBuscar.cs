using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuarioRolBuscar
    {
        public class Ejecuta : IRequest<List<string>>
        {
            public string Username { get; set; }
        }
        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.Username).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta, List<string>>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                this._userManager = userManager;
                this._roleManager = roleManager;
            }
            public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var user = await this._userManager.FindByNameAsync(request.Username);
                if(user == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new { mensaje = "No se encontro al usuario" });
                }
                var roles = await this._userManager.GetRolesAsync(user);
                if(roles == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new { mensaje = "No se encontraron roles asociados al usuario" });
                }

                return roles.ToList();
            }
        }
    }
}
