﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Instructor
    {
        public Guid InstructorId { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Grado { get; set; }
        public byte[] FotoPerfil { get; set; }
        public DateTime? FechaCreacion { set; get; }
        #region Relaciones
        //n-n
        public ICollection<CursoInstructor> CursoLink { get; set; }
        #endregion
    }
}
