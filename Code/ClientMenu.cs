namespace AuctionHouse
{
    /// <summary>A class to represent the client menu, inherits from the generic menu.</summary>
    class ClientMenu : GenericMenu
    {
        /// <summary>A constructor of the ClientMenu class.</summary>
        public ClientMenu()
        {

        }

        /// <summary>A method that displays the client menu in the console.</summary>
        /// <param name="client">A Client object.</param>
        /// <param name="manager">A ProductManager object.</param>
        /// <param name="clientManager">A ClientManager object.</param>
        public void Menu(Client client, ProductManager manager, ClientManager clientManager){
            string input = "";
            const string ADVERTISE = "1", VIEW = "2", SEARCH = "3", BIDS = "4", PURCHASED = "5", LOGOFF = "6";
            do{
                Console.WriteLine(@"
Client Menu
-----------
(1) Advertise Product
(2) View My Product List
(3) Search For Advertised Products
(4) View Bids On My Products
(5) View My Purchased Items
(6) Log off

Please select an option between 1 and 6");
                input = Input();
                switch(input){
                    case ADVERTISE:
                    // Runs the advertise method
                        Advertise(client, manager);
                        break;
                    case VIEW:
                    // Runs the view method
                        View(client);
                        break;
                    case SEARCH:
                    // Runs the search method
                        Search(client, manager, clientManager);
                        break;
                    case BIDS:
                    // Runs the bids method
                        Bids(client, clientManager);
                        break;
                    case PURCHASED:
                    // Runs the purchased method
                        Purchased(client);
                        break;
                    case LOGOFF:
                    // Breaks the do while loop
                        break;
                    default:
                        Console.WriteLine("Please select an option between 1 and 6");
                        break;
                }
            } while(input != LOGOFF);
        }

        /// <summary>A method that enables the client to put products up for sale.</summary>
        /// <param name="client">A Client object.</param>
        /// <param name="manager">A ProductManager object.</param>
        public void Advertise(Client client, ProductManager manager){
            string productAdvertisment = $"Product Advertisement for {client.Name}({client.Email})";
            Console.WriteLine($"\n\n{productAdvertisment}");
            string dashes = "";
            foreach (char c in productAdvertisment)
            {
                dashes += "-";
            }
            Console.WriteLine(dashes);
            // Calls the product manager's methods and then creates the product
            string name = manager.nameValidate();
            string desc = manager.descValidate(name);
            string price = manager.priceValidate();
            Product newProduct = new Product(name, desc, price, client.Email,"-", "-", "-", "-");
            // The product is then added to the client's list of product
            client.AddProduct(newProduct);
            Console.WriteLine($"\nSuccessfully added product {name}, {desc}, {price}.");
        }
        
        /// <summary>A method that enables the client to the products they've put up for sale.</summary>
        /// <param name="client">A Client object.</param>
        public void View(Client client){
            int productNum = client.products.Count;
            int counter = 1;
            string productAdvertisment = $"Product List for {client.Name}({client.Email})";
            Console.WriteLine($"\n{productAdvertisment}");
            string dashes = "";
            foreach (char c in productAdvertisment)
            {
                dashes += "-";
            }
            Console.WriteLine(dashes);
            if (productNum == 0)
            {
                Console.WriteLine("\nYou have no advertised products at the moment.");
            } else {
                // loops through client's list of products
                Console.WriteLine("\nItem #\tProduct name\tDescription\tList price\tBidder name\tBidder email\tBid amt");
                foreach (Product item in client.products)
                {
                    Console.WriteLine($"{counter}\t{item.Name}\t{item.Desc}\t{item.Price}\t{item.BidName}\t{item.BidEmail}\t{item.BidPrice}");
                    counter += 1;
                }
            }
        }

        /// <summary>A method that enables the client to search for products.</summary>
        /// <param name="client">A Client object.</param>
        /// <param name="manager">A ProductManager object.</param>
        /// <param name="clientManager">A ClientManager object.</param>
        public void Search(Client client, ProductManager manager, ClientManager clientManager){
            int counter = 1;
            string productSearch = $"Product Search for {client.Name}({client.Email})";
            Console.WriteLine($"\n{productSearch}");
            string dashes = "";
            foreach (char c in productSearch)
            {
                dashes += "-";
            }
            Console.WriteLine(dashes);
            string searchTerm = clientManager.searchTermValidate();
            // A list of products that satisfy the search term are obtained
            List<Product> searchList = clientManager.returnList(searchTerm, client);
            if (searchList.Count == 0)
            {
                Console.WriteLine($"\nNo items found under the search term {searchTerm}!");
            } else
            {
                // The list of products is looped through
                Console.WriteLine($"\nSearch results");
                Console.WriteLine($"--------------");
                Console.WriteLine("\nItem #\tProduct name\tDescription\tList price\tBidder name\tBidder email\tBid amt");
                foreach (Product item in searchList)
                {
                    Console.WriteLine($"{counter}\t{item.Name}\t{item.Desc}\t{item.Price}\t{item.BidName}\t{item.BidEmail}\t{item.BidPrice}");
                    counter += 1;
                }
                // The client is asked if they want to purchase a product
                string bidResponse = clientManager.bidResponseValidate();
                if (bidResponse == "yes")
                {
                    // The item row is then asked for and the product is then returned
                    int searchListIndex = clientManager.rowSelectionValidate(counter - 1);
                    Product selectedProduct = searchList[searchListIndex-1];
                    Console.WriteLine($"\nBidding for {selectedProduct.Name} (regular price {selectedProduct.Price}), current highest bid {selectedProduct.returnBidPrice()}\n");
                    // A bid is then placed, and the products information is then updated
                    string bidPrice = manager.bidPriceValidate(selectedProduct.returnBidPrice());
                    Console.WriteLine($"\nYour bid of {bidPrice} for {selectedProduct.Name} is placed.");
                    selectedProduct.UpdateBidderInfo(client.Name, client.Email, bidPrice);
                    // Call the delivery method
                    Delivery(clientManager, selectedProduct);
                }
            }
        }

        /// <summary>A method that enables the client to specify how they want the product to be delivered.</summary>
        /// <param name="clientManager">A ClientManager.</param>
        /// <param name="selectedProduct">A Product object.</param>
        public void Delivery(ClientManager clientManager, Product selectedProduct){
            Console.WriteLine("\nDelivery Instructions");
            Console.WriteLine("---------------------");
            Console.WriteLine("(1) Click and collect");
            Console.WriteLine("(2) Home Delivery");
            // User is asked for preferred method of delivery
            string delOption = clientManager.delOptionValidate();
            if (delOption == "1")
            {
                // If option 1 is selected, the client is asked to give a valid click and collect time frame, this information is then updated in the product
                DateTime windowStart = clientManager.windowStartValidate();
                DateTime windowEnd = clientManager.windowEndValidate(windowStart);
                Console.WriteLine($"\nThank you for your bid. If successful, the item will be provided via collection between {windowStart.ToString("hh:mm tt")} on {windowStart.ToString("dd/MM/yyyy")} and {windowEnd.ToString("hh:mm tt")} on {windowEnd.ToString("dd/MM/yyyy")}");
                selectedProduct.UpdateDelOption($"Collection between {windowStart.ToString("hh:mm tt")} on {windowStart.ToString("dd/MM/yyyy")} and {windowEnd.ToString("hh:mm tt")} on {windowEnd.ToString("dd/MM/yyyy")}");
            }
            if (delOption == "2")
            {
                // If option 2 is selected, the client is asked to give a valid delivery address, this information is then updated in the product
                string delAddress = clientManager.delAddressValidate();
                Console.WriteLine($"\nThank you for your bid. If successful, the item will be provided via delivery to {delAddress}");
                selectedProduct.UpdateDelOption($"Delivery to {delAddress}");
            }
        }

        /// <summary>A method that displays the client's products that have been bidded on.</summary>
        /// <param name="client">A Client object.</param>
        /// <param name="clientManager">A ClientManager object.</param>
        public void Bids(Client client, ClientManager clientManager){
            // A list of the client's products that have been bidded upon are obtained
            List<Product> bidProducts = client.bidProducts();
            int productNum = bidProducts.Count;
            int counter = 1;
            string productBidsString = $"List Product Bids for {client.Name}({client.Email})";
            Console.WriteLine($"\n{productBidsString}");
            string dashes = "";
            foreach (char c in productBidsString)
            {
                dashes += "-";
            }
            Console.WriteLine(dashes);
            if (productNum == 0)
            {
                Console.WriteLine("\nNo bids were found.");
            } else
            {
                // The list of products is looped through
                Console.WriteLine("Item #\tProduct name\tDescription\tList price\tBidder name\tBidder email\tBid amt");
                foreach (Product item in client.bidProducts())
                {
                    Console.WriteLine($"{counter}\t{item.Name}\t{item.Desc}\t{item.Price}\t{item.BidName}\t{item.BidEmail}\t{item.BidPrice}");
                    counter += 1;
                    
                }
                // The user is asked if they want to sell an item
                string bidResponse = clientManager.sellResponseValidate();
                if (bidResponse == "yes")
                {
                    // Confirms which item is being sold (-1 because the counter starts ar 1 but list indexing starts at 0)
                    int searchListIndex = clientManager.rowSelectionValidate(counter - 1);
                    Product selectedProduct = bidProducts[searchListIndex-1];
                    // Once the product to be sold is retrieved, the bidders Client object is then as well
                    Console.WriteLine($"You have sold {selectedProduct.Name} to {selectedProduct.BidName} for {selectedProduct.BidPrice}");
                    Client buyer = clientManager.returnBuyer(selectedProduct.BidName, selectedProduct.BidEmail);
                    // The buyer's purchased product is appended with this new product and the seller's products list is decreased by this item
                    buyer.AddPurchased(selectedProduct);
                    client.Remove(selectedProduct);
                }
            }
        }

        /// <summary>A method that displays the client's purchased products.</summary>
        /// <param name="client">A Client object.</param>
        public void Purchased(Client client){
            int productNum = client.purchasedProducts.Count;
            int counter = 1;
            string productAdvertisment = $"Purchased Items for {client.Name}({client.Email})";
            Console.WriteLine($"\n{productAdvertisment}");
            string dashes = "";
            foreach (char c in productAdvertisment)
            {
                dashes += "-";
            }
            Console.WriteLine(dashes);
            if (productNum == 0)
            {
                Console.WriteLine("\nYou have no purchased products at the moment.");
            } else {
                // The list of products is looped through
                Console.WriteLine("Item #\tSeller email\tProduct name\tDescription\tList price\tAmt paid\tDelivery option");
                foreach (Product item in client.purchasedProducts)
                {
                    Console.WriteLine($"{counter}\t{item.SellerEmail}\t{item.Name}\t{item.Desc}\t{item.Price}\t{item.BidPrice}\t{item.DelOption}");
                    counter += 1;
                }
            }
        }

    }
}