using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public interface IInstructor
    {
        Task<IEnumerable<InstructorModel>> ObtenerLista();

        Task<InstructorModel> ObtenerPorId(Guid id);

        Task <int> Nuevo(string nombre, string apellido, string gradol);
        Task<int> Actualiza(Guid instructorId, string nombre, string apellidos, string grado);
        Task<int> Elimina(Guid id);

    }
}
