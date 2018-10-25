using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class CampGround
    {
        public int CampGroundId { get; set; }
        public string Name { get; set; }
        public DateTime OpenFrom { get; set; }
        public DateTime OpenTo { get; set; }
        public decimal DailyFee { get; set; }
    }
}
