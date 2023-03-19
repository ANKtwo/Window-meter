using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
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
using static System.Net.Mime.MediaTypeNames;

namespace Window_size
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory+"settings.txt"))
            {
                using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "settings.txt"))
                {
                    Header.Opacity = Convert.ToDouble(reader.ReadLine());
                    Body.Opacity = Convert.ToDouble(reader.ReadLine());
                    Body.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(reader.ReadLine());
                    BodySize.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(reader.ReadLine());
                }
            }
            else 
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + "settings.txt");
            }

            HeaderSlider.Value = Math.Round(Header.Opacity*100);
            BodySlider.Value = Math.Round(Body.Opacity*100);
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "settings.txt", false))
            {
                writer.WriteLine(Header.Opacity);
                writer.WriteLine(Body.Opacity);
                writer.WriteLine(Body.Background);
                writer.WriteLine(BodySize.Foreground);
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Header.Width = this.Width;
            Body.Width = this.Width;
            Body.Height= this.Height;

            BodySize.Content = this.Height>this.Width ? Math.Round(this.Width) + " × " + Math.Round(this.Height):Math.Round(this.Height) + " × " + Math.Round(this.Width);
            HeaderSize.Content = "H:" + Math.Round(this.Height,1) + " " + "W:" + Math.Round(this.Width,1);
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void HideWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void MaxWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
        private void HeaderSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            HeaderLabel.Content = "Header opacity " + HeaderSlider.Value;
            Header.Opacity = HeaderSlider.Value/100;
        }
        private void BodySliser_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BodyLabel.Content = "Body opacity " + BodySlider.Value;
            Body.Opacity = BodySlider.Value/100;
        }
        private void ComboBoxColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBoxColor.SelectedIndex)
            {
                case 0:
                    Body.Background = new SolidColorBrush(Color.FromRgb(0,0,0));
                    break; 
                case 1:
                    Body.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    break;
                case 2:
                    try
                    {
                        Body.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorTextBox.Text);
                    }
                    catch (Exception)
                    {
                        ColorTextBox.Text = "";
                        MessageBox.Show("Wrong color hex code", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    break;
                default:
                    break;
            }
        }
        private void ComboBoxTextColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBoxTextColor.SelectedIndex)
            {
                case 0:
                    BodySize.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    break;
                case 1:
                    BodySize.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    break;
                case 2:
                    try
                    {
                        BodySize.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(TextColorTextBox.Text);
                    }
                    catch (Exception)
                    {
                        ColorTextBox.Text = "";
                        MessageBox.Show("Wrong color hex code", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
