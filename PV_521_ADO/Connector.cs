using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PV_521_ADO
{
    class Connector
    {
        string connection_string;
        SqlConnection connection;

        public Connector(string connection_string)
        {
            Console.WriteLine(connection_string);
            this.connection_string = connection_string;
            connection = new SqlConnection(connection_string);
        }

        public void Select(string cmd)
        {
            connection.Open();
            SqlCommand command = new SqlCommand(cmd, connection);

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
            connection.Close();
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
            object result = null;
            connection.Open();

            SqlCommand command = new SqlCommand(cmd, connection);
            result = command.ExecuteScalar(); //Выполнение скалярного запроса.

            connection.Close();
            return result;
        }

        public int GetMaxPrimaryKey(string table)
        {
            string cmd = $"SELECT * FROM {table}";
            SqlCommand command = new SqlCommand(cmd, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            string pk_name = reader.GetName(0);
            reader.Close();
            connection.Close();
            return (int)Scalar($"SELECT MAX({pk_name}) FROM {table}");
        }

        public int GetNextPrimaryKey(string table)
        {
            return GetMaxPrimaryKey(table) + 1;
        }

        public void Insert(string cmd)
        {
            SqlCommand command = new SqlCommand(cmd, connection);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType());
                Console.WriteLine(ex.Message);
                if (ex.GetType() == typeof(SqlException) && ex.Message.Contains("_id"))
                {
                    Console.WriteLine("Good");
                }
            }

            connection.Close();
        }

        public string GetPrimaryKeyName(string table)
        {
            // невозбранно попячено тут - https://ru.stackoverflow.com/questions/608817/sql-%D0%BF%D0%BE%D0%BB%D1%83%D1%87%D0%B8%D1%82%D1%8C-%D0%BD%D0%B0%D0%B7%D0%B2%D0%B0%D0%BD%D0%B8%D1%8F-%D0%BF%D0%BE%D0%BB%D1%8F-%D1%81-primary-key/608882#608882
            // и творчески обработано напильником
            
            string cmd = $"""
                          SELECT C.COLUMN_NAME FROM information_schema.table_constraints AS pk 
                          INNER JOIN information_schema.KEY_COLUMN_USAGE AS C ON C.TABLE_NAME = pk.TABLE_NAME 
                          AND C.CONSTRAINT_NAME = pk.CONSTRAINT_NAME 
                          AND C.TABLE_SCHEMA = pk.TABLE_SCHEMA WHERE  pk.TABLE_NAME  = '{table}' 
                          AND pk.CONSTRAINT_TYPE = 'PRIMARY KEY';
                          """;
            return Scalar(cmd).ToString();
        }
    }
}