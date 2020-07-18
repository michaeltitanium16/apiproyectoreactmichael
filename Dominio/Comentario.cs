using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Comentario
    {
        public Guid ComentarioId { get; set; }
        public string Alumno { get; set; }
        public int Puntaje { get; set; }
        public string ComentarioTexto { get; set; }
        public Guid CursoId { get; set; }
        public DateTime? FechaCreacion { get; set; }

        #region Relaciones
        //1-n
        public Curso Curso { get; set; }

        #endregion
    }
}
