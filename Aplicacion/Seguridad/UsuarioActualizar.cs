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
    public class UsuarioActualizar
    {

        public class Ejecuta : IRequest<UsuarioData>
        {
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
            public ImagenGeneral ImagenPerfil { get; set; } 

        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta,UsuarioData>
        {
            private readonly CursosOnlineContext _cursosOnlineContext;
            private readonly UserManager<Usuario> _usuarioManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IPasswordHasher<Usuario> _passwordHasher;


            public Manejador(CursosOnlineContext cursosOnlineContext, UserManager<Usuario> usuarioManager, IJwtGenerator jwtGenerator, IPasswordHasher<Usuario> passwordHasher)
            {
                this._cursosOnlineContext = cursosOnlineContext;
                this._usuarioManager = usuarioManager;
                this._jwtGenerator = jwtGenerator;
                this._passwordHasher = passwordHasher;
            }
            public async  Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
               var usuario = await  this._usuarioManager.FindByNameAsync(request.Username);

                if(usuario == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, "El usuario no fue encontrado");
                }

                var emailEncontrado = await this._cursosOnlineContext.Users.AnyAsync(x => x.Email == request.Email && x.UserName != request.Username);
                if(emailEncontrado)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.InternalServerError, new { mensaje = "El correo electronico se encuentra asignado a otro usuario" });
                }

                if (request.ImagenPerfil != null)
                {
                    var imagenUsuario = await this._cursosOnlineContext.Documento.FirstOrDefaultAsync(x => x.ObjetoReferencia == new Guid(usuario.Id));

                    if (imagenUsuario == null)
                    {
                        var imagen = new Documento
                        {
                            Contenido = Convert.FromBase64String(request.ImagenPerfil.Data),
                            Extension = request.ImagenPerfil.Extension,
                            ObjetoReferencia = new Guid(usuario.Id),
                            FechaCreacion = DateTime.UtcNow,
                            Nombre = request.ImagenPerfil.Nombre,
                            DocumentoId = new Guid()
                        };
                        _cursosOnlineContext.Documento.Add(imagen);
                    }
                    else
                    {
                        imagenUsuario.Contenido = Convert.FromBase64String(request.ImagenPerfil.Data);
                        imagenUsuario.Nombre = request.ImagenPerfil.Nombre;
                        imagenUsuario.Extension = request.ImagenPerfil.Extension;

                    }
                }

                usuario.NombreCompleto = request.NombreCompleto;
                usuario.Email = request.Email;
                usuario.PasswordHash = this._passwordHasher.HashPassword(usuario, request.Password);

                var resultado = await _usuarioManager.UpdateAsync(usuario);

                var roles = await _usuarioManager.GetRolesAsync(usuario);
                var listaRoles = roles.ToList();

                var imagenPerfil = await _cursosOnlineContext.Documento.FirstAsync(x => x.ObjetoReferencia == new Guid(usuario.Id));
                ImagenGeneral imagenGeneral = null;

                if (imagenPerfil != null)
                {
                    imagenGeneral = new ImagenGeneral
                    {
                        Data = Convert.ToBase64String(imagenPerfil.Contenido),
                        Extension = imagenPerfil.Extension,
                        Nombre = imagenPerfil.Nombre
                    };
                }

                if (resultado.Succeeded)
                {
                    return new UsuarioData { 
                        NombreCompleto = usuario.NombreCompleto,
                        Username = usuario.UserName,
                        Email = usuario.Email,
                        Token = _jwtGenerator.CrearToken(usuario, listaRoles),
                        ImagenPerfil = imagenGeneral
                    };
                }

                throw new ManejadorExcepcion(System.Net.HttpStatusCode.InternalServerError, "No se pudo actualizar al usuario");

            }
        }
    }
}
