using System;
using System.Data.SqlClient;
using PV_521_ADO.HomeWork;


namespace PV_521_ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            string connectionString =
                @"Data Source=MITRIY\TEST;
                Initial Catalog=Movies_PV_521;
                Integrated Security=True;
                Connect Timeout=30;
                Encrypt=False;
                TrustServerCertificate=False;
                ApplicationIntent=ReadWrite;
                MultiSubnetFailover=False";

            Connector connector = new Connector(connectionString);

            string cmd =
                "SELECT movie_id,title,release_date,first_name,last_name FROM Movies,Directors WHERE director=director_id";
            HomeWork1.PrintAdvTable(connectionString, cmd);

            string commandScalar = "SELECT COUNT(*) FROM Movies";
            Console.WriteLine($"Количество записей:\t{HomeWork1.PrintScalar(connectionString, commandScalar)}");

            Console.WriteLine();
            connector.Select("SELECT COUNT(*) FROM Movies");
            Console.WriteLine($"Количество записей: {connector.Scalar("SELECT COUNT(*) FROM Directors")}");
        }
    }
}