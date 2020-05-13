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

namespace AbaloneGameWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Borad Game_Borad;
        
        public static Label black_ejected_label;
        public static Label white_ejected_label;
        public static Label currnt_turn_label;

        public MainWindow()
        {
            
            InitializeComponent();
            
            black_ejected_label = Black_Ejected_Label;
            white_ejected_label = White_Ejected_Label;
            currnt_turn_label = Currnt_Turn_Label;
            //Black_Ejected_Label.Content = "Black:\n0/6";
            //Black_Ejected_Label.FontSize = 30;
            //White_Ejected_Label.Content = "0/6\nWhite:";
            //White_Ejected_Label.FontSize = 30;
            //Currnt_Turn_Label.Content = "Currnt Player:\nWhite Player";
            //FontSize = 30;
            Game_Borad = new Borad();
            Game_Borad.Draw_Borad(CanvasXML);
            Play_Against_Human_RadioButton.IsChecked = true;
            
            //White_Ejected_Label.Visibility = Visibility.Hidden;
            
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Settings.TESTING_MODE)
            {
                Point a = new Point();
                New_Game_Button.FontSize = 20;
                Graphics graph = new Graphics(CanvasXML);
                a = graph.Pixel_Point_To_Array_Point(Mouse.GetPosition(CanvasXML));
                New_Game_Button.Content = "left(x):" + Mouse.GetPosition(CanvasXML).X + " top(y):" + Mouse.GetPosition(CanvasXML).Y + "\n arrX: " + a.X + ",arrY: " + a.Y + "";
                //Game_Text_Block.Text = "it is " + Game_Borad.Get_Turn_Name() + " turn \nthe amount of black pieces ejected is " + Game_Borad.Get_Num_Of_Ejected_Black() + "\nthe amount of white pieces ejected is " + Game_Borad.Get_Num_Of_Ejected_White() + "";
            }

            Game_Borad.Canvas_Clicked(CanvasXML, Mouse.GetPosition(CanvasXML));
        }
        private void New_Game_Click(object sender, RoutedEventArgs e)
        {
            Restart_Game();
        }
        public void Restart_Game()
        {
            Game_Borad = new Borad();
            Game_Borad.Draw_Borad(CanvasXML);
            New_Game_Button.Content = "New Game";
            New_Game_Button.FontSize = 35;
        }
        private void Play_Against_Human_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Settings.IS_BOT_ENABLED = false;
            Restart_Game();
        }
        private void Play_Against_Bot_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Settings.IS_BOT_ENABLED = true;
            Restart_Game();
        }
    }
}
