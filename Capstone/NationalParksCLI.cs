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

                // Added if statement to require a valid input
                if (command != "1" && command != "2" && command != "3" && command != "4" && command.ToLower() != "q")
                {
                    Console.WriteLine("Invalid entry, please enter a valid selection.");

                }
                else if (command.ToLower() != "q")
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

                    Console.WriteLine();
                    ReservationMenu(parkId);
                }
            }
            else if(input == "2")
            {
                if (campGrounds.Count > 0)
                {
                    Console.WriteLine(park.Name + " " + "National Park Campgrounds");
                    Console.WriteLine();
                    Console.WriteLine("  Campgound Id".PadRight(24) + "Name".PadRight(40) + "Open".PadRight(20) + "Close".PadRight(20) + "Daily Fee");
                    int choice = 1;
                    foreach (CampGround campGround in campGrounds)
                    {
                        Console.WriteLine(choice + ")" + " " + "#" + campGround.CampGroundId.ToString().PadRight(20) + campGround.Name.ToString().PadRight(40) + GetMonthString(campGround.OpenFrom).ToString().PadRight(20) + GetMonthString(campGround.OpenTo).ToString().PadRight(20) + "$" + campGround.DailyFee.ToString("0.00"));
                        choice++;
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
            else
            {
                Console.WriteLine("Invalid Entry, please try again.");
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
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Which campground (enter 0 to cancel)? ");
                string inputCG = Console.ReadLine();
                if (inputCG == "0")
                {
                    Console.WriteLine();
                    ReservationMenu(parkId);
                }
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
                if (sites.Count <= 0)
                {
                    Console.WriteLine("No available campsites, please enter an alternate DateRange. ");
                }
                else if (sites.Count > 0 && sites.Count < 8)
                {
                    Console.Clear();
                    Console.WriteLine("Results Matching Your Search Criteria");
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("  Site No.".PadRight(20) + "Max Occup.".PadRight(20) + "Accessible?".PadRight(20) + "RV Len".PadRight(10) + "Utility".PadRight(10) + "Cost");


                    int choice = 1;
                    foreach (Site site in sites)
                    {
                        // Added ToString() to limit cost value to two decimal places
                        Console.WriteLine(choice + ")" + " " + site.SiteNumber.ToString().PadRight(20) + site.MaxOccupancy.ToString().PadRight(20) + site.IsAccessible.ToString().PadRight(20) + site.MaxRvLength.ToString().PadRight(10) + site.Utilities.ToString().PadRight(10) + "$" + (campGrounds[campGroundId - 1].DailyFee * (decimal)totalDays).ToString("0.00"));
                        choice++;

                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Which site should be reserved (enter 0 to cancel)?");
                    string inputSite = Console.ReadLine();
                    Console.WriteLine("What name should the reservation be made under?");
                    string inputName = Console.ReadLine();

                    Reservation reservation = new Reservation()
                    {
                        Name = inputName,
                        SiteId = int.Parse(inputSite),
                        FromDate = fromDate,
                        ToDate = toDate,
                        BookDate = DateTime.Now
                    };

                    IReservationDAL resDal = new ReservationSqlDAL(DatabaseConnection);
                    IList<Reservation> reservations = resDal.BookReservation(reservation);

                    Console.WriteLine();
                    Console.WriteLine($"The reservation has been made and the Confirmation ID is {reservation.ReservationId}");
                }
            }
            else if (input == "2")
            {

                Console.Clear();
                IParkDAL parkDal = new ParkSqlDAL(DatabaseConnection);
                ICampGroundDAL cgDal = new CampGroundSqlDAL(DatabaseConnection);

                Park park = parkDal.GetParkById(parkId);
                IList<CampGround> campGrounds = cgDal.GetCampGroundByPark(parkId);
                if (campGrounds.Count > 0)
                {
                    Console.WriteLine(park.Name + " " + "National Park Campgrounds");
                    Console.WriteLine();
                    Console.WriteLine("Campgound Id".PadRight(21) + "Name".PadRight(40) + "Open".PadRight(20) + "Close".PadRight(20) + "Daily Fee");

                    foreach (CampGround campGround in campGrounds)
                    {
                        Console.WriteLine("#" + campGround.CampGroundId.ToString().PadRight(20) + campGround.Name.ToString().PadRight(40) + GetMonthString(campGround.OpenFrom).ToString().PadRight(20) + GetMonthString(campGround.OpenTo).ToString().PadRight(20) + "$" + campGround.DailyFee.ToString("0.00"));
                    }

                }

                Console.WriteLine();
                CampGroundMenu(parkId);
            }
        }
    }
}

    

    