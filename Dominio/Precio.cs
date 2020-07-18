using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dominio
{
    public class Precio
    {
        public Guid PrecioId { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal PrecioActual { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Promocion { get; set; }
        public Guid CursoId { get; set; }

        #region Relaciones
        //1-1
        public Curso Curso { get; set; }
        #endregion
    }
}
