using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace PV_521_ADO.HomeWork

{
    public static class HomeWork1
    {
        /// <summary>
        /// Возвращает результат SQL-запроса в виде таблицы
        /// </summary>
        /// <param name="connectionString">Строка-подключение к БД</param>
        /// <param name="sqlCommand">SQL-запрос</param>
        public static void PrintTable(string connectionString, string sqlCommand)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlCommand, connection);

            SqlDataReader reader = command.ExecuteReader();
            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write(reader.GetName(i) + "\t");
            Console.WriteLine();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write($"{reader[i]}\t\t");
                Console.WriteLine();
            }

            reader.Close();
            connection.Close();
        }
        

        /// <summary>
        /// Возвращает строку - результат скалярного запроса
        /// </summary>
        /// <param name="connectionString">Строка-подключение к БД</param>
        /// <param name="sqlCommand">SQL-запрос</param>
        /// <returns></returns>
        public static string PrintScalar(string connectionString, string sqlCommand)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlCommand, connection);
            string result = command.ExecuteScalar().ToString(); // 
            connection.Close();
            return result;
        }
        
        public static void PrintAdvTable(string connectionString, string sqlCommand)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlCommand, connection);

            SqlDataReader reader = command.ExecuteReader();
            var tableSize = reader.FieldCount;
            List<string[]> tableContents = new List<string[]>();  // результаты запроса
            
            string[] headers = new string[tableSize];               // заголовки столбцов
            
            int[] columnLength = new int[tableSize];                // массив с максимальными длинами столбцов

            for (int i = 0; i < tableSize; i++)
            {
                headers[i] = reader.GetName(i);
            }
            
            // Вычитываем результаты запроса в tableContents и одновременно собираем данные о максимальной длине строки в каждом столбце, с учетом длины заголовка столбца
            while (reader.Read())
            {
                string[] tmpColumn = new string[tableSize];
                for (int i = 0; i < tableSize; i++)
                {
                    tmpColumn[i] = reader[i].ToString();
                    columnLength[i] = Math.Max(columnLength[i], Math.Max(tmpColumn[i].Length, headers[i].Length));
                }

                tableContents.Add(tmpColumn);
            }
            reader.Close();
            connection.Close();

            for (int i = 0; i < tableSize; i++)
            {
                Console.Write($"{headers[i].PadRight(columnLength[i]+1)}");
            }
            Console.WriteLine();
            foreach (var str in tableContents)
            {
                for (int i = 0; i < tableSize; i++)
                {
                Console.Write($"{str[i].PadRight(columnLength[i]+1)}");
                }
                Console.WriteLine();
            }
            
        }
    }
}