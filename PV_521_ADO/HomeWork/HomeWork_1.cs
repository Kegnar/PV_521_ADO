using System.Data.SqlClient;
using System;
namespace PV_521_ADO.HomeWork


{
    public class HomeWork1
    {
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
        
        public static string PrintScalar(string connectionString, string sqlCommand)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            SqlCommand command = new SqlCommand(sqlCommand, connection);
            string result = command.ExecuteScalar().ToString();
            connection.Close();
            return result;
        }
        
    }
}