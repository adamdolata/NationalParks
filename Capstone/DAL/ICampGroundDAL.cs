using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ICampGroundDAL
    {
        IList<CampGround> GetCampGround();
        IList<CampGround> GetCampGroundByPark(Park park);
    }
}
