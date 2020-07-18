using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.DapperConexion
{
    public class InstructorModel
    {
        public Guid InstructorId { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Grado { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
