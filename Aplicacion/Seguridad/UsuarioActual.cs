using Aplicacion.Contratos;
using Dominio;
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
    public class UsuarioActual
    {
        public class Ejecuta : IRequest<UsuarioData> { }
        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUsuarioSession _usuarioSession;
            private readonly CursosOnlineContext _context;

            public Manejador(UserManager<Usuario> userManager, IJwtGenerator jwtGenerator, IUsuarioSession usuarioSession, CursosOnlineContext context)
            {
                this._jwtGenerator = jwtGenerator;
                this._userManager = userManager;
                this._usuarioSession = usuarioSession;
                this._context = context;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_usuarioSession.ObtenerUsuarioSession());

                var resultadoRoles = await _userManager.GetRolesAsync(usuario);

                var roles = resultadoRoles.ToList();

                var imagenPerfil = await _context.Documento.FirstOrDefaultAsync(x => x.ObjetoReferencia == new Guid(usuario.Id));

                if (imagenPerfil != null)
                {
                    var imagenCliente = new ImagenGeneral
                    {
                        Data = Convert.ToBase64String(imagenPerfil.Contenido),
                        Extension = imagenPerfil.Extension,
                        Nombre = imagenPerfil.Nombre
                    };

                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Username = usuario.UserName,
                        Email = usuario.Email,
                        Token = _jwtGenerator.CrearToken(usuario, roles),
                        ImagenPerfil = imagenCliente,
                    };
                }
                else
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Username = usuario.UserName,
                        Email = usuario.Email,
                        Token = _jwtGenerator.CrearToken(usuario, roles),
                        ImagenPerfil = null,
                    };
                }

            }
        }
    }
}
