using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace PV_521_ADO
{
	class Program
	{
		static void Main(string[] args)
		{
			// убрал подключение в app.config
			string connectionString = ConfigurationManager.ConnectionStrings["Movies_PV_521"].ConnectionString;

			
			Connector connector = new Connector(connectionString);

			// connector.Insert("INSERT Directors (first_name,last_name) VALUES (N'Guy', N'Richie');");
			//
			// Console.WriteLine($"PK Max:\t{connector.GetMaxPrimaryKey("Directors")}");
			//
			// //string cmd ="SELECT movie_id,title,release_date,first_name,last_name FROM Movies,Directors WHERE director=director_id";
			// //connector.Select(cmd);
			//
			// connector.Select("*", "Directors");
			// Console.WriteLine($"Количество записей: {connector.Scalar("SELECT COUNT(*) FROM Directors")}");
			//
			// connector.Select
			// 	(
			// 	"title,release_date,first_name,last_name",
			// 	"Movies,Directors",
			// 	"director=director_id"
			// 	);
			//Console.WriteLine($"Количество записей: {connector.Scalar("SELECT COUNT(*) FROM Movies")}");

			//command.CommandText = "SELECT COUNT(*) FROM Movies";
			//Console.WriteLine($"Количество записей:\t{command.ExecuteScalar()}");

			//connection.Close();
			
			Console.WriteLine($"PK_Name: {connector.GetPrimaryKeyName("Movies")}");
			Console.WriteLine($"PK_Pos: {connector.GetPrimaryKeyPosition("Movies")}");
			
			
			//connector.Insert("first_name, last_name","N'Stephen',N'Spielberg'","Directors");
			Console.WriteLine(connector.Exists("Directors","first_name = N'Ds'"));
			//connector.Select("first_name, last_name","Directors");
		}
	}
}
