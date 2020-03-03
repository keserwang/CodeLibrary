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
            using (SqlConnection sqlCnt = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(commandText, sqlCnt))
            {
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);

                if (sqlCnt.State != System.Data.ConnectionState.Open)
                    sqlCnt.Open();

                return cmd.ExecuteNonQuery();
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
            using (SqlConnection sqlCnt = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(commandText, sqlCnt))
            {
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);

                if (sqlCnt.State != System.Data.ConnectionState.Open)
                    sqlCnt.Open();

                return cmd.ExecuteScalar();
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
            SqlConnection sqlCnt = new SqlConnection(connectionString);
            using (SqlCommand cmd = new SqlCommand(commandText, sqlCnt))
            {
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);

                if (sqlCnt.State != System.Data.ConnectionState.Open)
                    sqlCnt.Open();

                // When using CommandBehavior.CloseConnection, the connection will be closed when the IDataReader is closed.  
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static DataTable Select(
            string connectionString,
            string commandText,
            CommandType commandType = CommandType.Text,
            params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (SqlConnection sqlCnt = new SqlConnection(connectionString))
            using (SqlDataAdapter sqlDa = new SqlDataAdapter(commandText, sqlCnt))
            {
                sqlDa.SelectCommand.CommandType = commandType;
                sqlDa.SelectCommand.Parameters.AddRange(parameters);

                if (sqlCnt.State != System.Data.ConnectionState.Open)
                    sqlCnt.Open();

                sqlDa.Fill(dt);

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

            using (SqlConnection sqlCnt = new SqlConnection(connectionString))
            using (SqlDataAdapter sqlDa = new SqlDataAdapter(commandText, sqlCnt))
            {
                sqlDa.SelectCommand.CommandType = commandType;
                sqlDa.SelectCommand.Parameters.AddRange(parameters);
            
                if (sqlCnt.State != System.Data.ConnectionState.Open)
                    sqlCnt.Open();

                sqlDa.Fill(ds);

                return ds;
            }
        }
    }
}
