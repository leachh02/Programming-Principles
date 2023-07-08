namespace AuctionHouse
{
    /// <summary>A class to represent the main menu, inherits from the generic menu.</summary>
    class MainMenu : GenericMenu
    {
        /// <summary>A constructor of the MainMenu class.</summary>
        public MainMenu()
        {

        }

        /// <summary>A method that displays the main menu in the console.</summary>
        /// <param name="manager">A ClientManager object.</param>
        /// <param name="clientMenu">A ClientMenu object.</param>
        /// <param name="productManager">A ProductManager object.</param>
        public void Menu(ClientManager manager, ClientMenu clientMenu, ProductManager productManager){
            // Loop until the user chooses to exit
            while(true){
                const string REGISTER = "1", SIGNIN = "2", EXIT = "3";
                Console.WriteLine(@"
Main Menu
---------
(1) Register
(2) Sign In
(3) Exit

Please select an option between 1 and 3");
                string choice = Input();
                if (choice == REGISTER){
                    // Runs the register method
                    Register(manager);
                } 
                else if (choice == SIGNIN){
                    // Runs the sign method
                    SignIn(manager, clientMenu, productManager);
                } 
                else if (choice == EXIT){
                    manager.DataWrite();
                    Console.WriteLine(@"+--------------------------------------------------+
| Good bye, thank you for using the Auction House! |
+--------------------------------------------------+");
                    break;
                }
            }
        }
        
        /// <summary>A method that executes the registration process.</summary>
        /// <param name="manager">A ClientManager object.</param>
        public void Register(ClientManager manager){
            Console.WriteLine(@"
Registration
------------");
            // Validates the name, email and password of the user
            string name = manager.nameValidate();
            string email = manager.emailValidate();
            string password = manager.passwordValidate();
            // Creates the client object and adds it to the list of clients already in the system, then informs the user
            Client clientRegister = new Client(name, email, password, "-", false);
            manager.Add(clientRegister);
            Console.WriteLine($"\nClient {name}({email}) has successfully registered at the Auction House.");
        }

        /// <summary>A method that executes the sign in sequence.</summary>
        /// <param name="manager">A ClientManager object.</param>
        /// <param name="clientMenu">A ClientMenu object.</param>
        /// <param name="productManager">A ProductManager object.</param>
        public void SignIn(ClientManager manager, ClientMenu clientMenu, ProductManager productManager){
            Console.WriteLine(@"
Sign In
-------");
            Console.WriteLine("\nPlease enter your email address");
            string email = Input();
            Console.WriteLine("\nPlease enter your password");
            string password = Input();
            // After obtaining the email and password of a potential client, the login method is used to return a client and then the client menu is enacted
            Client clientSignIn = manager.Login(email, password);
            if (clientSignIn != null){
                clientMenu.Menu(clientSignIn, productManager, manager);
            }
        }
    }
}