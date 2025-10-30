using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StardewViewerEvents.Events;
using System.Text;

namespace StardewViewerEvents.Credits
{
    public class CreditAccounts
    {
        private static readonly Random _random = new Random();
        private IBotCommunicator _communications;
        private readonly string _directory;

        private Dictionary<ulong, CreditAccount> _accounts;

        private DateTime _lastBackupTime;

        public CreditAccounts(string path)
        {
            _directory = path;
            _accounts = new Dictionary<ulong, CreditAccount>();
        }

        public void SetCommunicator(IBotCommunicator botCommunications)
        {
            _communications = botCommunications;
        }

        public CreditAccount this[ulong discordId]
        {
            get { return GetAccount(discordId); }
        }

        public CreditAccount GetRandomAccount()
        {
            var values = Enumerable.ToList(_accounts.Values);
            var size = _accounts.Count;
            return values[_random.Next(size)];
        }

        public void ResetAll()
        {
            CreateBackup();
            _accounts = new Dictionary<ulong, CreditAccount>();
        }

        public void CreateBackup(int minMinutesSinceLastBackup = 0)
        {
            var backupTime = DateTime.Now;

            if ((backupTime - _lastBackupTime).TotalMinutes < minMinutesSinceLastBackup)
            {
                return;
            }

            var backupShortDateFormat = $"{backupTime.Year}-{backupTime.Month}-{backupTime.Day}";
            var backupShortTimeFormat = $"{backupTime.Hour}-{backupTime.Minute}-{backupTime.Second}";
            var backupsFolder = Path.Combine(_directory, "Backups");
            var creditBackups = Path.Combine(backupsFolder, "Credits");
            if (!Directory.Exists(creditBackups))
            {
                Directory.CreateDirectory(creditBackups);
            }

            var currentBackupFileName = $"Credits - Backup {backupShortDateFormat}-{backupShortTimeFormat}.json";
            var backupPath = Path.Combine(creditBackups, currentBackupFileName);
            ExportTo(backupPath);

            _lastBackupTime = backupTime;
        }

        public void ImportFrom(string creditsFile)
        {
            if (!File.Exists(creditsFile))
            {
                ExportTo(creditsFile);
                return;
            }

            var lines = File.ReadAllText(creditsFile, Encoding.UTF8);
            dynamic jsonData = JsonConvert.DeserializeObject(lines);
            foreach (JObject creditAccountString in jsonData)
            {
                var creditAccount = new CreditAccount(creditAccountString);
                Add(creditAccount.discordId, creditAccount);
            }
        }

        public void ExportTo(string creditsFile)
        {
            var json = JsonConvert.SerializeObject(ToList(), Formatting.Indented);
            File.WriteAllText(creditsFile, json);
        }

        private void Add(ulong discordId, CreditAccount creditAccount)
        {
            _accounts.Add(discordId, creditAccount);
        }

        private List<CreditAccount> ToList()
        {
            return _accounts.Values.ToList();
        }

        private CreditAccount GetAccount(ulong discordId)
        {
            if (!_accounts.ContainsKey(discordId))
            {
                Add(discordId, new CreditAccount
                {
                    discordId = discordId,
                    discordName = _communications.GetDisplayName(discordId),
                });
            }

            return _accounts[discordId];
        }

        public CreditAccount[] GetAccountsActiveInThePastMinutes(ulong maxMinutesSinceLastActivity)
        {
            var activeAccounts = _accounts
                .Where(kvp => (DateTime.Now - kvp.Value.lastActivityTime).TotalMinutes <= maxMinutesSinceLastActivity)
                .OrderByDescending(kvp => kvp.Value.lastActivityTime);
            return activeAccounts.Select(x => x.Value).ToArray();
        }

        public CreditAccount[] GetAccountsActiveInThePastMinutes(ulong maxMinutesSinceLastActivity, ulong exceptId)
        {
            var activeAccounts = _accounts.Where(kvp => kvp.Key != exceptId)
                .Where(kvp => (DateTime.Now - kvp.Value.lastActivityTime).TotalMinutes <= maxMinutesSinceLastActivity)
                .OrderByDescending(kvp => kvp.Value.lastActivityTime);
            return activeAccounts.Select(x => x.Value).ToArray();
        }
    }
}
