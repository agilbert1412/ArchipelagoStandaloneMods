namespace RealLifeViewerEvents.Events.Constants
{
    public class KaraokeSong
    {
        public string Title { get; set; }
        public string Context { get; set; }
        public bool Duet { get; set; }

        public KaraokeSong(string title, string context, bool duet = false)
        {
            Title = title;
            Context = context;
            Duet = duet;
        }
    }

    public static class KaraokeSongs
    {
        public static readonly List<KaraokeSong> ALL_SONGS = new List<KaraokeSong>()
        {
            new("Golden", "K-Pop Demon Hunters"),
            new("A Million Dreams", "The Greatest Showman"),
            new("The Other Side", "The Greatest Showman"),
            new("Day of Fate", "Jonathan Young"),
            new("You Belong With Me", "Taylor Switft"),
            new("Today is gonna be a great day", "Phineas and Ferb"),
            new("Summer (Where do we begin?)", "Phineas and Ferb"),
            new("Perry The Platypus", "Phineas and Ferb"),
            new("Chop Away At My Heart", "The Lumberzacks"),
            new("Bezos", "Bo Burnham"),
            new("White Woman's Instagram", "Bo Burnham"),
            new("The Fox", "Ylvis"),
            new("Still Alive", "Portal"),
            new("Want You Gone", "Portal"),
            new("How Bad Can I Be", "The Lorax"),
            new("Just Ken", "Barbie and Ken"),
            new("The Duck Song", "The Duck Song"),
        }.Where(x => !string.IsNullOrWhiteSpace(x.Title)).OrderBy(x => x.Context).ThenBy(x => x.Title).ToList();
    }
}
