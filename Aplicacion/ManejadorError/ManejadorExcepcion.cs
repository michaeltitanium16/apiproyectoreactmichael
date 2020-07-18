using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Aplicacion.ManejadorError
{
    public class ManejadorExcepcion : Exception
    {
        public HttpStatusCode Codigo { get; }
        public object Errores { get; }
        public ManejadorExcepcion(HttpStatusCode code,object errores = null)
        {
            this.Codigo = code;
            this.Errores = errores;
        }
    }
}
