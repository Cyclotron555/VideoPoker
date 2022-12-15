using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Poker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public delegate void StringImage(Image i, string s);
    public partial class mainWindow : Window
    {
        public mainWindow()//**************Constructor
        {
            InitializeComponent();
            BetLongPressPlus = false;
            BetLongPressMinus = false;
        }

        public void CreateWinningsTable()
        {
            //Winnings Left (text)
            Shape.CreateRectangleWithText("rect00", 200, 18, 630, 25,  "red", "red", "white", 0, 5, 1, 1, 1, "FIVE OF A KIND", HorizontalAlignment.Left, VerticalAlignment.Center, 0.8);
            Shape.CreateRectangleWithText("rect01", 200, 18, 630, 61,  "red", "red", "white", 0, 1, 1, 1, 1, "ROYAL FLUSH", HorizontalAlignment.Left, VerticalAlignment.Center, 0.5);
            Shape.CreateRectangleWithText("rect02", 200, 18, 630, 97,  "red", "red", "white", 0, 1, 1, 1, 1, "STRAIGHT FLUSH", HorizontalAlignment.Left, VerticalAlignment.Center, 0.8);
            Shape.CreateRectangleWithText("rect03", 200, 18, 630, 133, "red", "red", "white", 0, 1, 1, 1, 1, "FOUR OF A KIND", HorizontalAlignment.Left, VerticalAlignment.Center, 0.5);
            Shape.CreateRectangleWithText("rect04", 200, 18, 630, 169, "red", "red", "white", 0, 1, 1, 1, 1, "FULL HOUSE", HorizontalAlignment.Left, VerticalAlignment.Center, 0.8);
            Shape.CreateRectangleWithText("rect05", 200, 18, 630, 205, "red", "red", "white", 0, 1, 1, 1, 1, "FLUSH", HorizontalAlignment.Left, VerticalAlignment.Center, 0.5);
            Shape.CreateRectangleWithText("rect06", 200, 18, 630, 241, "red", "red", "white", 0, 1, 1, 1, 1, "STRAIGHT", HorizontalAlignment.Left, VerticalAlignment.Center, 0.8);
            Shape.CreateRectangleWithText("rect07", 200, 18, 630, 277, "red", "red", "white", 0, 1, 1, 1, 1, "THREE OF A KIND", HorizontalAlignment.Left, VerticalAlignment.Center, 0.5);
            Shape.CreateRectangleWithText("rect08", 200, 18, 630, 313, "red", "red", "white", 0, 1, 1, 1, 5, "TWO PAIR", HorizontalAlignment.Left, VerticalAlignment.Center, 0.8);
            //Winnings Right (numbers)
            Shape.CreateRectangleWithText("rect0", 200, 18, 1030, 25, " red", "white", "white", 0, 1, 5, 1, 1, FiveOfAKind.ToString(), HorizontalAlignment.Right, VerticalAlignment.Center, 0.8);
            Shape.CreateRectangleWithText("rect1", 200, 18, 1030, 61, " red", "white", "white", 0, 1, 1, 1, 1, RoyalFlush.ToString(), HorizontalAlignment.Right, VerticalAlignment.Center, 0.5);
            Shape.CreateRectangleWithText("rect2", 200, 18, 1030, 97, " red", "white", "white", 0, 1, 1, 1, 1, StraightFlush.ToString(), HorizontalAlignment.Right, VerticalAlignment.Center, 0.8);
            Shape.CreateRectangleWithText("rect3", 200, 18, 1030, 133, "red", "white", "white", 0, 1, 1, 1, 1, FourOfAKind.ToString(), HorizontalAlignment.Right, VerticalAlignment.Center, 0.5);
            Shape.CreateRectangleWithText("rect4", 200, 18, 1030, 169, "red", "white", "white", 0, 1, 1, 1, 1, FullHouse.ToString(), HorizontalAlignment.Right, VerticalAlignment.Center, 0.8);
            Shape.CreateRectangleWithText("rect5", 200, 18, 1030, 205, "red", "white", "white", 0, 1, 1, 1, 1, Flush.ToString(), HorizontalAlignment.Right, VerticalAlignment.Center, 0.5);
            Shape.CreateRectangleWithText("rect6", 200, 18, 1030, 241, "red", "white", "white", 0, 1, 1, 1, 1, Straight.ToString(), HorizontalAlignment.Right, VerticalAlignment.Center, 0.8);
            Shape.CreateRectangleWithText("rect7", 200, 18, 1030, 277, "red", "white", "white", 0, 1, 1, 1, 1, ThreeOfAKind.ToString(), HorizontalAlignment.Right, VerticalAlignment.Center, 0.5);
            Shape.CreateRectangleWithText("rect8", 200, 18, 1030, 313, "red", "white", "white", 0, 1, 1, 5, 1, TwoPair.ToString(), HorizontalAlignment.Right, VerticalAlignment.Center, 0.8);

            return;
        }

        #region START VARS *****START VARS *****START VARS *****START VARS *****START VARS *****START VARS *****       

        private int bonusCounter = 0;
        public bool drawButtonSwitch { get; private set; }
        public int drawWasClicked { get; private set; } = 0;
        public static bool holdOn, holdOn2 = false, holdOn3 = false, holdOn4 = false, holdOn5 = false, continueDraw = false, handEnded = false;
        public static int c1, c2, c3, c4, c5, card, itemNumber = 0, startCounter = 0, ect;
        HoldCards holdCards = new HoldCards();//Declare the HoldCards Class
        Winner winner = new Winner();//Declare the winner Class
        Stats stats = new Stats();//Declare the Stats Class
        public double RealWidth { get; private set; } 
        public double RealHeight { get; private set; }
        public double MonitorHeight { get; private set; }
        public double MonitorWidth { get; private set; }
        public int BonusPrize { get; set; } = 25;
        public int CardDisplaySpeed { get; set; } 
        public int CardDSpeed { get; set; }
        public int TwoPair { get; private set; } = 5;
        public int ThreeOfAKind { get; private set; } = 10;
        public int Straight { get; private set; } = 15;
        public int Flush { get; private set; } = 20;
        public int FullHouse { get; private set; } = 25;
        public int FourOfAKind { get; private set; } = 100;
        public int StraightFlush { get; private set; } = 250;
        public int RoyalFlush { get; private set; } = 1000;
        public int FiveOfAKind { get; private set; } = 5000;
        public int Bet { get; set; } = 1;
        public bool BetLongPressPlus { get; private set; } = true;
        public bool BetLongPressMinus { get; private set; } = true;
        public double BetIntervalPlus { get; private set; }
        public double BetIntervalMinus { get; private set; }
        public int BetIncrement { get; private set; } = 0;
        public int TimeBomb { get; private set; }
        public int BonusSwitch { get;  set; }
        public bool WinnerForGuessing { get; set; }
        public bool Card1Wins { get; set; }
        public bool Card2Wins { get; set; }
        public bool Card3Wins { get; set; }
        public bool Card4Wins { get; set; }
        public bool Card5Wins { get; set; }
        public bool TwoPairWon { get; set; }
        public bool ThreeOfAKindWon { get; set; }
        public bool StraightWon { get; set; }
        public bool FlushWon { get; set; }
        public bool FullHouseWon { get; set; }
        public bool FourOfAKindWon { get; set; }
        public bool StraightFlushWon { get; set; }
        public bool RoyalFlushWon { get; set; }
        public bool FiveOfAKindWon { get; set; }
        public bool BonusWon { get; set; }
        public int MainScore { get; set; } = 10;
        public int Wallet { get; set; } = 0;
        public int doubleUp { get; set; } = 0;
        public int Looper { get; set; }
        public int MaxLooper { get; set; }
        public bool DoubleUpWasPressed { get; set; }
        public bool CollectWasPressed { get; set; }
        public bool RedBlackWasOpened { get; set; } = false;
        public string BackOfCards { get; set; } 
        public int BackOfCardsChoice { get; set; }
        public int RoyalFlushStatsCount { get;  set; }
        public int StraightFlushStatsCount { get;  set; }
        public int FourOfAKindStatsCount { get;  set; }
        public int FullHouseStatsCount { get;  set; }
        public int FlushStatsCount { get;  set; }
        public int StraightStatsCount { get;  set; }
        public int ThreeOfAKindStatsCount { get;  set; }
        public int TwoPairStatsCount { get;  set; }
        public int BonusStatsCount { get;  set; }
        public int FiveOfAKindStatsCount { get;  set; }
        public int JackpotCount { get; set; }
        private int textBoxColorChanger = 0;
        SolidColorBrush BlackBrush = new SolidColorBrush(Colors.Black);
        SolidColorBrush AzureBrush = new SolidColorBrush(Colors.Azure);
        public List<Card> CardsAlreadyGuessed = new List<Card>();
        DispatcherTimer PauseTimer = new DispatcherTimer();
        DispatcherTimer BetPlusTimer = new DispatcherTimer();
        DispatcherTimer BetMinusTimer = new DispatcherTimer();
        DispatcherTimer DoubleUpTimer = new DispatcherTimer();
        DispatcherTimer CashOutTimer = new DispatcherTimer();
        DispatcherTimer TextBoxTimer = new DispatcherTimer();
        DispatcherTimer BonusTimer = new DispatcherTimer();
        SoundPlayer sp;
        SoundPlayer SyncP;
        #endregion END VARS *****END VARS *****END VARS *****END VARS *****END VARS *****END VARS *****END VARS *****
        //Window Creation Event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MoveLbl.IsEnabled = true;
            MoveLbl.Visibility = Visibility.Visible;
            RealHeight = Height;
            RealWidth = Width;
            CreateWinningsTable();
            BetIntervalPlus = 200;
            BetIntervalMinus = 200;
            BonusBorder1.Visibility = Visibility.Hidden;
            BonusBorder2.Visibility = Visibility.Hidden;
            BonusBorder3.Visibility = Visibility.Hidden;
            BonusBorder4.Visibility = Visibility.Hidden;
            BonusBorder5.Visibility = Visibility.Hidden;
            BonusBorder6.Visibility = Visibility.Hidden;
            BonusBorder7.Visibility = Visibility.Hidden;
            BonusBorder8.Visibility = Visibility.Hidden;
            BonusBorder9.Visibility = Visibility.Hidden;
            MainFrame.Background = null;
            MainFrame.IsEnabled = false;
            DoubleUpButton.Visibility = Visibility.Hidden;
            ColButton.Visibility = Visibility.Hidden;
            WithrawButton.IsEnabled = true;
            CashOutButton.IsEnabled = true;
            DepositButton.IsEnabled = true;
            //Statistics Object ( has to be in this order )
            stats.ReadFromFile();
            MainScore = stats.Cash;
            LblScore.Content = MainScore.ToString();
            Wallet = Convert.ToInt32(stats.WalletCount.ToString());
            lblAccountAmount.Content = Wallet.ToString();
            BonusSwitch = Convert.ToInt32(stats.BonusStack.ToString());
            CardDSpeed = Convert.ToInt32(stats.CSpeed.ToString());
            BackOfCardsChoice = Convert.ToInt32(stats.CBackChoice.ToString());
            BonusStatsCount = Convert.ToInt32(stats.BonusStats.ToString());
            TwoPairStatsCount = Convert.ToInt32(stats.TwoPairStats.ToString());
            ThreeOfAKindStatsCount = Convert.ToInt32(stats.ThreeOfAKindStats.ToString());
            StraightStatsCount = Convert.ToInt32(stats.StraightStats.ToString());
            FlushStatsCount = Convert.ToInt32(stats.FlushStats.ToString());
            FullHouseStatsCount = Convert.ToInt32(stats.FullHouseStats.ToString());
            FourOfAKindStatsCount = Convert.ToInt32(stats.FourOfAKindStats.ToString());
            StraightFlushStatsCount = Convert.ToInt32(stats.StraightFlushStats.ToString());
            RoyalFlushStatsCount = Convert.ToInt32(stats.RoyalFlushStats.ToString());
            FiveOfAKindStatsCount = Convert.ToInt32(stats.FiveOfAKindStats.ToString());
            JackpotCount = Convert.ToInt32(stats.JackpotCounter.ToString());
            BonusStackCheck();
            //Check if the Cards Display Speed is set previously or empty and if empty set it to Medium
            if (CardDSpeed == 0)
            {
                CardDSpeed = 2;
            }
            if (CardDSpeed == 1)
            {
                CardDisplaySpeed = 350;
            }
            else if (CardDSpeed == 2)
            {
                CardDisplaySpeed = 275;
            }
            else if (CardDSpeed == 3)
            {
                CardDisplaySpeed = 200;
            }
            //Check if the BackOfCards variable isn't zero, Otherwise set it to the red design
            if (BackOfCardsChoice == 0)
            {
                BackOfCards = "backdesign_7";
            }
            if (BackOfCardsChoice == 1)
            {
                BackOfCards = "backdesign_6";
            }
            else if (BackOfCardsChoice == 2)
            {
                BackOfCards = "backdesign_7";
            }
            else if (BackOfCardsChoice == 3)
            {
                BackOfCards = "backdesign_8";
            }
            else if (BackOfCardsChoice == 4)
            {
                BackOfCards = "backdesign_9";
            }
            else if (BackOfCardsChoice == 5)
            {
                BackOfCards = "backdesign_10";
            }
            else if (BackOfCardsChoice == 6)
            {
                BackOfCards = "backdesign_11";
            }
            else if (BackOfCardsChoice == 7)
            {
                BackOfCards = "back_6";
            }
            else if (BackOfCardsChoice == 8)
            {
                BackOfCards = "backdesign_2";
            }
            else if (BackOfCardsChoice == 9)
            {
                BackOfCards = "backdesign_4";
            }
            else if (BackOfCardsChoice == 10)
            {
                BackOfCards = "backdesign_13";
            }
            else if (BackOfCardsChoice == 11)
            {
                BackOfCards = "backdesign_14";
            }
            else if (BackOfCardsChoice == 12)
            {
                BackOfCards = "backdesign_15";
            }
            //Changes the back design in the Start Event of the game
            DisplayStringImage(card_1);
            DisplayStringImage(card_2);
            DisplayStringImage(card_3);
            DisplayStringImage(card_4);
            DisplayStringImage(card_5);
            //Check if Cash Amount is greater than Zero
            if (Wallet == 0 && MainScore == 0)
            {
                Draw.IsEnabled = false;
                TextBoxDisplay("ADD MONEY TO WALLET");
                TextBoxTimer.IsEnabled = true;
                TextBoxTimer.Interval = TimeSpan.FromMilliseconds(500);
                TextBoxTimer.Tick += TextBoxTimer_Tick;
                TextBoxTimer.Start();
            }
            else  if (MainScore == 0)
            {
                Draw.IsEnabled = false;
                TextBoxDisplay("TRANSFER TO CASH");
                TextBoxTimer.IsEnabled = true;
                TextBoxTimer.Interval = TimeSpan.FromMilliseconds(500);
                TextBoxTimer.Tick += TextBoxTimer_Tick;
                TextBoxTimer.Start();
            }
            else
            {
                Draw.IsEnabled = true;
            }
        }
        //This timer tick is only for when the CASH is empty in the beggining of the game
        private void TextBoxTimer_Tick(object sender, EventArgs e)
        {
            textBoxColorChanger++;
            if (textBoxColorChanger % 2 == 1)
            {
                shufflingTextBox.Visibility = Visibility.Hidden;
            }
            else
            {
                shufflingTextBox.Visibility = Visibility.Visible;
            }

        }

        //------------------->Start Guessing Card Code
        public List<Card> GuessingCardList()
        {
            //Hearts
            List<Card> GuessingCardList = new List<Card>();
            Card joker = new Card(0, "0.png", "joker", "joker");
            GuessingCardList.Add(joker);
            Card aceHearts = new Card(1, "1.png", "red", "ace");
            GuessingCardList.Add(aceHearts);
            Card twoHearts = new Card(2, "2.png", "red", "two");
            GuessingCardList.Add(twoHearts);
            Card threeHearts = new Card(3, "3.png", "red", "three");
            GuessingCardList.Add(threeHearts);
            Card fourHearts = new Card(4, "4.png", "red", "four");
            GuessingCardList.Add(fourHearts);
            Card fiveHearts = new Card(5, "5.png", "red", "five");
            GuessingCardList.Add(fiveHearts);
            Card sixHearts = new Card(6, "6.png", "red", "six");
            GuessingCardList.Add(sixHearts);
            Card sevenHearts = new Card(7, "7.png", "red", "seven");
            GuessingCardList.Add(sevenHearts);
            Card eightHearts = new Card(8, "8.png", "red", "eight");
            GuessingCardList.Add(eightHearts);
            Card nineHearts = new Card(9, "9.png", "red", "nine");
            GuessingCardList.Add(nineHearts);
            Card tenHearts = new Card(10, "10.png", "red", "ten");
            GuessingCardList.Add(tenHearts);
            Card jackHearts = new Card(11, "11.png", "red", "jack");
            GuessingCardList.Add(jackHearts);
            Card queenHearts = new Card(12, "12.png", "red", "queen");
            GuessingCardList.Add(queenHearts);
            Card kingHearts = new Card(13, "13.png", "red", "king");
            GuessingCardList.Add(kingHearts);
            //Diamonds
            Card aceDiamonds = new Card(14, "14.png", "red", "ace");
            GuessingCardList.Add(aceDiamonds);
            Card twoDiamonds = new Card(15, "15.png", "red", "two");
            GuessingCardList.Add(twoDiamonds);
            Card threeDiamonds = new Card(16, "16.png", "red", "three");
            GuessingCardList.Add(threeDiamonds);
            Card fourDiamonds = new Card(17, "17.png", "red", "four");
            GuessingCardList.Add(fourDiamonds);
            Card fiveDiamonds = new Card(18, "18.png", "red", "five");
            GuessingCardList.Add(fiveDiamonds);
            Card sixDiamonds = new Card(19, "19.png", "red", "six");
            GuessingCardList.Add(sixDiamonds);
            Card sevenDiamonds = new Card(20, "20.png", "red", "seven");
            GuessingCardList.Add(sevenDiamonds);
            Card eightDiamonds = new Card(21, "21.png", "red", "eight");
            GuessingCardList.Add(eightDiamonds);
            Card nineDiamonds = new Card(22, "22.png", "red", "nine");
            GuessingCardList.Add(nineDiamonds);
            Card tenDiamonds = new Card(23, "23.png", "red", "ten");
            GuessingCardList.Add(tenDiamonds);
            Card jackDiamonds = new Card(24, "24.png", "red", "jack");
            GuessingCardList.Add(jackDiamonds);
            Card queenDiamonds = new Card(25, "25.png", "red", "queen");
            GuessingCardList.Add(queenDiamonds);
            Card kingDiamonds = new Card(26, "26.png", "red", "king");
            GuessingCardList.Add(kingDiamonds);
            //Spades
            Card aceSpades = new Card(27, "27.png", "black", "ace");
            GuessingCardList.Add(aceSpades);
            Card twoSpades = new Card(28, "28.png", "black", "two");
            GuessingCardList.Add(twoSpades);
            Card threeSpades = new Card(29, "29.png", "black", "three");
            GuessingCardList.Add(threeSpades);
            Card fourSpades = new Card(30, "30.png", "black", "four");
            GuessingCardList.Add(fourSpades);
            Card fiveSpades = new Card(31, "31.png", "black", "five");
            GuessingCardList.Add(fiveSpades);
            Card sixSpades = new Card(32, "32.png", "black", "six");
            GuessingCardList.Add(sixSpades);
            Card sevenSpades = new Card(33, "33.png", "black", "seven");
            GuessingCardList.Add(sevenSpades);
            Card eightSpades = new Card(34, "34.png", "black", "eight");
            GuessingCardList.Add(eightSpades);
            Card nineSpades = new Card(35, "35.png", "black", "nine");
            GuessingCardList.Add(nineSpades);
            Card tenSpades = new Card(36, "36.png", "black", "ten");
            GuessingCardList.Add(tenSpades);
            Card jackSpades = new Card(37, "37.png", "black", "jack");
            GuessingCardList.Add(jackSpades);
            Card queenSpades = new Card(38, "38.png", "black", "queen");
            GuessingCardList.Add(queenSpades);
            Card kingSpades = new Card(39, "39.png", "black", "king");
            GuessingCardList.Add(kingSpades);
            //Clubs
            Card aceClubs = new Card(40, "40.png", "black", "ace");
            GuessingCardList.Add(aceClubs);
            Card twoClubs = new Card(41, "41.png", "black", "two");
            GuessingCardList.Add(twoClubs);
            Card threeClubs = new Card(42, "42.png", "black", "three");
            GuessingCardList.Add(threeClubs);
            Card fourClubs = new Card(43, "43.png", "black", "four");
            GuessingCardList.Add(fourClubs);
            Card fiveClubs = new Card(44, "44.png", "black", "five");
            GuessingCardList.Add(fiveClubs);
            Card sixClubs = new Card(45, "45.png", "black", "six");
            GuessingCardList.Add(sixClubs);
            Card sevenClubs = new Card(46, "46.png", "black", "seven");
            GuessingCardList.Add(sevenClubs);
            Card eightClubs = new Card(47, "47.png", "black", "eight");
            GuessingCardList.Add(eightClubs);
            Card nineClubs = new Card(48, "48.png", "black", "nine");
            GuessingCardList.Add(nineClubs);
            Card tenClubs = new Card(49, "49.png", "black", "ten");
            GuessingCardList.Add(tenClubs);
            Card jackClubs = new Card(50, "50.png", "black", "jack");
            GuessingCardList.Add(jackClubs);
            Card queenClubs = new Card(51, "51.png", "black", "queen");
            GuessingCardList.Add(queenClubs);
            Card kingClubs = new Card(52, "52.png", "black", "king");
            GuessingCardList.Add(kingClubs);
            return GuessingCardList;
        }
        public Card Ncard()
        {
            Random random = new Random();
            if (CardsAlreadyGuessed.Count() <= 52)
            {
                var ncard = random.Next(GuessingCardList().Count());
                var j = GuessingCardList()[ncard];

            S: if (!CardsAlreadyGuessed.Any(c => c.Nr == j.Nr))
                {
                    CardsAlreadyGuessed.Add(j);
                    return j;
                }
                else
                {
                    ncard = random.Next(GuessingCardList().Count());
                    j = GuessingCardList()[ncard];
                    goto S;
                }
            }
            else
            {
                CardsAlreadyGuessed.Clear();
                var ncard = random.Next(GuessingCardList().Count());
                var j = GuessingCardList()[ncard];

            S: if (!CardsAlreadyGuessed.Any(c => c.Nr == j.Nr))
                {
                    CardsAlreadyGuessed.Add(j);
                    return j;
                }
                else
                {
                    ncard = random.Next(GuessingCardList().Count());
                    j = GuessingCardList()[ncard];
                    goto S;
                }
            }

        }
        //------------------->End Guessing Card Code

        //Plus Bet Button UP
        private void BetPlus_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BetPlusTimer.Tick -= dtPlusTicker;
            BetLongPressPlus = false;
            BetIntervalPlus = 200;
        }
        //Minus Bet Button UP
        private void BetMinus_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BetMinusTimer.Tick -= dtMinusTicker;
            BetLongPressMinus = false;
            BetIntervalMinus = 200;
        }
        //LongPress Increase Bet Button Down Plus
        private void BetPlus_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BetLongPressPlus = true;
            if (BetLongPressPlus == true)
            {
                BetPlusTimer.Tick += dtPlusTicker;
                BetPlusTimer.Start();
            }
        }
        //Ticker for Plus
        private void dtPlusTicker(object sender, EventArgs e)
        {
            if (BetLongPressPlus == true)
            {
                if (Bet >= 100)
                {
                    Bet = 100;
                    PlaySound(Properties.Resources.bet_min_max);
                    TwoPair = 500;
                    ThreeOfAKind = 1000;
                    Straight = 1500;
                    Flush = 2000;
                    FullHouse = 2500;
                    FourOfAKind = 10000;
                    StraightFlush = 25000;
                    RoyalFlush = 100000;
                    FiveOfAKind = 500000;
                    BetPlusTimer.Stop();
                    return;
                }
                if (Bet >= 1 && Bet <= 100)
                {
                    Bet += 1;
                    BetLabel.Content = Bet.ToString();
                    PlaySound(Properties.Resources.bet_plus);
                    TwoPair += 5;
                    ThreeOfAKind += 10;
                    Straight += 15;
                    Flush += 20;
                    FullHouse += 25;
                    FourOfAKind += 100;
                    StraightFlush += 250;
                    RoyalFlush += 1000;
                    FiveOfAKind += 5000;
                    DestroyWinningTable();
                    CreateWinningsTable();
                    if (BetIntervalPlus > 75)
                    {
                        BetPlusTimer.Interval = TimeSpan.FromMilliseconds(BetIntervalPlus -= 10);
                    }
                    else
                    {
                        BetPlusTimer.Interval = TimeSpan.FromMilliseconds(75);
                    }
                }
            }
            else
            {
                BetLongPressPlus = false;
                BetPlusTimer.Stop();
                BetPlusTimer.Tick -= dtPlusTicker;
                return;
            }
        }

        //LongPress Decrease Bet Button Down
        private void BetMinus_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BetLongPressMinus = true;
            if (BetLongPressMinus == true)
            {
                BetMinusTimer.Tick += dtMinusTicker;
                BetMinusTimer.Start();
            }
        }
        private void dtMinusTicker(object sender, EventArgs e)
        {
            if (BetLongPressMinus == true)
            {
                if (Bet <= 1)
                {
                    Bet = 1;
                    PlaySound(Properties.Resources.bet_min_max);
                    TwoPair = 5;
                    ThreeOfAKind = 10;
                    Straight = 15;
                    Flush = 20;
                    FullHouse = 25;
                    FourOfAKind = 100;
                    StraightFlush = 250;
                    RoyalFlush = 1000;
                    FiveOfAKind = 5000;
                    BetMinusTimer.Stop();
                    return;
                }
                if (Bet >= 1 && Bet <= 100)
                {
                    Bet -= 1;
                    BetLabel.Content = Bet.ToString();
                    PlaySound(Properties.Resources.bet_minus);
                    TwoPair -= 5;
                    ThreeOfAKind -= 10;
                    Straight -= 15;
                    Flush -= 20;
                    FullHouse -= 25;
                    FourOfAKind -= 100;
                    StraightFlush -= 250;
                    RoyalFlush -= 1000;
                    FiveOfAKind -= 5000;
                    DestroyWinningTable();
                    CreateWinningsTable();
                    if (BetIntervalMinus > 75)
                    {
                        BetMinusTimer.Interval = TimeSpan.FromMilliseconds(BetIntervalMinus -= 10);
                    }
                    else
                    {
                        BetMinusTimer.Interval = TimeSpan.FromMilliseconds(75);
                    }
                }
            }
            else
            {
                BetLongPressMinus = false;
                BetMinusTimer.Tick -= dtMinusTicker;
                BetMinusTimer.Stop();
                return;
            }
        }
        //Method for Double Up
        private void FlashDoubleUp()
        {
            DoubleUpTimer.IsEnabled = true;
            DoubleUpTimer.Interval = TimeSpan.FromMilliseconds(420);
            DoubleUpTimer.Tick += DoubleUpTimer_Tick;
            DoubleUpTimer.Start();
        }
        //Stop Falshing DoubleUp
        private void StopFlashDoubleUp()
        {
            DoubleUpTimer.Stop();
            DoubleUpTimer.IsEnabled = false;
            DoubleUpTimer.Tick -= DoubleUpTimer_Tick;
        }
        private void DoubleUpTimer_Tick(object sender, EventArgs e)
        {
            doubleUp++;
            if (doubleUp % 2 == 0)
            {
                DoubleUpButton.Foreground = BlackBrush;
                ColButton.Foreground = AzureBrush;
            }
            else
            {
                DoubleUpButton.Foreground = AzureBrush;
                ColButton.Foreground = BlackBrush;
            }
        }

        //Destroys the Winning Table
        private void DestroyWinningTable()
        {
            Shape.Destroy("rect00");
            Shape.Destroy("rect01");
            Shape.Destroy("rect02");
            Shape.Destroy("rect03");
            Shape.Destroy("rect04");
            Shape.Destroy("rect05");
            Shape.Destroy("rect06");
            Shape.Destroy("rect07");
            Shape.Destroy("rect08");
            Shape.Destroy("rect0");
            Shape.Destroy("rect1");
            Shape.Destroy("rect2");
            Shape.Destroy("rect3");
            Shape.Destroy("rect4");
            Shape.Destroy("rect5");
            Shape.Destroy("rect6");
            Shape.Destroy("rect7");
            Shape.Destroy("rect8");
        }
        //Start Pause Timer for Songs etc
        private void StartTime()
        {
            PauseTimer.IsEnabled = true;
            PauseTimer.Interval = TimeSpan.FromMilliseconds(1);
            PauseTimer.Tick += PauseTimer_Tick;
            PauseTimer.Start();
        }
        //Pause Timer For Songs Tick
        private void PauseTimer_Tick(object sender, EventArgs e)
        {
            Looper++;
            //Five of a Kind
            if (FiveOfAKindWon == true)
            {
                if (Looper == 1)
                {
                    PlaySound(Properties.Resources.win_five_of_a_kind); //play sound for 5 of a kind
                }
                if (Looper == 380)
                {
                    FlashDoubleUp();
                    PlaySound(Properties.Resources.five_of_a_kind_melody);
                }
                if (Looper == 850)
                {
                    DestroyWinningTable();
                    StopFlashDoubleUp();
                    StopTime();
                    MainFrame.IsEnabled = true;
                    MainFrame.Content = new RedOrBlack();
                    return;
                }
            }
            //Royal Flush
            if (RoyalFlushWon == true)
            {
                if (Looper == 1)
                {
                    PlaySound(Properties.Resources.royal_flush_win); //play sound for royal flush
                }
                if (Looper == 200)
                {
                    FlashDoubleUp();
                    PlaySound(Properties.Resources.royal_flush_sound);
                }
                if (Looper == 1110)
                {
                    DestroyWinningTable();
                    StopFlashDoubleUp();
                    StopTime();
                    MainFrame.IsEnabled = true;
                    MainFrame.Content = new RedOrBlack();
                    return;
                }
            }
            //Straight Flush
            else if (StraightFlushWon == true)
            {
                if (Looper == 1)
                {
                    PlaySound(Properties.Resources.royal_flush_win); //play sound for straight flush
                }
                if (Looper == 200)
                {
                    FlashDoubleUp();
                    PlaySound(Properties.Resources.royal_flush_sound);
                }
                if (Looper == 1110)
                {
                    DestroyWinningTable();
                    StopFlashDoubleUp();
                    StopTime();
                    MainFrame.IsEnabled = true;
                    MainFrame.Content = new RedOrBlack();
                    return;
                }
            }
            //4 of a kind
            else if (FourOfAKindWon == true)
            {
                if (Looper == 1)
                {
                    PlaySound(Properties.Resources._4_of_a_kind_win); //play sound for 4 of a kind
                }
                if (Looper == 240)
                {
                    FlashDoubleUp();
                    PlaySound(Properties.Resources.full_house_melody);
                }
                if (Looper == 1200)
                {
                    DestroyWinningTable();
                    StopFlashDoubleUp();
                    StopTime();
                    MainFrame.IsEnabled = true;
                    MainFrame.Content = new RedOrBlack();
                    return;
                }
            }
            //Full house
            else if (FullHouseWon == true)
            {
                if (Looper == 1)
                {
                    PlaySound(Properties.Resources.win_full_house); ;//play sound for full house
                }
                if (Looper == 115)
                {
                    FlashDoubleUp();
                    PlaySound(Properties.Resources.flush2_melody);
                }
                if (Looper == 1080)
                {
                    DestroyWinningTable();
                    StopFlashDoubleUp();
                    StopTime();
                    MainFrame.IsEnabled = true;
                    MainFrame.Content = new RedOrBlack();
                    return;
                }
            }
            //Flush
            else if (FlushWon == true)
            {
                if (Looper == 1)
                {
                    PlaySound(Properties.Resources.win_flush);//play sound for flush
                }
                if (Looper == 115)
                {
                    FlashDoubleUp();
                    PlaySound(Properties.Resources.straight_melody);
                }
                if (Looper == 725)
                {
                    DestroyWinningTable();
                    StopFlashDoubleUp();
                    StopTime();
                    MainFrame.IsEnabled = true;
                    MainFrame.Content = new RedOrBlack();
                    return;
                }
            }
            //Straight
            else if (StraightWon == true)
            {
                if (Looper == 1)
                {
                    PlaySound(Properties.Resources.win_straight_sound);//play sound for winning straight
                }
                if (Looper == 85)
                {
                    FlashDoubleUp();
                    PlaySound(Properties.Resources._3_of_a_kind_melody);
                }
                if (Looper == 550)
                {
                    DestroyWinningTable();
                    StopFlashDoubleUp();
                    StopTime();
                    MainFrame.IsEnabled = true;
                    MainFrame.Content = new RedOrBlack();
                    return;
                }
            }
            //3 of a kind
            else if (ThreeOfAKindWon == true)
            {
                if (Looper == 1)
                {
                    PlaySound(Properties.Resources.win_3_of_a_kind);//play sound for winning 3 of a kind
                }
                if (Looper == 60)
                {
                    FlashDoubleUp();
                    PlaySound(Properties.Resources.threeOfAKindMelody);
                }
                if (Looper == 300)
                {
                    DestroyWinningTable();
                    StopFlashDoubleUp();
                    StopTime();
                    MainFrame.IsEnabled = true;
                    MainFrame.Content = new RedOrBlack();
                    return;
                }
            }
            //2 pair
            else if (TwoPairWon == true)
            {
                if (Looper == 1)
                {
                    PlaySound(Properties.Resources.win_2_pair);//play sound for winning 2 pair
                }
                if (Looper == 50)
                {
                    FlashDoubleUp();
                    PlaySound(Properties.Resources.transition);
                }
                if (Looper == 340)
                {
                    DestroyWinningTable();
                    StopFlashDoubleUp();
                    StopTime();
                    MainFrame.IsEnabled = true;
                    MainFrame.Content = new RedOrBlack();
                    return;
                }
            }
        }
        //Stop Pause Timer for Songs etc
        private void StopTime()
        {
            PauseTimer.Stop();
            PauseTimer.Tick -= PauseTimer_Tick;
            PauseTimer.IsEnabled = false;
            Looper = 0;
            doubleUp = 0;
            sp.Stop();
            sp.Dispose();
        }
        //Sets the source of an image 
        private void ChangeImage(Image whichImage, bool visible, string whichBitmap)
        {
            if (visible == true)
            {
                whichImage.Source = new BitmapImage(new Uri(@"Resources\" + whichBitmap + ".png", UriKind.Relative));
            }
            if (visible == false)
            {
                whichImage.Source = null;
            }
        }
        //Method displays the WIN signs
        private void WinningSigns(Image sign, int v)
        {
            if (v == 1)
            {
                sign.Source = null;
                sign.Source = new BitmapImage(new Uri(@"Resources\win.png", UriKind.Relative));
            }
            if (v == 0)
            {
                sign.Source = null;
            }
        }
        //Event handler for the Double Up Button
        private void DoubleUpButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StopTime();
            DoubleUpWasPressed = true;
            DestroyWinningTable();
            doubleUp = 0;
            StopFlashDoubleUp();
            MainFrame.IsEnabled = true;
            MainFrame.Content = new RedOrBlack();
        }
        //Collect Button Event Handler
        private void ColButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PauseTimer.IsEnabled)
            {
                StopTime();
            }
            CollectWasPressed = true;
            if (FiveOfAKindWon == true)
            {
                MainScore += FiveOfAKind;
            }
            else if (RoyalFlushWon == true)
            {
                MainScore += RoyalFlush;
            }
            else if (StraightFlushWon == true)
            {
                MainScore += StraightFlush;
            }
            else if (FourOfAKindWon == true)
            {
                MainScore += FourOfAKind;
            }
            else if (FullHouseWon == true)
            {
                MainScore += FullHouse;
            }
            else if (FlushWon == true)
            {
                MainScore += Flush;
            }
            else if (StraightWon == true)
            {
                MainScore += Straight;
            }
            else if (ThreeOfAKindWon == true)
            {
                MainScore += ThreeOfAKind;
            }
            else if (TwoPairWon == true)
            {
                MainScore += TwoPair;
            }
            StopFlashDoubleUp();
            LblScore.Content = MainScore.ToString();
            DoubleUpButton.Visibility = Visibility.Hidden;
            ColButton.Visibility = Visibility.Hidden;
            DoubleUpButton.IsEnabled = false;
            ColButton.IsEnabled = false;
            Draw.IsEnabled = true;
            sp.Dispose();
            Draw_Click(sender, e);
            PlaySoundSync(Properties.Resources.collect_award_sound2);
        }

        //Method that displays the text hand over , you won etc
        private void TextBoxDisplay(string insertText)
        {
            shufflingTextBox.Text = insertText + "\r\n";
        }

        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            shufflingTextBox.Visibility = Visibility.Visible;
            if (TextBoxTimer.IsEnabled)
            {
                TextBoxTimer.Stop();
                TextBoxTimer.Tick -= TextBoxTimer_Tick;
                TextBoxTimer.IsEnabled = false;
                shufflingTextBox.Foreground = AzureBrush;
                textBoxColorChanger = 0;
                TextBoxDisplay("");
            }
            PlaySound(Properties.Resources.insert_money);
            Wallet += 5;
            lblAccountAmount.Content = Wallet.ToString();
            var total = (MainScore + Wallet).ToString();
            if (Wallet >= Bet)
            {
                TextBoxDisplay("$" + total + " TOTAL FUNDS");
            }
        }
        //Transfer to Cash Button
        private void WithrawButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            shufflingTextBox.Visibility = Visibility.Visible;
            if (TextBoxTimer.IsEnabled)
            {
                TextBoxTimer.Stop();
                TextBoxTimer.Tick -= TextBoxTimer_Tick;
                TextBoxTimer.IsEnabled = false;
                shufflingTextBox.Foreground = AzureBrush;
                textBoxColorChanger = 0;
                TextBoxDisplay("");
            }
            if (Wallet >= 5)
            {
                PlaySound(Properties.Resources.to_cash);
                MainScore += 5;
                LblScore.Content = MainScore.ToString();
                Wallet -= 5;
                lblAccountAmount.Content = Wallet.ToString();
                Draw.IsEnabled = true;
            }
            else if (Wallet < 5 && Wallet > 0)
            {
                PlaySound(Properties.Resources.to_cash);
                MainScore += 1;
                LblScore.Content = MainScore.ToString();
                Wallet -= 1;
                lblAccountAmount.Content = Wallet.ToString();
                Draw.IsEnabled = true;
            }
            else
            {
                PlaySound(Properties.Resources.empty_wallet_sound);
            }
        }
        //Cash Out Button Event Handler ( Has Timer )
        private void CashOutButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CashOutTimer.IsEnabled = true;
            CashOutTimer.Interval = TimeSpan.FromMilliseconds(1);
            CashOutTimer.Tick += CashOutTimer_Tick;
            CashOutTimer.Start();
            sp = new SoundPlayer(Properties.Resources.cash_to_wallet);
            sp.PlayLooping();
        }

        private void CashOutTimer_Tick(object sender, EventArgs e)
        {
            if (MainScore > 0)
            {
                MainScore -= 1;
                Wallet += 1;
                lblAccountAmount.Content = Wallet.ToString();
                LblScore.Content = MainScore.ToString();
                Draw.IsEnabled = false;
            }
            else if (MainScore == 0)
            {
                sp.Stop();
                PlaySound(Properties.Resources.empty_wallet_sound);
                ///Draw.IsEnabled = true;
                CashOutTimer.Stop();
                CashOutTimer.Tick -= PauseTimer_Tick;
                CashOutTimer.IsEnabled = false;
            }
        }
        #region Window Controls Menu
        //Window Closed Event 
        private void Window_Closed(object sender, EventArgs e)
        {
            //Settings Page: All Money Saved Automatically + Display Speed + Choice of Design + All Stats Count + Jackpot
            stats.WriteToFile( MainScore, Wallet, BonusSwitch, CardDSpeed, BackOfCardsChoice, BonusStatsCount, TwoPairStatsCount, ThreeOfAKindStatsCount, 
            StraightStatsCount, FlushStatsCount, FullHouseStatsCount, FourOfAKindStatsCount, StraightFlushStatsCount, RoyalFlushStatsCount, FiveOfAKindStatsCount, JackpotCount );
        }
        //Close Window label Event handler Mouse Enter
        private void CloseLbl_MouseEnter(object sender, MouseEventArgs e)
        {
            closePopUp.IsOpen = true;
            CloseLbl.Effect = new DropShadowEffect()
            {
                BlurRadius = 15,
                ShadowDepth = 0,
                Color = Colors.Crimson
            };
        }
        //Close Window label Event handler Mouse Leave
        private void CloseLbl_MouseLeave(object sender, MouseEventArgs e)
        {
            closePopUp.IsOpen = false;
            CloseLbl.Effect = new DropShadowEffect()
            {
                BlurRadius = 0,
                ShadowDepth = 0,
                Color = Colors.Transparent
            };
        }
        //Resize Window label Event handler Mouse Enter
        private void ResizeLbl_MouseEnter(object sender, MouseEventArgs e)
        {
            resizePopUp.IsOpen = true;
            ResizeLbl.Effect = new DropShadowEffect()
            {
                BlurRadius = 15,
                ShadowDepth = 0,
                Color = Colors.Crimson
            };
        }
        //Resize Window label Event handler Mouse Leave
        private void ResizeLbl_MouseLeave(object sender, MouseEventArgs e)
        {
            resizePopUp.IsOpen = false;
            ResizeLbl.Effect = new DropShadowEffect()
            {
                BlurRadius = 0,
                ShadowDepth = 0,
                Color = Colors.Transparent
            };
        }
        //Minimize Window label Event handler Mouse Enter
        private void MinMaxLbl_MouseEnter(object sender, MouseEventArgs e)
        {
            minMaxPopUp.IsOpen = true;
            MinMaxLbl.Effect = new DropShadowEffect()
            {
                BlurRadius = 15,
                ShadowDepth = 0,
                Color = Colors.Crimson
            };
        }
        //Minimize Window label Event handler Mouse Leave
        private void MinMaxLbl_MouseLeave(object sender, MouseEventArgs e)
        {
            minMaxPopUp.IsOpen = false;
            MinMaxLbl.Effect = new DropShadowEffect()
            {
                BlurRadius = 0,
                ShadowDepth = 0,
                Color = Colors.Transparent
            };
        }
        //Move Label  Event handler Mouse Enter
        private void MoveLbl_MouseEnter(object sender, MouseEventArgs e)
        {
            moveInfo.IsOpen = true;
            MoveLbl.Effect = new DropShadowEffect()
            {
                BlurRadius = 15,
                ShadowDepth = 0,
                Color = Colors.Crimson
            };
        }
        //Move Label  Event handler Mouse Leave
        private void MoveLbl_MouseLeave(object sender, MouseEventArgs e)
        {
            moveInfo.IsOpen = false;
            MoveLbl.Effect = new DropShadowEffect()
            {
                BlurRadius = 0,
                ShadowDepth = 0,
                Color = Colors.Transparent
            };
        }
       //Move Window
        private void MoveLbl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            moveInfo.IsOpen = false;
            DragMove();
        }
        //Close Window
        private void CloseLbl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            closePopUp.IsOpen = false;
            this.Close();
        }
        //Resize Window
        private void ResizeLbl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            resizePopUp.IsOpen = false;
            if (WindowState != WindowState.Maximized)
            {
                WindowState = WindowState.Maximized;
                MoveLbl.IsEnabled = false;
                MoveLbl.Visibility = Visibility.Hidden;
            }
            else if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MoveLbl.IsEnabled = true;
                MoveLbl.Visibility = Visibility.Visible;
            }
           
        }
        //Minimize Maximize Window
        private void MinMaxLbl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            minMaxPopUp.IsOpen = false;
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Minimized;
                MoveLbl.IsEnabled = false;
                MoveLbl.Visibility = Visibility.Hidden;
            }
            else if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Minimized;
                MoveLbl.IsEnabled = false;
                MoveLbl.Visibility = Visibility.Hidden;
            }
            else if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Maximized;
                MoveLbl.IsEnabled = true;
                MoveLbl.Visibility = Visibility.Visible;
            }
        }
        //Settings Event Handler Opens Settings Page
        private void SettingsLbl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            settingsPopUp.IsOpen = false;
            DestroyWinningTable();
            MainFrame.IsEnabled = true;
            MainFrame.Content = new RedBlackPokerSettings();
        }  
        //SettingsLabel Mouse Enter Event
        private void SettingsLbl_MouseEnter(object sender, MouseEventArgs e)
        {
            settingsPopUp.IsOpen = true;
            SettingsLbl.Effect = new DropShadowEffect()
            {
                BlurRadius = 15,
                ShadowDepth = 0,
                Color = Colors.Crimson
            };
        }
        //SettingsLabel Mouse Leave Event
        private void SettingsLbl_MouseLeave(object sender, MouseEventArgs e)
        {
            settingsPopUp.IsOpen = false;
            SettingsLbl.Effect = new DropShadowEffect()
            {
                BlurRadius = 0,
                ShadowDepth = 0,
                Color = Colors.Transparent
            };
        }
        #endregion Window Controls Menu

        //Play any sound method
        private void PlaySound(Stream stream)
        {
            sp = new SoundPlayer(stream);
            sp.Play();
        }
        //Plays Sync Sound
        private void PlaySoundSync(Stream stream)
        {
            SyncP = new SoundPlayer(stream);
            SyncP.PlaySync();
            SyncP.Stop();
            SyncP.Dispose();
        }
        //Method to display images ( but image inserted is dynamic so it's an integer - 2nd var)
        private void DisplayImage(Image imageSource, int imageDisplayed)
        {
            imageSource.Source = new BitmapImage(new Uri(@"Resources\" + imageDisplayed + ".png", UriKind.Relative));
        }
        //Method to display images ( image inserted is not dynamic so it's a string( link name in resources - 2nd var)
        private void DisplayStringImage( Image imageS )
        {
            imageS.Source = new BitmapImage(new Uri(@"Resources\" + BackOfCards + ".png", UriKind.Relative));
        }
        //Method to display HOLD GIGNS images ( image inserted is not dynamic so it's a string( link name in resources - 2nd var)
        private void DisplayHoldSigns(Image imageS)
        {
            imageS.Source = new BitmapImage(new Uri(@"Resources\hold.png", UriKind.Relative));
        }
        //Function that erases images ( returns null image )
        private void ImageDestroy(Image nullImage)
        {
            nullImage.Source = null;
        }
        //The Bonus Method
        private void Bonus()
        {
            Draw.IsEnabled = false;
            BonusWon = true;
            TextBoxDisplay("");
            BonusTimer.IsEnabled = true;
            BonusTimer.Interval = TimeSpan.FromMilliseconds(1);
            BonusTimer.Tick += BonusTimer_Tick;
            BonusTimer.Start();
            BonusBorder1.Visibility = Visibility.Hidden;
            BonusBorder2.Visibility = Visibility.Hidden;
            BonusBorder3.Visibility = Visibility.Hidden;
            BonusBorder4.Visibility = Visibility.Hidden;
            BonusBorder5.Visibility = Visibility.Hidden;
            BonusBorder6.Visibility = Visibility.Hidden;
            BonusBorder7.Visibility = Visibility.Hidden;
            BonusBorder8.Visibility = Visibility.Hidden;
            BonusBorder9.Visibility = Visibility.Hidden;
            
        }

        private void BonusTimer_Tick(object sender, EventArgs e)
        {
            bonusCounter++;
            if (bonusCounter == 40)
            {
                PlaySound(Properties.Resources.bonus_sound);
            }
            else if (bonusCounter == 50)
            {
                TextBoxDisplay("");
            }
            else if (bonusCounter == 60)
            {
                TextBoxDisplay("BONUS");
                BonusBorder1.Visibility = Visibility.Visible;
            }
            else if (bonusCounter == 70)
            {
                TextBoxDisplay("");
            }
            else if (bonusCounter == 80)
            {
                TextBoxDisplay("BONUS");
                BonusBorder2.Visibility = Visibility.Visible;
            }
            else if (bonusCounter == 90)
            {
                TextBoxDisplay("");
            }
            else if (bonusCounter == 100)
            {
                TextBoxDisplay("BONUS");
                BonusBorder3.Visibility = Visibility.Visible;
            }
            else if (bonusCounter == 110)
            {
                TextBoxDisplay("");
            }
            else if (bonusCounter == 120)
            {
                TextBoxDisplay("BONUS");
                BonusBorder4.Visibility = Visibility.Visible;
            }
            else if (bonusCounter == 130)
            {
                TextBoxDisplay("");
            }
            else if (bonusCounter == 140)
            {
                TextBoxDisplay("BONUS");
                BonusBorder5.Visibility = Visibility.Visible;
            }
            else if (bonusCounter == 150)
            {
                TextBoxDisplay("");
            }
            else if (bonusCounter == 160)
            {
                TextBoxDisplay("BONUS");
                BonusBorder6.Visibility = Visibility.Visible;
            }
            else if (bonusCounter == 170)
            {
                TextBoxDisplay("");
            }
            else if (bonusCounter == 180)
            {
                TextBoxDisplay("BONUS");
                BonusBorder7.Visibility = Visibility.Visible;
            }
            else if (bonusCounter == 190)
            {
                TextBoxDisplay("");
            }
            else if (bonusCounter == 200)
            {
                TextBoxDisplay("BONUS");
                BonusBorder8.Visibility = Visibility.Visible;
            }
            else if (bonusCounter == 210)
            {
                TextBoxDisplay("");
            }
            else if (bonusCounter == 220)
            {
                TextBoxDisplay("BONUS");
                BonusBorder9.Visibility = Visibility.Visible;
            }
            else if (bonusCounter == 225)
            {
                TextBoxDisplay("");
            }
            else if (bonusCounter == 230)
            {
                TextBoxDisplay("BONUS");
            }
            else if (bonusCounter == 235)
            {
                TextBoxDisplay("");
            }
            else if (bonusCounter == 240)
            {
                TextBoxDisplay("BONUS");
                MainFrame.IsEnabled = true;
                MainFrame.Content = new RedOrBlack();
                DestroyWinningTable();
                bonusCounter = 0;
                BonusTimer.Stop();
                BonusTimer.Tick -= BonusTimer_Tick;
                BonusTimer.IsEnabled = false;
                return;
            }
           
        }
        //Reset Bonus Stack
        private void ResetBonusStack()
        {
            if (BonusSwitch == 0)
            {
                BonusBorder1.Visibility = Visibility.Hidden;
                BonusBorder2.Visibility = Visibility.Hidden;
                BonusBorder3.Visibility = Visibility.Hidden;
                BonusBorder4.Visibility = Visibility.Hidden;
                BonusBorder5.Visibility = Visibility.Hidden;
                BonusBorder6.Visibility = Visibility.Hidden;
                BonusBorder7.Visibility = Visibility.Hidden;
                BonusBorder8.Visibility = Visibility.Hidden;
                BonusBorder9.Visibility = Visibility.Hidden;
            }
        }
        //Checks the status of the bonus stack
        private void BonusStackCheck()
        {
            stats.BonusStack = BonusSwitch;
            if (BonusSwitch == 1)
            {
                BonusBorder1.Visibility = Visibility.Visible;
            }
            else if (BonusSwitch == 2)
            {
                BonusBorder1.Visibility = Visibility.Visible;
                BonusBorder2.Visibility = Visibility.Visible;
            }
            else if (BonusSwitch == 3)
            {
                BonusBorder1.Visibility = Visibility.Visible;
                BonusBorder2.Visibility = Visibility.Visible;
                BonusBorder3.Visibility = Visibility.Visible;
            }
            else if (BonusSwitch == 4)
            {
                BonusBorder1.Visibility = Visibility.Visible;
                BonusBorder2.Visibility = Visibility.Visible;
                BonusBorder3.Visibility = Visibility.Visible;
                BonusBorder4.Visibility = Visibility.Visible;
            }
            else if (BonusSwitch == 5)
            {
                BonusBorder1.Visibility = Visibility.Visible;
                BonusBorder2.Visibility = Visibility.Visible;
                BonusBorder3.Visibility = Visibility.Visible;
                BonusBorder4.Visibility = Visibility.Visible;
                BonusBorder5.Visibility = Visibility.Visible;
            }
            else if (BonusSwitch == 6)
            {
                BonusBorder1.Visibility = Visibility.Visible;
                BonusBorder2.Visibility = Visibility.Visible;
                BonusBorder3.Visibility = Visibility.Visible;
                BonusBorder4.Visibility = Visibility.Visible;
                BonusBorder5.Visibility = Visibility.Visible;
                BonusBorder6.Visibility = Visibility.Visible;
            }
            else if (BonusSwitch == 7)
            {
                BonusBorder1.Visibility = Visibility.Visible;
                BonusBorder2.Visibility = Visibility.Visible;
                BonusBorder3.Visibility = Visibility.Visible;
                BonusBorder4.Visibility = Visibility.Visible;
                BonusBorder5.Visibility = Visibility.Visible;
                BonusBorder6.Visibility = Visibility.Visible;
                BonusBorder7.Visibility = Visibility.Visible;
            }
            else if (BonusSwitch == 8)
            {
                BonusBorder1.Visibility = Visibility.Visible;
                BonusBorder2.Visibility = Visibility.Visible;
                BonusBorder3.Visibility = Visibility.Visible;
                BonusBorder4.Visibility = Visibility.Visible;
                BonusBorder5.Visibility = Visibility.Visible;
                BonusBorder6.Visibility = Visibility.Visible;
                BonusBorder7.Visibility = Visibility.Visible;
                BonusBorder8.Visibility = Visibility.Visible;
            }
            else if (BonusSwitch == 9)
            {
                BonusStatsCount++;
                BonusBorder1.Visibility = Visibility.Visible;
                BonusBorder2.Visibility = Visibility.Visible;
                BonusBorder3.Visibility = Visibility.Visible;
                BonusBorder4.Visibility = Visibility.Visible;
                BonusBorder5.Visibility = Visibility.Visible;
                BonusBorder6.Visibility = Visibility.Visible;
                BonusBorder7.Visibility = Visibility.Visible;
                BonusBorder8.Visibility = Visibility.Visible;
                BonusBorder9.Visibility = Visibility.Visible;
                Bonus();
            }
        }
        //Shuffle Method starts here
        #region START SHUFFLE METHOD
        private async void Shuffle()
        {
        restart:

            DisplayStringImage(card_1);
            DisplayStringImage(card_2);
            DisplayStringImage(card_3);
            DisplayStringImage(card_4);
            DisplayStringImage(card_5);
            Random randomCard = new Random();
            List<int> list1 = Enumerable.Range(0, 52).ToList();
            List<int> newRandomList = list1.OrderBy(i => randomCard.Next()).ToList();


            for (int i = 0; i < newRandomList.Count();)
            {
                if (newRandomList.Count() > 1 && continueDraw == true)
                {
                    // Disable the Draw button while cards are being displayed
                    Draw.IsEnabled = false;
                    BetPlus.IsEnabled = false;
                    BetMinus.IsEnabled = false;
                    DoubleUpButton.IsEnabled = false;
                    ColButton.IsEnabled = false;
                    DoubleUpButton.Visibility = Visibility.Hidden;
                    ColButton.Visibility = Visibility.Hidden;
                    WithrawButton.IsEnabled = false;
                    CashOutButton.IsEnabled = false;
                    DepositButton.IsEnabled = false;
                    // Disable click on cards while cards are being displayed
                    card_1.IsEnabled = false;
                    card_2.IsEnabled = false;
                    card_3.IsEnabled = false;
                    card_4.IsEnabled = false;
                    card_5.IsEnabled = false;

                    // If the cards were held on the first hand reset the cards with the back image and erase the HOLD signs
                    if ((holdOn == true) && (drawWasClicked % 2 == 1))
                    {
                        ImageDestroy(hold_sign);
                        DisplayStringImage(card_1);
                    }
                    if ((holdOn2 == true) && (drawWasClicked % 2 == 1))
                    {
                        DisplayStringImage(card_2);
                        ImageDestroy(hold_sign2);
                    }
                    if ((holdOn3 == true) && (drawWasClicked % 2 == 1))
                    {
                        DisplayStringImage(card_3);
                        ImageDestroy(hold_sign3);
                    }
                    if ((holdOn4 == true) && (drawWasClicked % 2 == 1))
                    {
                        DisplayStringImage(card_4);
                        ImageDestroy(hold_sign4);
                    }
                    if ((holdOn5 == true) && (drawWasClicked % 2 == 1))
                    {
                        DisplayStringImage(card_5);
                        ImageDestroy(hold_sign5);
                    }
                    // Check if the card1 is held
                    if (holdOn == false)
                    {
                        itemNumber += 1;
                        await Task.Delay(CardDisplaySpeed);
                        PlaySound(Properties.Resources.flip);
                        card = newRandomList[i];
                        c1 = card;
                        DisplayImage(card_1, card);
                        newRandomList.Remove(card);
                    }
                    else if (holdOn == true && drawWasClicked % 2 != 1)
                    {
                        card = c1;
                        DisplayImage(card_1, card);
                    }
                    // If the hand is over reset the first card and hold sign
                    else if (holdOn == true && drawWasClicked % 2 == 1)
                    {
                        holdOn = false;
                        itemNumber += 1;
                        await Task.Delay(CardDisplaySpeed);
                        ImageDestroy(card_1);
                        PlaySound(Properties.Resources.flip);
                        card = newRandomList[i]; c1 = card;
                        DisplayImage(card_1, card);
                        newRandomList.Remove(card);
                    }
                    // -End 1st card  
                    // Check if the card2 is held
                    if (holdOn2 == false)
                    {
                        itemNumber += 1;
                        await Task.Delay(CardDisplaySpeed);
                        PlaySound(Properties.Resources.flip);
                        card = newRandomList[i]; c2 = card;
                        DisplayImage(card_2, card);
                        newRandomList.Remove(card);
                    }
                    else if (holdOn2 == true && drawWasClicked % 2 != 1)
                    {
                        card = c2;
                        DisplayImage(card_2, card);
                    }
                    // If the hand is over reset the 2nd card and hold sign
                    else if (holdOn2 == true && drawWasClicked % 2 == 1)
                    {
                        holdOn2 = false;
                        itemNumber += 1;
                        await Task.Delay(CardDisplaySpeed);
                        ImageDestroy(card_2);
                        PlaySound(Properties.Resources.flip);
                        card = newRandomList[i]; c2 = card;
                        DisplayImage(card_2, card);
                        newRandomList.Remove(card);
                    }
                    // -End 2nd card
                    // Check if the card3 is held
                    if (holdOn3 == false)
                    {
                        itemNumber += 1;
                        await Task.Delay(CardDisplaySpeed);
                        PlaySound(Properties.Resources.flip);
                        card = newRandomList[i]; c3 = card;
                        DisplayImage(card_3, card);
                        newRandomList.Remove(card);
                    }
                    else if (holdOn3 == true && drawWasClicked % 2 != 1)
                    {
                        card = c3;
                        DisplayImage(card_3, card);
                    }
                    // If the hand is over reset the 3rd card and hold sign
                    else if (holdOn3 == true && drawWasClicked % 2 == 1)
                    {
                        holdOn3 = false;
                        itemNumber += 1;
                        await Task.Delay(CardDisplaySpeed);
                        ImageDestroy(card_3);
                        PlaySound(Properties.Resources.flip);
                        card = newRandomList[i]; c3 = card;
                        DisplayImage(card_3, card);
                        newRandomList.Remove(card);
                    }
                    // -End 3rd card
                    // Check if the card4 is held
                    if (holdOn4 == false)
                    {
                        itemNumber += 1;
                        await Task.Delay(CardDisplaySpeed);
                        PlaySound(Properties.Resources.flip);
                        card = newRandomList[i]; c4 = card;
                        DisplayImage(card_4, card);
                        newRandomList.Remove(card);
                    }
                    else if (holdOn4 == true && drawWasClicked % 2 != 1)
                    {
                        card = c4;
                        DisplayImage(card_4, card);
                    }
                    // If the hand is over reset the 4th card and hold sign
                    else if (holdOn4 == true && drawWasClicked % 2 == 1)
                    {
                        holdOn4 = false;
                        itemNumber += 1;
                        await Task.Delay(CardDisplaySpeed);
                        ImageDestroy(card_4);
                        PlaySound(Properties.Resources.flip);
                        card = newRandomList[i]; c4 = card;
                        DisplayImage(card_4, card);
                        newRandomList.Remove(card);
                    }
                    // -End 4th card

                    // Check if card5 is held
                    if (holdOn5 == false)
                    {
                        itemNumber += 1;
                        await Task.Delay(CardDisplaySpeed);
                        PlaySound(Properties.Resources.flip);
                        card = newRandomList[i]; c5 = card;
                        DisplayImage(card_5, card);
                        newRandomList.Remove(card);
                    }
                    else if (holdOn5 == true && drawWasClicked % 2 != 1)
                    {
                        card = c5;
                        DisplayImage(card_5, card);
                    }
                    // If the hand is over reset the 5th card and hold sign
                    else if (holdOn5 == true && drawWasClicked % 2 == 1)
                    {
                        holdOn5 = false;
                        itemNumber += 1;
                        await Task.Delay(CardDisplaySpeed);
                        ImageDestroy(card_5);
                        PlaySound(Properties.Resources.flip);
                        card = newRandomList[i]; c5 = card;
                        DisplayImage(card_5, card);
                        newRandomList.Remove(card);
                    }
                    // -End 5th card
                    // Loop of displaying cards ends above
                    // Enable the draw button again
                    Draw.IsEnabled = true;
                    BetPlus.IsEnabled = true;
                    BetMinus.IsEnabled = true;
                    WithrawButton.IsEnabled = true;
                    CashOutButton.IsEnabled = true;
                    DepositButton.IsEnabled = true;
                    //Eneble click on cards again
                    card_1.IsEnabled = true;
                    card_2.IsEnabled = true;
                    card_3.IsEnabled = true;
                    card_4.IsEnabled = true;
                    card_5.IsEnabled = true;
                    //Reset that RedBlack Page was Opened Boolean
                    RedBlackWasOpened = false;
                    DoubleUpWasPressed = false;
                    //Dispose the sound player
                    sp.Dispose();

                    // START TextBox for HAND OVER ***********SECOND HAND STARTS HERE**************INSERT WINNING CODE HERE**************

                    if (drawWasClicked % 2 != 1)
                    {
                        FiveOfAKindWon = false;
                        RoyalFlushWon = false;
                        StraightFlushWon = false;
                        FourOfAKindWon = false;
                        FullHouseWon = false;
                        FlushWon = false;
                        StraightWon = false;
                        ThreeOfAKindWon = false;
                        TwoPairWon = false;
                        BetPlus.IsEnabled = false;
                        BetMinus.IsEnabled = false;
                        //WithrawButton.IsEnabled = false;
                        //CashOutButton.IsEnabled = false;
                        //DepositButton.IsEnabled = false;
                        handEnded = true;
                        winner.CheckWithJoker(); //Run CheckWithJoker Function (inside that Function it'll run the WinningCard Function)

                        if (winner.TrueWinner == false && drawButtonSwitch == false)
                        {
                            TextBoxDisplay("HAND OVER");
                        }
                        if (winner.TrueWinner == true)
                        {
                            //Winning signs flashing
                            //turn on the switch for the winning table
                            Shape.flashShwitch = true;
                            WinnerForGuessing = true;
                            if (winner.Card1Won == true)
                            {
                                Card1Wins = true;
                                WinningSigns(hold_sign, 1);
                                Sprite.Flash("hold_sign", 0.3, 0.2, 1, h1Parent);
                            }
                            else if (winner.Card1Won == false)
                            {
                                Card1Wins = false;
                                WinningSigns(hold_sign, 0);
                            }
                            if (winner.Card2Won == true)
                            {
                                Card2Wins = true;
                                WinningSigns(hold_sign2, 1);
                                Sprite.Flash("hold_sign2", 0.3, 0.2, 1, h2Parent);
                            }
                            else if (winner.Card2Won == false)
                            {
                                Card2Wins = false;
                                WinningSigns(hold_sign2, 0);
                            }
                            if (winner.Card3Won == true)
                            {
                                Card3Wins = true;
                                WinningSigns(hold_sign3, 1);
                                Sprite.Flash("hold_sign3", 0.3, 0.2, 1, h3Parent);
                            }
                            else if (winner.Card3Won == false)
                            {
                                Card3Wins = false;
                                WinningSigns(hold_sign3, 0);
                            }
                            if (winner.Card4Won == true)
                            {
                                Card4Wins = true;
                                WinningSigns(hold_sign4, 1);
                                Sprite.Flash("hold_sign4", 0.3, 0.2, 1, h4Parent);
                            }
                            else if (winner.Card4Won == false)
                            {
                                Card4Wins = false;
                                WinningSigns(hold_sign4, 0);
                            }
                            if (winner.Card5Won == true)
                            {
                                Card5Wins = true;
                                WinningSigns(hold_sign5, 1);
                                Sprite.Flash("hold_sign5", 0.3, 0.2, 1, h5Parent);
                            }
                            else if (winner.Card5Won == false)
                            {
                                Card5Wins = false;
                                WinningSigns(hold_sign5, 0);
                            }
                        }
                        //Begin Winning table flashing code and TextBox Display
                        //Five of a Kind
                        if (winner.FiveOfAKind == true && winner.TrueWinner == true)
                        {
                            FiveOfAKindStatsCount++;
                            DoubleUpButton.Visibility = Visibility.Visible;
                            ColButton.Visibility = Visibility.Visible;
                            DoubleUpButton.IsEnabled = true;
                            ColButton.IsEnabled = true;
                            FiveOfAKindWon = true;
                            TextBoxDisplay("5 OF A KIND");
                            Shape.ChangeColorAnimation("rect0", Colors.Red, Colors.CornflowerBlue);
                            Shape.ChangeColorAnimation("rect00", Colors.Red, Colors.CornflowerBlue);
                            winner.JokerWins = false;
                            Draw.IsEnabled = false;
                            StartTime();
                        }
                        //Royal Flush
                        else if (winner.RoyalFlush == true && winner.TrueWinner == true)
                        {
                            RoyalFlushStatsCount++;
                            DoubleUpButton.Visibility = Visibility.Visible;
                            ColButton.Visibility = Visibility.Visible;
                            DoubleUpButton.IsEnabled = true;
                            ColButton.IsEnabled = true;
                            RoyalFlushWon = true;
                            TextBoxDisplay("ROYAL FLUSH");
                            Shape.ChangeColorAnimation("rect1", Colors.Red, Colors.CornflowerBlue);
                            Shape.ChangeColorAnimation("rect01", Colors.Red, Colors.CornflowerBlue);
                            winner.JokerWins = false;
                            Draw.IsEnabled = false;
                            StartTime();
                        }
                        //Straight Flush
                        else if (winner.StraightFlush == true && winner.TrueWinner == true)
                        {
                            StraightFlushStatsCount++;
                            DoubleUpButton.Visibility = Visibility.Visible;
                            ColButton.Visibility = Visibility.Visible;
                            DoubleUpButton.IsEnabled = true;
                            ColButton.IsEnabled = true;
                            StraightFlushWon = true;
                            TextBoxDisplay("STRAIGHT FLUSH");
                            Shape.ChangeColorAnimation("rect2", Colors.Red, Colors.CornflowerBlue);
                            Shape.ChangeColorAnimation("rect02", Colors.Red, Colors.CornflowerBlue);
                            winner.JokerWins = false;
                            Draw.IsEnabled = false;
                            StartTime();
                        }
                        //Four of a Kind
                        else if (winner.FourOfAKind == true && winner.TrueWinner == true)
                        {
                            FourOfAKindStatsCount++;
                            DoubleUpButton.Visibility = Visibility.Visible;
                            ColButton.Visibility = Visibility.Visible;
                            DoubleUpButton.IsEnabled = true;
                            ColButton.IsEnabled = true;
                            FourOfAKindWon = true;
                            TextBoxDisplay("4 OF A KIND");
                            Shape.ChangeColorAnimation("rect3", Colors.Red, Colors.CornflowerBlue);
                            Shape.ChangeColorAnimation("rect03", Colors.Red, Colors.CornflowerBlue);
                            winner.JokerWins = false;
                            Draw.IsEnabled = false;
                            StartTime();
                        }
                        //Full House
                        else if (winner.FullHouse == true && winner.TrueWinner == true)
                        {
                            FullHouseStatsCount++;
                            DoubleUpButton.Visibility = Visibility.Visible;
                            ColButton.Visibility = Visibility.Visible;
                            DoubleUpButton.IsEnabled = true;
                            ColButton.IsEnabled = true;
                            FullHouseWon = true;
                            TextBoxDisplay("FULL HOUSE");
                            Shape.ChangeColorAnimation("rect4", Colors.Red, Colors.CornflowerBlue);
                            Shape.ChangeColorAnimation("rect04", Colors.Red, Colors.CornflowerBlue);
                            winner.JokerWins = false;
                            Draw.IsEnabled = false;
                            StartTime();
                        }
                        //Flush
                        else if (winner.Flush == true && winner.TrueWinner == true)
                        {
                            FlushStatsCount++;
                            DoubleUpButton.Visibility = Visibility.Visible;
                            ColButton.Visibility = Visibility.Visible;
                            DoubleUpButton.IsEnabled = true;
                            ColButton.IsEnabled = true;
                            FlushWon = true;
                            TextBoxDisplay("FLUSH");
                            Shape.ChangeColorAnimation("rect5", Colors.Red, Colors.CornflowerBlue);
                            Shape.ChangeColorAnimation("rect05", Colors.Red, Colors.CornflowerBlue);
                            winner.JokerWins = false;
                            Draw.IsEnabled = false;
                            StartTime();
                        }
                        //Straight
                        else if (winner.Straight == true && winner.TrueWinner == true)
                        {
                            StraightStatsCount++;
                            DoubleUpButton.Visibility = Visibility.Visible;
                            ColButton.Visibility = Visibility.Visible;
                            DoubleUpButton.IsEnabled = true;
                            ColButton.IsEnabled = true;
                            StraightWon = true;
                            TextBoxDisplay("STRAIGHT");
                            Shape.ChangeColorAnimation("rect6", Colors.Red, Colors.CornflowerBlue);
                            Shape.ChangeColorAnimation("rect06", Colors.Red, Colors.CornflowerBlue);
                            winner.JokerWins = false;
                            Draw.IsEnabled = false;
                            StartTime();
                        }
                        //3 of a Kind
                        else if (winner.ThreeOfAKind == true && winner.TrueWinner == true)
                        {
                            ThreeOfAKindStatsCount++;
                            DoubleUpButton.Visibility = Visibility.Visible;
                            ColButton.Visibility = Visibility.Visible;
                            DoubleUpButton.IsEnabled = true;
                            ColButton.IsEnabled = true;
                            ThreeOfAKindWon = true;
                            TextBoxDisplay("3 OF A KIND");
                            Shape.ChangeColorAnimation("rect7", Colors.Red, Colors.CornflowerBlue);
                            Shape.ChangeColorAnimation("rect07", Colors.Red, Colors.CornflowerBlue);
                            winner.JokerWins = false;
                            Draw.IsEnabled = false;
                            StartTime();
                        }
                        //2 Pair
                        else if (winner.TwoPair == true && winner.TrueWinner == true)
                        {
                            TwoPairStatsCount++;
                            DoubleUpButton.Visibility = Visibility.Visible;
                            ColButton.Visibility = Visibility.Visible;
                            DoubleUpButton.IsEnabled = true;
                            ColButton.IsEnabled = true;
                            TwoPairWon = true;
                            TextBoxDisplay("2 PAIRS");
                            Shape.ChangeColorAnimation("rect8", Colors.Red, Colors.CornflowerBlue);
                            Shape.ChangeColorAnimation("rect08", Colors.Red, Colors.CornflowerBlue);
                            winner.JokerWins = false;
                            Draw.IsEnabled = false;
                            StartTime();
                        }
                        //Single Pair
                        else if (winner.OnePair == true && winner.TrueWinner == true)
                        {
                            BonusSwitch++;
                            PlaySound(Properties.Resources.win_single_pair);//play sound for winning 1 pair
                            TextBoxDisplay("JACKS OR BETTER");
                            winner.JokerWins = false;
                            BonusStackCheck();
                        }
                    }

                    else
                    {
                        //Stop The Flashing Signs ( WIN )  -- If there are any 
                        Sprite.Flash("hold_sign", 0, 1, 1, h1Parent);
                        Sprite.Flash("hold_sign2", 0, 1, 1, h2Parent);
                        Sprite.Flash("hold_sign3", 0, 1, 1, h3Parent);
                        Sprite.Flash("hold_sign4", 0, 1, 1, h4Parent);
                        Sprite.Flash("hold_sign5", 0, 1, 1, h5Parent);
                        TextBoxDisplay("CHOOSE CARDS TO HOLD");
                        handEnded = false;
                        // Calling HoldCards Class***********HOLD CARDS CLASS IS CALLED HERE**************
                        //Run Hold Cards Class
                        holdCards.JokerSwitch();
                    }
                    // END Shuffling TextBox
                    continueDraw = false;
                    while (continueDraw == false)
                    {
                        await Task.Delay(5); // End loop with loop ( wait until Draw button is pushed again )
                    }
                }
                //shuffle cards and display backs real fast
                if (newRandomList.Count() <= 10 && handEnded == true)
                {
                    Draw.IsEnabled = false;
                    BetPlus.IsEnabled = false;
                    BetMinus.IsEnabled = false;
                    itemNumber = 0;
                    TextBoxDisplay("SHUFFLING");
                    ImageDestroy(card_1);
                    ImageDestroy(card_2);
                    ImageDestroy(card_3);
                    ImageDestroy(card_4);
                    ImageDestroy(card_5);
                    ImageDestroy(hold_sign);
                    ImageDestroy(hold_sign2);
                    ImageDestroy(hold_sign3);
                    ImageDestroy(hold_sign4);
                    ImageDestroy(hold_sign5);
                    await Task.Delay(800);
                    DisplayStringImage(card_1);
                    PlaySound(Properties.Resources.shuffleCard);
                    await Task.Delay(150);
                    DisplayStringImage(card_2);
                    PlaySound(Properties.Resources.shuffleCard);
                    await Task.Delay(150);
                    DisplayStringImage(card_3);
                    PlaySound(Properties.Resources.shuffleCard);
                    await Task.Delay(150);
                    DisplayStringImage(card_4);
                    PlaySound(Properties.Resources.shuffleCard);
                    await Task.Delay(150);
                    DisplayStringImage(card_5);
                    PlaySound(Properties.Resources.shuffleCard);
                    TextBoxDisplay(null);
                    goto restart;  // Restart displaying when cards are over
                }

            }//End FOR Loop
        }
        
        #endregion END SHUFFLE METHOD 

        #region START DISPLAY CARDS METHOD
        private void Draw_Click(object sender, RoutedEventArgs e)//*********************************************************Draw Button

        {
            shufflingTextBox.Visibility = Visibility.Visible;
            if (drawButtonSwitch == false)//reset the winning signs
            {
                ResetBonusStack();
                drawButtonSwitch = true;
                ImageDestroy(hold_sign);
                ImageDestroy(hold_sign2);
                ImageDestroy(hold_sign3);
                ImageDestroy(hold_sign4);
                ImageDestroy(hold_sign5);
                //Reset the flashing table winnings bool
                Shape.flashShwitch = false;
                Shape.ChangeColorAnimation("rect08", Colors.Transparent, Colors.Transparent);
            }
            else
            {
                drawButtonSwitch = false;

            }
            drawWasClicked += 1;
            //Cash Management
            if (drawWasClicked % 2 == 1 && MainScore >= Bet)
            {
                MainScore -= Bet;
                LblScore.Content = MainScore.ToString();
                Draw.IsEnabled = true;
            }
            else if (drawWasClicked % 2 == 1 && MainScore < Bet)
            {
                PlaySound(Properties.Resources.empty_wallet_sound);
                shufflingTextBox.Text = "INSERT MONEY";
                Draw.IsEnabled = false;
                return;
            }
            shufflingTextBox.Text = null; // Reset the HAND OVER textbox


            //Display back of cards between hands
            if (holdOn == false)
            {
                DisplayStringImage(card_1);
            }
            if (holdOn2 == false)
            {
                DisplayStringImage(card_2);
            }
            if (holdOn3 == false)
            {
                DisplayStringImage(card_3);
            }
            if (holdOn4 == false)
            {
                DisplayStringImage(card_4);
            }
            if (holdOn5 == false)
            {
                DisplayStringImage(card_5);
            }

            continueDraw = true;
            if (drawWasClicked == 1)
            {
                Shuffle();  // Shuffle only once for a whole cycle ! 
            }


        }
        #endregion END DISPLAY CARDS METHOD




        #region DISPLAY HOLD IMAGES METHODS

        private void card1WasClicked(object sender, MouseEventArgs e)

        {
            if ((drawWasClicked % 2 == 1) && (holdOn == false)) // Hold sign 1
            {
                holdOn = true;
                PlaySound(Properties.Resources.shold1);
                DisplayHoldSigns(hold_sign);
            }
            else if ((drawWasClicked % 2 != 1) && (holdOn == true))
            {
                DisplayHoldSigns(hold_sign);
            }
            else
            {
                holdOn = false;
                ImageDestroy(hold_sign);
                PlaySound(Properties.Resources.shold1);
            } // END Hold sign 1
        }
        private void card2WasClicked(object sender, MouseEventArgs e)  // Hold sign 2

        {
            if ((drawWasClicked % 2 == 1) && (holdOn2 == false))
            {
                holdOn2 = true;
                PlaySound(Properties.Resources.shold2);
                DisplayHoldSigns(hold_sign2);
            }
            else if ((drawWasClicked % 2 != 1) && (holdOn2 == true))
            {
                DisplayHoldSigns(hold_sign2);
            }

            else
            {
                holdOn2 = false;
                ImageDestroy(hold_sign2);
                PlaySound(Properties.Resources.shold2);
            }

        } // END Hold sign 2

        private void card3WasClicked(object sender, MouseEventArgs e) // Hold sign 3

        {
            if ((drawWasClicked % 2 == 1) && (holdOn3 == false))
            {
                holdOn3 = true;
                PlaySound(Properties.Resources.shold3);
                DisplayHoldSigns(hold_sign3);
            }
            else if ((drawWasClicked % 2 != 1) && (holdOn3 == true))
            {
                DisplayHoldSigns(hold_sign3);
            }

            else
            {
                holdOn3 = false;
                ImageDestroy(hold_sign3);
                PlaySound(Properties.Resources.shold3);
            }

        } // END Hold sign 3

        private void card4WasClicked(object sender, MouseEventArgs e)  // Hold sign 4

        {
            if ((drawWasClicked % 2 == 1) && (holdOn4 == false))
            {
                holdOn4 = true;
                PlaySound(Properties.Resources.shold4);
                DisplayHoldSigns(hold_sign4);
            }
            else if ((drawWasClicked % 2 != 1) && (holdOn4 == true))
            {
                DisplayHoldSigns(hold_sign4);
            }

            else
            {
                holdOn4 = false;
                ImageDestroy(hold_sign4);
                PlaySound(Properties.Resources.shold4);
            }

        } // END Hold sign 4

        private void card5WasClicked(object sender, MouseEventArgs e) // Hold sign 5

        {
            if ((drawWasClicked % 2 == 1) && (holdOn5 == false))
            {
                holdOn5 = true;
                PlaySound(Properties.Resources.shold5);
                DisplayHoldSigns(hold_sign5);
            }
            else if ((drawWasClicked % 2 != 1) && (holdOn5 == true))
            {
                DisplayHoldSigns(hold_sign5);
            }

            else
            {
                holdOn5 = false;
                ImageDestroy(hold_sign5);
                PlaySound(Properties.Resources.shold5);
            }
        }// END Hold sign 5
        #endregion END HOLD IMAGES METHODS




        #region START KEEP ASPECT RATIO ON RESIZING METHOD
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            RealHeight = Height;
            RealWidth = Width;
            if (sizeInfo.WidthChanged)
            {
                Width = sizeInfo.NewSize.Height * 1.85; // TRANSLATES TO 16:9 RATIO
            }
            else
            {
                Height = sizeInfo.NewSize.Width / 1;
            }
        }
        #endregion END KEEP ASPECT RATIO ON RESIZING METHOD





    }

}
