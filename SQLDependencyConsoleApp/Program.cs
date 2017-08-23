using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDependencyConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
                Teste t = new Teste();
                Initialization();

                t.RegisterNotification();

                Console.ReadKey();
        }

        static void  Initialization()
        {
            // Create a dependency connection.
            SqlDependency.Start(@"Data Source=DESKTOP-D3PU4S0\SQLEXPRESS;Initial Catalog=TESTESQLNOTIFICATION;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }

    public class Teste
    {
        
        public void RegisterNotification()
        {
            
            var cs = @"Data Source=DESKTOP-D3PU4S0\SQLEXPRESS;Initial Catalog=TESTESQLNOTIFICATION;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            var sql = @"SELECT CAMPO_2  FROM dbo.TESTE";
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Notification = null;
                    var sqlDependency = new SqlDependency(cmd);
                    sqlDependency.OnChange += SqlDependency_OnChange;

                    var reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        Console.WriteLine(reader["CAMPO_2"]);
                    
                    }
                }
            }

        }

        private void SqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                RegisterNotification();

            }
        }
    }
}
    

