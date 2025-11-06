using Newtonsoft.Json.Linq;

namespace StardewViewerEvents.Credits
{
    public class CreditAccount
    {
        private const int CREDITS_STARTING_AMOUNT = 100; // Equivalent to donating one dollar

        public string discordName;
        public ulong discordId;
        public int credits;
        public DateTime lastActivityTime;
        public string twitchlink;

        public CreditAccount()
        {
            Reset();
        }

        public CreditAccount(JObject data)
        {
            discordName = data["discordName"].ToString();
            discordId = ulong.Parse(data["discordId"].ToString());
            credits = Int32.Parse(data["credits"].ToString());
            lastActivityTime = data.ContainsKey("lastActivityTime") ? DateTime.Parse(data["lastActivityTime"].ToString()) : DateTime.Now;
            twitchlink = data["twitchlink"]?.ToString();
        }

        public void Reset()
        {
            credits = CREDITS_STARTING_AMOUNT;
        }

        public int GetCredits()
        {
            lastActivityTime = DateTime.Now;
            return credits;
        }

        public void AddCredits(int amount)
        {
            SetCredits(credits + amount);
        }

        public void RemoveCredits(int amount)
        {
            SetCredits(credits - amount);
        }

        public void SetCredits(int amount)
        {
            lastActivityTime = DateTime.Now;
            credits = amount;
        }

        public void LinkTo(string twitchName)
        {
            if (HasTwitchLink())
            {
                return;
            }

            twitchlink = twitchName;
        }

        public void TwitchUnlink()
        {
            twitchlink = null;
        }

        public bool HasTwitchLink()
        {
            return !string.IsNullOrWhiteSpace(twitchlink);
        }

        public string GetTwitchLink()
        {
            return HasTwitchLink() ? twitchlink : null;
        }
    }
}
