using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Server=chatbot2.ptha.io.vn,12433;Database=DaTruongThanh;User Id=sa1;Password=Toantom@123;Encrypt=False;TrustServerCertificate=True;";
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                Console.WriteLine("Connection successful!");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
            }
        }
    }
}
