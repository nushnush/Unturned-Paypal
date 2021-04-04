using Rocket.API;

namespace PayPal
{
    public class Configuration : IRocketPluginConfiguration
    {
        public string ConnectionString { get; set; }

        public string YourEmail { get; set; }
        public string BaseURL { get; set; }
        public string IPNLink { get; set; }
        public string Package { get; set; }
        public int Price { get; set; } 

        public void LoadDefaults()
        {
            YourEmail = "bruh11@gmail.com";
            BaseURL = "http://www.yelksdev.xyz/";
            IPNLink = "http://www.yelksdev.xyz/ipnhandler.php";
            Package = "VIP+";
            Price = 5;

            ConnectionString = "SERVER=YOUR_IP;DATABASE=Unturned;UID=admin;PASSWORD=123;PORT=3306";
        }
    }
}
