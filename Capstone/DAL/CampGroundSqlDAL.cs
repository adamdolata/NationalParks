using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CampGroundSqlDAL : ICampGroundDAL
    {
        public string connectionString;

        public CampGroundSqlDAL(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
        }

        public IList<CampGround> GetCampGroundByPark(Park park)
        {
            List<CampGround> output = new List<CampGround>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("Select * From campground INNER JOIN park ON campground.park_id = park.park_id WHERE campground.park_id = @park", conn);
                    cmd.Parameters.AddWithValue("@park", GetCampGroundByPark);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CampGround campGround = new CampGround();
                        campGround.ParkId = Convert.ToInt32(reader["park_id"]);
                        campGround.CampGroundId = Convert.ToInt32(reader["campground_id"]);
                        campGround.Name = Convert.ToString(reader["name"]);
                        campGround.OpenFrom = Convert.ToInt32(reader["open_from_mm"]);
                        campGround.OpenTo = Convert.ToInt32(reader["open_to_mm"]);
                        campGround.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
                        output.Add(campGround);
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
        public IList<CampGround> GetCampGround()
        {
            List<CampGround> output = new List<CampGround>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("Select * From campGround", conn);


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CampGround campGround = new CampGround();
                        campGround.CampGroundId = Convert.ToInt32(reader["campground_id"]);
                        campGround.Name = Convert.ToString(reader["name"]);
                        campGround.OpenFrom = Convert.ToInt32(reader["open_from_mm"]);
                        campGround.OpenTo = Convert.ToInt32(reader["open_to_mm"]);
                        campGround.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
                        output.Add(campGround);
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

       
    }
}
