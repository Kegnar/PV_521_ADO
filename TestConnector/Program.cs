using System;
using System.Collections.Generic;
using System.Configuration;
using Connection;
namespace TestConnector
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TestDb"].ConnectionString;
            Connector connector = new Connector(connectionString);
            Console.WriteLine(connector.GetPrimaryKeyColumnName("TableA"));
        }
        
    }
}