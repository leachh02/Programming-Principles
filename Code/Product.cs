namespace AuctionHouse
{
    /// <summary>A class to represent a product.</summary>
    class Product
    {
        /// <summary>The name of tge product.</summary>
        public string Name { get; }
        /// <summary>The description of the product.</summary>
        public string Desc { get; }
        /// <summary>The price of the product.</summary>
        public string Price { get; }
        /// <summary>The email of the seller of a product.</summary>
        public string SellerEmail { get; }
        /// <summary>The name of the highest bidder.</summary>
        public string BidName { get; private set; }
        /// <summary>The email of the highest bidder.</summary>
        public string BidEmail { get; private set; }
        /// <summary>The price of the highest bid.</summary>
        public string BidPrice { get; private set; }
        /// <summary>The delivery option chosen by the highest bidder.</summary>
        public string DelOption { get; private set; }

        /// <summary>The product constructor to create Product objects.</summary>
        /// <param name="name">Product name.</param>
        /// <param name="desc">Product description.</param>
        /// <param name="price">Product price.</param>
        /// <param name="sellerEmail">Email of the owner of the product.</param>
        /// <param name="bidName">Highest bidder's name.</param>
        /// <param name="bidEmail">Highest bidder's email.</param>
        /// <param name="bidPrice">Highest bidder's price.</param>
        /// <param name="delOption">Bidder's preferred method of delivery.</param>
        public Product(string name, string desc, string price, string sellerEmail, string bidName, string bidEmail, string bidPrice, string delOption)
        {
            Name = name;
            Desc = desc;
            Price = price;
            SellerEmail = sellerEmail;
            BidName = bidName;
            BidEmail = bidEmail;
            BidPrice = bidPrice;
            DelOption = delOption;
        }

        /// <summary>Method to update new highest bidder's information.</summary>
        /// <param name="bidName">Highest bidder's name.</param>
        /// <param name="bidEmail">Highest bidder's email.</param>
        /// <param name="bidPrice">Highest bidder's price.</param>
        public void UpdateBidderInfo(string newBidName, string newBidEmail, string newBidPrice)
        {
            BidName = newBidName;
            BidEmail = newBidEmail;
            BidPrice = newBidPrice;
        }
        /// <summary>Method to update the chosen delivery option.</summary>
        /// <param name="delOption">Bidder's preferred method of delivery.</param>
        public void UpdateDelOption(string newDelOption)
        {
            DelOption = newDelOption;
        }
        /// <summary>Method to return the highest bid price even in situations where the bid price is, '-'.</summary>
        /// <returns>Current highest bid price.</returns>
        public string returnBidPrice(){
            if (BidPrice == "-")
            {
                return "$0.00";
            }else
            {
                return BidPrice;
            }
        }
    }
}