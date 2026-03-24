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
			Console.WriteLine(connector.GetPrimaryKeyColumnName("Directors"));
			Console.WriteLine(connector.GetPrimaryKeyColumnName("Movies"));

			//			connector.Insert($@"INSERT Directors (director_id,first_name,last_name) 
			//VALUES ({connector.GetNextPrimaryKey("Directors")}, N'Guy', N'Richie');");

			connector.Insert
			(
				"Directors",
				"director_id,first_name,last_name",
				$"{connector.GetNextPrimaryKey("Directors")},John,Singleton"
			);

			Console.WriteLine($"PK Max:\t{connector.GetMaxPrimaryKey("Directors")}");

			//string cmd ="SELECT movie_id,title,release_date,first_name,last_name FROM Movies,Directors WHERE director=director_id";
			//connector.Select(cmd);

			connector.Select("*", "Directors");
			Console.WriteLine($"Количество записей: {connector.Scalar("SELECT COUNT(*) FROM Directors")}");

			connector.Select
				(
				"title,release_date,first_name,last_name",
				"Movies,Directors",
				"director=director_id"
				);
			Console.WriteLine($"Количество записей: {connector.Scalar("SELECT COUNT(*) FROM Movies")}");

			//command.CommandText = "SELECT COUNT(*) FROM Movies";
			//Console.WriteLine($"Количество записей:\t{command.ExecuteScalar()}");

			//connection.Close();
		}
	}
}
