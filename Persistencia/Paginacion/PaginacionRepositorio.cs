using Dapper;
using Microsoft.Data.SqlClient;
using Persistencia.DapperConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Paginacion
{
    public class PaginacionRepositorio : IPaginacion
    {
        private readonly IFactoryConnection _factoryConnection;

        public PaginacionRepositorio(IFactoryConnection factoryConnection)
        {
            this._factoryConnection = factoryConnection;
        }
        public async Task<PaginacionModel> devolverPaginacion(string storeProcedure, int numeroPagina, int cantidadElementos, IDictionary<string, object> parametrosFiltro, string ordenamientoColumna)
        {
            PaginacionModel paginacionModel = new PaginacionModel();
            List<IDictionary<string, object>> listaReporte = null;
            int totalRecords = 0;
            int totalPaginas = 0;

            try
            {
                var connection = this._factoryConnection.GetConnection();

                //parametros de entrada para el procedimiento almacenado
                DynamicParameters parametros = new DynamicParameters();

                //si tiene filtro creamos dichos filtros personalizados
                foreach (var param in  parametrosFiltro)
                {
                    parametros.Add("@"+param.Key, param.Value);
                }


                parametros.Add("@NumeroPagina", numeroPagina);
                parametros.Add("@CantidadElementos", cantidadElementos);
                parametros.Add("@Ordenamiento", ordenamientoColumna);

                parametros.Add("TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                parametros.Add("TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);
        


                var resultado = await connection.QueryAsync(storeProcedure, parametros, commandType: System.Data.CommandType.StoredProcedure);

                listaReporte = resultado.Select(x => (IDictionary<string, object>)x).ToList();

                paginacionModel.ListaRecords = listaReporte;
                paginacionModel.NumeroPaginas = parametros.Get<int>("@TotalPaginas");
                paginacionModel.TotalRecords = parametros.Get<int>("@TotalRecords");
            }
            catch(Exception e)
            {
                throw new Exception("No se pudo ejecutar el procedimiento almacenado", e);
            }
            finally
            {
                this._factoryConnection.CloseConnection();
            }

            return  paginacionModel;
        }
    }
}
