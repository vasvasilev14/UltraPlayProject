namespace UltraPlay.Data.ViewModels
{
    public class BetViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<OddViewModel> Odds { get; set; }
    }
}
