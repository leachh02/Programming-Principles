using System.Text.RegularExpressions;

namespace AuctionHouse
{
    /// <summary>A class to manage all clients, inherits from the main manager.</summary>
    class ClientManager : MainManager
    {
        
        /// <summary>A list that stores all clients in the system.</summary>
        private List<Client> clients;
    
        /// <summary>A constructor to create an instance of the ClientManager class.</summary>
        public ClientManager()
        {
            clients = new List<Client>();
        }

        /// <summary>A method to return a list of products based on the search term.</summary>
        /// <param name="searchTerm">The search term used for finding products.</param>
        /// <param name="skipClient">All clients are checked for products, except the client using the client menu.</param>
        /// <returns>An ordered list of all products that satisfy the search term.</returns>
        public List<Product> returnList(string searchTerm, Client skipClient)
        {
            List<Product> searchReturn = new List<Product>();
            //  Loops through all clients the client manager has stored except the client that is skipClient
            foreach (Client user in clients)
            {
                if (user != skipClient){
                    foreach (Product item in user.products)
                    {
                        // If the search term is ALL, return all products, otherwise check if the search term is contained within either the name or description of products
                        if (searchTerm == "ALL")
                        {
                            searchReturn.Add(item);
                        }
                        else if (item.Name.Contains(searchTerm) || item.Desc.Contains(searchTerm))
                        {
                            searchReturn.Add(item);
                        }   
                    }
                }
            }
            // Sorts the list before returning
            List<Product> sorted = searchReturn.OrderBy(o=>o.Name).ThenBy(o=>o.Desc).ThenBy(o=>o.Price).ToList();
            searchReturn = sorted;
            return searchReturn;
        }

        /// <summary>A method that returns a client.</summary>
        /// <param name="buyerName">The name of the buyer.</param>
        /// <param name="buyerEmail">The email of the buyer.</param>
        /// <returns>A client that has purchased an item.</returns>
        public Client returnBuyer(string buyerName, string buyerEmail){
            Client buyer = new Client("","","","",false);
            foreach (Client person in clients)
            {
                // Loop through all clients and if the name and email match return this user
                if (person.Name == buyerName && person.Email == buyerEmail)
                {
                    buyer = person;
                }
            }
            return buyer;
        }

        /// <summary>A method used to add new clients that have registered.</summary>
        public void Add(Client newClient)
        {
            clients.Add(newClient);
        }

        /// <summary>A method used to check if the persistent database file exists and if it doesn't, creates it.</summary>
        /// <param name="filename">The name of the database file.</param>
        public void FileCheck(string filename){
            if (!File.Exists(filename)) {
                using FileStream fs = File.Create(filename);
            }
        }

        /// <summary>Writes data TO the database file.</summary>
        public void DataWrite(){
            string addressGivenString;
            string filename = "Database.txt";
            // Creates the stream writer
            using StreamWriter writer = new StreamWriter(filename);
            // For every client in the system, add the client's details and all the details of the client's products and purchased products
            foreach (Client client in clients)
            {
                writer.Write('\n' +"Client");
                writer.Write('\n' + client.Name);
                writer.Write('\n' + client.Email);
                writer.Write('\n' + client.Password);
                writer.Write('\n' + client.Address);
                addressGivenString = client.AddressGiven.ToString();
                writer.Write('\n' + addressGivenString);
                foreach (Product product in client.products)
                {
                    writer.Write('\n' +"Product");
                    writer.Write('\n' + product.Name);
                    writer.Write('\n' + product.Desc);
                    writer.Write('\n' + product.Price);
                    writer.Write('\n' + product.SellerEmail);
                    writer.Write('\n' + product.BidName);
                    writer.Write('\n' + product.BidEmail);
                    writer.Write('\n' + product.BidPrice);
                    writer.Write('\n' + product.DelOption);
                }
                foreach (Product product in client.purchasedProducts)
                {
                    writer.Write('\n' +"PurchasedProduct");
                    writer.Write('\n' + product.Name);
                    writer.Write('\n' + product.Desc);
                    writer.Write('\n' + product.Price);
                    writer.Write('\n' + product.SellerEmail);
                    writer.Write('\n' + product.BidName);
                    writer.Write('\n' + product.BidEmail);
                    writer.Write('\n' + product.BidPrice);
                    writer.Write('\n' + product.DelOption);
                }
            }

        }

        /// <summary>Reads data FROM the database file.</summary>
        public void DataRead(){
            bool addressGivenBool;
            string filename = "Database.txt";
            // Creates the stream reader
            using StreamReader reader = new StreamReader(filename);
            string data = reader.ReadToEnd();
            string[] clientData = data.Split("\nClient");
            // The data from the file is split everytime the word 'Client' is found
            // Then further splitting this data by newlines, the data can be restored to lists
            foreach (string client in clientData)
            {
                if (notNullWhiteEmpty(client))
                {
                    string[] dataLine = client.Split('\n');
                    addressGivenBool = bool.Parse(dataLine[5]);
                    Client clientSystemStart = new Client(dataLine[1], dataLine[2], dataLine[3], dataLine[4], addressGivenBool);
                    Add(clientSystemStart);
                    for (int i = 5; i < dataLine.Length; i++)
                    {
                        if (dataLine[i] == "Product")
                        {
                            Product productSystemStart = new Product(dataLine[i+1], dataLine[i+2], dataLine[i+3], dataLine[i+4], dataLine[i+5], dataLine[i+6], dataLine[i+7], dataLine[i+8]);
                            clientSystemStart.AddProduct(productSystemStart);
                        }
                        if (dataLine[i] == "PurchasedProduct")
                        {
                            Product productSystemStart = new Product(dataLine[i+1], dataLine[i+2], dataLine[i+3], dataLine[i+4], dataLine[i+5], dataLine[i+6], dataLine[i+7], dataLine[i+8]);
                            clientSystemStart.AddPurchased(productSystemStart);
                        }
                    }
                }   
            }
        }

        /// <summary>A method used to validate a client's name.</summary>
        /// <returns>A valid name for a client.</returns>
        public string nameValidate(){
            string name = "";
            bool nameValid = false;
            while (!nameValid)
            {
                Console.WriteLine("\nPlease enter your name");
                name = Input();
                
                if (notNullWhiteEmpty(name))
                {
                    nameValid = true;                    
                } else {
                    Console.WriteLine("The supplied value is not a valid Name");
                } 
            }
            return name;
        } 
        
        /// <summary>A method used to validate a client's email.</summary>
        /// <returns>A valid email for a client.</returns>
        public string emailValidate(){
            string email = "";
            bool emailValid = false;
            bool newUser = true;
            // Regex used to check if the user's input follows the correct format
            string regex = @"^[A-Za-z0-9_\-\.]+[A-Za-z0-9]@[A-Za-z0-9\-][A-Za-z0-9\-\.]+\.[A-Za-z]+$";
            while (!emailValid)
            {
                newUser = true;
                Console.WriteLine("\nPlease enter your email address");
                email = Input();
                // Checks for new user
                foreach (Client client in clients)
                {
                    if (client.Email == email)
                    {
                        newUser = false;
                    }
                }
                // Validates email input
                if (newUser)
                {
                    if (Regex.IsMatch(email, regex)){
                        emailValid = true;
                    } else {
                        Console.WriteLine("The supplied value is not a valid email address");
                    }
                } else {
                    Console.WriteLine("The supplied address is already in use");
                }
            }
            return email;
        }

        /// <summary>A method used to validate a client's password.</summary>
        /// <returns>A valid password for a client.</returns>
        public string passwordValidate(){
            string password = "";
            bool passwordValid = false;
            // Regex used to check if the user's input follows the correct format
            string regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).*$";
            while (!passwordValid)
            {
                Console.WriteLine(@"
Please choose a password
* At least 8 characters
* No white space characters
* At least one upper-case letter
* At least one lower-case letter
* At least one digit
* At least one special character");
                password = Input();
                // Validates password input
                if (password.Length >= 8 && Regex.IsMatch(password, regex)){
                    passwordValid = true;
                } else {
                    Console.WriteLine("The supplied value is not a valid password");
                }
            }
            return password;
        }

        /// <summary>A method to login a user.</summary>
        /// <param name="email">The given email of the user.</param>
        /// <param name="password">The given password of the user.</param>
        /// <returns>A Client with the corresponding email and password.</returns>
        public Client Login(string email, string password)
        {
            foreach (Client client in clients)
            {
                // Loop through all clients and if the email and password match return this user
                if (client.Email == email && client.Password == password)
                {
                    if (client.AddressGiven)
                    {
                        return client;
                    } else {
                        addressValidate(client);
                        return client;
                    }
                }
            }
            Console.WriteLine("Email or password do not match");
            return null;
        }

        /// <summary>A method to validate a user's address.</summary>
        /// <param name="client">The client who's address is going to be validated.</param>
        public void addressValidate(Client client){
            string address;
            string personalDets = $"Personal Details for {client.Name}({client.Email})";
            Console.WriteLine($"\n{personalDets}");
            string dashes = "";
            foreach (char c in personalDets)
            {
                dashes += "-";
            }
            Console.WriteLine(dashes);
            Console.WriteLine("\nPlease provide your home address.");
            // Obtains all neccessary information by callling other methods
            int unitNum = unitNumValidate();
            int streetNum = streetNumValidate();
            string streetName = streetNameValidate();
            string streetSuffix = streetSuffixValidate();
            string city = cityValidate();
            string state = stateValidate();
            int postcode = postcodeValidate();
            // Create the address
            if (unitNum == 0)
            {
                address = $"{streetNum} {streetName} {streetSuffix}, {city} {state} {postcode}";
                Console.WriteLine($"\nAddress has been updated to {address}");
            } else {
                address = $"{unitNum}/{streetNum} {streetName} {streetSuffix}, {city} {state} {postcode}";
                Console.WriteLine($"\nAddress has been updated to {address}");
            }
            // Updates the client's address
            client.Update(address, true);
        } 

        /// <summary>A method to validate a user's unit number.</summary>
        /// <returns>An integer unit number.</returns>
        public int unitNumValidate(){
            string unitNum = "";
            int unitNumInt = 0;
            bool unitNumValid = false;
            while (!unitNumValid)
            {
                Console.WriteLine("\nUnit number (0 = none):");
                unitNum = Input();
                unitNumInt = int.Parse(unitNum);
                if (notNullWhiteEmpty(unitNum) && unitNumInt >= 0)
                {
                    unitNumValid = true;
                } else {
                    Console.WriteLine("Unit Number must be a positive non zero interger if present or 0");
                }
            }
            return unitNumInt;
        }

        /// <summary>A method to validate a user's street number.</summary>
        /// <returns>An integer street number.</returns>
        public int streetNumValidate(){
            string streetNum = "";
            int streetNumInt = 0;
            bool streetNumValid = false;
            while (!streetNumValid)
            {
                Console.WriteLine("\nStreet number:");
                streetNum = Input();
                streetNumInt = int.Parse(streetNum);
                if (streetNumInt > 0)
                {
                    streetNumValid = true;
                } else {
                    Console.WriteLine("Street Number must be a positive non zero interger");
                }
            }
            return streetNumInt;
        }

        /// <summary>A method to validate a user's street name.</summary>
        /// <returns>A string street name.</returns>
        public string streetNameValidate(){
            string streetName = "";
            bool streetNameValid = false;
            while (!streetNameValid)
            {
                Console.WriteLine("\nStreet name:");
                streetName = Input();
                if (notNullWhiteEmpty(streetName))
                {
                    streetNameValid = true;
                } else {
                    Console.WriteLine("Street Name must be some non-blank text");
                }
            }
            return streetName;
        }

        /// <summary>A method to validate a user's street suffix.</summary>
        /// <returns>A string street suffix.</returns>
        public string streetSuffixValidate(){
            string streetSuffix = "";
            bool streetSuffixValid = false;
            while (!streetSuffixValid)
            {
                Console.WriteLine("\nStreet suffix:");
                streetSuffix = Input();
                if (notNullWhiteEmpty(streetSuffix))
                {
                    streetSuffixValid = true;
                } else {
                    Console.WriteLine("Street Suffix must be a common Australian suffix");
                }
            }
            return streetSuffix;
        }

        /// <summary>A method to validate a user's city.</summary>
        /// <returns>A string city.</returns>
        public string cityValidate(){
            string city = "";
            bool cityValid = false;
            while (!cityValid)
            {
                Console.WriteLine("\nCity:");
                city = Input();
                if (notNullWhiteEmpty(city))
                {
                    cityValid = true;
                } else {
                    Console.WriteLine("City must be some non-blank text");
                }
            }
            return city;
        }

        /// <summary>A method to validate a user's state.</summary>
        /// <returns>A string state.</returns>
        public string stateValidate(){
            string state = "";
            // The list of allowable states
            string[] states = new string[8]{"ACT", "NSW", "NT", "QLD", "SA", "TAS", "VIC", "WA"};
            bool stateValid = false;
            while (!stateValid)
            {
                Console.WriteLine("\nState (ACT, NSW, NT, QLD, SA, TAS, VIC, WA):");
                state = Input();
                state = state.ToUpper();
                foreach (string stat in states)
                {
                    if (state == stat)
                    {
                        stateValid = true;
                    }
                }
                if (!stateValid)
                {
                    Console.WriteLine("State must be an Australian state");
                }
            }
            return state;
        }

        /// <summary>A method to validate a user's postcode.</summary>
        /// <returns>An int postcode.</returns>
        public int postcodeValidate(){
            string postcode = "";
            int postcodeInt = 0;
            bool postcodeValid = false;
            while (!postcodeValid)
            {
                Console.WriteLine("\nPostcode (1000 .. 9999):");
                postcode = Input();
                postcodeInt = int.Parse(postcode);
                if (postcodeInt >= 1000 && postcodeInt <= 9999)
                {
                    postcodeValid = true;
                } else {
                    Console.WriteLine("Postcode must be between 1000 and 9999");
                }                
            }
            return postcodeInt;
        }

        /// <summary>A method to validate a user's search term.</summary>
        /// <returns>The valid search term.</returns>
        public string searchTermValidate(){
            string searchTerm = "";
            bool searchTermValid = false;
            while (!searchTermValid)
            {
                Console.WriteLine("\n\nPlease supply a search phrase (ALL to see all products)");
                searchTerm = Input();
                if (notNullWhiteEmpty(searchTerm))
                {
                    searchTermValid = true;                    
                } else {
                    Console.WriteLine("The supplied value is not a valid Search Term");
                } 
            }
            return searchTerm;
        }

        /// <summary>A method to validate a user's bid response.</summary>
        /// <returns>The valid bid response, either yes or no.</returns>
        public string bidResponseValidate(){
            string bidResponse = "";
            bool bidResponseValid = false;
            while (!bidResponseValid)
            {
                Console.WriteLine("\n\nWould you like to place a bid on any of these items (yes or no)?");
                bidResponse = Input();
                if (notNullWhiteEmpty(bidResponse) && (bidResponse == "yes" || bidResponse == "no"))
                {
                    bidResponseValid = true;                    
                } else {
                    Console.WriteLine("The supplied value is not a valid response");
                } 
            }
            return bidResponse;
        }

        /// <summary>A method to validate a user's item (row number) selection.</summary>
        /// <param name="limit">The largest number a user can select.</param>
        /// <returns>A valid integer between 1 and the limit.</returns>
        public int rowSelectionValidate(int limit){
            string rowSelection = "";
            int rowSelectionInt = 0;
            bool rowSelectionValid = false;
            while (!rowSelectionValid)
            {
                Console.WriteLine($"\nPlease enter a non-negative integer between 1 and {limit}:");
                rowSelection = Input();
                rowSelectionInt = int.Parse(rowSelection);
                if (notNullWhiteEmpty(rowSelection) && rowSelectionInt >= 1 && rowSelectionInt <= limit)
                {
                    rowSelectionValid = true;                    
                } else {
                    Console.WriteLine("The supplied value is not a valid row");
                } 
            }
            return rowSelectionInt;
        }

        /// <summary>A method to validate a user's sell response.</summary>
        /// <returns>The valid sell response, either yes or no.</returns>
        public string sellResponseValidate(){
            string sellResponse = "";
            bool sellResponseValid = false;
            while (!sellResponseValid)
            {
                Console.WriteLine("\nWould you like to sell something (yes or no)?");
                sellResponse = Input();
                if (notNullWhiteEmpty(sellResponse) && (sellResponse == "yes" || sellResponse == "no"))
                {
                    sellResponseValid = true;                    
                } else {
                    Console.WriteLine("The supplied value is not a valid response");
                } 
            }
            return sellResponse;
        }

        /// <summary>A method to validate a buyer's delivery option.</summary>
        /// <returns>The valid delivery option, either 1 or 2.</returns>
        public string delOptionValidate(){
            string delOption = "";
            bool delOptionValid = false;
            while (!delOptionValid)
            {
                Console.WriteLine("\nPlease select an option between 1 and 2");
                delOption = Input();
                if (notNullWhiteEmpty(delOption) && (delOption == "1" || delOption == "2"))
                {
                    delOptionValid = true;                    
                } else {
                    Console.WriteLine("The supplied value is not a valid response");
                } 
            }
            return delOption;
        }

        /// <summary>A method to validate a buyer's starting time frame.</summary>
        /// <returns>The valid DateTime of the starting delivery time frame.</returns>
        public DateTime windowStartValidate(){
            // Timespan of an hour
            TimeSpan hour = TimeSpan.FromHours(1);
            string windowStartString = "";
            DateTime windowStart = new DateTime();
            bool windowStartValid = false;
            while (!windowStartValid)
            {
                Console.WriteLine("\nDelivery window start (dd/mm/yyyy hh:mm)");
                windowStartString = Input();
                windowStart = DateTime.Parse(windowStartString);
                // Current time
                DateTime now = DateTime.Now;
                TimeSpan startSpan = windowStart.Subtract(now);
                // Checks if the timespan of the current time to the start of the time window is greater than an hour
                if (startSpan > hour)
                {
                    windowStartValid = true;                    
                } else {
                    Console.WriteLine("\nDelivery window start must be at least one hour in the future.");
                } 
            }
            return windowStart;
        }

        /// <summary>A method to validate a buyer's ending time frame.</summary>
        /// <returns>The valid DateTime of the end of the delivery time frame.</returns>
        public DateTime windowEndValidate(DateTime windowStart){
            // Timespan of an hour
            TimeSpan hour = TimeSpan.FromHours(1);
            string windowEndString = "";
            DateTime windowEnd = new DateTime();
            bool windowEndValid = false;
            while (!windowEndValid)
            {
                Console.WriteLine("\nDelivery window end (dd/mm/yyyy hh:mm)");
                windowEndString = Input();
                windowEnd = DateTime.Parse(windowEndString);
                TimeSpan endSpan = windowEnd.Subtract(windowStart);
                // Checks if the timespan of the starting time to the end time is greater than an hour
                if (endSpan > hour)
                {
                    windowEndValid = true;                    
                } else {
                    Console.WriteLine("\nDelivery window end must be at least one hour later than the start.");
                } 
            }
            return windowEnd;
        }
        
        /// <summary>A method to validate a client's delivery address.</summary>
        /// <returns>The client's validated delivery address.</returns>
        public string delAddressValidate(){
            string address;
            Console.WriteLine("\nPlease provide your delivery address.");
            int unitNum = unitNumValidate();
            int streetNum = streetNumValidate();
            string streetName = streetNameValidate();
            string streetSuffix = streetSuffixValidate();
            string city = cityValidate();
            string state = stateValidate();
            int postcode = postcodeValidate();
            if (unitNum == 0)
            {
                address = $"{streetNum} {streetName} {streetSuffix}, {city} {state} {postcode}";
            } else {
                address = $"{unitNum}/{streetNum} {streetName} {streetSuffix}, {city} {state} {postcode}";
            }
            return address;
        } 

    }
}