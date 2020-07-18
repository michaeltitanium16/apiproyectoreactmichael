using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConnection _factoryConnection;
        public InstructorRepositorio(IFactoryConnection FactoryConnection)
        {
            _factoryConnection = FactoryConnection;
        }
        public async Task<int> Actualiza(Guid instructorId,string nombre,string apellidos,string grado)
        {
            var storeProcedure = "usp_Instructor_Editar";
            int? resultado = null;

            try
            {
                var connection = _factoryConnection.GetConnection();

                resultado = await connection.ExecuteAsync(
                            storeProcedure,
                            new
                            {
                                InstructorId = instructorId,
                                Nombre = nombre,
                                Apellidos = apellidos,
                                Grado = grado
                            },
                            commandType: System.Data.CommandType.StoredProcedure);
            }
            catch(Exception e)
            {
                throw new Exception("Error al momento de editar el instructor", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }

            return resultado.Value;
        }

        public async Task<int> Elimina(Guid id)
        {
            var storeProcedure = "usp_Instructor_Eliminar";
            int? resultado = null;

            try
            {
                var connection = _factoryConnection.GetConnection();

                resultado = await connection.ExecuteAsync(
                            storeProcedure,new {InstructorId =  id },
                            commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error al momento de eliminar el instructor", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }

            return resultado.Value;
        }

        public async  Task<int> Nuevo(string nombre,string apellido,string grado)
        {
            var storeProcedure = "usp_Instructor_Nuevo";
            int? resultado = null;
            try
            {
                var connection = _factoryConnection.GetConnection();

                resultado = await  connection.ExecuteAsync(
                    storeProcedure, 
                    new {
                         InstructorId = Guid.NewGuid(),
                         Nombre = nombre,
                         Apellidos = apellido,
                         Grado = grado
                         },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
            catch(Exception e)
            {
                throw new Exception("Error al crear nuevo instructor", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
                
            }

            return resultado.Value;
        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;
            var storeProcedure = "usp_Obtener_Instructores";
            try
            {
                var connection = _factoryConnection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storeProcedure, null, commandType: System.Data.CommandType.StoredProcedure);
            }
            catch(Exception e)
            {
                throw new Exception("Error en la consulta de datos", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return instructorList;
        }

        public async Task<InstructorModel> ObtenerPorId(Guid id)
        {
            var storeProcedure = "usp_Instructor_By_Id";
            InstructorModel instructorModel = null;

            try
            {
                var connection = _factoryConnection.GetConnection();

                instructorModel = await connection.QueryFirstAsync<InstructorModel>(
                    storeProcedure,
                    new { Id = id },
                    commandType: System.Data.CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                throw new Exception("Error al buscar el instructor", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();

            }

            return instructorModel;
        }
    }
}
