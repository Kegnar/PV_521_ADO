using System;
using System.Data.SqlClient;

namespace PV_521_ADO
{
    class Connector
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
            SqlCommand command = new SqlCommand(cmd, _connection);
            _connection.Open();

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
            object result = null;
            _connection.Open();

            SqlCommand command = new SqlCommand(cmd, _connection);
            result = command.ExecuteScalar(); //Выполнение скалярного запроса.

            _connection.Close();
            return result;
        }

        

        public void Insert(string cmd)
        {
            SqlCommand command = new SqlCommand(cmd, _connection);
            Console.WriteLine(_connection.State);
         
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

        public void Insert(string fields, string values, string table)
        {
            // примем пока за данность то, что ПК всегда первое поле
            string cmd = $"INSERT INTO {table} ({GetPrimaryKeyName(table)},{fields}) VALUES ({GetNextPrimaryKey(table)},{values});";
            Insert(cmd);
        }
        /// <summary>
        /// Возвращает название столбца с первичным ключом
        /// </summary>
        /// <param name="table">Таблица, для которой нужно узнать название столбца</param>
        /// <returns>string имя столбца</returns>
        public string GetPrimaryKeyName(string table)
        {
            // невозбранно попячено тут - https://ru.stackoverflow.com/questions/608817/sql-%D0%BF%D0%BE%D0%BB%D1%83%D1%87%D0%B8%D1%82%D1%8C-%D0%BD%D0%B0%D0%B7%D0%B2%D0%B0%D0%BD%D0%B8%D1%8F-%D0%BF%D0%BE%D0%BB%D1%8F-%D1%81-primary-key/608882#608882
            // и творчески обработано напильником
            // не использовать на первичных составных ключах - запрос вернет больше одного значения 
            string cmd = $"""
                          SELECT c.COLUMN_NAME FROM information_schema.table_constraints AS k 
                          INNER JOIN information_schema.KEY_COLUMN_USAGE AS c ON c.TABLE_NAME = k.TABLE_NAME 
                          AND c.CONSTRAINT_NAME = k.CONSTRAINT_NAME 
                          AND c.TABLE_SCHEMA = k.TABLE_SCHEMA WHERE  k.TABLE_NAME  = '{table}' 
                          AND k.CONSTRAINT_TYPE = 'PRIMARY KEY';
                          """;
            return Scalar(cmd).ToString();
        }
        /// <summary>
        /// Возвращает номер столбца с первичным ключом
        /// </summary>
        /// <param name="table">Таблица, для которой нужно узнать положение столбца</param>
        /// <returns>Int номер столбца</returns>
        public int GetPrimaryKeyPosition(string table) 
        {
            // не использовать на первичных составных ключах - запрос вернет больше одного значения 
            string cmd = $"""
                          SELECT 
                              k.ORDINAL_POSITION 
                          FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS c
                          JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS k 
                              ON c.CONSTRAINT_NAME = k.CONSTRAINT_NAME
                          WHERE c.TABLE_NAME = '{table}' 
                            AND c.CONSTRAINT_TYPE = 'PRIMARY KEY'
                            ORDER BY k.ORDINAL_POSITION; -- Сортировка критична
                          """;
//           
            return Convert.ToInt32(Scalar(cmd));
        }
        
        public int GetMaxPrimaryKey(string table)
        {
            string cmd = $"SELECT * FROM {table}";
            SqlCommand command = new SqlCommand(cmd, _connection);
            _connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            string pkName = GetPrimaryKeyName(table);       //вместо string pk_name = reader.GetName(0);
            reader.Close();
            _connection.Close();
            return (int)Scalar($"SELECT MAX({pkName}) FROM {table}");
        }
        /// <summary>
        /// Вычисляет значение первичного ключа для вставляемой в таблицу строки
        /// </summary>
        /// <param name="table">Имя таблицы</param>
        /// <returns>Int - значение первичного ключа для строки</returns>
        public int GetNextPrimaryKey(string table)
        {
            return GetMaxPrimaryKey(table) + 1;
        }

        public bool Exists(string table, string condition = "")
        {
            string cmd = $"SELECT COUNT(*) FROM {table}";
            if (condition != "") cmd += $" WHERE {condition}";
            cmd += ";";
            return Convert.ToBoolean(Scalar(cmd));
        }
    }
}