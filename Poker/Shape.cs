using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Poker
{
    static class Shape
    {
        //*******VARS*******
        public static int ID = 0;
        public static bool flashShwitch { get; set; }

        //*******VARS*******

        public static void CreateRectangleWithText(string name, double width, double height,
            double locationX, double locationY, string fillColor, string strokeColor, string textColor,
            double borderThick, double cornRadius1, double cornRadius2, double cornRadius3,
            double cornRadius4, string content, HorizontalAlignment hAlignement, VerticalAlignment vAlignement, double opacity)
        {
            mainWindow main1 = Application.Current.Windows[0] as mainWindow;
            BrushConverter converter = new BrushConverter();
            Brush backgroundCol = converter.ConvertFromString(fillColor) as Brush;
            Brush border = converter.ConvertFromString(strokeColor) as Brush;
            Brush textCol = converter.ConvertFromString(textColor) as Brush;
            Label label = new Label();
            label.Width = width;
            label.Height = height;
            label.Padding = new Thickness(5, 0, 5, 2);
            label.HorizontalContentAlignment = hAlignement;
            label.VerticalContentAlignment = vAlignement;
            label.FontWeight = FontWeights.Bold;
            label.FontSize = 10;
            label.Name = name.ToString();
            label.Background = backgroundCol;
            label.Foreground = textCol;
            label.Opacity = opacity;
            label.Content = content; 
            //Convert to Vector
            double halfWidth = main1.mainGrid.Width;
            halfWidth = -halfWidth;
            double w = (halfWidth + locationX + width);
            double halfHeight = main1.mainGrid.Height;
            halfHeight = -halfHeight;
            double h = (halfHeight + locationY + height);
            label.Margin = new Thickness(w, h, 0, 0);//left, top, right, bottom
            main1.mainGrid.Children.Add(label);
            main1.mainGrid.RegisterName(label.Name, label);
            ID += 1;
        }
        public static void Destroy(string name)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            foreach (Label lb in main.mainGrid.Children.OfType<Label>().ToList())
            {
                if (main.mainGrid.FindName(name) != null)
                {
                    main.mainGrid.Children.Remove(lb);
                    main.mainGrid.UnregisterName(name);
                }
                else
                {
                    return;
                }
            }
        }
        public static void ChangeText(string name, string text)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            foreach  (Label lb in main.mainGrid.Children.OfType<Label>().Where(n => n.Name == name))
            {
                if (main.mainGrid.FindName(name) != null)
                {
                    lb.Content = text;
                    lb.HorizontalAlignment = HorizontalAlignment.Center;
                    lb.VerticalAlignment = VerticalAlignment.Center;
                }
                else
                {
                    return;
                }
            }
        }
        public static ColorAnimation colorAnimation = new ColorAnimation();
        public static void ChangeColorAnimation(string controlToChangeColor, Color color1, Color color2)
        {
            mainWindow main1 = Application.Current.Windows[0] as mainWindow;
            foreach (Label c in main1.mainGrid.Children.OfType<Label>().ToList())
            {
                if (c == null)
                {
                    break;
                }
                else if (c.Name.Contains(controlToChangeColor) && flashShwitch == true)
                {
                    //background color
                    Storyboard.SetTargetName(colorAnimation, ("animationBrush" + ID.ToString()));
                    Storyboard.SetTargetProperty(colorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));
                    Storyboard storyboard = new Storyboard();
                    SolidColorBrush solidColorBrush = new SolidColorBrush();
                    solidColorBrush.Color = Colors.Transparent;
                    main1.mainGrid.RegisterName("animationBrush" + ID.ToString(), solidColorBrush);
                    c.Background = solidColorBrush;
                    colorAnimation.From = color1;
                    colorAnimation.To = color2;
                    colorAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                    colorAnimation.RepeatBehavior = RepeatBehavior.Forever;
                    storyboard.Children.Add(colorAnimation);
                    storyboard.Begin(c);
                    storyboard.Remove(c);
                    ID += 1;
                }
                else if (flashShwitch == false && colorAnimation != null)
                {
                    SolidColorBrush originalColorBrush = new SolidColorBrush();
                    SolidColorBrush originalTxtColor = new SolidColorBrush();
                    originalTxtColor.Color = Colors.White;
                    originalColorBrush.Color = Colors.Red;
                    c.Background = originalColorBrush;
                    c.Foreground = originalTxtColor;
                }
            }
        }
    } 
}
