namespace AuctionHouse
{
    /// <summary>An abstract class that parents all menus, includes a useful method to give to it's children.</summary>
    abstract class GenericMenu
    {
        /// <summary>A constructor of the GenericMenu class.</summary>
        public GenericMenu()
        {

        }

        /// <summary>A method used as a means to improve the UI.</summary>
        /// <returns>The string input given from the user.</returns>
        public string Input(){
            Console.Write("> ");
            string input = Console.ReadLine();
            return input;
        }
    }
}
