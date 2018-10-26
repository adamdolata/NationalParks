using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ReservationSqlDAL : IReservationDAL
    {
        public string connectionString;

        public ReservationSqlDAL(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
        }

        public IList<Reservation> BookReservation(Reservation reservation)
        {
            Reservation res = new Reservation();
            List<Reservation> newReservations = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("Insert Into reservation Values (@site_id, @name, @to_date, @from_date, @create_date);", conn);

                    cmd.Parameters.AddWithValue("@site_id", reservation.SiteId);
                    cmd.Parameters.AddWithValue("@name", reservation.Name);
                    cmd.Parameters.AddWithValue("@to_date", reservation.ToDate);
                    cmd.Parameters.AddWithValue("@from_date", reservation.FromDate);
                    cmd.Parameters.AddWithValue("@create_date", reservation.BookDate);

                    cmd.ExecuteNonQuery();
                    newReservations.Add(reservation);
                    return newReservations;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred making the reservation.");
                throw;
            }

        }

    }
}
