using System;
using System.Data.SqlClient;

namespace Connection
{
    public class Connector
    {
        string _connectionString;
        SqlConnection _connection;

        public Connector(string connectionString)
        {
            Console.WriteLine(connectionString);
            this._connectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        public void Select(string cmd)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand(cmd, _connection);

            SqlDataReader reader = command.ExecuteReader();
            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write(reader.GetName(i) + "\t");
            Console.WriteLine();
            while (reader.Read())
            {
                //Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}\t{reader[3]}");
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write($"{reader[i]}\t\t");
                Console.WriteLine();
            }

            reader.Close();
            _connection.Close();
        }

        public void Select(string fields, string tables, string condition = "")
        {
            string cmd = $"SELECT {fields} FROM {tables}";
            if (condition != "") cmd += $" WHERE {condition}";
            cmd += ";";
            Select(cmd);
        }

        public object Scalar(string cmd)
        {
            _connection.Open();

            SqlCommand command = new SqlCommand(cmd, _connection);
            var result = command.ExecuteScalar(); //Выполнение скалярного запроса.

            _connection.Close();
            return result;
        }

        public int GetMaxPrimaryKey(string table)
        {
            string cmd = $"SELECT * FROM {table}";
            SqlCommand command = new SqlCommand(cmd, _connection);
            _connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            string pkName = reader.GetName(0);
            reader.Close();
            _connection.Close();
            return (int)Scalar($"SELECT MAX({pkName}) FROM {table}");
        }

        public int GetNextPrimaryKey(string table)
        {
            return GetMaxPrimaryKey(table) + 1;
        }

        public string GetPrimaryKeyColumnName(string table)
        {
            string cmd =
                $@"
                 SELECT	INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME 
                 FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                 WHERE   TABLE_NAME = N'{table}' AND CONSTRAINT_NAME LIKE N'PK_%'
                 ";
            return (string)Scalar(cmd);
        }

        public void Insert(string cmd)
        {
            SqlCommand command = new SqlCommand(cmd, _connection);
            _connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType());
                Console.WriteLine(ex.Message);
                if (ex is SqlException && ex.Message.Contains("_id"))
                {
                    Console.WriteLine("Good");
                }
            }

            _connection.Close();
        }

        public void Insert(string table, string fields, string values)
        {
            string condition = "";
            string[] sFields = fields.Split(',');
            string[] sValues = values.Split(',');
            string parsedValues = $"N'{sValues[0]}',";
            for (int i = 1; i < sFields.Length; i++)
            {
                condition += $" {sFields[i]}=N'{sValues[i]}' ";
                parsedValues += sValues[i][0] != 'N' && sValues[i][1] != '\'' ? $"N'{sValues[i]}'" : sValues[i];
                if (i != sFields.Length - 1)
                {
                    condition += "AND";
                    parsedValues += ",";
                }
            }

            string cmd = $"IF NOT EXISTS (SELECT {GetPrimaryKeyColumnName(table)} FROM {table} WHERE {condition})";
            cmd += $"INSERT {table}({fields}) VALUES ({parsedValues})";
            Insert(cmd);
        }
    }
}