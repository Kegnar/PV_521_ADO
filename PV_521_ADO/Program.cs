using System;
using System.Data.SqlClient;
using PV_521_ADO.HomeWork;

namespace PV_521_ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(connection_string);
            string connectionString =
                @"Data Source=MITRIY\TEST;
                Initial Catalog=Movies_PV_521;
                Integrated Security=True;
                Connect Timeout=30;
                Encrypt=False;
                TrustServerCertificate=False;
                ApplicationIntent=ReadWrite;
                MultiSubnetFailover=False";

            // SqlConnection connection = new SqlConnection(connectionString);
            // connection.Open();

            string cmd =
                "SELECT movie_id,title,release_date,first_name,last_name FROM Movies,Directors WHERE director=director_id";
            HomeWork1.PrintTable(connectionString, cmd);
            
            // SqlCommand command = new SqlCommand(cmd, connection);
            //
            // SqlDataReader reader = command.ExecuteReader();
            // for (int i = 0; i < reader.FieldCount; i++)
            //     Console.Write(reader.GetName(i) + "\t");
            // Console.WriteLine();
            // while (reader.Read())
            // {
            //     
            //     for (int i = 0; i < reader.FieldCount; i++)
            //         Console.Write($"{reader[i]}\t\t");
            //     Console.WriteLine();
            // }
            //
            // reader.Close();

            // command.CommandText = "SELECT COUNT(*) FROM Movies";
            
            string commandScalar = "SELECT COUNT(*) FROM Movies";
            Console.WriteLine($"Количество записей:\t{HomeWork1.PrintScalar(connectionString, commandScalar)}");

            Console.WriteLine();
            // Console.WriteLine($"Количество записей:\t{command.ExecuteScalar()}");
            //
            //  connection.Close();
        }

        

    
    }
}