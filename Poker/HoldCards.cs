using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Media;
using System.Threading.Tasks;

namespace Poker
{

    class HoldCards

    {
        #region VARS
        //*****VARS**********VARS**********VARS**********VARS**********VARS**********VARS**********VARS**********VARS**********VARS*****

        public bool weWon { get; set; }
        public List<string> AList { get; private set; }
        public bool AISPresent { get; private set; }
        public int JokerColorPosition { get; set; }
        public bool blinkOn = true;
        public int holdDelay { get; private set; } = 275;//change time here for the autohold signs delay
        public int beforeHold { get; private set; } = 120;//change time here before the autohold engages
        public int JokerIndexPosition { get; set; }
        public bool jwon { get; set; }
        public bool _OnePair { get; set; }
        public bool _TwoPair { get; set; }
        public bool _ThreeOfAKind { get; set; }
        public bool _Straight { get; set; }
        public bool _Flush { get; set; }
        public bool _FourOfAKind { get; set; }
        public bool _StraightFlush { get; set; }
        public bool _FullHouse { get; set; }
        public List<int> JokeList { get; set; }
        public List<string> JokeListCol { get; set; }
        public bool JokerExists { get; set; }
        public bool isAllStraight { get; private set; }
        public bool isInSequence1 { get; private set; }
        public bool isInSequence2 { get; private set; }
        public bool isInSequence3 { get; private set; }
        public bool isInSequence4 { get; private set; }
        public bool isInSequence5 { get; private set; }
        public bool _FiveOfAKind { get; private set; }
        public int AcePosition { get; private set; }
        public int AceColor { get; private set; }
        public bool areSameColorsWOJ { get; private set; }
        public bool isStraightFush { get; private set; }
        public int APos { get; private set; }
        public string ColorOfA { get; private set; }
        public List<int> LiveList { get; set; }
        public List<string> ListWithOutJoke { get; set; }
        CardDefinitions cardDefinitions = new CardDefinitions();


        //*****VARS**********VARS**********VARS**********VARS**********VARS**********VARS**********VARS**********VARS**********VARS*****
        #endregion VARS


        private List<int> kindOfCards()
        {
            List<int> kindOfCards = new List<int>();
            kindOfCards.Clear();
            kindOfCards.Add(cardDefinitions.ReturnCardKind(mainWindow.c1));
            kindOfCards.Add(cardDefinitions.ReturnCardKind(mainWindow.c2));
            kindOfCards.Add(cardDefinitions.ReturnCardKind(mainWindow.c3));
            kindOfCards.Add(cardDefinitions.ReturnCardKind(mainWindow.c4));
            kindOfCards.Add(cardDefinitions.ReturnCardKind(mainWindow.c5));
            return kindOfCards;
        }
        private List<string> colorOfCards()
        {
            List<string> colorOfCards = new List<string>();
            colorOfCards.Clear();
            colorOfCards.Add(cardDefinitions.ReturnCardColor(mainWindow.c1));
            colorOfCards.Add(cardDefinitions.ReturnCardColor(mainWindow.c2));
            colorOfCards.Add(cardDefinitions.ReturnCardColor(mainWindow.c3));
            colorOfCards.Add(cardDefinitions.ReturnCardColor(mainWindow.c4));
            colorOfCards.Add(cardDefinitions.ReturnCardColor(mainWindow.c5));
            return colorOfCards;
        }
        public List<string> TypeOfCards()
        {
            List<string> typeOfCards = new List<string>();
            typeOfCards.Clear();
            typeOfCards.Add(cardDefinitions.ReturnCardType(mainWindow.c1));
            typeOfCards.Add(cardDefinitions.ReturnCardType(mainWindow.c2));
            typeOfCards.Add(cardDefinitions.ReturnCardType(mainWindow.c3));
            typeOfCards.Add(cardDefinitions.ReturnCardType(mainWindow.c4));
            typeOfCards.Add(cardDefinitions.ReturnCardType(mainWindow.c5));
            return typeOfCards;
        }
        //Method that enebles or disables the Draw Button
        private void DrawButton(bool b)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            if (b == false)
            {
                main.Draw.IsEnabled = false;
            }
            else
            {
                main.Draw.IsEnabled = true;
            }
        }
        #region Methods that Access the Main Window
        // These methods provide link to the main window and Play Sounds
        public static void HoldSign1ImageAndSound()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(mainWindow))
                {
                    (window as mainWindow).hold_sign.Source = new BitmapImage(new Uri(@"Resources\hold.png", UriKind.Relative));
                    SoundPlayer soundPlayer = new SoundPlayer();
                    soundPlayer = new SoundPlayer(Properties.Resources.shold1);
                    soundPlayer.Play();
                }
            }
        }
        public static void HoldSign2ImageAndSound()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(mainWindow))
                {
                    (window as mainWindow).hold_sign2.Source = new BitmapImage(new Uri(@"Resources\hold.png", UriKind.Relative));
                    SoundPlayer soundPlayer = new SoundPlayer();
                    soundPlayer = new SoundPlayer(Properties.Resources.shold2);
                    soundPlayer.Play();
                }
            }
        }
        public static void HoldSign3ImageAndSound()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(mainWindow))
                {
                    (window as mainWindow).hold_sign3.Source = new BitmapImage(new Uri(@"Resources\hold.png", UriKind.Relative));
                    SoundPlayer soundPlayer = new SoundPlayer();
                    soundPlayer = new SoundPlayer(Properties.Resources.shold3);
                    soundPlayer.Play();
                }
            }
        }
        public static void HoldSign4ImageAndSound()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(mainWindow))
                {
                    (window as mainWindow).hold_sign4.Source = new BitmapImage(new Uri(@"Resources\hold.png", UriKind.Relative));
                    SoundPlayer soundPlayer = new SoundPlayer();
                    soundPlayer = new SoundPlayer(Properties.Resources.shold4);
                    soundPlayer.Play();
                }
            }
        }
        public static void HoldSign5ImageAndSound()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(mainWindow))
                {
                    (window as mainWindow).hold_sign5.Source = new BitmapImage(new Uri(@"Resources\hold.png", UriKind.Relative));
                    SoundPlayer soundPlayer = new SoundPlayer();
                    soundPlayer = new SoundPlayer(Properties.Resources.shold5);
                    soundPlayer.Play();
                }
            }
        }
        #endregion Methods that Access the Main Window and Play Hold Sounds
        //Switch Ace Method
        private void SwitchAce()
        {
            if (TypeOfCards().Contains("ace"))
            {
                //If the Joker is NOT Present
                if (!TypeOfCards().Contains("jk"))
                {
                    if (
                       TypeOfCards().Contains("two") && TypeOfCards().Contains("three") && TypeOfCards().Contains("four") && TypeOfCards().Contains("five") ||
                       TypeOfCards().Contains("two") && TypeOfCards().Contains("three") && TypeOfCards().Contains("four")
                       )
                    {
                        for (int a = 0; a < TypeOfCards().Count(); a++)//Beware Count() Starts from 1 not 0
                        {
                            if (TypeOfCards()[a] == "ace")
                            {
                                JokeList[a] = 1;
                            }
                        }
                    }
                    else
                    {
                        for (int all = 0; all < TypeOfCards().Count(); all++)
                        {
                            if (TypeOfCards()[all] == "ace")
                            {
                                JokeList[all] = 14;
                            }
                        }
                    }
                }
                //If the Joker IS Present
                if (TypeOfCards().Contains("jk"))
                {
                    if (
                       (TypeOfCards().Contains("two") && TypeOfCards().Contains("three")) ||
                       (TypeOfCards().Contains("two") && TypeOfCards().Contains("four")) ||
                       (TypeOfCards().Contains("three") && TypeOfCards().Contains("four")) ||
                       (TypeOfCards().Contains("three") && TypeOfCards().Contains("four") && TypeOfCards().Contains("five")) ||
                       (TypeOfCards().Contains("two") && TypeOfCards().Contains("three") && TypeOfCards().Contains("four")) ||
                       (TypeOfCards().Contains("two") && TypeOfCards().Contains("three") && TypeOfCards().Contains("five"))
                       )
                    {
                        for (int tw = 0; tw < TypeOfCards().Count(); tw++)
                        {
                            if (TypeOfCards()[tw] == "ace")
                            {
                                JokeList[tw] = 1;
                            }
                        }
                    }
                    else
                    {
                        for (int all = 0; all < TypeOfCards().Count(); all++)
                        {
                            if (TypeOfCards()[all] == "ace")
                            {
                                JokeList[all] = 14;
                            }
                        }
                    }
                }
            }
            else
            {
                return;
            }  
        }
        //Returns true is cards are the same color without the joker
        private bool AreSameColorsWithoutJoker()
        {
            var ListWOJoke = JokeListCol.Where(j => j != "joker").ToList();
            if (ListWOJoke.TrueForAll(i => i.Equals(ListWOJoke.FirstOrDefault())))
            {
                areSameColorsWOJ = true;
            }
            else
            {
                areSameColorsWOJ = false;
            }

            return areSameColorsWOJ;
        }
        #region Joker
        //<---------------------------------------Joker Switch Starts Here--------------------------------------->
        public void JokerSwitch()
        {
            _FiveOfAKind = false;
            _StraightFlush = false;
            _FourOfAKind = false;
            _FullHouse = false;
            _Flush = false;
            _Straight = false;
            _ThreeOfAKind = false;
            _TwoPair = false;
            _OnePair = false;
            weWon = false;
            JokeListCol = colorOfCards();
            JokeList = kindOfCards();
            JokerIndexPosition = JokeList.IndexOf(0);//Return the index position of the joker
            JokerColorPosition = JokeListCol.IndexOf("joker");
            ListWithOutJoke = new List<string>(JokeListCol.Where(j => j != "joker"));
            SwitchAce();
            if (JokeListCol.Contains("joker"))
            {
                JokerExists = true;
            }
            else if(!JokeListCol.Contains("joker"))
            {
                JokerExists = false;
            }
            if (JokerExists == true)
            {
                RunFiveOfAKind();
            }
            else
            {
                RunFiveOfAKindWOJ();
            }
        }
        #endregion Joker
        //5 Of A Kind With Joker
        private void RunFiveOfAKind()
        {
            for (int t = 1; t <= 14; t++)
            {
                JokeList[JokerColorPosition] = t;
                CheckFiveOfAKind();
                if (_FiveOfAKind == true && JokeList[JokerColorPosition] <= 14)
                {
                    return;
                }
                else if (_FiveOfAKind == false && JokeList[JokerColorPosition] == 14)
                {
                    RunRoyalFlush();
                    return;
                }
            }
        }
        //Straight Flush or Royal Flush With Joker
        private void RunRoyalFlush()
        {
            for (int s = 1; s <= 14; s++)
            {
                JokeList[JokerColorPosition] = s;
                JokeListCol[JokerColorPosition] = ListWithOutJoke[0];
                CheckStraightFlush();
                if (_StraightFlush == true && JokeList[JokerColorPosition] <= 14)
                {
                    return;
                }
                else if( _StraightFlush == false && JokeList[JokerColorPosition] == 14 )
                {
                    RunFourOFAKind();
                    return;
                }
            }
        }
        //Four of a Kind with Joker
        private void RunFourOFAKind()
        {
            for (int p = 1; p <= 14; p++)
            {
                CheckFourOfAKind();
                if (_FourOfAKind == true && JokeList[JokerColorPosition] <= 14)
                {
                    return;
                }
                else if(JokeList[JokerColorPosition] == 14 && _FourOfAKind == false)
                {
                    RunFullHouse();
                    return;
                }
            }
        }
        //Full House with Joker
        private void RunFullHouse()
        {
            for (int h = 14; h >= 1; h--)
            {
                JokeList[JokerColorPosition] = h;
                CheckFullHouse();
                if (_FullHouse == true  && JokeList[JokerColorPosition] >= 1)
                {
                    return;
                }
                else if(JokeList[JokerColorPosition] == 1 && _FullHouse == false)
                {
                    JokeList[JokerColorPosition] = 0;
                    RunFlush();
                    return;
                }
            }
        }
        //Flush with Joker
        private void RunFlush()
        {
            for (int c = 1; c <= 5; c++)
            {
                if (c == 1)
                {
                    JokeListCol[JokerIndexPosition] = "hearts";
                    CheckFlush();
                    if (_Flush == true)
                    {
                        return;
                    }
                }
                else if (c == 2)
                {
                    JokeListCol[JokerIndexPosition] = "diamonds";
                    CheckFlush();
                    if (_Flush == true)
                    {
                        return;
                    }
                }
                else if (c == 3)
                {
                    JokeListCol[JokerIndexPosition] = "spades";
                    CheckFlush();
                    if (_Flush == true)
                    {
                        return;
                    }
                }
                else if (c == 4)
                {
                    JokeListCol[JokerIndexPosition] = "clubs";
                    CheckFlush();
                    if (_Flush == true)
                    {
                        return;
                    }
                }
                else if(c == 5)
                {
                    RunThreeOfAKind();
                    return;
                }
            }
        }
        //Three of a Kind with Joker
        private void RunThreeOfAKind()
        {
            for (int pt = 14; pt >= 1; pt--)
            {
                JokeList[JokerColorPosition] = pt;
                CheckThreeOfAKind();
                if (_ThreeOfAKind == true && JokeList[JokerColorPosition] >= 1)
                {
                    return; 
                }
                else if (JokeList[JokerColorPosition] == 1 && _ThreeOfAKind == false)
                {
                    JokeList[JokerColorPosition] = 0;
                    RunStraight();
                    return;
                }
            }
        }
        //Straight with Joker
        private void RunStraight()
        {
            for (int str = 14; str >= 1; str--)
            {
                JokeList[JokerColorPosition] = str;
                CheckStraight();
                if (_Straight == true && JokeList[JokerColorPosition] >= 1)
                {
                    return;
                }
                else if (JokeList[JokerColorPosition] == 1 && _Straight == false)
                {
                    RunTwoPair();
                    return;
                }
            }
        }
        private void RunTwoPair()
        {
            for (int tp = 14; tp >= 1; tp--)
            {
                JokeList[JokerColorPosition] = tp;
                CheckTwoPair();
                if (_TwoPair == true && JokeList[JokerColorPosition] >= 1)
                {
                    return;
                }
                if (JokeList[JokerColorPosition] == 1 && _TwoPair == false)
                {
                    RunSinglePair();
                    return;
                }
            }
        }
        //Single Pair With Joker
        private void RunSinglePair()
        {
            for (int j = 14; j >=1; j--)
            {
                JokeList[JokerColorPosition] = j;
                CheckSinglePair();
                if (_OnePair == true && JokeList[JokerColorPosition] >= 1)
                {
                    return;
                }
                if (JokeList[JokerColorPosition] == 1 && _OnePair == false)
                {
                    return;
                }
            }
        }

        //Start Checking without Joker --------------------->
        //Five of a Kind Without Joker
        private void RunFiveOfAKindWOJ()
        {
            CheckFiveOfAKind();
            if (_FiveOfAKind == true)
            {
                return;
            }
            else if (_FiveOfAKind == false)
            {
                RunStraightFlushWOJ();
                return;
            }
        }
        //Royal/Staright Flush Without Joker
        private void RunStraightFlushWOJ()
        {
            CheckStraightFlush();
            if (_StraightFlush == true)
            {
                return;
            }
            else if (_StraightFlush == false)
            {
                RunFourOfAKindWOJ();
                return;
            }
        }
        //4 of a Kind Without Joker
        private void RunFourOfAKindWOJ()
        {
            CheckFourOfAKind();
            if (_FourOfAKind == true)
            {
                return;
            }
            else if (_FourOfAKind == false)
            {
                RunFullHouseWOJ();
                return;
            }
        }
        //Full House Without Joker
        private void RunFullHouseWOJ()
        {
            CheckFullHouse();
            if (_FullHouse == true)
            {
                return;
            }
            else if (_FullHouse == false)
            {
                RunFlushWOJ();
                return;
            }
        }
        //Flush Without Joker
        private void RunFlushWOJ()
        {
            CheckFlush();
            if (_Flush == true)
            {
                return;
            }
            else if (_Flush == false)
            {
                RunThreeOfAKindWOJ();
                return;
            }
        }
        //3 of a Kind Without Joker
        private void RunThreeOfAKindWOJ()
        {
            CheckThreeOfAKind();
            if (_ThreeOfAKind == true)
            {
                return;
            }
            else if (_ThreeOfAKind == false)
            {
                RunStraightWOJ();
                return;
            }
        }
        //Straight Without Joker
        private void RunStraightWOJ()
        {
            CheckStraight();
            if (_Straight == true)
            {
                return;
            }
            else if (_Straight == false)
            {
                RunTwoPairWOJ();
                return;
            }
        }
        //SinglePair Without Joker
        private void RunTwoPairWOJ()
        {
            CheckTwoPair();
            if (_TwoPair == true)
            {
                return;
            }
            else if (_TwoPair == false)
            {
                RunSinglePairWOJ();
                return;
            }
        }
        //SinglePair Without Joker
        private void RunSinglePairWOJ()
        {
            CheckSinglePair();
            if (_OnePair == true)
            {
                return;
            }
            else if (_OnePair == false)
            {
                return;
            }
        }
        #region Posibilities
        //<---------------------------------------5 Of A Kind Function--------------------------------------->
        private async void CheckFiveOfAKind()
        {
            if (JokeList[0] == JokeList[1] && JokeList[1] == JokeList[2] && JokeList[2] == JokeList[3] && JokeList[3] == JokeList[4] && _FiveOfAKind == false)
            {
                weWon = true;
                _FiveOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            {
                //Exit 5 of a kind if false
                _FiveOfAKind = false;
                return;
            }
        }
        //<---------------------------------------Straight Flush Function--------------------------------------->
        private async void CheckStraightFlush()
        {

            List<int> StraightFushCards = new List<int>(); //List Full All 5 Cards Straight
            StraightFushCards.Clear();
            StraightFushCards.Add(JokeList[0]);
            StraightFushCards.Add(JokeList[1]);
            StraightFushCards.Add(JokeList[2]);
            StraightFushCards.Add(JokeList[3]);
            StraightFushCards.Add(JokeList[4]);
            StraightFushCards.Sort();
            isStraightFush = StraightFushCards.SequenceEqual(Enumerable.Range(StraightFushCards.First(), StraightFushCards.Count()));

            if (areSameColorsWOJ && isStraightFush && _StraightFlush == false)
            {
                //Check for a Straight Flush
                weWon = true;
                _StraightFlush = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else
            {
                //Exit if false
                _StraightFlush = false;
                return;
            }
        }
        //<---------------------------------------Four Of A Kind Function--------------------------------------->
        private async void CheckFourOfAKind()
        {
            //START Checking for 4 of a kind
            // 4 of a kind 1 2 3 4
            if (JokeList[0] == JokeList[1] && JokeList[1] == JokeList[2] && JokeList[2] == JokeList[3] && _FourOfAKind == false)
            {
                weWon = true;
                _FourOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                DrawButton(true);
                return;
            }
            // 4 of a kind 2 3 4 5
            else if (JokeList[1] == JokeList[2] && JokeList[2] == JokeList[3] && JokeList[3] == JokeList[4] && _FourOfAKind == false)
            {
                weWon = true;
                _FourOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 4 of a kind 1 2 4 5
            else if (JokeList[0] == JokeList[1] && JokeList[1] == JokeList[3] && JokeList[3] == JokeList[4] && _FourOfAKind == false)
            {
                weWon = true;
                _FourOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 4 of a kind 1 2 3 5
            else if (JokeList[0] == JokeList[1] && JokeList[1] == JokeList[2] && JokeList[2] == JokeList[4] && _FourOfAKind == false)
            {
                weWon = true;
                _FourOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 4 of a kind 1 3 4 5
            else if (JokeList[0] == JokeList[2] && JokeList[2] == JokeList[3] && JokeList[3] == JokeList[4] && _FourOfAKind == false)
            {
                weWon = true;
                _FourOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else
            {
                //Exit Four Of A Kind Function if false
                _FourOfAKind = false;
                return;
            }
        }
        //<---------------------------------------Full House Function--------------------------------------->
        private async void CheckFullHouse()
        {
            //Start checking for a Full House
            // Full House 1,2,3 + 4,5
            if ((JokeList[0] == JokeList[1] && JokeList[1] == JokeList[2]) &&
                (JokeList[3] == JokeList[4]) && _FullHouse == false)
            {
                weWon = true;
                _FullHouse = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // Full House 1,2,3 + 4,5
            else if ((JokeList[0] == JokeList[1] && JokeList[1] == JokeList[2]) &&
                     (JokeList[3] == JokeList[4]) && _FullHouse == false)
            {
                weWon = true;
                _FullHouse = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // Full House 1,2,4 + 3,5
            else if ((JokeList[0] == JokeList[1] && JokeList[1] == JokeList[3]) &&
                (JokeList[2] == JokeList[4]) && _FullHouse == false)
            {
                weWon = true;
                _FullHouse = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // Full House 1,2,5 + 3,4
            else if ((JokeList[0] == JokeList[1] && JokeList[1] == JokeList[4]) &&
                (JokeList[2] == JokeList[3]) && _FullHouse == false)
            {
                _FullHouse = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // Full House 1,4,5 + 2,3
            else if ((JokeList[0] == JokeList[3] && JokeList[3] == JokeList[4]) &&
                (JokeList[1] == JokeList[2]) && weWon == false)
            {
                _FullHouse = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // Full House 2,3,4 + 1,5
            else if ((JokeList[1] == JokeList[2] && JokeList[2] == JokeList[3]) &&
                (JokeList[0] == JokeList[4]) && _FullHouse == false)
            {
                weWon = true;
                _FullHouse = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // Full House 2,3,5 + 1,4
            else if ((JokeList[1] == JokeList[2] && JokeList[2] == JokeList[4]) &&
                (JokeList[0] == JokeList[3]) && _FullHouse == false)
            {
                weWon = true;
                _FullHouse = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // Full House 2,4,5 + 1,3
            else if ((JokeList[1] == JokeList[3] && JokeList[3] == JokeList[4]) &&
                (JokeList[0] == JokeList[2]) && _FullHouse == false)
            {
                weWon = true;
                _FullHouse = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // Full House 3,4,5 + 1,2
            else if ((JokeList[2] == JokeList[3] && JokeList[3] == JokeList[4]) &&
                (JokeList[0] == JokeList[1]) && _FullHouse == false)
            {
                weWon = true;
                _FullHouse = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // Full House 1,3,5 + 2,4
            else if ((JokeList[0] == JokeList[2] && JokeList[2] == JokeList[4]) &&
                (JokeList[1] == JokeList[3]) && _FullHouse == false)
            {
                weWon = true;
                _FullHouse = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // Full House 1,3,4 + 2,5
            else if ((JokeList[0] == JokeList[2] && JokeList[2] == JokeList[3]) &&
                (JokeList[1] == JokeList[4]) && _FullHouse == false)
            {
                weWon = true;
                _FullHouse = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else
            {
                //Exit FullHouse Function if False
                _FullHouse = false;
                return;
            }
        }
        //<---------------------------------------Flush Function--------------------------------------->

        private async void CheckFlush()
        {
            //START Checking for a Flush
            //Checking for a Flush 1,2,3,4,5 
            //Check first for Full Full Flush                                  
            if (JokeListCol[0] == JokeListCol[1] && JokeListCol[1] == JokeListCol[2] && JokeListCol[2] == JokeListCol[3] && JokeListCol[3] == JokeListCol[4] && _Flush == false)
            {
                weWon = true;
                _Flush = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            //Checking for a Flush 1,2,3,4 
            else if (JokeListCol[0] == JokeListCol[1] && JokeListCol[1] == JokeListCol[2] && JokeListCol[2] == JokeListCol[3] && _Flush == false)
            {
                weWon = true;
                _Flush = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                DrawButton(true);
                return;
            }
            //Checking for a Flush 2,3,4,5 
            else if (JokeListCol[1] == JokeListCol[2] && JokeListCol[2] == JokeListCol[3] && JokeListCol[3] == JokeListCol[4] && _Flush == false)
            {
                weWon = true;
                _Flush = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            //Checking for a Flush 1,3,4,5 
            else if (JokeListCol[0] == JokeListCol[2] && JokeListCol[2] == JokeListCol[3] && JokeListCol[3] == JokeListCol[4] && _Flush == false)
            {
                weWon = true;
                _Flush = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            //Checking for a Flush 1,2,3,5 
            else if (JokeListCol[0] == JokeListCol[1] && JokeListCol[1] == JokeListCol[2] && JokeListCol[2] == JokeListCol[4] && _Flush == false)
            {
                weWon = true;
                _Flush = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            //Checking for a Flush 1,2,4,5 
            else if (JokeListCol[0] == JokeListCol[1] && JokeListCol[1] == JokeListCol[3] && JokeListCol[3] == JokeListCol[4] && _Flush == false)
            {
                weWon = true;
                _Flush = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else
            {
                //Exit Flush Function if not true
                _Flush = false;
                return;
            }
            //END Checking for a Flush
        }
        //<---------------------------------------3 Of A Kind Function--------------------------------------->
        private async void CheckThreeOfAKind()
        {
            //Start Checking for 3 of a Kind
            if (JokeList[0] == JokeList[2] && JokeList[2] == JokeList[3] && _ThreeOfAKind == false)// 3 of a kind 1,3,4 
            {
                weWon = true;
                _ThreeOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[0] == JokeList[2] && JokeList[2] == JokeList[4] && _ThreeOfAKind == false)// 3 of a kind 1,3,5
            {
                weWon = true;
                _ThreeOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[2] == JokeList[3] && JokeList[3] == JokeList[4] && _ThreeOfAKind == false)// 3 of a kind 3,4,5
            {
                weWon = true;
                _ThreeOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[1] == JokeList[3] && JokeList[3] == JokeList[4] && _ThreeOfAKind == false)// 3 of a kind 2,4,5
            {
                weWon = true;
                _ThreeOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[1] == JokeList[2] && JokeList[2] == JokeList[4] && _ThreeOfAKind == false)// 3 of a kind 2,3,5
            {
                weWon = true;
                _ThreeOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[1] == JokeList[2] && JokeList[2] == JokeList[3] && _ThreeOfAKind == false)// 3 of a kind 2,3,4
            {
                weWon = true;
                _ThreeOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[0] == JokeList[3] && JokeList[3] == JokeList[4] && _ThreeOfAKind == false)// 3 of a kind 1,4,5
            {
                weWon = true;
                _ThreeOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[0] == JokeList[1] && JokeList[1] == JokeList[4] && _ThreeOfAKind == false)// 3 of a kind 1,2,5
            {
                weWon = true;
                _ThreeOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[0] == JokeList[1] && JokeList[1] == JokeList[3] && _ThreeOfAKind == false)// 3 of a kind 1,2,4
            {
                weWon = true;
                _ThreeOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[0] == JokeList[1] && JokeList[1] == JokeList[2] && _ThreeOfAKind == false)// 3 of a kind 1,2,3
            {
                weWon = true;
                _ThreeOfAKind = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                DrawButton(true);
                return;
            }
            else
            {
                //Exit 3 of a Kind Function if not true
                _ThreeOfAKind = false;
                return;
            }
        }
        //<---------------------------------------Straight Function--------------------------------------->
        private async void CheckStraight()
        {
            isAllStraight = false;
            isInSequence1 = false;
            isInSequence2 = false;
            isInSequence3 = false;
            isInSequence4 = false;
            isInSequence5 = false;
            #region Lists For Straights
            //START Definitions for Straight
            List<int> newKindOfCards12345 = new List<int>(); //List Full All 5 Cards Straight
            newKindOfCards12345.Clear();
            newKindOfCards12345.Add(JokeList[0]);
            newKindOfCards12345.Add(JokeList[1]);
            newKindOfCards12345.Add(JokeList[2]);
            newKindOfCards12345.Add(JokeList[3]);
            newKindOfCards12345.Add(JokeList[4]);
            newKindOfCards12345.Sort();
            isAllStraight = newKindOfCards12345.SequenceEqual(Enumerable.Range(newKindOfCards12345[0], newKindOfCards12345.Count()));

            List<int> newKindOfCards1234 = new List<int>();//List 1,2,3,4 Straight
            newKindOfCards1234.Clear();
            newKindOfCards1234.Add(JokeList[0]);
            newKindOfCards1234.Add(JokeList[1]);
            newKindOfCards1234.Add(JokeList[2]);
            newKindOfCards1234.Add(JokeList[3]);
            newKindOfCards1234.Sort();
            isInSequence1 = newKindOfCards1234.SequenceEqual(Enumerable.Range(newKindOfCards1234[0], newKindOfCards1234.Count()));

            List<int> newKindOfCards2345 = new List<int>();//List 2,3,4,5 Straight
            newKindOfCards2345.Clear();
            newKindOfCards2345.Add(JokeList[1]);
            newKindOfCards2345.Add(JokeList[2]);
            newKindOfCards2345.Add(JokeList[3]);
            newKindOfCards2345.Add(JokeList[4]);
            newKindOfCards2345.Sort();
            isInSequence2 = newKindOfCards2345.SequenceEqual(Enumerable.Range(newKindOfCards2345[0], newKindOfCards2345.Count()));

            List<int> newKindOfCards1345 = new List<int>();//List 1,3,4,5 Straight
            newKindOfCards1345.Clear();
            newKindOfCards1345.Add(JokeList[0]);
            newKindOfCards1345.Add(JokeList[2]);
            newKindOfCards1345.Add(JokeList[3]);
            newKindOfCards1345.Add(JokeList[4]);
            newKindOfCards1345.Sort();
            isInSequence3 = newKindOfCards1345.SequenceEqual(Enumerable.Range(newKindOfCards1345[0], newKindOfCards1345.Count()));

            List<int> newKindOfCards1245 = new List<int>();//List 1,2,4,5 Straight
            newKindOfCards1245.Clear();
            newKindOfCards1245.Add(JokeList[0]);
            newKindOfCards1245.Add(JokeList[1]);
            newKindOfCards1245.Add(JokeList[3]);
            newKindOfCards1245.Add(JokeList[4]);
            newKindOfCards1245.Sort();
            isInSequence4 = newKindOfCards1245.SequenceEqual(Enumerable.Range(newKindOfCards1245[0], newKindOfCards1245.Count()));

            List<int> newKindOfCards1235 = new List<int>(); //List 1,2,3,5 Straight
            newKindOfCards1235.Clear();
            newKindOfCards1235.Add(JokeList[0]);
            newKindOfCards1235.Add(JokeList[1]);
            newKindOfCards1235.Add(JokeList[2]);
            newKindOfCards1235.Add(JokeList[4]);
            newKindOfCards1235.Sort();
            isInSequence5 = newKindOfCards1235.SequenceEqual(Enumerable.Range(newKindOfCards1235[0], newKindOfCards1235.Count()));
            //END Definitions for Straight
            #endregion END Straights Lists Region


            //Start Checking for Straight
            //Check for All Straight
            if (isAllStraight == true && _Straight == false)
            {
                weWon = true;
                _Straight = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            //Check 1,2,3,4 for a Straight 
            else if (isInSequence1 == true && _Straight == false)
            {
                weWon = true;
                _Straight = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                DrawButton(true);
                return;
            }
            //Check 2,3,4,5 for a Straight 
            else if (isInSequence2 == true && _Straight == false)
            {
                weWon = true;
                _Straight = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            //Check 1,3,4,5 for a Straight 
            else if (isInSequence3 == true && _Straight == false)
            {
                weWon = true;
                _Straight = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            //Check 1,2,4,5 for a Straight 
            else if (isInSequence4 == true && _Straight == false)
            {
                weWon = true;
                _Straight = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            //Check 1,2,3,5 for a Straight 
            else if (isInSequence5 == true && _Straight == false)
            {
                weWon = true;
                _Straight = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else
            {
                //Exit Straight Function if not true
                _Straight = false;
                return;
            }
        }

        //<---------------------------------------2 Pair Function --------------------------------------->
        private async void CheckTwoPair()
        // 2 Pairs 1 to 4, 3 to 5
        {
            if (JokeList[0] == JokeList[3] && JokeList[2] == JokeList[4] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 2 to 4, 3 to 5
            else if (JokeList[1] == JokeList[3] && JokeList[2] == JokeList[4] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 2 to 5, 3 to 4
            else if (JokeList[1] == JokeList[4] && JokeList[2] == JokeList[3] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 2 to 3, 4 to 5
            else if (JokeList[1] == JokeList[2] && JokeList[3] == JokeList[4] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 1 to 3, 4 to 5
            else if (JokeList[0] == JokeList[2] && JokeList[3] == JokeList[4] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 1 to 2, 4 to 5
            else if (JokeList[0] == JokeList[1] && JokeList[3] == JokeList[4] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 1 to 2, 3 to 4
            else if (JokeList[0] == JokeList[1] && JokeList[2] == JokeList[3] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 1 to 2, 3 to 5
            else if (JokeList[0] == JokeList[1] && JokeList[2] == JokeList[4] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 1 to 4, 2 to 3
            else if (JokeList[0] == JokeList[3] && JokeList[1] == JokeList[2] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 1 to 5, 3 to 4
            else if (JokeList[0] == JokeList[4] && JokeList[2] == JokeList[3] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 1 to 3, 2 to 4
            else if (JokeList[0] == JokeList[2] && JokeList[1] == JokeList[3] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 1 to 3, 2 to 5
            else if (JokeList[0] == JokeList[2] && JokeList[1] == JokeList[4] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 1 to 5, 2 to 3
            else if (JokeList[0] == JokeList[4] && JokeList[1] == JokeList[2] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 1 to 5, 2 to 4
            else if (JokeList[0] == JokeList[4] && JokeList[1] == JokeList[3] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            // 2 Pairs 1 to 4, 2 to 5
            else if (JokeList[0] == JokeList[3] && JokeList[1] == JokeList[4] && _TwoPair == false)
            {
                weWon = true;
                _TwoPair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else
            {
                //Exit 2 Pair Function if not true
                _TwoPair = false;
                return;
            }
        }
        //<---------------------------------------Single Pair Function --------------------------------------->
        private async void CheckSinglePair()
        {

            // Start Checking for Single Pairs
            if (JokeList[0] == JokeList[1] && _TwoPair == false && _OnePair == false)// Pair 1,2
            {
                weWon = true;
                _OnePair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign2ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[1] == JokeList[2] && _TwoPair == false && _OnePair == false)// Pair 2,3
            {
                weWon = true;
                _OnePair = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[2] == JokeList[3] && _TwoPair == false && _OnePair == false)// Pair 3, 4
            {
                weWon = true;
                _OnePair = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[3] == JokeList[4] && _TwoPair == false && _OnePair == false)// Pair 4, 5
            {
                weWon = true;
                _OnePair = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign4ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[1] == JokeList[3] && _TwoPair == false && _OnePair == false)//Pair 2, 4
            {
                weWon = true;
                _OnePair = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[1] == JokeList[4] && _TwoPair == false && _OnePair == false)//Pair 2, 5
            {
                weWon = true;
                _OnePair = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign2ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[2] == JokeList[4] && _TwoPair == false && _OnePair == false)// Pair 3, 5
            {
                weWon = true;
                _OnePair = true;
                DrawButton(false);
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign3ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[0] == JokeList[2] && _TwoPair == false && _OnePair == false)// Pair 1, 3
            {
                weWon = true;
                _OnePair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign3ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[0] == JokeList[3] && _TwoPair == false && _OnePair == false)// Pair 1, 4
            {
                weWon = true;
                _OnePair = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign4ImageAndSound();
                DrawButton(true);
                return;
            }
            else if (JokeList[0] == JokeList[4] && _TwoPair == false && _OnePair == false)// Pair 1, 5
            {
                _OnePair = true;
                weWon = true;
                DrawButton(false);
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                await Task.Delay(beforeHold);
                HoldSign1ImageAndSound();
                await Task.Delay(holdDelay);
                HoldSign5ImageAndSound();
                DrawButton(true);
                return;
            }
            else
            {
                jwon = false;
                _FiveOfAKind = false;
                _StraightFlush = false;
                _FourOfAKind = false;
                _FullHouse = false;
                _Flush = false;
                _Straight = false;
                _ThreeOfAKind = false;
                _TwoPair = false;
                _OnePair = false;
                weWon = false;
                return;
            }
        }
        #endregion END Possibilities
    }

}

