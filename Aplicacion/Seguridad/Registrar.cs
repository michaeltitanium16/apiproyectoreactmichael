using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Manejador(CursosOnlineContext context,UserManager<Usuario> userManager,IJwtGenerator jwtGenerator)
            {
                this._jwtGenerator = jwtGenerator;
                this._userManager = userManager;
                this._context = context;

            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var existe = await _context.Users.Where(x => x.Email.ToLower() == request.Email.ToLower()).AnyAsync();

                if (existe)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, new { mensaje = "El correo ingresado ya existe dentro de la base de datos" });
                }
                var existeUserName = await _context.Users.Where(x => x.UserName.ToLower() == request.UserName.ToLower()).AnyAsync();

                if (existeUserName)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, new { mensaje = "El UserName ingresado ya existe dentro de la base de datos" });
                }
                var usuario = new Usuario
                {
                    Email = request.Email,
                    NombreCompleto = request.NombreCompleto,
                    UserName = request.UserName
                };

                var resultado = await _userManager.CreateAsync(usuario,request.Password);


                if (resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _jwtGenerator.CrearToken(usuario,null),
                        Username = usuario.UserName,
                        Email = usuario.Email
                    };
                }
                else
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.BadRequest, "no se puedo crear al usuario");
                }
            }
        }
    }
}
