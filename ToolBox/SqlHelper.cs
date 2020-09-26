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
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Int32 ExecuteNonQuery(
            string connectionString,
            string commandText,
            CommandType commandType = CommandType.Text,
            params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                command.Parameters.AddRange(parameters);

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                return command.ExecuteNonQuery();
            }
        }

        // 
        /// <summary>
        /// Set the connection, command, and then execute the command and only return one value.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Object ExecuteScalar(
            string connectionString,
            string commandText,
            CommandType commandType = CommandType.Text,
            params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                command.Parameters.AddRange(parameters);

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Set the connection, command, and then execute the command with query and return the reader.
        /// The SqlConnection will be closed when the SqlDataReader is closed.  
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(
            string connectionString,
            string commandText,
            CommandType commandType = CommandType.Text,
            params SqlParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                command.Parameters.AddRange(parameters);

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                // When using CommandBehavior.CloseConnection, the connection will be closed when the IDataReader is closed.  
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static DataTable Select(
            string connectionString,
            string commandText,
            CommandType commandType = CommandType.Text,
            params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(commandText, connection))
            {
                dataAdapter.SelectCommand.CommandType = commandType;
                dataAdapter.SelectCommand.Parameters.AddRange(parameters);

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                dataAdapter.Fill(dt);

                return dt;
            }
        }

        public static DataSet MultiSelect(
            string connectionString,
            string commandText,
            CommandType commandType = CommandType.Text,
            params SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(commandText, connection))
            {
                dataAdapter.SelectCommand.CommandType = commandType;
                dataAdapter.SelectCommand.Parameters.AddRange(parameters);
            
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                dataAdapter.Fill(ds);

                return ds;
            }
        }
    }
}
