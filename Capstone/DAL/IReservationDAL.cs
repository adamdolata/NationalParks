using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAL
    {
        IList<Reservation> BookReservation(Reservation reservation);
    }
}
