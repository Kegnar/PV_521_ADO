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
            Console.Write(reader.GetName(i) + '\t');
        }

        Console.WriteLine();
        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader[i]}\t\t");
            }

            Console.WriteLine();
        }

        connection.Close();
    }

    public void Select(string fields, string tables, string conditions = null)
    {
        string cmd = $"SELECT {fields} FROM {tables}";
        if (conditions != null) cmd += $" WHERE {conditions}";
        Select(cmd);
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

    public void Insert(string cmd)
    {
        SqlCommand command = new SqlCommand(cmd, connection);
        connection.Open();

        try
        {
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
            Console.WriteLine(e.Message);
           
        }
        connection.Close();
    }

    public int GetMaxPrimaryKey(string table)
    {
        string cmd = $"Select * from {table}";
        SqlCommand command = new SqlCommand(cmd, connection);
        connection.Open();
        SqlDataReader reader = command.ExecuteReader();
        string pkName = reader.GetName(0);
        reader.Close();
        connection.Close();
        return (int)Scalar($"SELECT MAX({pkName}) FROM {table}");
    }

    public int GetNextPrimaryKey(string table)
    {
        return GetMaxPrimaryKey(table) + 1;
    }
}