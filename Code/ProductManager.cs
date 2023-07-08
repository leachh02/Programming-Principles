using System.Text.RegularExpressions;

namespace AuctionHouse
{
    /// <summary>A class to manage all products, inherits from the main manager.</summary>
    class ProductManager : MainManager
    {
        /// <summary>A constructor to create an instance of the ProductManager class.</summary>
        public ProductManager()
        {
            
        }

        /// <summary>A method used to validate a product's name.</summary>
        /// <returns>A valid name for a product.</returns>
        public string nameValidate(){
            string name = "";
            bool nameValid = false;
            while (!nameValid)
            {
                Console.WriteLine("\nProduct name");
                name = Input();
                if (notNullWhiteEmpty(name))
                {
                    nameValid = true;
                } else {
                    Console.WriteLine("Product Name must be some non-blank text");
                }
            }
            return name;
        }

        /// <summary>A method used to validate a product's description.</summary>
        /// <param name="name">Product's name used to check if the proposed description matches the name.</param>
        /// <returns>A valid description for a product.</returns>
        public string descValidate(string name){
            string desc = "";
            bool descValid = false;
            while (!descValid)
            {
                Console.WriteLine("\nProduct description");
                desc = Input();
                if (notNullWhiteEmpty(desc) && desc != name)
                {
                    descValid = true;
                } else {
                    Console.WriteLine("Product Description must be some non-blank text and not the same as the product name");
                }
            }
            return desc;
        }

        /// <summary>A method used to validate a product's price.</summary>
        /// <returns>A valid list price for a product.</returns>
        public string priceValidate(){
            string price = "";
            // Regex to confirm string has been given in the correct format
            string regex = @"^\$[0-9]+\.[0-9][0-9]$";
            bool priceValid = false;
            while (!priceValid)
            {
                Console.WriteLine("\nProduct price ($d.cc)");
                price = Input();
                // Confirms whether the price is valid or not
                if (notNullWhiteEmpty(price) && Regex.IsMatch(price, regex))
                {
                    priceValid = true;
                } else {
                    Console.WriteLine("A currency value is required, e.g. $54.95, $9.99, $2314.15");
                }
            }
            return price;
        }

        /// <summary>A method used to validate a product's new bid price.</summary>
        /// <param name="currentBidPrice">The current highest bid price.</param>
        /// <returns>A valid price that is the higher than the previous bid.</returns>
        public string bidPriceValidate(string currentBidPrice){
            string bidPrice = "";
            // Regex to confirm string has been given in the correct format
            string regex = @"^\$[0-9]+\.[0-9][0-9]$";
            bool bidPriceValid = false;
            // Converting the current bid price from string to decimal
            decimal currentBidPriceDec = decimal.Parse(currentBidPrice, System.Globalization.NumberStyles.Currency);
            while (!bidPriceValid)
            {
                Console.WriteLine("\nHow much do you bid?");
                bidPrice = Input();
                // Confirms whether the price is in a valid format or not
                if (notNullWhiteEmpty(bidPrice) && Regex.IsMatch(bidPrice, regex))
                {
                    // Converting the new bid price from string to decimal to test if it is greater than the current
                    decimal bidPriceDec = decimal.Parse(bidPrice, System.Globalization.NumberStyles.Currency);
                    if (bidPriceDec > currentBidPriceDec)
                    {
                        bidPriceValid = true;   
                    } else
                    {
                        Console.WriteLine($"    Bid amount must be greater than {currentBidPrice}");
                    }
                    
                } else {
                    Console.WriteLine("A currency value is required, e.g. $54.95, $9.99, $2314.15");
                }
            }
            return bidPrice;
        }

    }
}