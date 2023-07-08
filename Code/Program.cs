using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AuctionHouse {
    class Program
    {
        /// <summary>The Main program creates all neccessary objects and extracts the data from the persistent database before executing the console.</summary>
        /// <param name="args">A selection of arguments.</param>
        public static void Main(string[] args)
        {
            // Initialise objects
            MainMenu mainMenu = new MainMenu();
            ClientMenu clientMenu = new ClientMenu();
            ClientManager clientManager = new ClientManager();
            ProductManager productManager = new ProductManager();
            
            // Check database exists otherwise creates the database file
            string filename = "Database.txt";
            clientManager.FileCheck(filename);

            // Extracts data from the database
            clientManager.DataRead();
            Console.WriteLine(@"+------------------------------+
| Welcome to the Auction House |
+------------------------------+");
            //Executes the console's main menu
            mainMenu.Menu(clientManager, clientMenu, productManager);
        }
    }
} 