using System;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;

namespace KPS272_gateway
{
    class HandlerNew
    {
        string imei;
        DateTime time;
        float[] ai_di_Data;
        float temperature, humidity;
        byte[] f5_0; byte[] f5_1; byte[] f5_2; byte[] f5_3;
        float ai_0, ai_1, ai_2, ai_3, ai_4, ai_5,
            di_0, di_1, di_2, di_3, di_4, di_5, di_6, di_7;

        public HandlerNew(string imei, DateTime dateTime,
            byte[] f5_0, byte[] f5_1, byte[] f5_2, byte[] f5_3,
            float temperature, float humidity, params float[] ai_di_Data)
        {
            this.imei = imei;
            time = dateTime;
            this.temperature = temperature;
            this.humidity = humidity;

            this.ai_di_Data = ai_di_Data;

          
              ai_0 = ai_di_Data[0];
            ai_1 = ai_di_Data[1];
            ai_2 = ai_di_Data[2];
            ai_3 = ai_di_Data[3];
            ai_4 = ai_di_Data[4];
            ai_5 = ai_di_Data[5];

            di_0 = ai_di_Data[6];
            di_1 = ai_di_Data[7];
            di_2 = ai_di_Data[8];
            di_3 = ai_di_Data[9];
            di_4 = ai_di_Data[10];
            di_5 = ai_di_Data[11];
            di_6 = ai_di_Data[12];
            di_7 = ai_di_Data[13];

            this.f5_0 = f5_0;
            this.f5_1 = f5_1;
            this.f5_2 = f5_2;
            this.f5_3 = f5_3;
        }

        public void SaveDataInDb()
        {

            //Packet packet = new Packet();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DbGateWay"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection == null || String.IsNullOrEmpty(connection.ConnectionString))
                    {
                        throw new Exception("Fatal error: missing connecting string in web.config file");
                    }
                    else
                    {
                        // Open the connection  
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        
                        connection.Open();

                        string sqlExp = "SELECT * FROM vfd_node_registry";

                        SqlCommand cmd = new SqlCommand(sqlExp, connection);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    object imeiDB = reader.GetValue(3);

                                    if (imeiDB.ToString() == imei)
                                    {
                                        if (CheckDatabaseExists(connectionString, "MesApp"))
                                        {
                                            string sqlExpression = @"SELECT CASE WHEN OBJECT_ID('dbo.vfd_node00001', 'U') IS NOT NULL THEN 1 ELSE 0 END";

                                            cmd = new SqlCommand(sqlExpression, connection);
                                            cmd.ExecuteNonQuery();
                                            int x = Convert.ToInt32(cmd.ExecuteScalar());

                                            if (x == 1)
                                            {
                                                //string format = "yyyyMMddHHmmss";

                                                //
                                                sqlExpression = String.Format(@"INSERT INTO [dbo].[vfd_node00001]([date_time] ,[ai_0],[ai_1],[ai_2],[ai_3],[ai_4],
                                                                    [ai_5],[di_0],[di_1],[di_2],[di_3],[di_4],[di_5],[di_6],[di_7],[temperature],[humidity],[is_delete]) 
                                                                VALUES (@time, @ai_0,@ai_1,@ai_2,@ai_3,@ai_4,
                                                                    @ai_5,@di_0,@di_1,@di_2,@di_3,@di_4,@di_5,@di_6,@di_7, @temperature, @humidity, @is_delete )");

                                                using (cmd= new SqlCommand(sqlExpression, connection))
                                                {
                                                    // Adding records the table  
                                                    cmd.Parameters.AddWithValue("@time", time);
                                                    cmd.Parameters.AddWithValue("@ai_0", ai_di_Data[0]);
                                                    cmd.Parameters.AddWithValue("@ai_1", @ai_di_Data[1]);
                                                    cmd.Parameters.AddWithValue("@ai_2", @ai_di_Data[2]);
                                                    cmd.Parameters.AddWithValue("@ai_3", @ai_di_Data[3]);
                                                    cmd.Parameters.AddWithValue("@ai_4", @ai_di_Data[4]);
                                                    cmd.Parameters.AddWithValue("@ai_5", @ai_di_Data[5]);
                                                    cmd.Parameters.AddWithValue("@di_0", @ai_di_Data[6]);
                                                    cmd.Parameters.AddWithValue("@di_1", @ai_di_Data[7]);
                                                    cmd.Parameters.AddWithValue("@di_2", @ai_di_Data[8]);
                                                    cmd.Parameters.AddWithValue("@di_3", @ai_di_Data[9]);
                                                    cmd.Parameters.AddWithValue("@di_4", @ai_di_Data[10]);
                                                    cmd.Parameters.AddWithValue("@di_5", @ai_di_Data[11]);
                                                    cmd.Parameters.AddWithValue("@di_6", @ai_di_Data[12]);
                                                    cmd.Parameters.AddWithValue("@di_7", @ai_di_Data[13]);
                                                    cmd.Parameters.AddWithValue("@temperature", temperature);
                                                    cmd.Parameters.AddWithValue("@humidity", humidity);
                                                    cmd.Parameters.AddWithValue("@is_delete", 0);

                                                 
                                                    cmd.ExecuteNonQuery();
                                                }  
                                            }
                                        }
                                    }
                                }
                                connection.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private bool CheckDatabaseExists(string connectionString, string databaseName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"SELECT db_id('{databaseName}')", connection))
                {
                    connection.Open();
                    return (command.ExecuteScalar() != DBNull.Value);
                }
            }
        }
    }
}
