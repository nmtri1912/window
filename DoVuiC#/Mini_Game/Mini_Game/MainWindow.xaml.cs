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

namespace Mini_Game
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

        int index;
        int diem = 0;
        int socau = 0;
        int countClick = 0;
        Random _generator = new Random();
        string[] avatars = new string[] {"Anh.png", "Ao.png", "Argentina.png", "BaLan.png", "Canada.png",
                                        "DanMach.png", "Duc.png","Georgia.png","HaLan.png","HoaKy.png" };
        string[] DapAnDung = new string[] {"Anh", "Ao", "Argentina", "BaLan", "Canada", "DanMach",
                                            "Duc", "Georgia", "HaLan", "HoaKy"};
        string[] DapAn1 = new string[] { "Anh", "Ao", "Duc", "BaLan", "Canada",
                                        "Ao", "Anh", "Georgia", "Argentina", "HoaKy" };
        string[] DapAn2 = new string[] { "Duc", "HaLan", "Argentina","HoaKy", "Anh",
                                        "DanMach", "Duc", "BaLan", "HaLan", "Anh"};
        int[] check = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            start.Visibility = System.Windows.Visibility.Hidden;       // an nut start
            answer1.Visibility = System.Windows.Visibility.Visible;    // hien nut answer
            answer2.Visibility = System.Windows.Visibility.Visible;
            image.Visibility = System.Windows.Visibility.Visible;       // hien anh
            cauhoi.Visibility = System.Windows.Visibility.Visible;
            countCau.Visibility = System.Windows.Visibility.Visible;
            countDiem.Visibility = System.Windows.Visibility.Visible;

            index = _generator.Next(avatars.Length);
            image.Source = new BitmapImage(new Uri(avatars[index], UriKind.Relative));
            check[index] = 1;
            answer1.Content = DapAn1[index];
            answer2.Content = DapAn2[index];
            socau++;
            countCau.Content = "Câu: " + socau + "/10";
            countDiem.Content = "Điểm: " + diem;
        }

        private void answer1_Click(object sender, RoutedEventArgs e)
        {
            countClick++;

            // kiem tra dap an co dung hay ko
            if (answer1.Content == DapAnDung[index])
            {
                diem++;
            }
            countDiem.Content = "Điểm: " + diem;

            if (10 == socau)
            {
                MessageBox.Show($"Số điểm của bạn là: {diem}");
                Close();
                return;
            }


            while (1 == check[index])
            {
                index = _generator.Next(avatars.Length);
            }
            image.Source = new BitmapImage(new Uri(avatars[index], UriKind.Relative));
            check[index] = 1;
            answer1.Content = DapAn1[index];
            answer2.Content = DapAn2[index];

            socau++;
            countCau.Content = "Câu: " + socau + "/10";  

        }

        private void answer2_Click(object sender, RoutedEventArgs e)
        {
            countClick++;

            // kiem tra dap an co dung hay ko
            if (answer2.Content == DapAnDung[index])
            {
                diem++;
            }
            countDiem.Content = "Điểm: " + diem;

            if (10 == countClick)
            {
                MessageBox.Show($"Số điểm của bạn là: {diem}");
                Close();
                return;
            }

            while (1 == check[index])
            {
                index = _generator.Next(avatars.Length);
            }
            image.Source = new BitmapImage(new Uri(avatars[index], UriKind.Relative));
            check[index] = 1;
            answer1.Content = DapAn1[index];
            answer2.Content = DapAn2[index];

            socau++;
            countCau.Content = "Câu: " + socau + "/10";
        }
    }
}
