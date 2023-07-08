namespace AuctionHouse
{
    /// <summary>An abstract class that parents all managers, includes useful methods to give to it's children.</summary>
    abstract class MainManager
    {
        /// <summary>A constructor of the MainManager class.</summary>
        public MainManager()
        {

        }

        /// <summary>A method used as a means to improve the UI.</summary>
        /// <returns>The string input given from the user.</returns>
        public string Input(){
            Console.Write("> ");
            string input = Console.ReadLine();
            return input;
        }

        /// <summary>A method used to check whether the input is either null, empty or white space.</summary>
        /// <param name="inputString">The string that will beck checked for null, empty or white space.</param>
        /// <returns>True if the input is neither null, empty or white space. Otherwise return False.</returns>
        public bool notNullWhiteEmpty(string inputString){
            if (!(string.IsNullOrEmpty(inputString) || string.IsNullOrWhiteSpace(inputString)))
            {
                return true;
            } else {
                return false;
            }
        }
    }
}