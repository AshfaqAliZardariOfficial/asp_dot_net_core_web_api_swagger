using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Testing.Utils
{
    public sealed class DatabaseUtils
    {
        /// <summary>
        /// Initialize database Instance.
        /// </summary>
        private static DatabaseUtils instance = new DatabaseUtils();
        /// <summary>
        /// Declear database connection string.
        /// </summary>
        private string DatabaseConnection;

        /// <summary>
        /// Database Utils private constructor.
        /// </summary>
        private DatabaseUtils()
        {
            // Get default connection string from appsettings.json file.
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            DatabaseConnection = builder.Build().GetConnectionString("DefaultConnection");
        }
        /// <summary>
        /// Get database utils class instance.
        /// </summary>
        public static DatabaseUtils Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Insert, Update or Delete data return true if query successful executed.
        /// </summary>
        /// <param name="company">Set company as string, by default "Default".</param>
        /// <param name="query">Set text query or store procedure.</param>
        /// <param name="parameters">Set database data parameters.</param>
        /// <param name="isStoreProcedure">If store procedure then set isStoreProcedure to true, by default false.</param>
        /// <param name="Timeout">Set query Timeout in seconds, by default 30 seconds.</param>
        /// <returns>If query or procedure successfully executed then returns true, otherwise false.</returns>
        public bool InsertOrUpdateOrDelete(string company, string query, IDictionary<string, object> parameters, bool isStoreProcedure = false, int Timeout = 0)
        {
            int isQueryExecuted = 0;

            if (string.IsNullOrEmpty(company))
            {
                company = "Default";
            }

            using (var con = new SqlConnection(DatabaseConnection))
            {
                con.Open();
                using (var command = new SqlCommand(query, con))
                {
                    // Store query procedure or normal text.
                    command.CommandType = isStoreProcedure ? CommandType.StoredProcedure : CommandType.Text;
                    // Timout Seconds.
                    command.CommandTimeout = Timeout > 0 ? Timeout : 30;
                    // Query Parameters
                    if (parameters != null)
                    {
                        IDbDataParameter dbParam;
                        foreach (KeyValuePair<string, object> param in parameters)
                        {
                            dbParam = command.CreateParameter();
                            dbParam.ParameterName = param.Key;
                            dbParam.Value = param.Value != null ? param.Value : DBNull.Value;

                            command.Parameters.Add(dbParam);
                        }
                    }
                    
                    // return int if inserted, updated or deleted.
                    isQueryExecuted = command.ExecuteNonQuery();
                    
                }
                con.Close();

            }

            return isQueryExecuted > 0;

        }


        public DataTable GetByModel(string company, string query, IDictionary<string, object> parameters, bool isStoreProcedure = false, int Timeout = 0)
        {
            var Table = new DataTable();
            IDataReader DataReader = null;

            if (string.IsNullOrEmpty(company))
            {
                company = "Default";
            }

            using (var con = new SqlConnection(DatabaseConnection))
            {
                con.Open();
                using (var command = new SqlCommand(query, con))
                {
                    // Store query procedure or normal text.
                    command.CommandType = isStoreProcedure ? CommandType.StoredProcedure : CommandType.Text;
                    // Timout Seconds.
                    command.CommandTimeout = Timeout > 0 ? Timeout : 30;
                    // Query Parameters
                    if (parameters != null)
                    {
                        IDbDataParameter dbParam;
                        foreach (KeyValuePair<string, object> param in parameters)
                        {
                            
                            dbParam = command.CreateParameter();
                            dbParam.ParameterName = param.Key;
                            dbParam.Value = param.Value != null ? param.Value : DBNull.Value;

                            command.Parameters.Add(dbParam);
                        }
                    }

                    // return int if inserted or updated.
                    DataReader = command.ExecuteReader();

                }
                if (DataReader != null)
                    Table.Load(DataReader);
                con.Close();

            }
            //Dictionary<object, List<Version>> data = null;
            //if (Table != null && Table.Rows.Count > 0)
            //{
            //    data = Table.AsEnumerable()
            //    .GroupBy(r => r.Field<object>(0))
            //    .ToDictionary(
            //    g => g.Key,
            //    g => g.Select(r => Version.Parse(r.Field<string>(1))).ToList());
            //}


            return Table != null && Table.Rows.Count > 0 ? Table : null;

        }

    }
}
