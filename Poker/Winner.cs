namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    internal class Winner
    {
        //*************VARS & PROPERTIES***************
        /// Defines the cardDefinitions2
        internal CardDefinitions cardDefinitions2 = new CardDefinitions();
        /// Defines the holdCards
        internal HoldCards holdCards = new HoldCards();
        Helper helper = new Helper();
        public bool JokerWins { get; set; } = false;
        public bool TrueWinner { get; set; }
        public bool TwoPair { get; set; }
        public bool OnePair { get; set; }
        public bool ThreeOfAKind { get; set; }
        public bool Card1Won { get; set; }
        public bool Card2Won { get; set; }
        public bool Card3Won { get; set; }
        public bool Card4Won { get; set; }
        public bool Card5Won { get; set; }
        public bool Straight { get; set; } = false;
        public bool Flush { get; private set; }
        public bool FullHouse { get; private set; }
        public bool FourOfAKind { get; private set; }
        public bool StraightFlush { get; private set; }
        public bool RoyalFlush { get; private set; }
        public bool FiveOfAKind { get; private set; }
        public bool sWon { get; private set; } = false;
        public int JPosCol { get; private set; }
        public int JPos { get; private set; }
        public int AcePos { get; private set; }
        public string ColorOfAce { get; private set; }
        public bool JokerIsPresent { get; set; }
        List<int> straightList = new List<int>();
        //*************END VARS & PROPERTIES***************

        // The WinningCards
        private List<int> WinningCards()
        {
            List<int> WinningCards = new List<int>();
            WinningCards.Clear();
            WinningCards.Add(cardDefinitions2.ReturnCardKind(mainWindow.c1));
            WinningCards.Add(cardDefinitions2.ReturnCardKind(mainWindow.c2));
            WinningCards.Add(cardDefinitions2.ReturnCardKind(mainWindow.c3));
            WinningCards.Add(cardDefinitions2.ReturnCardKind(mainWindow.c4));
            WinningCards.Add(cardDefinitions2.ReturnCardKind(mainWindow.c5));
            return WinningCards;
        }
        private List<string> WinningCardsColors()
        {
            List<string> WinningCardsColors = new List<string>();
            WinningCardsColors.Clear();
            WinningCardsColors.Add(cardDefinitions2.ReturnCardColor(mainWindow.c1));
            WinningCardsColors.Add(cardDefinitions2.ReturnCardColor(mainWindow.c2));
            WinningCardsColors.Add(cardDefinitions2.ReturnCardColor(mainWindow.c3));
            WinningCardsColors.Add(cardDefinitions2.ReturnCardColor(mainWindow.c4));
            WinningCardsColors.Add(cardDefinitions2.ReturnCardColor(mainWindow.c5));
            return WinningCardsColors;
        }
        private List<string> WinningCardsTypes()
        {
            List<string> WinningCardsTypes = new List<string>();
            WinningCardsTypes.Clear();
            WinningCardsTypes.Add(cardDefinitions2.ReturnCardType(mainWindow.c1));
            WinningCardsTypes.Add(cardDefinitions2.ReturnCardType(mainWindow.c2));
            WinningCardsTypes.Add(cardDefinitions2.ReturnCardType(mainWindow.c3));
            WinningCardsTypes.Add(cardDefinitions2.ReturnCardType(mainWindow.c4));
            WinningCardsTypes.Add(cardDefinitions2.ReturnCardType(mainWindow.c5));
            return WinningCardsTypes;
        }
        public List<int> JokerList { get; set; }
        public List<string> JokerListColors { get; set; }
        public bool isStraightWOJ { get; private set; }

        //Returns Cards in JokerList
        private int Wcard(int returner)
        {
            if (returner == 1)
            {
                return JokerList[0];
            }
            if (returner == 2)
            {
                return JokerList[1];
            }
            if (returner == 3)
            {
                return JokerList[2];
            }
            if (returner == 4)
            {
                return JokerList[3];
            }
            if (returner == 5)
            {
                return JokerList[4];
            }
            else
            {
                return 0;
            }
        }
        // The List Without Joker
        private List<int> ListWithoutJoker()
        {
            int jokerIPosition = WinningCardsTypes().IndexOf("jk");//Return the index position of the joker(string)
            List<int> ListWithoutJoker = new List<int>(WinningCards().Where((v, i) => i != jokerIPosition).OrderBy(i => i).ToList());//Create list without the Joker for numbers and sort in ascending order
            return ListWithoutJoker;
        }
        private bool EnumWOJoker()
        {
            bool IsEnum = ListWithoutJoker().SequenceEqual(Enumerable.Range(ListWithoutJoker().First(), ListWithoutJoker().Last()));
            return IsEnum;
        }
        private List<string> ReturnColorsWOJ()
        {
            var ColorListWOJ = new List<string>(WinningCardsColors().Where(i => i != "joker")).ToList();
            return ColorListWOJ;
        }
        private bool AreAllSameColors()
        {
            if (ReturnColorsWOJ().Distinct().Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Joker Switch for Winnings
        private void AceSwitcher()
        {
            if (WinningCardsTypes().Contains("ace"))
            {
                //If the Joker is NOT Present
                if (!WinningCardsTypes().Contains("jk"))
                {
                    if (
                       WinningCardsTypes().Contains("two") && WinningCardsTypes().Contains("three") && WinningCardsTypes().Contains("four") && WinningCardsTypes().Contains("five") 
                       )
                    {
                        for (int a = 0; a < WinningCardsTypes().Count(); a++)//Beware Count() Starts from 1 not 0
                        {
                            if (WinningCardsTypes()[a] == "ace")
                            {
                                JokerList[a] = 1;
                            }
                        }
                    }
                    else
                    {
                        for (int all = 0; all < WinningCardsTypes().Count(); all++)
                        {
                            if (WinningCardsTypes()[all] == "ace")
                            {
                                JokerList[all] = 14;
                            }
                        }
                    }
                }
                //If the Joker IS Present
                if (WinningCardsTypes().Contains("jk"))
                {
                    if (
                       (WinningCardsTypes().Contains("three") && WinningCardsTypes().Contains("four") && WinningCardsTypes().Contains("five")) ||
                       (WinningCardsTypes().Contains("two") && WinningCardsTypes().Contains("three") && WinningCardsTypes().Contains("four")) ||
                       (WinningCardsTypes().Contains("two") && WinningCardsTypes().Contains("three") && WinningCardsTypes().Contains("five"))
                       )
                    {
                        for (int tw = 0; tw < WinningCardsTypes().Count(); tw++)
                        {
                            if (WinningCardsTypes()[tw] == "ace")
                            {
                                JokerList[tw] = 1;
                            }
                        }
                    }
                    else
                    {
                        for (int all = 0; all < WinningCardsTypes().Count(); all++)
                        {
                            if (WinningCardsTypes()[all] == "ace")
                            {
                                JokerList[all] = 14;
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
        //Joker Method <-----------------------------------------------------------Joker----------------------------------------------------------->
        public void CheckWithJoker()
        {
            TrueWinner = false;
            TwoPair = false;
            OnePair = false;
            ThreeOfAKind = false;
            Straight = false;
            Flush = false;
            FourOfAKind = false;
            StraightFlush = false;
            FullHouse = false;
            JokerList = WinningCards();
            JokerListColors = WinningCardsColors();
            AceSwitcher();
            //Bool for Joker if is in the Hand
            if (WinningCardsTypes().Contains("jk"))//bool return true if joker is present
            {
                JokerIsPresent = true;
            }
            else
            {
                JokerIsPresent = false;
            }
            //Start Loops for Joker<---------------------------------------Loops For Joker--------------------------------------->
            if (JokerIsPresent)
            {
                JPos = WinningCardsTypes().IndexOf("jk");
                CheckFiveOfAKind();//Starts the Chain of Methods
            }
            else if (JokerIsPresent == false && JokerWins == false)
            {
                WinningHand();
            }
        }
        //Joker 5 of a Kind
        private void CheckFiveOfAKind()
        {
            if (FiveOfAKind == false)
            {
                for (int j = 1; j <= 14; j++)
                {
                    JokerList[JPos] = j;
                    WinningHand();
                    if (FiveOfAKind == true && JokerWins == true)//nested for 4 of a kind with Joker
                    {
                        JokerList.Clear();
                        return;
                    }
                    if (FiveOfAKind == false && JokerList[JPos] == 14)
                    {
                        CheckRoyalFlush();
                        return;
                    }
                }
            }
        }
        //Joker Royal Flush
        private void CheckRoyalFlush()
        {
            for (int ac = 1; ac <= 14; ac++)
            {
                JokerList[JPos] = ac;
                WinningHand();
                if (RoyalFlush == true && JokerWins == true)//nested for Royal Flush with Joker
                {
                    JokerList.Clear();
                    return;
                }
                if (RoyalFlush == false && JokerList[JPos] == 14)
                {
                    CheckStraightFlush();
                    return;
                }
            }
        }
        //Joker Straight Flush
        private void CheckStraightFlush()
        {
            for (int sf = 1; sf <= 14; sf++)
            {
                JokerList[JPos] = sf;
                WinningHand();
                if (StraightFlush == true && JokerWins == true)//nested for Straight Flush with Joker
                {
                    JokerList.Clear();
                    return;
                }
                if (StraightFlush == false && JokerList[JPos] == 14)
                {
                    CheckFourOfAKind();
                    return;
                }
            }
        }
        //Joker Four of a Kind
        private void CheckFourOfAKind()
        {
            for (int m = 1; m <= 14; m++)
            {
                JokerList[JPos] = m;
                WinningHand();
                if (FourOfAKind == true && JokerWins == true)//nested for 4 of a kind with Joker
                {
                    JokerList.Clear();
                    return;
                }
                if (FourOfAKind == false && JokerList[JPos] == 14)
                {
                    CheckFullHouse();
                    return;
                }
            }
        }
        //Joker Full House
        private void CheckFullHouse()
        {
            for (int l = 1; l <= 14; l++)
            {
                JokerList[JPos] = l;
                WinningHand();
                if (FullHouse == true && JokerWins == true)//nested for FullHouse with Joker
                {
                    JokerList.Clear();
                    return;
                }
                if (FullHouse == false && JokerList[JPos] == 14)
                {
                    CheckFlush();
                    return;
                }
            }
        }
        //Joker Flush
        private void CheckFlush()
        {
            if (AreAllSameColors() && Flush == false)
            {
                JokerListColors[JPos] = ReturnColorsWOJ()[0]; //Make the Joker the same color as the first card in the list without the joker for colors
                WinningHand();
                if (Flush == true && JokerWins == true)//nested
                {
                    JokerListColors.Clear();
                    return;
                }
            }
            else
            {
                CheckStraight();
                return;
            }
        }
        //Joker Straight
        private void CheckStraight()
        {
            for (int i = 1; i <= 14; i++)
            {
                JokerList[JPos] = i;
                WinningHand();
                if (Straight == true && JokerWins == true)//nested for Straight with Joker
                {
                    JokerList.Clear();
                    return;
                }
                if (Straight == false && JokerList[JPos] == 14)
                {
                    CheckThreeOfAKind();
                    return;
                }
            }
        }
        //Joker 3 of a Kind
        private void CheckThreeOfAKind()
        {
            for (int jas = 14; jas >=1; jas--)
            {
                JokerList[JPos] = jas;
                WinningHand();
                if (ThreeOfAKind == true && JokerWins == true)//nested for 3 of a kind with Joker
                {
                    JokerList.Clear();
                    return;
                }
                if (ThreeOfAKind == false && JokerList[JPos] == 1)
                {
                    CheckOnePair();
                    return;
                }
            }
        }
        //Joker One Pair
        private void CheckOnePair()
        {
            for (int p = 14; p >= 1; p--)
            {
                JokerList[JPos] = p;
                WinningHand();
                if (OnePair == true && JokerWins == true)//nested for 2 pair with Joker
                {
                    JokerList.Clear();
                    return;
                }
                else if (OnePair == false && JokerList[JPos] == 1)
                {
                    return;
                }
            }
        }
        // The WinningHand without Joker
        //<-----------------------------------------------------------Winning Hand----------------------------------------------------------->
        public void WinningHand()
        {
            if (straightList != null && straightList.Count() > 0)
            {
                straightList.Clear();
            }
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            TrueWinner = false;
            sWon = false;
            ThreeOfAKind = false;
            TwoPair = false;
            OnePair = false;
            Straight = false;
            Flush = false;
            FullHouse = false;
            FourOfAKind = false;
            StraightFlush = false;
            RoyalFlush = false;
            FiveOfAKind = false;
            straightList = (from n in JokerList where n > 0 orderby n ascending select n).ToList();
            bool straightEnum = straightList.SequenceEqual(Enumerable.Range(straightList[0], straightList.Count()));
            var ThreeDuplicates = JokerList.GroupBy(x => x).Where(y => y.Count() == 2).Where(y => y.Count() != 3).Select(y => y.Key); //Find 3 duplicates
            var TwoDuplicates = JokerList.GroupBy(x => x).Where(y => y.Count() == 3).Where(y => y.Count() != 2).Select(y => y.Key); //Find 2 duplicates

            //<-----------------------Winning Hand Start Checking--------------------->
            //Winner Five Of A Kind
            if (Wcard(1) == Wcard(2) && Wcard(2) == Wcard(3) && Wcard(3) == Wcard(4) && Wcard(4) == Wcard(5))// Five Of A Kind 
            {
                FiveOfAKind = true;
                sWon = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Winner Royal Flush
            if (straightEnum == true && AreAllSameColors() && JokerList.Max() == 14)
            {
                RoyalFlush = true;
                sWon = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Winner Straight Flush
            if (straightEnum == true && AreAllSameColors() && JokerList.Max() != 14)
            {
                StraightFlush = true;
                sWon = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Winner Four Of A Kind
            if (Wcard(1) == Wcard(2) && Wcard(2) == Wcard(3) && Wcard(3) == Wcard(4))// Four Of A Kind 1 2 3 4
            {
                FourOfAKind = true;
                sWon = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            else if (Wcard(2) == Wcard(3) && Wcard(3) == Wcard(4) && Wcard(4) == Wcard(5))// Four Of A Kind 2 3 4 5
            {
                FourOfAKind = true;
                sWon = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            else if (Wcard(1) == Wcard(2) && Wcard(2) == Wcard(4) && Wcard(4) == Wcard(5))// Four Of A Kind 1 2 4 5
            {
                FourOfAKind = true;
                sWon = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = false;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            else if (Wcard(1) == Wcard(2) && Wcard(2) == Wcard(3) && Wcard(3) == Wcard(5))// Four Of A Kind 1 2 3 5
            {
                FourOfAKind = true;
                sWon = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            else if (Wcard(1) == Wcard(3) && Wcard(3) == Wcard(4) && Wcard(4) == Wcard(5))// Four Of A Kind 1 3 4 5
            {
                FourOfAKind = true;
                sWon = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = false;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Winning Full House

            else if (ThreeDuplicates.Count() == 1 && TwoDuplicates.Count() == 1)
            {
                FullHouse = true;
                sWon = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Winning Flush
            else if (JokerListColors.Distinct().Count() == 1)
            {
                Flush = true;
                sWon = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Winning Straight 
            else if (straightEnum == true)
            {
                Straight = true;
                sWon = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //3 of a kind 1 3 4 
            else if (Wcard(1) == Wcard(3) && Wcard(3) == Wcard(4))
            {
                ThreeOfAKind = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = false;
                Card3Won = true;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //3 of a kind 1 3 5
            else if (Wcard(1) == Wcard(3) && Wcard(3) == Wcard(5))
            {
                ThreeOfAKind = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = false;
                Card3Won = true;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //3 of a kind 2 4 5
            else if (Wcard(2) == Wcard(4) && Wcard(4) == Wcard(5))
            {
                ThreeOfAKind = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = true;
                Card3Won = false;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //3 of a kind 2 3 5
            else if (Wcard(2) == Wcard(3) && Wcard(3) == Wcard(5))
            {
                ThreeOfAKind = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = true;
                Card3Won = true;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //3 of a kind 1 4 5
            else if (Wcard(1) == Wcard(4) && Wcard(4) == Wcard(5))
            {
                ThreeOfAKind = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = false;
                Card3Won = false;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //3 of a kind 1 2 5
            else if (Wcard(1) == Wcard(2) && Wcard(2) == Wcard(5))
            {
                ThreeOfAKind = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = false;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //3 of a kind 1 2 4
            else if (Wcard(1) == Wcard(2) && Wcard(2) == Wcard(4))
            {
                ThreeOfAKind = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = true;
                Card3Won = false;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //3 of a kind 3 4 5
            else if (Wcard(3) == Wcard(4) && Wcard(4) == Wcard(5))
            {
                ThreeOfAKind = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = false;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //3 of a kind 2 3 4
            else if (Wcard(2) == Wcard(3) && Wcard(3) == Wcard(4))
            {
                ThreeOfAKind = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = false;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //3 of a kind 1 2 3
            else if (Wcard(1) == Wcard(2) && Wcard(2) == Wcard(3))
            {
                ThreeOfAKind = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = false;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }

            // 2 Pairs 1 to 4, 3 to 5
            else if (Wcard(1) == Wcard(4) && Wcard(3) == Wcard(5))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = false;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 2 to 4, 3 to 5
            else if (Wcard(2) == Wcard(4) && Wcard(3) == Wcard(5))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 2 to 5, 3 to 4
            else if (Wcard(2) == Wcard(5) && Wcard(3) == Wcard(4))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 2 to 3, 4 to 5
            else if (Wcard(2) == Wcard(3) && Wcard(4) == Wcard(5))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 1 to 3, 4 to 5
            else if (Wcard(1) == Wcard(3) && Wcard(4) == Wcard(5))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = false;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 1 to 2, 4 to 5
            else if (Wcard(1) == Wcard(2) && Wcard(4) == Wcard(5))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = false;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 1 to 2, 3 to 4
            else if (Wcard(1) == Wcard(2) && Wcard(3) == Wcard(4))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 1 to 2, 3 to 5
            else if (Wcard(1) == Wcard(2) && Wcard(3) == Wcard(5))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 1 to 4, 2 to 3
            else if (Wcard(1) == Wcard(4) && Wcard(2) == Wcard(3))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 1 to 5, 3 to 4
            else if (Wcard(1) == Wcard(5) && Wcard(3) == Wcard(4))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = false;
                Card3Won = true;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 1 to 3, 2 to 4
            else if (Wcard(1) == Wcard(3) && Wcard(2) == Wcard(4))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 1 to 3, 2 to 5
            else if (Wcard(1) == Wcard(3) && Wcard(2) == Wcard(5))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 1 to 5, 2 to 3
            else if (Wcard(1) == Wcard(5) && Wcard(2) == Wcard(3))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = true;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 1 to 5, 2 to 4
            else if (Wcard(1) == Wcard(5) && Wcard(2) == Wcard(4))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = false;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            // 2 Pairs 1 to 4, 2 to 5
            else if (Wcard(1) == Wcard(4) && Wcard(2) == Wcard(5))
            {
                TwoPair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = true;
                Card3Won = false;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Winner Single Pair
            //Single Pair 1 2 Ace
            else if ((Wcard(1) == Wcard(2)) && (Wcard(1) == 1 && Wcard(2) == 1))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = true;
                Card3Won = false;
                Card4Won = false;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 2 3 Ace
            else if ((Wcard(2) == Wcard(3)) && (Wcard(2) == 1 && Wcard(3) == 1))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = false;
                Card1Won = false;
                Card2Won = true;
                Card3Won = true;
                Card4Won = false;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 3 4 Ace
            else if ((Wcard(3) == Wcard(4)) && (Wcard(3) == 1 && Wcard(4) == 1))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = false;
                Card2Won = false;
                Card3Won = true;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 4 5 Ace
            else if ((Wcard(4) == Wcard(5)) && (Wcard(4) == 1 && Wcard(5) == 1))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = false;
                Card3Won = false;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 2 4 Ace
            else if ((Wcard(2) == Wcard(4)) && (Wcard(2) == 1 && Wcard(4) == 1))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = false;
                Card2Won = true;
                Card3Won = false;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 2 5 Ace
            else if ((Wcard(2) == Wcard(5)) && (Wcard(2) == 1 && Wcard(5) == 1))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = true;
                Card3Won = false;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 3 5 Ace
            else if ((Wcard(3) == Wcard(5)) && (Wcard(3) == 1 && Wcard(5) == 1))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = false;
                Card3Won = true;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 1 3 Ace
            else if ((Wcard(1) == Wcard(3)) && (Wcard(1) == 1 && Wcard(3) == 1))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = false;
                Card3Won = true;
                Card4Won = false;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 1 4 Ace
            else if ((Wcard(1) == Wcard(4)) && (Wcard(1) == 1 && Wcard(4) == 1))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = false;
                Card3Won = false;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 1 5 Ace
            else if ((Wcard(1) == Wcard(5)) && (Wcard(1) == 1 && Wcard(5) == 1))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = false;
                Card3Won = false;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 1 2
            else if ((Wcard(1) == Wcard(2)) && (Wcard(1) > 10 && Wcard(2) > 10))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = true;
                Card3Won = false;
                Card4Won = false;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 2 3 
            else if ((Wcard(2) == Wcard(3)) && (Wcard(2) > 10 && Wcard(3) > 10))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = false;
                Card1Won = false;
                Card2Won = true;
                Card3Won = true;
                Card4Won = false;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 3 4
            else if ((Wcard(3) == Wcard(4)) && (Wcard(3) > 10 && Wcard(4) > 10))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = false;
                Card2Won = false;
                Card3Won = true;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 4 5
            else if ((Wcard(4) == Wcard(5)) && (Wcard(4) > 10 && Wcard(5) > 10))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = false;
                Card3Won = false;
                Card4Won = true;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 2 4
            else if ((Wcard(2) == Wcard(4)) && (Wcard(2) > 10 && Wcard(4) > 10))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = false;
                Card2Won = true;
                Card3Won = false;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 2 5
            else if ((Wcard(2) == Wcard(5)) && (Wcard(2) > 10 && Wcard(5) > 10))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = true;
                Card3Won = false;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 3 5
            else if ((Wcard(3) == Wcard(5)) && (Wcard(3) > 10 && Wcard(5) > 10))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = false;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = false;
                Card2Won = false;
                Card3Won = true;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 1 3
            else if ((Wcard(1) == Wcard(3)) && (Wcard(1) > 10 && Wcard(3) > 10))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = false;
                Card3Won = true;
                Card4Won = false;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 1 4
            else if ((Wcard(1) == Wcard(4)) && (Wcard(1) > 10 && Wcard(4) > 10))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = false;
                Card1Won = true;
                Card2Won = false;
                Card3Won = false;
                Card4Won = true;
                Card5Won = false;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            //Single Pair 1 5
            else if ((Wcard(1) == Wcard(5)) && (Wcard(1) > 10 && Wcard(5) > 10))
            {
                OnePair = true;
                TrueWinner = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = false;
                mainWindow.holdOn3 = false;
                mainWindow.holdOn4 = false;
                mainWindow.holdOn5 = true;
                Card1Won = true;
                Card2Won = false;
                Card3Won = false;
                Card4Won = false;
                Card5Won = true;
                if (JokerIsPresent)//nested
                {
                    JokerWins = true;
                    return;
                }
                return;
            }
            {
                TrueWinner = false;
                JokerWins = false;
                OnePair = false;
                TwoPair = false;
                ThreeOfAKind = false;
                Straight = false;
                Flush = false;
                FullHouse = false;
                FourOfAKind = false;
                StraightFlush = false;
                StraightFlush = false;
                FiveOfAKind = false;
                sWon = false;
                //Card1Won = false;
                //Card2Won = false;
                //Card3Won = false;
                //Card4Won = false;
                //Card5Won = false;
                return;
            }

        }
    }
}
