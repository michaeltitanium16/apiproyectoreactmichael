using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta :  IRequest<UsuarioData> {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly CursosOnlineContext _context;
            public Manejador(UserManager<Usuario> userManager,SignInManager<Usuario> signInManager,IJwtGenerator jwtGenerator,CursosOnlineContext context)
            {
                this._userManager = userManager;
                this._signInManager = signInManager;
                this._jwtGenerator = jwtGenerator;
                this._context = context;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if(usuario == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.Unauthorized);
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                var resultadoRoles = await _userManager.GetRolesAsync(usuario);

                var roles = resultadoRoles.ToList();

                var imagenPerfil = await _context.Documento.FirstOrDefaultAsync(x => x.ObjetoReferencia ==new Guid( usuario.Id));

                if (resultado.Succeeded)
                {

                    if (imagenPerfil != null)
                    {
                        var imagen = new ImagenGeneral
                        {
                            Data = Convert.ToBase64String(imagenPerfil.Contenido),
                            Extension = imagenPerfil.Extension,
                            Nombre = imagenPerfil.Nombre
                        };

                        return new UsuarioData
                        {
                            NombreCompleto = usuario.NombreCompleto,
                            Token = _jwtGenerator.CrearToken(usuario, roles),
                            Username = usuario.UserName,
                            Email = usuario.Email,
                            ImagenPerfil = imagen
                        };
                    }
                    else
                    {
                        return new UsuarioData
                        {
                            NombreCompleto = usuario.NombreCompleto,
                            Token = _jwtGenerator.CrearToken(usuario, roles),
                            Username = usuario.UserName,
                            Email = usuario.Email,
                            ImagenPerfil = null
                        };
                    }
                }
                throw new ManejadorExcepcion(System.Net.HttpStatusCode.Unauthorized);

            }
        }
    }
}
