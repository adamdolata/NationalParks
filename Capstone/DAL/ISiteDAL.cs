using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ISiteDAL
    {
        IList<Site> GetAvailableSite(CampGround campGround, DateTime fromDate, DateTime toDate);
    }
}
