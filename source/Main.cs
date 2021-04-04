using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace PayPal
{
    class Main : RocketPlugin<Configuration>
    {
        public static Main instance { private set; get; }
        protected override void Load()
        {
            instance = this;
            Provider.onServerConnected += OnConnected;
        }

        protected override void Unload()
        {
            instance = null;
            Provider.onServerConnected -= OnConnected;
        }

        void OnConnected(CSteamID player)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.Instance.ConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT COUNT(*) FROM unturned_vips WHERE steamid = {(ulong)player}";

                if (cmd.ExecuteScalar().ToString() == "1")
                {
                    R.Permissions.AddPlayerToGroup("VIP+", UnturnedPlayer.FromCSteamID(player));
                    R.Permissions.Reload();

                    cmd = connection.CreateCommand();
                    cmd.CommandText = $"DELETE FROM unturned_vips WHERE steamid = {(ulong)player}";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string CreatePurchaseLink(ulong steamid)
        {
            var dict = new Dictionary<string, string>()
            {
                { "cmd", "_cart" },
                { "upload", "1" },
                { "business", Configuration.Instance.YourEmail }, // seller email
                { "custom", steamid.ToString() },    // steam id
                { "currency_code", "USD" },
                { "no_shipping", "1" },
                { "no_note", "1" },
                { "notify_url", Configuration.Instance.IPNLink },  // IPN PAGE
                { "return", Configuration.Instance.BaseURL },
                { "cancel_return", Configuration.Instance.BaseURL },
                { "item_name_1", Configuration.Instance.Package },
                { "item_number_1", "1" },
                { "amount_1", Configuration.Instance.Price.ToString() }
            };

            return QueryHelpers.AddQueryString("https://www.paypal.com/cgi-bin/", dict);
        }
    }
}
