using System;
using System.Data;
using System.Data.SqlClient;

namespace ToolBox
{
    public static class SqlHelper
    {
        /// <summary>
        /// Set the connection, command, and then execute the command with non query.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="dbParameters"></param>
        /// <returns>The number of rows affected.</returns>
        public static Int32 ExecuteNonQuery(
            string connectionString,
            string commandText,
            CommandType commandType = CommandType.Text,
            params SqlParameter[] dbParameters)
        {
            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            using (SqlCommand dbCommand = new SqlCommand(commandText, dbConnection))
            {
                dbCommand.CommandType = commandType;
                dbCommand.Parameters.AddRange(dbParameters);

                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                return dbCommand.ExecuteNonQuery();
            }
        }

        // 
        /// <summary>
        /// Set the connection, command, and then execute the command and only return one value.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public static Object ExecuteScalar(
            string connectionString,
            string commandText,
            CommandType commandType = CommandType.Text,
            params SqlParameter[] dbParameters)
        {
            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            using (SqlCommand dbCommand = new SqlCommand(commandText, dbConnection))
            {
                dbCommand.CommandType = commandType;
                dbCommand.Parameters.AddRange(dbParameters);

                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                return dbCommand.ExecuteScalar();
            }
        }

        /// <summary>
        /// Set the connection, command, and then execute the command with query and return the reader.
        /// The SqlConnection will be closed when the SqlDataReader is closed.  
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(
            string connectionString,
            string commandText,
            CommandType commandType = CommandType.Text,
            params SqlParameter[] dbParameters)
        {
            SqlConnection dbConnection = new SqlConnection(connectionString);
            using (SqlCommand dbCommand = new SqlCommand(commandText, dbConnection))
            {
                dbCommand.CommandType = commandType;
                dbCommand.Parameters.AddRange(dbParameters);

                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                // When using CommandBehavior.CloseConnection, the connection will be closed when the IDataReader is closed.  
                return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static DataTable ReadTable(
            string connectionString,
            string commandText,
            CommandType commandType = CommandType.Text,
            params SqlParameter[] dbParameters)
        {
            DataTable dt = new DataTable();

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(commandText, dbConnection))
            {
                dataAdapter.SelectCommand.CommandType = commandType;
                dataAdapter.SelectCommand.Parameters.AddRange(dbParameters);

                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                dataAdapter.Fill(dt);

                return dt;
            }
        }

        public static DataTable ReadTable(
            string connectionString,
            SqlCommand dbCommand)
        {
            DataTable dt = new DataTable();

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                dbCommand.Connection = dbConnection;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(dbCommand);

                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                dataAdapter.Fill(dt);

                return dt;
            }
        }

        public static DataSet ReadSet(
            string connectionString,
            string commandText,
            CommandType commandType = CommandType.Text,
            params SqlParameter[] dbParameters)
        {
            DataSet ds = new DataSet();

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(commandText, dbConnection))
            {
                dataAdapter.SelectCommand.CommandType = commandType;
                dataAdapter.SelectCommand.Parameters.AddRange(dbParameters);
            
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                dataAdapter.Fill(ds);

                return ds;
            }
        }
    }
}
