using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class SiteSqlDAL : ISiteDAL
    {
        public IList<Site> GetAvailableSite(CampGround campGround, DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }
    }
}
