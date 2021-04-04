using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace PayPal
{
    public class DonateCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "donate";
        public string Help => "Donate the server via PayPal and get awesome perks in return!";
        public string Syntax => "<>";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>() { "perm.donate" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = caller as UnturnedPlayer;
            player.Player.sendBrowserRequest("After purchasing, reconnect the server to get VIP perks!", Main.instance.CreatePurchaseLink((ulong)player.CSteamID));
        }
    }
}
