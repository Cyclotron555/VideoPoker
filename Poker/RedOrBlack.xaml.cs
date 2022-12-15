using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Poker
{
    /// <summary>
    /// Interaction logic for RedOrBlack.xaml
    /// </summary>
    public partial class RedOrBlack : Page
    {

        //----------->Vars
        Helper H = new Helper();
        public int CardDisplayed { get; private set; }
        DispatcherTimer ReFlick = new DispatcherTimer();
        DispatcherTimer winTimer = new DispatcherTimer();
        DispatcherTimer GuessingCardsTimer = new DispatcherTimer();
        DispatcherTimer GuessingCardTimer = new DispatcherTimer();
        DispatcherTimer LblHandTimer = new DispatcherTimer();
        DispatcherTimer PauseBeforeDisplay = new DispatcherTimer();
        DispatcherTimer WinAllTimer = new DispatcherTimer();
        DispatcherTimer LblWinEndTimer = new DispatcherTimer();
        DispatcherTimer HandEndTimer = new DispatcherTimer();
        public int Counter1 { get; set; }
        Winner w = new Winner();
        mainWindow main = Application.Current.Windows[0] as mainWindow;
        SoundPlayer GPlayer = new SoundPlayer(Properties.Resources.G_Sound);
        SoundPlayer SongPlayer = new SoundPlayer(Properties.Resources.guess_win_song1);
        SoundPlayer ArpeggioPlayer = new SoundPlayer();
        SolidColorBrush BlueBrush = new SolidColorBrush(Colors.Navy);
        SolidColorBrush WhiteBrush = new SolidColorBrush(Color.FromArgb(255, 255, 242, 242));
        SolidColorBrush RedBrush = new SolidColorBrush(Colors.DarkRed);
        SolidColorBrush TrueRed = new SolidColorBrush(Colors.Red);
        SoundPlayer SP;
        public int ScoreCount { get; private set; }
        public bool card1W { get; set; }
        public bool card2W { get; set; }
        public bool card3W { get; set; }
        public bool card4W { get; set; }
        public bool card5W { get; set; }
        public bool isPressed { get; private set; }
        public int cardNumber { get; set; }
        public int randNr { get; set; }
        public bool Present { get; private set; }
        public Card cardWon { get; private set; }
        public int WaitAWhile { get; private set; }
        public bool StartFlickering { get; private set; }
        public bool lost { get; set; }
        public Card presentCard { get; private set; }
        private bool firstGuessed, secondGuessed, thirdGuessed, fourthGuessed, fifthGuessed, sixthGuessed;
        private int winAllCounter, lblWinEndCounter, handEndCounter;
        private bool scoreIsDone;
        int j;
        int c;
        int z;
        int f;
        //----------->Vars End

        //Constructor
        public RedOrBlack()
        {
            InitializeComponent();
        }

        private void RedOrBlack1_Loaded(object sender, RoutedEventArgs e)
        {
            firstGuessed = false;
            secondGuessed = false;
            thirdGuessed = false;
            fourthGuessed = false;
            firstGuessed = false;
            sixthGuessed = false;
            B1.Visibility = Visibility.Hidden;
            B1.Background = TrueRed;
            TB1.Visibility = Visibility.Hidden;
            B2.Visibility = Visibility.Hidden;
            B2.Background = TrueRed;
            TB2.Visibility = Visibility.Hidden;
            B3.Visibility = Visibility.Hidden;
            B3.Background = TrueRed;
            TB3.Visibility = Visibility.Hidden;
            B4.Visibility = Visibility.Hidden;
            B4.Background = TrueRed;
            TB4.Visibility = Visibility.Hidden;
            B5.Visibility = Visibility.Hidden;
            B5.Background = TrueRed;
            TB5.Visibility = Visibility.Hidden;
            B6.Visibility = Visibility.Hidden;
            B6.Background = TrueRed;
            TB6.Visibility = Visibility.Hidden;
            RedButton.IsEnabled = false;
            BlackButton.IsEnabled = false;
            RedButton.Visibility = Visibility.Hidden;
            BlackButton.Visibility = Visibility.Hidden;
            main.RedBlackWasOpened = true;
            LblHand.Content = null;
            LblHand.HorizontalContentAlignment = HorizontalAlignment.Center;
            LblHand.VerticalContentAlignment = VerticalAlignment.Center;
            LblWinCount.HorizontalContentAlignment = HorizontalAlignment.Center;
            LblWinCount.VerticalContentAlignment = VerticalAlignment.Center;
            LblHand.Content = main.shufflingTextBox.Text;
            card1.Visibility = Visibility.Hidden;
            card2.Visibility = Visibility.Hidden;
            card3.Visibility = Visibility.Hidden;
            card4.Visibility = Visibility.Hidden;
            card5.Visibility = Visibility.Hidden;
            card6.Visibility = Visibility.Hidden;
            tcard1.Visibility = Visibility.Hidden;
            tcard2.Visibility = Visibility.Hidden;
            tcard3.Visibility = Visibility.Hidden;
            tcard4.Visibility = Visibility.Hidden;
            tcard5.Visibility = Visibility.Hidden;
            DisplayImageOnLocation(originalCard1, mainWindow.c1);
            DisplayImageOnLocation(originalCard2, mainWindow.c2);
            DisplayImageOnLocation(originalCard3, mainWindow.c3);
            DisplayImageOnLocation(originalCard4, mainWindow.c4);
            DisplayImageOnLocation(originalCard5, mainWindow.c5);
            GuessingCard.Visibility = Visibility.Hidden;
            LblWin.Visibility = Visibility.Visible;
            //Initialize this int to store the winning amount
            if (main.TwoPairWon == true)
            {
                ScoreCount = main.TwoPair;
            }
            else if (main.ThreeOfAKindWon == true)
            {
                ScoreCount = main.ThreeOfAKind;
            }
            else if (main.StraightWon == true)
            {
                ScoreCount = main.Straight;
            }
            else if (main.FlushWon == true)
            {
                ScoreCount = main.Flush;
            }
            else if (main.FullHouseWon == true)
            {
                ScoreCount = main.FullHouse;
            }
            else if (main.FourOfAKindWon == true)
            {
                ScoreCount = main.FourOfAKind;
            }
            else if (main.StraightFlushWon == true)
            {
                ScoreCount = main.StraightFlush;
            }
            else if (main.RoyalFlushWon == true)
            {
                ScoreCount = main.RoyalFlush;
            }
            else if (main.FiveOfAKindWon == true)
            {
                ScoreCount = main.FiveOfAKind;
            }
            else if (main.BonusWon == true)
            {
                ScoreCount = main.BonusPrize;
            }
            if (main.WinnerForGuessing == true)
            {
                UpScore();
            }
        }
        //Updates Score at an Interval
        private void UpScore()
        {
            winTimer.Tick += WinTimer_Tick;
            winTimer.Interval = TimeSpan.FromMilliseconds(30);
            winTimer.Start();
            ArpeggioPlayer = new SoundPlayer(Properties.Resources.win_bleep);
            ArpeggioPlayer.PlayLooping();
        }

        private void WinTimer_Tick(object sender, EventArgs e)
        {
            scoreIsDone = false;
            //Score Counting
            j++;
            if (Convert.ToInt32(LblWinCount.Content) < ScoreCount)
            {
                LblWinCount.Content = j;
            }
            //Win Label Changing Color
            if (j % 2 == 1)
            {
                LblWin.Foreground = BlueBrush;
            }
            else
            {
                LblWin.Foreground = WhiteBrush;
            }
            //Reset Score and Start Displaying Cards Event
            if (j == main.BonusPrize && main.BonusWon == true)
            {
                LblWin.Foreground = WhiteBrush;
                scoreIsDone = true;
                StartDisplayingGuessingCards();
                winTimer.Stop();
                winTimer.Tick -= WinTimer_Tick;
                winTimer.IsEnabled = false;
                return;
            }
            if (j == main.TwoPair && main.TwoPairWon == true)
            {
                LblWin.Foreground = WhiteBrush;
                scoreIsDone = true;
                StartDisplayingGuessingCards();
                winTimer.Stop();
                winTimer.Tick -= WinTimer_Tick;
                winTimer.IsEnabled = false;
                return;
            }
            if (j == main.ThreeOfAKind && main.ThreeOfAKindWon == true)
            {
                LblWin.Foreground = WhiteBrush;
                scoreIsDone = true;
                StartDisplayingGuessingCards();
                winTimer.Stop();
                winTimer.Tick -= WinTimer_Tick;
                winTimer.IsEnabled = false;
                return;
            }
            if (j == main.Straight && main.StraightWon == true)
            {
                LblWin.Foreground = WhiteBrush;
                scoreIsDone = true;
                StartDisplayingGuessingCards();
                winTimer.Stop();
                winTimer.Tick -= WinTimer_Tick;
                winTimer.IsEnabled = false;
                return;
            }
            if (j == main.Flush && main.FlushWon == true)
            {
                LblWin.Foreground = WhiteBrush;
                scoreIsDone = true;
                StartDisplayingGuessingCards();
                winTimer.Stop();
                winTimer.Tick -= WinTimer_Tick;
                winTimer.IsEnabled = false;
                return;
            }
            if (j == main.FullHouse && main.FullHouseWon == true)
            {
                LblWin.Foreground = WhiteBrush;
                scoreIsDone = true;
                StartDisplayingGuessingCards();
                winTimer.Stop();
                winTimer.Tick -= WinTimer_Tick;
                winTimer.IsEnabled = false;
                return;
            }
            if (j == main.FourOfAKind && main.FourOfAKindWon == true)
            {
                LblWin.Foreground = WhiteBrush;
                scoreIsDone = true;
                StartDisplayingGuessingCards();
                winTimer.Stop();
                winTimer.Tick -= WinTimer_Tick;
                winTimer.IsEnabled = false;
                return;
            }
            if (j == main.StraightFlush && main.StraightFlushWon == true)
            {
                LblWin.Foreground = WhiteBrush;
                scoreIsDone = true;
                StartDisplayingGuessingCards();
                winTimer.Stop();
                winTimer.Tick -= WinTimer_Tick;
                winTimer.IsEnabled = false;
                return;
            }
            if (j == main.RoyalFlush && main.RoyalFlushWon == true)
            {
                LblWin.Foreground = WhiteBrush;
                scoreIsDone = true;
                StartDisplayingGuessingCards();
                winTimer.Stop();
                winTimer.Tick -= WinTimer_Tick;
                winTimer.IsEnabled = false;
                return;
            }
            if (j == main.FiveOfAKind && main.FiveOfAKindWon == true)
            {
                LblWin.Foreground = WhiteBrush;
                scoreIsDone = true;
                StartDisplayingGuessingCards();
                winTimer.Stop();
                winTimer.Tick -= WinTimer_Tick;
                winTimer.IsEnabled = false;
                return;
            }
        }
        //Tick for Displaying the Cards on Top
        private void GuessingCardsTimer_Tick(object sender, EventArgs e)
        {
            c++;
            if (main.BonusWon == true || main.TwoPairWon == true || main.ThreeOfAKindWon == true || main.StraightWon == true ||
                main.FlushWon == true || main.FullHouseWon == true || main.FourOfAKindWon == true ||
                main.StraightFlushWon == true || main.RoyalFlushWon || main.FiveOfAKindWon == true)
            {
                if (c == 1)
                {
                    PlaySound(Properties.Resources.ascend7);
                    card6.Visibility = Visibility.Visible;
                    B1.Visibility = Visibility.Visible;
                    TB1.Visibility = Visibility.Visible;


                }
                else if (c == 2)
                {
                    PlaySound(Properties.Resources.ascend6);
                    card5.Visibility = Visibility.Visible;
                    B2.Visibility = Visibility.Visible;
                    TB2.Visibility = Visibility.Visible;
                }
                else if (c == 3)
                {
                    PlaySound(Properties.Resources.ascend5);
                    card4.Visibility = Visibility.Visible;
                    B3.Visibility = Visibility.Visible;
                    TB3.Visibility = Visibility.Visible;
                }
                else if (c == 4)
                {
                    PlaySound(Properties.Resources.ascend4);
                    card3.Visibility = Visibility.Visible;
                    B4.Visibility = Visibility.Visible;
                    TB4.Visibility = Visibility.Visible;
                }
                else if (c == 5)
                {
                    PlaySound(Properties.Resources.ascend3);
                    card2.Visibility = Visibility.Visible;
                    B5.Visibility = Visibility.Visible;
                    TB5.Visibility = Visibility.Visible;
                }
                else if (c == 6)
                {
                    PlaySound(Properties.Resources.ascend2);
                    card1.Visibility = Visibility.Visible;
                    B6.Visibility = Visibility.Visible;
                    TB6.Visibility = Visibility.Visible;
                }
                else if (c == 7)
                {
                    PlaySound(Properties.Resources.ascend1);
                    GuessingCardDisplay();
                    RedButton.IsEnabled = true;
                    BlackButton.IsEnabled = true;
                    RedButton.Visibility = Visibility.Visible;
                    BlackButton.Visibility = Visibility.Visible;
                }

            }
        }
        //Display Image Method
        private void DisplayImageOnLocation(Image imageSource, int imageDisplayed)
        {
            imageSource.Source = new BitmapImage(new Uri(@"Resources\" + imageDisplayed + ".png", UriKind.Relative));
        }
        //Displays the guessing cards
        private void StartDisplayingGuessingCards()
        {
            //Stop the Sound For The Money Won Count
            ArpeggioPlayer.Stop();
            ArpeggioPlayer.Dispose();
            //Timer for the Top Cards
            GuessingCardsTimer.Tick += GuessingCardsTimer_Tick;
            GuessingCardsTimer.Interval = TimeSpan.FromMilliseconds(130);
            GuessingCardsTimer.Start();
        }
        private void PlaySound(Stream stream)
        {
            SP = new SoundPlayer(stream);
            SP.Play();
        }
        private void PlaySoundSync(Stream stream)
        {
            SP = new SoundPlayer(stream);
            SP.PlaySync();
        }
        //Displays the Last Card ( the Guessing Card )
        private void GuessingCardDisplay()
        {
            z = 0;
            GuessingCard.Source = null;
            GuessingCard.Source = new BitmapImage(new Uri(@"Resources\backdesign_4.png", UriKind.Relative));
            GuessingCardTimer.Tick += GuessingCardTimer_Tick;
            GuessingCardTimer.Interval = TimeSpan.FromMilliseconds(25);
            GuessingCardTimer.Start();
            GuessingCard.Visibility = Visibility.Visible;
            GPlayer.PlayLooping();
            LblHandTimer.Tick += LblHandTimer_Tick;
            LblHandTimer.Interval = TimeSpan.FromMilliseconds(500);
            LblHandTimer.Start();
        }



        //Guessing Card Tick Method
        private void GuessingCardTimer_Tick(object sender, EventArgs e)
        {
            z++;
            if (z % 2 == 0)
            {
                GuessingCard.Opacity = 1;
            }
            else
            {
                GuessingCard.Opacity = 0.25;
            }

        }
        //Ticker for label Hand ( WIN Signs Flashing on Cards)
        private void LblHandTimer_Tick(object sender, EventArgs e)
        {
            //Reset the visibility on the winning cards
            tcard1.Visibility = Visibility.Hidden;
            tcard2.Visibility = Visibility.Hidden;
            tcard3.Visibility = Visibility.Hidden;
            tcard4.Visibility = Visibility.Hidden;
            tcard5.Visibility = Visibility.Hidden;

            f++;
            if (f % 2 == 0)
            {
                LblHand.Foreground = RedBrush;
                tcard1.Foreground = WhiteBrush;
                tcard2.Foreground = WhiteBrush;
                tcard3.Foreground = WhiteBrush;
                tcard4.Foreground = WhiteBrush;
                tcard5.Foreground = WhiteBrush;
                //card1
                if (main.Card1Wins == true)
                {
                    tcard1.Visibility = Visibility.Visible;
                    tcard1.Opacity = 0.5f;
                }
                if (main.Card1Wins == false)
                {
                    tcard1.Visibility = Visibility.Hidden;
                }
                //card2
                if (main.Card2Wins == true)
                {
                    tcard2.Visibility = Visibility.Visible;
                    tcard2.Opacity = 0.5f;
                }
                if (main.Card2Wins == false)
                {
                    tcard2.Visibility = Visibility.Hidden;
                }
                //card3
                if (main.Card3Wins == true)
                {
                    tcard3.Visibility = Visibility.Visible;
                    tcard3.Opacity = 0.5f;
                }
                if (main.Card3Wins == false)
                {
                    tcard3.Visibility = Visibility.Hidden;
                }
                //card4
                if (main.Card4Wins == true)
                {
                    tcard4.Visibility = Visibility.Visible;
                    tcard4.Opacity = 0.5f;
                }
                if (main.Card4Wins == false)
                {
                    tcard4.Visibility = Visibility.Hidden;
                }
                //card5
                if (main.Card5Wins == true)
                {
                    tcard5.Visibility = Visibility.Visible;
                    tcard5.Opacity = 0.5f;
                }
                if (main.Card5Wins == false)
                {
                    tcard5.Visibility = Visibility.Hidden;
                }
                //Flashing For Stack
                if (firstGuessed == false)
                {
                    B1.Visibility = Visibility.Visible;
                    TB1.Visibility = Visibility.Visible;
                }
                if (firstGuessed == true)
                {
                    B2.Visibility = Visibility.Visible;
                    TB2.Visibility = Visibility.Visible;
                    B1.Visibility = Visibility.Hidden;
                    TB1.Visibility = Visibility.Hidden;
                }
                if (secondGuessed == true)
                {
                    B3.Visibility = Visibility.Visible;
                    TB3.Visibility = Visibility.Visible;
                    B1.Visibility = Visibility.Hidden;
                    TB1.Visibility = Visibility.Hidden;
                    B2.Visibility = Visibility.Hidden;
                    TB2.Visibility = Visibility.Hidden;
                }
                if (thirdGuessed == true)
                {
                    B4.Visibility = Visibility.Visible;
                    TB4.Visibility = Visibility.Visible;
                    B3.Visibility = Visibility.Hidden;
                    TB3.Visibility = Visibility.Hidden;
                    B1.Visibility = Visibility.Hidden;
                    TB1.Visibility = Visibility.Hidden;
                    B2.Visibility = Visibility.Hidden;
                    TB2.Visibility = Visibility.Hidden;
                }
                if (fourthGuessed == true)
                {
                    B5.Visibility = Visibility.Visible;
                    TB5.Visibility = Visibility.Visible;
                    B4.Visibility = Visibility.Hidden;
                    TB4.Visibility = Visibility.Hidden;
                    B3.Visibility = Visibility.Hidden;
                    TB3.Visibility = Visibility.Hidden;
                    B1.Visibility = Visibility.Hidden;
                    TB1.Visibility = Visibility.Hidden;
                    B2.Visibility = Visibility.Hidden;
                    TB2.Visibility = Visibility.Hidden;
                }
                if (fifthGuessed == true)
                {
                    B6.Visibility = Visibility.Visible;
                    TB6.Visibility = Visibility.Visible;
                    B5.Visibility = Visibility.Hidden;
                    TB5.Visibility = Visibility.Hidden;
                    B4.Visibility = Visibility.Hidden;
                    TB4.Visibility = Visibility.Hidden;
                    B3.Visibility = Visibility.Hidden;
                    TB3.Visibility = Visibility.Hidden;
                    B1.Visibility = Visibility.Hidden;
                    TB1.Visibility = Visibility.Hidden;
                    B2.Visibility = Visibility.Hidden;
                    TB2.Visibility = Visibility.Hidden;
                }
            }
            else if (f % 2 == 1)
            {
                LblHand.Foreground = WhiteBrush;
                tcard1.Foreground = RedBrush;
                tcard2.Foreground = RedBrush;
                tcard3.Foreground = RedBrush;
                tcard4.Foreground = RedBrush;
                tcard5.Foreground = RedBrush;
                //card1
                if (main.Card1Wins == true)
                {
                    tcard1.Visibility = Visibility.Visible;
                    tcard1.Opacity = 1f;
                }
                if (main.Card1Wins == false)
                {
                    tcard1.Visibility = Visibility.Hidden;
                }
                //card2
                if (main.Card2Wins == true)
                {
                    tcard2.Visibility = Visibility.Visible;
                    tcard2.Opacity = 1f;
                }
                if (main.Card2Wins == false)
                {
                    tcard2.Visibility = Visibility.Hidden;
                }
                //card3
                if (main.Card3Wins == true)
                {
                    tcard3.Visibility = Visibility.Visible;
                    tcard3.Opacity = 1f;
                }
                if (main.Card3Wins == false)
                {
                    tcard3.Visibility = Visibility.Hidden;
                }
                //card4
                if (main.Card4Wins == true)
                {
                    tcard4.Visibility = Visibility.Visible;
                    tcard4.Opacity = 1f;
                }
                if (main.Card4Wins == false)
                {
                    tcard4.Visibility = Visibility.Hidden;
                }
                //card5
                if (main.Card5Wins == true)
                {
                    tcard5.Visibility = Visibility.Visible;
                    tcard5.Opacity = 1f;
                }
                if (main.Card5Wins == false)
                {
                    tcard5.Visibility = Visibility.Hidden;
                }
                //Flashing For Stack
                if (firstGuessed == false)
                {
                    B1.Visibility = Visibility.Hidden;
                    TB1.Visibility = Visibility.Hidden;
                }
                if (firstGuessed == true)
                {
                    B2.Visibility = Visibility.Hidden;
                    TB2.Visibility = Visibility.Hidden;
                    B1.Visibility = Visibility.Hidden;
                    TB1.Visibility = Visibility.Hidden;
                }
                if (secondGuessed == true)
                {
                    B3.Visibility = Visibility.Hidden;
                    TB3.Visibility = Visibility.Hidden;
                    B1.Visibility = Visibility.Hidden;
                    TB1.Visibility = Visibility.Hidden;
                    B2.Visibility = Visibility.Hidden;
                    TB2.Visibility = Visibility.Hidden;
                }
                if (thirdGuessed == true)
                {
                    B4.Visibility = Visibility.Hidden;
                    TB4.Visibility = Visibility.Hidden;
                    B3.Visibility = Visibility.Hidden;
                    TB3.Visibility = Visibility.Hidden;
                    B1.Visibility = Visibility.Hidden;
                    TB1.Visibility = Visibility.Hidden;
                    B2.Visibility = Visibility.Hidden;
                    TB2.Visibility = Visibility.Hidden;
                }
                if (fourthGuessed == true)
                {
                    B5.Visibility = Visibility.Hidden;
                    TB5.Visibility = Visibility.Hidden;
                    B4.Visibility = Visibility.Hidden;
                    TB4.Visibility = Visibility.Hidden;
                    B3.Visibility = Visibility.Hidden;
                    TB3.Visibility = Visibility.Hidden;
                    B1.Visibility = Visibility.Hidden;
                    TB1.Visibility = Visibility.Hidden;
                    B2.Visibility = Visibility.Hidden;
                    TB2.Visibility = Visibility.Hidden;
                }
                if (fifthGuessed == true)
                {
                    B6.Visibility = Visibility.Hidden;
                    TB6.Visibility = Visibility.Hidden;
                    B5.Visibility = Visibility.Hidden;
                    TB5.Visibility = Visibility.Hidden;
                    B4.Visibility = Visibility.Hidden;
                    TB4.Visibility = Visibility.Hidden;
                    B3.Visibility = Visibility.Hidden;
                    TB3.Visibility = Visibility.Hidden;
                    B1.Visibility = Visibility.Hidden;
                    TB1.Visibility = Visibility.Hidden;
                    B2.Visibility = Visibility.Hidden;
                    TB2.Visibility = Visibility.Hidden;
                }
            }
        }
        //Collect / Return Button ( Collects the winnings and exits )
        private void CollectButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            PlaySoundSync(Properties.Resources.rb_collect);
            //Reset Bonus Bool
            if (main.BonusWon == true)
            {
                main.BonusWon = false;
            }
            if (main.BonusSwitch == 9)
            {
                main.BonusSwitch = 0;
            }
            main.MainScore += ScoreCount;
            main.LblScore.Content = main.MainScore.ToString();
            main.doubleUp = 0;
            main.DoubleUpWasPressed = false;
            main.Draw.IsEnabled = true;
            main.DoubleUpButton.IsEnabled = false;
            main.ColButton.IsEnabled = false;
            main.DoubleUpButton.Visibility = Visibility.Hidden;
            main.ColButton.Visibility = Visibility.Hidden;
            //Remove PauseBeforeDisplay Timer 
            PauseBeforeDisplay.Stop();
            PauseBeforeDisplay.Tick -= PauseBeforeDisplay_Tick;
            PauseBeforeDisplay.IsEnabled = false;
            //Remove ReFlick Timer 
            ReFlick.Stop();
            ReFlick.IsEnabled = false;
            ReFlick.Tick -= ReFlick_Tick;
            //Remove GuessingCardTimer Timer 
            GuessingCardTimer.Stop();
            GuessingCardTimer.Tick -= GuessingCardTimer_Tick;
            GuessingCardTimer.IsEnabled = false;
            //Remove winTimer Timer 
            winTimer.Stop();
            winTimer.IsEnabled = false;
            winTimer.Tick -= WinTimer_Tick;
            //Remove GuessingCardsTimer Timer 
            GuessingCardsTimer.Stop();
            GuessingCardsTimer.Tick -= GuessingCardsTimer_Tick;
            GuessingCardsTimer.IsEnabled = false;
            //Remove LblHandTimer Timer 
            LblHandTimer.Stop();
            LblHandTimer.Tick -= LblHandTimer_Tick;
            LblHandTimer.IsEnabled = false;
            //Remove WinAll Timer 
            WinAllTimer.Stop();
            WinAllTimer.Tick -= LblHandTimer_Tick;
            WinAllTimer.IsEnabled = false;
            //Dispose Sound Players
            GPlayer.Stop();
            GPlayer.Dispose();
            ArpeggioPlayer.Stop();
            ArpeggioPlayer.Dispose();
            SP.Stop();
            SP.Dispose();
            SongPlayer.Stop();
            SongPlayer.Dispose();
            main.CreateWinningsTable();
            //Reset Vars in Main
            main.WinnerForGuessing = false;
            main.TwoPairWon = false;
            main.ThreeOfAKindWon = false;
            main.StraightWon = false;
            main.FlushWon = false;
            main.FullHouseWon = false;
            main.FourOfAKindWon = false;
            main.StraightFlushWon = false;
            main.RoyalFlushWon = false;
            main.FiveOfAKindWon = false;
            main.TwoPairWon = false;
            Content = null;
            IsEnabled = false;
            main.MainFrame.Content = null;
            main.MainFrame.IsEnabled = false;
        }
        //Red Button Down
        private void RedButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ExecuteRed();
        }
        //Red Button Up
        private void RedButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnButtonsUp();
        }
        //Black Button Down
        private void BlackButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ExecuteBlack();
        }
        //Black Button Up
        private void BlackButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnButtonsUp();
        }
        private void OnButtonsUp()
        {
            ReFlick.Stop();
            ReFlick.Tick -= ReFlick_Tick;
            ReFlick.IsEnabled = false;
        }
        //For Red Button
        private void ExecuteRed()
        {
            GuessingCardTimer.Stop();
            GuessingCardTimer.Tick -= GuessingCardTimer_Tick;
            GuessingCardTimer.IsEnabled = false;
            var nextCard = H.NextCard();
            cardWon = nextCard;
            if (cardWon.Col == "red")
            {
                lost = false;
                RedButton.IsEnabled = false;
                BlackButton.IsEnabled = false;
                GuessingCard.Opacity = 1;
                GuessingCard.Visibility = Visibility.Visible;
                ReFlick.Stop();
                ReFlick.IsEnabled = false;
                ReFlick.Tick -= ReFlick_Tick;
                CardDisplayed++;
                GPlayer.Stop();
                GuessingCard.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));

                if (CardDisplayed == 1)
                {
                    PlaySound(Properties.Resources.arpeggio_1);
                    firstGuessed = true;
                }
                else if (CardDisplayed == 2)
                {
                    PlaySound(Properties.Resources.arpeggio_2);
                    secondGuessed = true;
                }
                else if (CardDisplayed == 3)
                {
                    PlaySound(Properties.Resources.arpeggio_3);
                    thirdGuessed = true;
                }
                else if (CardDisplayed == 4)
                {
                    PlaySound(Properties.Resources.arpeggio_4);
                    fourthGuessed = true;
                }
                else if (CardDisplayed == 5)
                {
                    PlaySound(Properties.Resources.arpeggio_5);
                    firstGuessed = true;
                }
                else if (CardDisplayed == 6)
                {
                    PlaySound(Properties.Resources.arpeggio_6);
                    sixthGuessed = true;
                }
                if (lost == false)
                {
                    Pause();
                }
                else
                {
                    return;
                }
            }
            else if (cardWon.Col == "black")
            {
                GuessingCard.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                Bust();
                lost = true;
                return;
            }
        }
        //For Black Button
        private void ExecuteBlack()
        {
            GuessingCardTimer.Stop();
            GuessingCardTimer.Tick -= GuessingCardTimer_Tick;
            GuessingCardTimer.IsEnabled = false;
            var nextCard = H.NextCard();
            cardWon = nextCard;
            if (cardWon.Col == "black")
            {
                lost = false;
                RedButton.IsEnabled = false;
                BlackButton.IsEnabled = false;
                GuessingCard.Opacity = 1;
                GuessingCard.Visibility = Visibility.Visible;
                ReFlick.Stop();
                ReFlick.Tick -= ReFlick_Tick;
                ReFlick.IsEnabled = false;
                CardDisplayed++;
                GPlayer.Stop();
                GuessingCard.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));

                if (CardDisplayed == 1)
                {
                    PlaySound(Properties.Resources.arpeggio_1);
                    firstGuessed = true;
                }
                else if (CardDisplayed == 2)
                {
                    PlaySound(Properties.Resources.arpeggio_2);
                    secondGuessed = true;
                }
                else if (CardDisplayed == 3)
                {
                    PlaySound(Properties.Resources.arpeggio_3);
                    thirdGuessed = true;
                }
                else if (CardDisplayed == 4)
                {
                    PlaySound(Properties.Resources.arpeggio_4);
                    fourthGuessed = true;
                }
                else if (CardDisplayed == 5)
                {
                    PlaySound(Properties.Resources.arpeggio_5);
                    fifthGuessed = true;
                }
                else if (CardDisplayed == 6)
                {
                    PlaySound(Properties.Resources.arpeggio_6);
                    sixthGuessed = true;
                }
                if (lost == false)
                {
                    Pause();
                }
                else
                {
                    return;
                }
            }
            else if (cardWon.Col == "red")
            {
                GuessingCard.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                Bust();
                lost = true;
                return;
            }

        }
        private void Pause()
        {
            //Double the score
            ScoreCount *= 2;
            LblWinCount.Content = ScoreCount;
            //Start the pause before displaying cards on right
            PauseBeforeDisplay.Start();
            PauseBeforeDisplay.Tick += PauseBeforeDisplay_Tick;
            PauseBeforeDisplay.Interval = TimeSpan.FromMilliseconds(777);
        }

        private void PauseBeforeDisplay_Tick(object sender, EventArgs e)
        {
            if (WaitAWhile < 2)
            {
                RedButton.IsEnabled = false;
                BlackButton.IsEnabled = false;
            }
            else
            {
                RedButton.IsEnabled = true;
                BlackButton.IsEnabled = true;
            }
            if (WaitAWhile == 2)
            {
                // Reset Reflicker counter z
                z = 0;
                if (CardDisplayed == 1)
                {
                    card1.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                    PauseBeforeDisplay.Stop();
                    PauseBeforeDisplay.Tick -= PauseBeforeDisplay_Tick;
                    PauseBeforeDisplay.IsEnabled = false;
                    ReFlicker();
                    RedButton.IsEnabled = true;
                    BlackButton.IsEnabled = true;
                    return;
                }
                else if (CardDisplayed == 2)
                {
                    card2.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                    PauseBeforeDisplay.Stop();
                    PauseBeforeDisplay.Tick -= PauseBeforeDisplay_Tick;
                    PauseBeforeDisplay.IsEnabled = false;
                    ReFlicker();
                    RedButton.IsEnabled = true;
                    BlackButton.IsEnabled = true;
                    return;
                }
                else if (CardDisplayed == 3)
                {
                    card3.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                    PauseBeforeDisplay.Stop();
                    PauseBeforeDisplay.Tick -= PauseBeforeDisplay_Tick;
                    PauseBeforeDisplay.IsEnabled = false;
                    ReFlicker();
                    RedButton.IsEnabled = true;
                    BlackButton.IsEnabled = true;
                    return;
                }
                else if (CardDisplayed == 4)
                {
                    card4.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                    PauseBeforeDisplay.Stop();
                    PauseBeforeDisplay.Tick -= PauseBeforeDisplay_Tick;
                    PauseBeforeDisplay.IsEnabled = false;
                    ReFlicker();
                    RedButton.IsEnabled = true;
                    BlackButton.IsEnabled = true;
                    return;
                }
                else if (CardDisplayed == 5)
                {
                    card5.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                    PauseBeforeDisplay.Stop();
                    PauseBeforeDisplay.Tick -= PauseBeforeDisplay_Tick;
                    PauseBeforeDisplay.IsEnabled = false;
                    ReFlicker();
                    RedButton.IsEnabled = true;
                    BlackButton.IsEnabled = true;
                    return;
                }
                else if (CardDisplayed == 6)
                {
                    card6.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                    PauseBeforeDisplay.Stop();
                    PauseBeforeDisplay.Tick -= PauseBeforeDisplay_Tick;
                    PauseBeforeDisplay.IsEnabled = false;
                    RedButton.IsEnabled = false;
                    BlackButton.IsEnabled = false;
                    //Play winning song
                    SongPlayer.PlayLooping();
                    //Start Flashing the Cards with Music
                    WinAll();
                    //Add To Jackpot Counter in Main Class
                    mainWindow main = Application.Current.Windows[0] as mainWindow;
                    main.JackpotCount++;
                    //exit if last card is won
                    return;
                }
                StartFlickering = false;
            }
            WaitAWhile++;
        }
        private void ReFlicker()
        {
            GuessingCard.Source = null;
            GuessingCard.Source = new BitmapImage(new Uri(@"Resources\backdesign_4.png", UriKind.Relative));
            ReFlick.IsEnabled = true;
            ReFlick.Tick += ReFlick_Tick;
            ReFlick.Interval = TimeSpan.FromMilliseconds(32);
            ReFlick.Start();
            GPlayer.PlayLooping();
        }

        private void ReFlick_Tick(object sender, EventArgs e)
        {
            z++;
            if (z % 2 == 0)
            {

                GuessingCard.Opacity = 1;
            }
            else
            {
                GuessingCard.Opacity = 0.40;
            }
        }

        //Guess the wrong card code Going BUST
        private void Bust()
        {
            //Reset Bonus Bool
            if (main.BonusWon == true)
            {
                main.BonusWon = false;
            }
            ScoreCount = 0;
            main.doubleUp = 0;
            RedButton.IsEnabled = false;
            BlackButton.IsEnabled = false;
            LblWin.Content = "BUST";
            LblWinCount.Content = "0";
            //Remove PauseBeforeDisplay Timer 
            PauseBeforeDisplay.Stop();
            PauseBeforeDisplay.Tick -= PauseBeforeDisplay_Tick;
            PauseBeforeDisplay.IsEnabled = false;
            //Remove ReFlick Timer 
            ReFlick.Stop();
            ReFlick.Tick -= ReFlick_Tick;
            ReFlick.IsEnabled = false;
            //Remove GuessingCardTimer Timer 
            GuessingCardTimer.Stop();
            GuessingCardTimer.IsEnabled = false;
            GuessingCardTimer.Tick -= GuessingCardTimer_Tick;
            //Remove winTimer Timer 
            winTimer.Stop();
            winTimer.Tick -= WinTimer_Tick;
            winTimer.IsEnabled = false;
            //Remove GuessingCardsTimer Timer 
            GuessingCardsTimer.Stop();
            GuessingCardsTimer.Tick -= GuessingCardsTimer_Tick;
            GuessingCardsTimer.IsEnabled = false;
            //Remove LblHandTimer Timer 
            LblHandTimer.Stop();
            LblHandTimer.Tick -= LblHandTimer_Tick;
            LblHandTimer.IsEnabled = false;
            //Dispose Sound Players
            GPlayer.Stop();
            GPlayer.Dispose();
            ArpeggioPlayer.Stop();
            ArpeggioPlayer.Dispose();
            SP.Stop();
            SP.Dispose();
            //Reset Guessing Card
            GuessingCard.Opacity = 1;
            GuessingCard.Visibility = Visibility.Visible;
            PlaySound(Properties.Resources.bust);
            CollectButton.Content = "RETURN";
            if (CardDisplayed == 0)
            {
                card1.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                B1.Visibility = Visibility.Visible;
                B1.Background = BlueBrush;
                TB1.Visibility = Visibility.Visible;
                return;
            }
            else if (CardDisplayed == 1)
            {
                card2.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                B2.Visibility = Visibility.Visible;
                B2.Background = BlueBrush;
                TB2.Visibility = Visibility.Visible;
                return;
            }
            else if (CardDisplayed == 2)
            {
                card3.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                B3.Visibility = Visibility.Visible;
                B3.Background = BlueBrush;
                TB3.Visibility = Visibility.Visible;
                return;
            }
            else if (CardDisplayed == 3)
            {
                card4.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                B4.Visibility = Visibility.Visible;
                B4.Background = BlueBrush;
                TB4.Visibility = Visibility.Visible;
                return;
            }
            else if (CardDisplayed == 4)
            {
                card5.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                B5.Visibility = Visibility.Visible;
                B5.Background = BlueBrush;
                TB5.Visibility = Visibility.Visible;
                return;
            }
            else if (CardDisplayed == 5)
            {
                card6.Source = new BitmapImage(new Uri(@"Resources\" + cardWon.Img, UriKind.Relative));
                B6.Visibility = Visibility.Visible;
                B6.Background = BlueBrush;
                TB6.Visibility = Visibility.Visible;
                return;
            }
        }
        private void WinAll()
        {
            LblWinEnd();
            HandEnd();
            GuessingCardsTimer.Stop();
            GuessingCardsTimer.Tick -= GuessingCardsTimer_Tick;
            GuessingCardsTimer.IsEnabled = false;
            LblHandTimer.Stop();
            LblHandTimer.Tick -= LblHandTimer_Tick;
            LblHandTimer.IsEnabled = false;
            WinAllTimer.IsEnabled = true;
            WinAllTimer.Tick += WinAllTimer_Tick;
            WinAllTimer.Interval = TimeSpan.FromMilliseconds(500);
            WinAllTimer.Start();
            B1.Visibility = Visibility.Visible;
            TB1.Visibility = Visibility.Visible;
            B2.Visibility = Visibility.Visible;
            TB2.Visibility = Visibility.Visible;
            B3.Visibility = Visibility.Visible;
            TB3.Visibility = Visibility.Visible;
            B4.Visibility = Visibility.Visible;
            TB4.Visibility = Visibility.Visible;
            B5.Visibility = Visibility.Visible;
            TB5.Visibility = Visibility.Visible;
            B6.Visibility = Visibility.Visible;
            TB6.Visibility = Visibility.Visible;
            LblWin.Visibility = Visibility.Visible;
            LblWinCount.Visibility = Visibility.Visible;
        }

        private void WinAllTimer_Tick(object sender, EventArgs e)
        {
            winAllCounter++;
            if (winAllCounter == 1)
            {
                card1.Opacity = 1;
                card2.Opacity = 0.2;
                card3.Opacity = 0.2;
                card4.Opacity = 0.2;
                card5.Opacity = 0.2;
                card6.Opacity = 0.2;
                B1.Opacity = 1;
                TB1.Opacity = 1;
                B2.Opacity = 0.5;
                TB2.Opacity = 0.5;
                B3.Opacity = 0.5;
                TB3.Opacity = 0.5;
                B4.Opacity = 0.5;
                TB4.Opacity = 0.5;
                B5.Opacity = 0.5;
                TB5.Opacity = 0.5;
                B6.Opacity = 0.5;
                TB6.Opacity = 0.5;
            }
            else if (winAllCounter == 2)
            {
                card1.Opacity = 0.2;
                card2.Opacity = 1;
                card3.Opacity = 0.2;
                card4.Opacity = 0.2;
                card5.Opacity = 0.2;
                card6.Opacity = 0.2;
                B1.Opacity = 0.5;
                TB1.Opacity = 0.5;
                B2.Opacity = 1;
                TB2.Opacity = 1;
                B3.Opacity = 0.5;
                TB3.Opacity = 0.5;
                B4.Opacity = 0.5;
                TB4.Opacity = 0.5;
                B5.Opacity = 0.5;
                TB5.Opacity = 0.5;
                B6.Opacity = 0.5;
                TB6.Opacity = 0.5;
            }
            else if (winAllCounter == 3)
            {
                card1.Opacity = 0.2;
                card2.Opacity = 0.2;
                card3.Opacity = 1;
                card4.Opacity = 0.2;
                card5.Opacity = 0.2;
                card6.Opacity = 0.2;
                B1.Opacity = 0.5;
                TB1.Opacity = 0.5;
                B2.Opacity = 0.5;
                TB2.Opacity = 0.5;
                B3.Opacity = 1;
                TB3.Opacity = 1;
                B4.Opacity = 0.5;
                TB4.Opacity = 0.5;
                B5.Opacity = 0.5;
                TB5.Opacity = 0.5;
                B6.Opacity = 0.5;
                TB6.Opacity = 0.5;
            }
            else if (winAllCounter == 4)
            {
                card1.Opacity = 0.2;
                card2.Opacity = 0.2;
                card3.Opacity = 0.2;
                card4.Opacity = 1;
                card5.Opacity = 0.2;
                card6.Opacity = 0.2;
                B1.Opacity = 0.5;
                TB1.Opacity = 0.5;
                B2.Opacity = 0.5;
                TB2.Opacity = 0.5;
                B3.Opacity = 0.5;
                TB3.Opacity = 0.5;
                B4.Opacity = 1;
                TB4.Opacity = 1;
                B5.Opacity = 0.5;
                TB5.Opacity = 0.5;
                B6.Opacity = 0.5;
                TB6.Opacity = 0.5;
            }
            else if (winAllCounter == 5)
            {
                card1.Opacity = 0.2;
                card2.Opacity = 0.2;
                card3.Opacity = 0.2;
                card4.Opacity = 0.2;
                card5.Opacity = 1;
                card6.Opacity = 0.2;
                B1.Opacity = 0.5;
                TB1.Opacity = 0.5;
                B2.Opacity = 0.5;
                TB2.Opacity = 0.5;
                B3.Opacity = 0.5;
                TB3.Opacity = 0.5;
                B4.Opacity = 0.5;
                TB4.Opacity = 0.5;
                B5.Opacity = 1;
                TB5.Opacity = 1;
                B6.Opacity = 0.5;
                TB6.Opacity = 0.5;
            }
            else if (winAllCounter == 6)
            {
                card1.Opacity = 0.2;
                card2.Opacity = 0.2;
                card3.Opacity = 0.2;
                card4.Opacity = 0.2;
                card5.Opacity = 0.2;
                card6.Opacity = 1;
                B1.Opacity = 0.5;
                TB1.Opacity = 0.5;
                B2.Opacity = 0.5;
                TB2.Opacity = 0.5;
                B3.Opacity = 0.5;
                TB3.Opacity = 0.5;
                B4.Opacity = 0.5;
                TB4.Opacity = 0.5;
                B5.Opacity = 0.5;
                TB5.Opacity = 0.5;
                B6.Opacity = 1;
                TB6.Opacity = 1;
            }
            else if (winAllCounter > 6)
            {
                winAllCounter = 0;
            }
        }
        private void LblWinEnd()
        {
           LblWinEndTimer.Tick += LblWinEndTimer_Tick;
           LblWinEndTimer.Interval = TimeSpan.FromMilliseconds(500);
           LblWinEndTimer.Start();
           LblHand.Content = "JACKPOT";
        }

        private void LblWinEndTimer_Tick(object sender, EventArgs e)
        {
            lblWinEndCounter++;
            if ( lblWinEndCounter % 2 == 0 )
            {
                LblWin.Foreground = TrueRed;
                LblWinCount.Foreground = WhiteBrush;
                LblHand.Foreground = TrueRed;
                GuessingCard.Opacity = 0.2;
            }
            if (lblWinEndCounter % 2 == 1 )
            {
                LblWin.Foreground = WhiteBrush;
                LblWinCount.Foreground = TrueRed;
                LblHand.Foreground = WhiteBrush;
                GuessingCard.Opacity = 1;
            }
        }
        private void HandEnd()
        {
            HandEndTimer.Tick += HandEndTimer_Tick;
            HandEndTimer.Interval = TimeSpan.FromMilliseconds(500);
            HandEndTimer.Start();
            
        }

        private void HandEndTimer_Tick(object sender, EventArgs e)
        {

            handEndCounter++;
            if (handEndCounter == 1)
            {
                originalCard1.Opacity = 1;
                originalCard2.Opacity = 0.2;
                originalCard3.Opacity = 0.2;
                originalCard4.Opacity = 0.2;
                originalCard5.Opacity = 1;
                tcard1.Opacity = 1;
                tcard2.Opacity = 0.2;
                tcard3.Opacity = 0.2;
                tcard4.Opacity = 0.2;
                tcard5.Opacity = 1;
            }
            else if (handEndCounter == 2)
            {
                originalCard1.Opacity = 0.2;
                originalCard2.Opacity = 1;
                originalCard3.Opacity = 0.2;
                originalCard4.Opacity = 1;
                originalCard5.Opacity = 0.2;
                tcard1.Opacity = 0.2;
                tcard2.Opacity = 1;
                tcard3.Opacity = 0.2;
                tcard4.Opacity = 1;
                tcard5.Opacity = 0.2;
            }
            else if (handEndCounter == 3)
            {
                originalCard1.Opacity = 0.2;
                originalCard2.Opacity = 0.2;
                originalCard3.Opacity = 1;
                originalCard4.Opacity = 0.2;
                originalCard5.Opacity = 0.2;
                tcard1.Opacity = 0.2;
                tcard2.Opacity = 0.2;
                tcard3.Opacity = 1; 
                tcard4.Opacity = 0.2;
                tcard5.Opacity = 0.2;
            }
            else if (handEndCounter == 4)
            {
                originalCard1.Opacity = 0.2;
                originalCard2.Opacity = 1;
                originalCard3.Opacity = 0.2;
                originalCard4.Opacity = 1;
                originalCard5.Opacity = 0.2;
                tcard1.Opacity = 0.2;
                tcard2.Opacity = 1;
                tcard3.Opacity = 0.2;
                tcard4.Opacity = 1;
                tcard5.Opacity = 0.2;
            }
            else if (handEndCounter == 5)
            {
                originalCard1.Opacity = 1;
                originalCard2.Opacity = 0.2;
                originalCard3.Opacity = 0.2;
                originalCard4.Opacity = 0.2;
                originalCard5.Opacity = 1;
                tcard1.Opacity = 1;
                tcard2.Opacity = 0.2;
                tcard3.Opacity = 0.2;
                tcard4.Opacity = 0.2;
                tcard5.Opacity = 1;
            }
            else if (handEndCounter == 6)
            {
                originalCard1.Opacity = 0.2;
                originalCard2.Opacity = 1;
                originalCard3.Opacity = 0.2;
                originalCard4.Opacity = 1;
                originalCard5.Opacity = 0.2;
                tcard1.Opacity = 0.2;
                tcard2.Opacity = 1;
                tcard3.Opacity = 0.2;
                tcard4.Opacity = 1;
                tcard5.Opacity = 0.2;
            }
            else if (handEndCounter == 7)
            {
                originalCard1.Opacity = 0.2;
                originalCard2.Opacity = 0.2;
                originalCard3.Opacity = 1;
                originalCard4.Opacity = 0.2;
                originalCard5.Opacity = 0.2;
                tcard1.Opacity = 0.2;
                tcard2.Opacity = 0.2;
                tcard3.Opacity = 1;
                tcard4.Opacity = 0.2;
                tcard5.Opacity = 0.2;
            }
            else if (handEndCounter == 8)
            {
                originalCard1.Opacity = 0.2;
                originalCard2.Opacity = 1;
                originalCard3.Opacity = 0.2;
                originalCard4.Opacity = 1;
                originalCard5.Opacity = 0.2;
                tcard1.Opacity = 0.2;
                tcard2.Opacity = 1;
                tcard3.Opacity = 0.2;
                tcard4.Opacity = 1;
                tcard5.Opacity = 0.2;
                handEndCounter = 0;
            }
        }
    }
}
