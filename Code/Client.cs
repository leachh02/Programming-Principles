namespace AuctionHouse
{
    /// <summary>A class to represent a client.</summary>
    class Client
    {
        /// <summary>The name of the client.</summary>
        public string Name { get; }
        /// <summary>The email of the client.</summary>
        public string Email { get; }
        /// <summary>The password of the client.</summary>
        public string Password { get; }

        /// <summary>The client's address.</summary>
        public string Address { get; private set; }
        /// <summary>A true or false bit if the client has given an address yet.</summary>
        public bool AddressGiven { get; private set; }
        /// <summary>A list of the client's products.</summary>
        public List<Product> products;
        /// <summary>A list of a client's purchased products.</summary>
        public List<Product> purchasedProducts;

        /// <summary>The client constructor to create Client objects.</summary>>
        /// <param name="name">Client's name.</param>
        /// <param name="email">Client's email.</param>
        /// <param name="password">Client's password.</param>
        /// <param name="address">Client's address.</param>
        /// <param name="addressGiven">Check if client has given address.</param>
        public Client(string name, string email, string password, string address, bool addressGiven)
        {
            Name = name;
            Email = email;
            Password = password;
            Address = address;
            AddressGiven = addressGiven;
            products = new List<Product>();
            purchasedProducts = new List<Product>();
        }

        /// <summary>A method that updates the client's address and now returns true for addressGiven.</summary>>
        /// <param name="newAddress">Client's address.</param>
        /// <param name="addressGivenConfirmation">Now that address is being given, this will be true.</param>
        public void Update(string newAddress, bool addressGivenConfirmation)
        {
            Address = newAddress;
            AddressGiven = addressGivenConfirmation;
        }

        /// <summary>A method that adds a new product to the client's list of products and then sorts the list.</summary>>
        /// <param name="newProduct">The new product to be added.</param>
        public void AddProduct(Product newProduct)
        {
            products.Add(newProduct);
            List<Product> sorted = products.OrderBy(o=>o.Name).ThenBy(o=>o.Desc).ThenBy(o=>o.Price).ToList();
            products = sorted;
        }

        /// <summary>A method that adds a new product to the client's list of purchased products and then sorts the list.</summary>>
        /// <param name="newProduct">The recently purchased product to be added.</param>
        public void AddPurchased(Product newProduct)
        {
            purchasedProducts.Add(newProduct);
            List<Product> sorted = purchasedProducts.OrderBy(o=>o.Name).ThenBy(o=>o.Desc).ThenBy(o=>o.Price).ToList();
            purchasedProducts = sorted;
        }

        /// <summary>A method that removes a purchased product from the client's list of purchased products.</summary>>
        /// <param name="productToRemove">The recently purchased product to be removed from the products up for sale.</param>
        public void Remove(Product productToRemove)
        {
            products.Remove(productToRemove);
        }

        /// <summary>A method that returns a list of products belonging to the client that have been bidded upon.</summary>>
        /// <returns>A list of products that have been bidded on.</returns>>
        public List<Product> bidProducts()
        {
            // Searches through all the products and checks to see if the most recent bidder's name doesn't equal '-', implying the product has been bidded on
            List<Product> bidProducts = new List<Product>();
            foreach (Product item in products)
            {
                if (item.BidName != "-")
                {
                    bidProducts.Add(item);
                }
            }
            return bidProducts;
        }

    }
}