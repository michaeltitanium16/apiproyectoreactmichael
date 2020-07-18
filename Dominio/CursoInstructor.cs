using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class CursoInstructor
    {
        public Guid CursoId { get; set; }
        public Guid InstructorId { get; set; }

        #region Relaciones
        //Ancla
        public Curso Curso { get; }
        //Ancla
        public Instructor Instructor { get; set; }

        #endregion
    }
}
