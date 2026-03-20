using System;

namespace PV_521_ADO;
using System.Data.SqlClient;

public class Connector
{
    string connectionString;
    SqlConnection connection;
    public Connector(string connectionString)
    {
        Console.WriteLine(connectionString);
        this.connectionString = connectionString;
        connection = new SqlConnection(connectionString);
    }
    
    public void Select(string cmd)
    {
            
        connection.Open();
        SqlCommand command = new SqlCommand(cmd, connection);
        SqlDataReader reader = command.ExecuteReader();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            Console.WriteLine(reader.GetName(i) + 't');
        }

        Console.WriteLine();
        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader[i]}\t\t");
                Console.WriteLine();
            }
        }
        connection.Close();
    }

    public object Scalar(string cmd)
    {
        connection.Open();
        object result = null;
        SqlCommand command = new SqlCommand(cmd, connection);
        result = command.ExecuteScalar();
                
        connection.Close();
        return result;
    }
}