using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class SiteSqlDAL : ISiteDAL
    {
        public string connectionString;

        public SiteSqlDAL(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
        }

        public IList<Site> GetAvailableSite(int campGroundId, DateTime fromDate, DateTime toDate)
        {
            List<Site> availableSites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"Select * From Site Where site.site_id Not In (select site_id from reservation
                                                    WHERE(@fromDate < from_date AND @toDate > from_date) Or
                                                    (@fromDate < to_date And @toDate > to_date) Or
                                                    (@fromDate < from_date And @toDate > to_date))", conn);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site availSite = new Site();
                        availSite.SiteId = Convert.ToInt32(reader["site_id"]);
                        availSite.campgroundId = Convert.ToInt32(reader["campground_id"]);
                        availSite.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        availSite.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        availSite.IsAccessible = Convert.ToBoolean(reader["accessible"]);
                        availSite.MaxRvLength = Convert.ToInt32(reader["max_rv_length"]);
                        availSite.Utilities = Convert.ToBoolean(reader["utilities"]);

                        availableSites.Add(availSite);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("There was a problem accessing the database.");
                throw;
            }
            return availableSites;
        }
    }
}
