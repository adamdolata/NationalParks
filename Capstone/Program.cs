using Capstone;
using System;

namespace capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            NationalParksCLI cli = new NationalParksCLI();
            cli.RunCLI();
        }
    }
}
