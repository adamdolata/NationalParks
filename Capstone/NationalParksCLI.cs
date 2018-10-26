using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class NationalParksCLI
    {

        const string Command_Acadia = "1";
        const string Command_Arches = "2";
        const string Command_Cuyahoga_National_Park = "3";
        const string Command_Quit = "q";
        const string DatabaseConnection = @"Data Source=.\SQLEXPRESS;Initial Catalog=NPCampsite;Integrated Security=True";

        public void RunCLI()
        {
            PrintMenu();
            

            while (true)
            {
                string command = Console.ReadLine();

                if (command.ToLower() != "q")
                {
                    int parkID = int.Parse(command);
                   
                    GetPark(parkID);
                    CampGroundMenu(parkID);
                    
                }
                else
                {
                    Environment.Exit(1);
                }

            }
        }

        public void GetPark(int parkId)
        {
            Console.WriteLine();
            Console.Clear();

            IParkDAL dal = new ParkSqlDAL(DatabaseConnection);
            Park park = dal.GetParkById(parkId);

            Console.WriteLine($"{park.Name} National Park");
            Console.WriteLine($"Location:           {park.Location}");
            Console.WriteLine($"Established:        {park.EstablishDate}");
            Console.WriteLine($"Area:               {park.Area} sq km");
            Console.WriteLine($"Annual Visitors:    {park.Visitors}");
            Console.WriteLine();
            Console.WriteLine($"{park.Description}");

            Console.WriteLine();
        }

        public void CampGroundMenu(int parkId)
        {
            Console.WriteLine("Select a CampGround Option: ");
            Console.WriteLine("1) View Campgrounds ");
            Console.WriteLine("2) Search for reservation ");
            Console.WriteLine("3) Return to Previous Screen ");
            string input = Console.ReadLine();

            Console.Clear();

            IParkDAL parkDal = new ParkSqlDAL(DatabaseConnection);
            ICampGroundDAL cgDal = new CampGroundSqlDAL(DatabaseConnection);

            Park park = parkDal.GetParkById(parkId);
            IList<CampGround> campGrounds = cgDal.GetCampGroundByPark(parkId);       

            if (input == "1")
            {
                if (campGrounds.Count > 0)
                {
                    Console.WriteLine(park.Name + " " + "National Park Campgrounds");
                    Console.WriteLine();
                    Console.WriteLine("Campgound Id".PadRight(21) + "Name".PadRight(40) + "Open".PadRight(20) + "Close".PadRight(20) + "Daily Fee");

                    foreach (CampGround campGround in campGrounds)
                    {
                        Console.WriteLine("#" + campGround.CampGroundId.ToString().PadRight(20) + campGround.Name.ToString().PadRight(40) + GetMonthString(campGround.OpenFrom).ToString().PadRight(20) +  GetMonthString(campGround.OpenTo).ToString().PadRight(20) + "$" + campGround.DailyFee.ToString("0.00"));
                    }

                }
            }
            else if(input == "2")
            {
                if (campGrounds.Count > 0)
                {
                    Console.WriteLine(park.Name + " " + "National Park Campgrounds");
                    Console.WriteLine();
                    Console.WriteLine("Campgound Id".PadRight(21) + "Name".PadRight(40) + "Open".PadRight(20) + "Close".PadRight(20) + "Daily Fee");

                    foreach (CampGround campGround in campGrounds)
                    {
                        Console.WriteLine("#" + campGround.CampGroundId.ToString().PadRight(20) + campGround.Name.ToString().PadRight(40) + GetMonthString(campGround.OpenFrom).ToString().PadRight(20) + GetMonthString(campGround.OpenTo).ToString().PadRight(20) + "$" + campGround.DailyFee.ToString("0.00"));
                    }


                    Console.WriteLine();
                    ReservationMenu(parkId);

                    
                }
                
            }
            else if(input == "3")
            {
                Console.Clear();
                PrintMenu();
            }
        }

        private void PrintMenu()
        {
            Console.WriteLine("Select a Park for Further Details");
            IParkDAL parkDal = new ParkSqlDAL(DatabaseConnection);
            IList<Park> parks = parkDal.GetParks();

            foreach (Park park in parks)
            {
                Console.WriteLine($"{park.ParkId}) {park.Name}");
            }
            Console.WriteLine("Q) Quit");
        }

       public string GetMonthString(int month)
        {
            Dictionary<int, string> monthName = new Dictionary<int, string>()
            {
                {1, "January" },
                {2, "February" },
                {3, "March" },
                {4, "April" },
                {5, "May" },
                {6, "June" },
                {7, "July" },
                {8, "August" },
                {9, "September" },
                {10, "October" },
                {11, "November" },
                {12, "December" }
            };
            return monthName[month];
       }

        public void ReservationMenu(int parkId)
        {
            

            Console.WriteLine("Select an Option: ");
            Console.WriteLine("1) Search for Available Reservation ");
            Console.WriteLine("2) Return to Previous Screen ");
            string input = Console.ReadLine();

            
            if(input == "1")
            {

                //CampGroundMenu(parkId);

                // Prompt&Get for campgroundID: campgroundId
                Console.WriteLine("Which campground (enter 0 to cancel)? ");
                string inputCG = Console.ReadLine();
                // Prompt&Get for arrive date: fromDate
                Console.WriteLine("What is the arrival date (mm/dd/yyyy)? ");
                string inputFD = Console.ReadLine();
                // Prompt&Get for depart date: toDate
                Console.WriteLine("What is the departure date (mm/dd/yyyy)? ");
                string inputTD = Console.ReadLine();

                int campGroundId = int.Parse(inputCG);
                DateTime fromDate = DateTime.Parse(inputFD);
                DateTime toDate = DateTime.Parse(inputTD);


                ISiteDAL siteDal = new SiteSqlDAL(DatabaseConnection);
                IList<Site> sites = siteDal.GetAvailableSite(campGroundId, fromDate, toDate);
                ICampGroundDAL cgDal = new CampGroundSqlDAL(DatabaseConnection);
                IList<CampGround> campGrounds = cgDal.GetCampGroundByPark(parkId);
                double totalDays = (toDate - fromDate).TotalDays;

                if (sites.Count > 0)
                {
                    Console.WriteLine("Results Matching Your Search Criteria");
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Site No.".PadRight(20) + "Max Occup.".PadRight(20) + "Accessible?".PadRight(20) + "RV Len".PadRight(10) + "Utility".PadRight(10) + "Cost");
                    
                    foreach (Site site in sites)
                    {
                        Console.WriteLine(site.SiteNumber.ToString().PadRight(20) + site.MaxOccupancy.ToString().PadRight(20) + site.IsAccessible.ToString().PadRight(20) + site.MaxRvLength.ToString().PadRight(10) + site.Utilities.ToString().PadRight(10) + "$" + (campGrounds[campGroundId].DailyFee) * (totalDays).ToString("0.00"));
                    }

                }


            }
        }
    }
}

    

    