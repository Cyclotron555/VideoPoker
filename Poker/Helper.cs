using System.Windows;

namespace Poker
{
    public class Helper
    {
        public bool cd1 { get; set; }
        public bool cd2 { get; set; }
        public bool cd3 { get; set; }
        public bool cd4 { get; set; }
        public bool cd5 { get; set; }
        public Card NextCard()
        {
            mainWindow m = Application.Current.Windows[0] as mainWindow;
            var NC = m.Ncard();
            return NC;
        }
    }
}
