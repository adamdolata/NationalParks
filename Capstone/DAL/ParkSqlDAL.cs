using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParkSqlDAL : IParkDAL
    {
        public string connectionString;
        
        public ParkSqlDAL(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
        }

        public Park GetParkById(int parkId)
        {
            Park output = new Park();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("Select * From park Where park.park_id = @park_id", conn);
                    cmd.Parameters.AddWithValue("@park_id", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = new Park();
                        park.ParkId = Convert.ToInt32(reader["park_id"]);
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Description = Convert.ToString(reader["description"]);
                        output = park;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("There was a problem accessing the database.");
                throw;
            }
            return output;
        }

        public IList<Park> GetParks()
        {
            List<Park> output = new List<Park>();

            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("Select * From park" , conn);
                    //cmd.Parameters.AddWithValue("@command", command);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = new Park();
                        park.ParkId = Convert.ToInt32(reader["park_id"]);
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Description = Convert.ToString(reader["description"]);
                        output.Add(park);
                    }
                    
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("There was a problem accessing the database.");
                throw;
            }
            return output;
        }


    }
}
