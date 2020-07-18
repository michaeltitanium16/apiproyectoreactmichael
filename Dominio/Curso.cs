using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Curso
    {
        public Guid CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public byte[] FotoPortada { get; set; }
        public DateTime? FechaCreacion { get; set; }

        #region Relaciones
        //1-1
        public Precio PrecioPromocion { get; set; }
        //1-n
        public ICollection<Comentario> ComentarioLista { get; set; }
        //n-n
        public ICollection<CursoInstructor> InstructoresLink { get; set; }

        #endregion
    }
}
