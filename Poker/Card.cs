namespace Poker
{
    public class Card
    {
        public int Nr { get; set; }
        public string Col { get; set; }
        public string Img { get; set; }
        public string Kind { get; set; }
        //Constructor
        public Card(int _nr, string _image, string _color, string _kind)
        {
            Nr = _nr;
            Col = _color;
            Img = _image;
            Kind = _kind;
        }
    }
}
