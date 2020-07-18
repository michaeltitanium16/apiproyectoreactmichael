using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Seguridad.TokenSeguridad
{
    public class UsuarioSession : IUsuarioSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsuarioSession(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;   
        }
        public string ObtenerUsuarioSession()
        {
            var userName = this._httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userName;
        }
    }
}
