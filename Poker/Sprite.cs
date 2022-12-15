using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Poker
{
    class Sprite
    {
        //*********VARS********
        static System.Windows.Controls.Image m;
        public static int imageID { get; set; }



        public Sprite()
        {
        }
        //Create Sprite Method , Parameters: Sprite Width, Sprite Height, Position X, Position Y, Image, Name
        public static void Create(double imageWidth, double imageHeight, double xPos, double yPos, string imageSource, string spriteName, int zIndex)
        {
            mainWindow main1 = Application.Current.Windows[0] as mainWindow;
            m = new System.Windows.Controls.Image();
            m.BeginInit();
            m.Name = spriteName + (imageID).ToString();
            m.Uid = spriteName + (imageID).ToString();
            main1.mainGrid.RegisterName(m.Uid, m);
            m.Source = new BitmapImage(new Uri(@"Resources\" + imageSource + ".png", UriKind.Relative));
            System.Windows.Controls.Canvas.SetZIndex(m as UIElement, zIndex);
            //Convert to Vector
            m.Height = imageHeight; //Image height
            m.Width = imageWidth;//Image width
            double halfWidth = main1.mainGrid.Width;
            halfWidth = -halfWidth;
            double w = (halfWidth + xPos + imageWidth);
            //Height conversion to Y
            double halfHeight = main1.mainGrid.Height;
            halfHeight = -halfHeight;
            double h = (halfHeight + yPos + imageHeight);
            m.Margin = new Thickness(w, h, 0, 0);//left, top, right, bottom
            main1.mainGrid.Children.Add(m);
            m.EndInit();
            imageID += 1;

        }


        //Destroy Sprite Method(Works only if the sprite requested for deletion matches the parameter, since it's programatical the array of controls[] changes every time a sprite is deleted )
        public static void Destroy(string spriteName)
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            if (main.mainGrid.FindName(spriteName) != null)
            {
                foreach (System.Windows.Controls.Image spr in main.mainGrid.Children.OfType<System.Windows.Controls.Image>().ToList())
                {
                    if (spr == null)
                    {
                        return;
                    }
                    else if (spr.Name.Contains(spriteName))
                        main.mainGrid.Children.Remove(spr);
                }
            }
            else
            {
                return;
            }

        }
        //This object function you can insert an image and it will flash it
        public static void Flash(string spr, double duration, double opaqueMin, double opaqueMax, System.Windows.Controls.StackPanel Parent )
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            DoubleAnimation da = new DoubleAnimation();//Declare the Flashing animation for an image
            da.From = opaqueMax;
            da.To = opaqueMin;
            da.Duration = new Duration(TimeSpan.FromSeconds(duration));
            da.AutoReverse = true;
            da.RepeatBehavior = RepeatBehavior.Forever;
            if (Parent.FindName(spr) != null)
            {
                foreach (System.Windows.Controls.Image sprite in
                Parent.Children.OfType<System.Windows.Controls.Image>().ToList())
                {
                    if (sprite == null)
                    {
                        break;
                    }
                    else if (sprite.Name.Contains(spr))
                    {
                        string indexNo = Parent.Children.IndexOf(sprite).ToString();
                        int IndexNo = int.Parse(indexNo);
                        foreach (UIElement oProperty in Parent.Children.OfType<System.Windows.Controls.Image>().ToList())
                        {
                            Parent.Children[IndexNo].BeginAnimation(UIElement.OpacityProperty, da);
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }
        public static void MakeInvisible(string spriteToMakeTransparent )
        {
            mainWindow main = Application.Current.Windows[0] as mainWindow;
            if (main.mainGrid.FindName(spriteToMakeTransparent) != null)
            {

                foreach (System.Windows.Controls.Image sprite in main.mainGrid.Children.OfType<System.Windows.Controls.Image>().ToList())
                {
                    if ( sprite == null )
                    {
                        break;
                    }
                    else if (sprite.Name.Contains(spriteToMakeTransparent))
                    {
                        string fileName = sprite.Source.ToString();
                        sprite.Visibility = Visibility.Hidden;
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
