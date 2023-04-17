namespace UltraPlay.Data.ViewModels
{
    public class MatchViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public List<BetViewModel> ActiveBets { get; set; }

        public List<BetViewModel> InactiveBets { get; set; }
    }
}
