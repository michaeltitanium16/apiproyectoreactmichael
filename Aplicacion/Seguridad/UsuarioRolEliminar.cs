using Aplicacion.ManejadorError;
using Dominio;
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
    public class UsuarioRolEliminar
    {
        public class Ejecuta : IRequest
        {
            public string Username { get; set; }
            public string RolNombre { get; set; }
        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.RolNombre).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly UserManager<Usuario> _userManager;

            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                this._roleManager = roleManager;
                this._userManager = userManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role = await this._roleManager.FindByNameAsync(request.RolNombre);
                if (role == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new { mensaje = "No se encontro el rol" });
                }

                var user = await this._userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new { mensaje = "No se encontro el usuario" });
                }

                var resultado = await this._userManager.RemoveFromRoleAsync(user, role.Name);
                if(resultado.Succeeded)
                {
                    return Unit.Value;
                }

                throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, "No se pudo eliminar el rol al usuario");
            }
        }
    }
}
