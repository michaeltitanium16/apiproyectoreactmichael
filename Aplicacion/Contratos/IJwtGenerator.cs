using Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Contratos
{
    public interface IJwtGenerator
    {
        string CrearToken(Usuario usuario,List<string> roles);
    }
}
