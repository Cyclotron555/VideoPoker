using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Poker
{
    /// <summary>
    /// Interaction logic for RedBlackPokerSettings.xaml
    /// </summary>
    public partial class RedBlackPokerSettings : Page
    {
        SolidColorBrush BlueBrush = new SolidColorBrush(Colors.Navy);
        SolidColorBrush TrueRed = new SolidColorBrush(Colors.Red);
        public RedBlackPokerSettings()
        {
            InitializeComponent();
        }

        private void Label_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            if (RBSMed.IsChecked == true)
            {
                main.CardDisplaySpeed = 275;
                main.CardDSpeed = 2;
            }
            else if (RBSlow.IsChecked == true)
            {
                main.CardDisplaySpeed = 350;
                main.CardDSpeed = 1;
            }
            else if (RBSFast.IsChecked == true)
            {
                main.CardDisplaySpeed = 200;
                main.CardDSpeed = 3;
            }
            Content = null;
            IsEnabled = false;
            main.MainFrame.Content = null;
            main.MainFrame.IsEnabled = false;
            main.CreateWinningsTable();
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseLbl.Foreground = BlueBrush;
            Cursor = Cursors.Hand;
        }

        private void CloseLbl_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseLbl.Foreground = TrueRed;
            Cursor = Cursors.Arrow;
        }

        private void CloseLbl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CloseLbl.Foreground = TrueRed;
        }

        private void CloseLbl_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            if (main.CardDSpeed == 1)
            {
                RBSlow.IsChecked = true;
            }
            else if (main.CardDSpeed == 2)
            {
                RBSMed.IsChecked = true;
            }
            else if (main.CardDSpeed == 3)
            {
                RBSFast.IsChecked = true;
            }
            else
            {
                RBSMed.IsChecked = true;
            }
            BonusesTDynamic.Text = main.BonusStatsCount.ToString();
            TwoPairsTDynamic.Text = main.TwoPairStatsCount.ToString();
            ThreeOfAKindTDynamic.Text = main.ThreeOfAKindStatsCount.ToString();
            StraightTDynamic.Text = main.StraightStatsCount.ToString();
            FlushTDynamic.Text = main.FlushStatsCount.ToString();
            FullHouseTDynamic.Text = main.FullHouseStatsCount.ToString();
            FourOfAKindTDynamic.Text = main.FourOfAKindStatsCount.ToString();
            StraightFlushTDynamic.Text = main.StraightFlushStatsCount.ToString();
            RoyalFlushTDynamic.Text = main.RoyalFlushStatsCount.ToString();
            FiveOfAKindTDynamic.Text = main.FiveOfAKindStatsCount.ToString();
            JackpotT.Text = " You Guessed All the Cards " + main.JackpotCount + " times ";
        }

        private void RBSFast_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RBSFast.IsChecked = true;
            CSTB.Text = "⯇Interval Set To: 200ms⯈";
        }

        private void RBSMed_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RBSMed.IsChecked = true;
            CSTB.Text = "⯇Interval Set To: 275ms⯈";
        }

        private void RBSlow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RBSlow.IsChecked = true;
            CSTB.Text = "⯇Interval Set To: 350ms⯈";
        }

        private void ResetButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.CardDSpeed = 2;
            RBSMed.IsChecked = true;
            main.Bet = 1;
            main.MainScore = 0;
            main.Wallet = 0;
            main.BonusStatsCount = 0;
            main.BackOfCardsChoice = 2;
            main.FiveOfAKindStatsCount = 0;
            main.RoyalFlushStatsCount = 0;
            main.StraightFlushStatsCount = 0;
            main.FourOfAKindStatsCount = 0;
            main.FullHouseStatsCount = 0;
            main.FlushStatsCount = 0;
            main.StraightStatsCount = 0;
            main.ThreeOfAKindStatsCount = 0;
            main.TwoPairStatsCount = 0;
            main.BonusStatsCount = 0;
            main.FiveOfAKindStatsCount = 0;
            main.JackpotCount = 0;
            main.BackOfCards = "backdesign_6";
            main.lblAccountAmount.Content = main.Wallet.ToString();
            main.LblScore.Content = main.MainScore.ToString();
            main.BonusBorder1.Visibility = Visibility.Hidden;
            main.BonusBorder2.Visibility = Visibility.Hidden;
            main.BonusBorder3.Visibility = Visibility.Hidden;
            main.BonusBorder4.Visibility = Visibility.Hidden;
            main.BonusBorder5.Visibility = Visibility.Hidden;
            main.BonusBorder6.Visibility = Visibility.Hidden;
            main.BonusBorder7.Visibility = Visibility.Hidden;
            main.BonusBorder8.Visibility = Visibility.Hidden;
            main.BonusBorder9.Visibility = Visibility.Hidden;
            BonusesTDynamic.Text = main.BonusStatsCount.ToString();
            TwoPairsTDynamic.Text = main.TwoPairStatsCount.ToString();
            ThreeOfAKindTDynamic.Text = main.ThreeOfAKindStatsCount.ToString();
            StraightTDynamic.Text = main.StraightStatsCount.ToString();
            FlushTDynamic.Text = main.FlushStatsCount.ToString();
            FullHouseTDynamic.Text = main.FullHouseStatsCount.ToString();
            FourOfAKindTDynamic.Text = main.FourOfAKindStatsCount.ToString();
            StraightFlushTDynamic.Text = main.StraightFlushStatsCount.ToString();
            RoyalFlushTDynamic.Text = main.RoyalFlushStatsCount.ToString();
            FiveOfAKindTDynamic.Text = main.FiveOfAKindStatsCount.ToString();
            JackpotT.Text = " You Guessed All the Cards " + main.JackpotCount + " times ";
            ResetAll.Text = "⯇Data Was Reset⯈";
        }
        //Back of Card 1
        private void B1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b1.Opacity = 0.9;
        }
        private void B1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.BackOfCards = "backdesign_6";
            main.BackOfCardsChoice = 1;
            ChooseDesign.Text = "⯇Design 1 Set⯈";
        }

        private void B1_MouseEnter(object sender, MouseEventArgs e)
        {
            b1.Opacity = 0.5;
        }

        private void B1_MouseLeave(object sender, MouseEventArgs e)
        {
            b1.Opacity = 1;
        }
        //Back of Card 2
        private void B2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b2.Opacity = 0.9;
        }

        private void B2_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.BackOfCards = "backdesign_7";
            main.BackOfCardsChoice = 2;
            ChooseDesign.Text = "⯇Design 2 Set⯈";
        }

        private void B2_MouseEnter(object sender, MouseEventArgs e)
        {
            b2.Opacity = 0.5;
        }

        private void B2_MouseLeave(object sender, MouseEventArgs e)
        {
            b2.Opacity = 1;
        }
        //Back of Card 3
        private void B3_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b3.Opacity = 0.9;
        }

        private void B3_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.BackOfCards = "backdesign_8";
            main.BackOfCardsChoice = 3;
            ChooseDesign.Text = "⯇Design 3 Set⯈";
        }

        private void B3_MouseEnter(object sender, MouseEventArgs e)
        {
            b3.Opacity = 0.5;
        }

        private void B3_MouseLeave(object sender, MouseEventArgs e)
        {
            b3.Opacity = 1;
        }
        //Back of Card 4
        private void B4_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b4.Opacity = 0.9;
        }

        private void B4_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.BackOfCards = "backdesign_9";
            main.BackOfCardsChoice = 4;
            ChooseDesign.Text = "⯇Design 4 Set⯈";
        }

        private void B4_MouseEnter(object sender, MouseEventArgs e)
        {
            b4.Opacity = 0.5;
        }

        private void B4_MouseLeave(object sender, MouseEventArgs e)
        {
            b4.Opacity = 1;
        }
        //Back of Card 5
        private void B5_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b5.Opacity = 0.9;
        }

        private void B5_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.BackOfCards = "backdesign_10";
            main.BackOfCardsChoice = 5;
            ChooseDesign.Text = "⯇Design 5 Set⯈";
        }

        private void B5_MouseEnter(object sender, MouseEventArgs e)
        {
            b5.Opacity = 0.5;
        }

        private void B5_MouseLeave(object sender, MouseEventArgs e)
        {
            b5.Opacity = 1;
        }
        //Back of Card 6
        private void B6_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b6.Opacity = 0.9;
        }

        private void B6_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.BackOfCards = "backdesign_11";
            main.BackOfCardsChoice = 6;
            ChooseDesign.Text = "⯇Design 6 Set⯈";
        }

        private void B6_MouseEnter(object sender, MouseEventArgs e)
        {
            b6.Opacity = 0.5;
        }

        private void B6_MouseLeave(object sender, MouseEventArgs e)
        {
            b6.Opacity = 1;
        }
        //Back of Card 7
        private void B7_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b7.Opacity = 0.9;
        }

        private void B7_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.BackOfCards = "back_6";
            main.BackOfCardsChoice = 7;
            ChooseDesign.Text = "⯇Design 7 Set⯈";
        }

        private void B7_MouseEnter(object sender, MouseEventArgs e)
        {
            b7.Opacity = 0.5;
        }

        private void B7_MouseLeave(object sender, MouseEventArgs e)
        {
            b7.Opacity = 1;
        }
        //Back of Card 8
        private void B8_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b8.Opacity = 0.9;
        }

        private void B8_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.BackOfCards = "backdesign_2";
            main.BackOfCardsChoice = 8;
            ChooseDesign.Text = "⯇Design 8 Set⯈";
        }

        private void B8_MouseEnter(object sender, MouseEventArgs e)
        {
            b8.Opacity = 0.5;
        }

        private void B8_MouseLeave(object sender, MouseEventArgs e)
        {
            b8.Opacity = 1;
        }
        //Back of Card 9
        private void B9_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b9.Opacity = 0.9;
        }

        private void B9_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.BackOfCards = "backdesign_4";
            main.BackOfCardsChoice = 9;
            ChooseDesign.Text = "⯇Design 9 Set⯈";
        }

        private void B9_MouseEnter(object sender, MouseEventArgs e)
        {
            b9.Opacity = 0.5;
        }

        private void B9_MouseLeave(object sender, MouseEventArgs e)
        {
            b9.Opacity = 1;
        }
        //Back of Card 10
        private void B10_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b10.Opacity = 0.9;
        }

        private void B10_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.BackOfCards = "backdesign_13";
            main.BackOfCardsChoice = 10;
            ChooseDesign.Text = "⯇Design 10 Set⯈";
        }

        private void B10_MouseEnter(object sender, MouseEventArgs e)
        {
            b10.Opacity = 0.5;
        }

        private void B10_MouseLeave(object sender, MouseEventArgs e)
        {
            b10.Opacity = 1;
        }
        //Back of Card 11
        private void B11_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b11.Opacity = 0.9;
        }

        private void B11_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.BackOfCards = "backdesign_14";
            main.BackOfCardsChoice = 11;
            ChooseDesign.Text = "⯇Design 11 Set⯈";
        }

        private void B11_MouseEnter(object sender, MouseEventArgs e)
        {
            b11.Opacity = 0.5;
        }

        private void B11_MouseLeave(object sender, MouseEventArgs e)
        {
            b11.Opacity = 1;
        }
        //Back of Card 12
        private void B12_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b12.Opacity = 0.9;
        }

        private void B12_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            main.BackOfCards = "backdesign_15";
            main.BackOfCardsChoice = 12;
            ChooseDesign.Text = "⯇Design 12 Set⯈";
        }

        private void B12_MouseEnter(object sender, MouseEventArgs e)
        {
            b12.Opacity = 0.5;
        }

        private void B12_MouseLeave(object sender, MouseEventArgs e)
        {
            b12.Opacity = 1;
        }
    }
}
