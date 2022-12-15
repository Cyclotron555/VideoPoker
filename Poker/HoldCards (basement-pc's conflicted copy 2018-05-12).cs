using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Media;
using System.Windows.Media.Imaging;




namespace Poker
{
    class HoldCards

    {
        #region VARS
        //*****VARS**********VARS**********VARS**********VARS**********VARS**********VARS**********VARS**********VARS**********VARS*****
        public int kindCardDisplayed1, kindCardDisplayed2, kindCardDisplayed3, kindCardDisplayed4, kindCardDisplayed5, newKindOfCards1,
            newKindOfCards2, newKindOfCards3, newKindOfCards4, newKindOfCards5;                    
          public string  colorCard1, colorCard2, colorCard3, colorCard4, colorCard5;
        bool weWon;

        //*****VARS**********VARS**********VARS**********VARS**********VARS**********VARS**********VARS**********VARS**********VARS*****
        #endregion VARS

        #region Methods to the main window
        // These methods provide link to the main window
        public static void accessToMainWindow1()
        { 
         foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(mainWindow))
                    {
                        (window as mainWindow).hold_sign.Source = new BitmapImage(new Uri(@"Resources\hold.png" , UriKind.Relative));
                    }
}       }
        public static void accessToMainWindow2()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(mainWindow))
                {
                    (window as mainWindow).hold_sign2.Source = new BitmapImage(new Uri(@"Resources\hold.png", UriKind.Relative));
                }
            }
        }
        public static void accessToMainWindow3()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(mainWindow))
                {
                    (window as mainWindow).hold_sign3.Source = new BitmapImage(new Uri(@"Resources\hold.png", UriKind.Relative));
                }
            }
        }
        public static void accessToMainWindow4()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(mainWindow))
                {
                    (window as mainWindow).hold_sign4.Source = new BitmapImage(new Uri(@"Resources\hold.png", UriKind.Relative));
                }
            }
        }
        public static void accessToMainWindow5()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(mainWindow))
                {
                    (window as mainWindow).hold_sign5.Source = new BitmapImage(new Uri(@"Resources\hold.png", UriKind.Relative));
                }
            }
        }
        #endregion Methods to the main window

        #region Card Definitions Lists
        CardDefinitions cardDefinitions = new CardDefinitions();
        public void CardsDisplayed()
            {
            List<int> kindOfCards = new List<int>();
            kindOfCards.Clear();
            kindOfCards.Add(cardDefinitions.ReturnCardKind(mainWindow.c1));
            kindOfCards.Add(cardDefinitions.ReturnCardKind(mainWindow.c2));
            kindOfCards.Add(cardDefinitions.ReturnCardKind(mainWindow.c3));
            kindOfCards.Add(cardDefinitions.ReturnCardKind(mainWindow.c4));
            kindOfCards.Add(cardDefinitions.ReturnCardKind(mainWindow.c5));

            List<string> colorOfCards = new List<string>();
            colorOfCards.Clear();
            colorOfCards.Add(cardDefinitions.ReturnCardColor(mainWindow.c1));
            colorOfCards.Add(cardDefinitions.ReturnCardColor(mainWindow.c2));
            colorOfCards.Add(cardDefinitions.ReturnCardColor(mainWindow.c3));
            colorOfCards.Add(cardDefinitions.ReturnCardColor(mainWindow.c4));
            colorOfCards.Add(cardDefinitions.ReturnCardColor(mainWindow.c5));
            
            kindCardDisplayed1 = kindOfCards[0];
            kindCardDisplayed2 = kindOfCards[1];
            kindCardDisplayed3 = kindOfCards[2];
            kindCardDisplayed4 = kindOfCards[3];
            kindCardDisplayed5 = kindOfCards[4];
            colorCard1 = colorOfCards[0];
            colorCard2 = colorOfCards[1];
            colorCard3 = colorOfCards[2];
            colorCard4 = colorOfCards[3];
            colorCard5 = colorOfCards[4];
          
            
            #endregion END Card Definitions Lists Region

            #region START Checking for Winning Cards
            weWon = false; // Reset the Boolean to Reset the Cards Each Cycle
                           
            kindOfCards.Sort();
            List<int> newKindOfCards = new List<int>();
            foreach (var c in kindOfCards)
            {
                newKindOfCards.Add(c);
            }
            newKindOfCards1 = newKindOfCards[0];
            newKindOfCards2 = newKindOfCards[1];
            newKindOfCards3 = newKindOfCards[2];
            newKindOfCards4 = newKindOfCards[3];
            newKindOfCards5 = newKindOfCards[4];




            //Start Checking for Straight
            if ((newKindOfCards5 - newKindOfCards4 == 0 && newKindOfCards4 - newKindOfCards3 == 0 && newKindOfCards3//Straight on last and first 3
                - newKindOfCards1 == 0) && weWon == false)
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn5 = true;
                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow3();
                accessToMainWindow5();
            }
            if ((newKindOfCards5 - newKindOfCards4 == 0 && newKindOfCards4 - newKindOfCards3 == 0 && newKindOfCards2//Straight on 1st and last 3
                - newKindOfCards1 == 0) && weWon == false)
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                accessToMainWindow1();
                accessToMainWindow3();
                accessToMainWindow4();
                accessToMainWindow5();
            }
            if ((newKindOfCards3 - newKindOfCards2 == 0 && newKindOfCards4 - newKindOfCards3 == 0 && newKindOfCards5//Straight on last 4
                - newKindOfCards4 == 0) && weWon == false)
            {
                weWon = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                accessToMainWindow2();
                accessToMainWindow3();
                accessToMainWindow4();
                accessToMainWindow5();
            }
            else if ((newKindOfCards2 - newKindOfCards1 == 0 && newKindOfCards3 - newKindOfCards2 == 0 && newKindOfCards4//Straight on first 4
                - newKindOfCards3 == 0) && weWon == false )
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow3();
                accessToMainWindow4();

            }//Start Checking for 3 of a Kind
            else if (kindCardDisplayed3 == kindCardDisplayed4 && kindCardDisplayed4 == kindCardDisplayed5 && weWon == false)// 3 of a kind 3,4,5
            {
                weWon = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                accessToMainWindow3();
                accessToMainWindow4();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed2 == kindCardDisplayed4 && kindCardDisplayed4 == kindCardDisplayed5 && weWon == false)// 3 of a kind 2,4,5
            {
                weWon = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                accessToMainWindow2();
                accessToMainWindow4();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed2 == kindCardDisplayed3 && kindCardDisplayed3 == kindCardDisplayed5 && weWon == false)// 3 of a kind 2,3,5
            {
                weWon = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn5 = true;
                accessToMainWindow2();
                accessToMainWindow3();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed2 == kindCardDisplayed3 && kindCardDisplayed3 == kindCardDisplayed4 && weWon == false)// 3 of a kind 2,3,4
            {
                weWon = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                accessToMainWindow2();
                accessToMainWindow3();
                accessToMainWindow4();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed4 && kindCardDisplayed4 == kindCardDisplayed5 && weWon == false)// 3 of a kind 1,4,5
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                accessToMainWindow1();
                accessToMainWindow4();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed2 && kindCardDisplayed2 == kindCardDisplayed5 && weWon == false)// 3 of a kind 1,2,5
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn5 = true;
                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed2 && kindCardDisplayed2 == kindCardDisplayed4 && weWon == false)// 3 of a kind 1,2,4
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn4 = true;
                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow4();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed2 && kindCardDisplayed2 == kindCardDisplayed3 && weWon == false)// 3 of a kind 1,2,3
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow3();
            }
            // Start Checking for 2 Pairs
            else if (kindCardDisplayed1 == kindCardDisplayed2 && kindCardDisplayed3 == kindCardDisplayed5 && weWon == false)// 2 Pairs 1 to 2, 3 to 5
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn5 = true;

                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow3();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed2 && kindCardDisplayed3 == kindCardDisplayed4 && weWon == false)// 2 Pairs 1 to 2, 3 to 4
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;

                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow3();
                accessToMainWindow4();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed4 && kindCardDisplayed2 == kindCardDisplayed3 && weWon == false)// 2 Pairs 1 to 4, 2 to 3
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;

                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow3();
                accessToMainWindow4();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed5 && kindCardDisplayed3 == kindCardDisplayed4 && weWon == false)// 2 Pairs 1 to 5, 3 to 4
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;

                accessToMainWindow1();
                accessToMainWindow3();
                accessToMainWindow4();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed3 && kindCardDisplayed2 == kindCardDisplayed4 && weWon == false)// 2 Pairs 1 to 3, 2 to 4
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;

                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow3();
                accessToMainWindow4();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed3 && kindCardDisplayed2 == kindCardDisplayed5 && weWon == false)// 2 Pairs 1 to 3, 2 to 5
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn5 = true;

                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow3();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed3 && kindCardDisplayed4 == kindCardDisplayed5 && weWon == false)// 2 Pairs 1 to 3, 4 to 5
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;

                accessToMainWindow1();
                accessToMainWindow3();
                accessToMainWindow4();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed5 && kindCardDisplayed2 == kindCardDisplayed3 && weWon == false)// 2 Pairs 1 to 5, 2 to 3
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn5 = true;

                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow3();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed5 && kindCardDisplayed2 == kindCardDisplayed4 && weWon == false)// 2 Pairs 1 to 5, 2 to 4
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;

                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow4();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed4 && kindCardDisplayed2 == kindCardDisplayed5 && weWon == false)// 2 Pairs 1 to 4, 2 to 5
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;

                accessToMainWindow1();
                accessToMainWindow2();
                accessToMainWindow4();
                accessToMainWindow5();
            }





            // Start Checking for Single Pairs
            else if (kindCardDisplayed1 == kindCardDisplayed2 && weWon == false)// Pair 1,2
            {
                weWon = true;
                mainWindow.holdOn = true;
                mainWindow.holdOn2 = true;
                accessToMainWindow1();
                accessToMainWindow2();
            }
            else if (kindCardDisplayed2 == kindCardDisplayed3 && weWon == false)// Pair 2,3
            {
                weWon = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn3 = true;
                accessToMainWindow2();
                accessToMainWindow3();
            }
            else if (kindCardDisplayed3 == kindCardDisplayed4 && weWon == false)// Pair 3, 4
            {
                weWon = true;
                mainWindow.holdOn3 = true;
                mainWindow.holdOn4 = true;
                accessToMainWindow3();
                accessToMainWindow4();
            }
            else if (kindCardDisplayed4 == kindCardDisplayed5 && weWon == false)// Pair 4, 5
            {
                mainWindow.holdOn4 = true;
                mainWindow.holdOn5 = true;
                weWon = true;
                accessToMainWindow4();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed2 == kindCardDisplayed4 && weWon == false)//Pair 2, 4
            {
                mainWindow.holdOn2 = true;
                mainWindow.holdOn4 = true;
                weWon = true;
                accessToMainWindow2();
                accessToMainWindow4();
            }
            else if (kindCardDisplayed2 == kindCardDisplayed5 && weWon == false)//Pair 2, 5
            {
                weWon = true;
                mainWindow.holdOn2 = true;
                mainWindow.holdOn5 = true;
                accessToMainWindow2();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed3 == kindCardDisplayed5 && weWon == false)// Pair 3, 5
            {
                mainWindow.holdOn3 = true;
                mainWindow.holdOn5 = true;
                weWon = true;
                accessToMainWindow3();
                accessToMainWindow5();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed3 && weWon == false)// Pair 1, 3
            {
                mainWindow.holdOn = true;
                mainWindow.holdOn3 = true;
                weWon = true;
                accessToMainWindow1();
                accessToMainWindow3();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed4 && weWon == false)// Pair 1, 4
            {
                mainWindow.holdOn = true;
                mainWindow.holdOn4 = true;
                weWon = true;
                accessToMainWindow1();
                accessToMainWindow4();
            }
            else if (kindCardDisplayed1 == kindCardDisplayed5 && weWon == false)// Pair 1, 5
            {
                mainWindow.holdOn = true;
                mainWindow.holdOn5 = true;
                weWon = true;
                accessToMainWindow1();
                accessToMainWindow5();
            }
            else
            {
                weWon = false; 
            }
            #endregion END cheking for winning cards
            //MessageBox.Show(
            //       "-----------CARDS DISPLAYED----------" +
            //       "\n\n 1st one: ---------------------" + kindCardDisplayed1 + "  " + colorCard1 +
            //       "\n\n 2nd one: --------------------" + kindCardDisplayed2 + "  " + colorCard2 +
            //       "\n\n 3rd one: ---------------------" + kindCardDisplayed3 + "  " + colorCard3 +
            //       "\n\n 4th one: ---------------------" + kindCardDisplayed4 + "  " + colorCard4 +
            //       "\n\n 5th one: ---------------------" + kindCardDisplayed5 + "  " + colorCard5 +
            //       "\n\n-------IN ASCENDING ORDER------"+
            //       "\n\n 1st #: -----------------------" + newKindOfCards1 + 
            //       "\n\n 2nd #: ----------------------" + newKindOfCards2 + 
            //       "\n\n 3rd #: -----------------------" + newKindOfCards3 + 
            //       "\n\n 4th #: -----------------------" + newKindOfCards4 +
            //       "\n\n 5th #: -----------------------" + newKindOfCards5 +
            //       "\n\n-----------GAME STATS----------" +
            //       "\n\n Item Number : --------------" + mainWindow.itemNumber +
            //       "\n\n Hand Number: -------------" + mainWindow.drawWasClicked +
            //       "\n\n Is there a pair ? -------------" + weWon);
           





        }
    }
     
    
}
