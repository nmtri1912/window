using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;

namespace _8_Puzzle
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
        int fullImageWidth = 430;
        int padding = 5;
        
        // cac anh nho
        Image[,] images;

        //file ảnh đã load
        string fileName;

        //size anh nho
        int imageWidth;
        int imageHeight;

        //xu li timer
        public DispatcherTimer Timer = new DispatcherTimer();
        int count ;

        //Cac bien trang thai
        bool isSelectedImage = false;
        bool isStart = false;
        bool isOver = false;

        //Reset 

        public void resetGame()
        {
            isOver = true;
            isSelectedImage = false;
            isStart = false;
            Result.Children.Clear();
            Container.Children.Clear();
        }

        private void ResetTimer()
        {
            if (Timer.IsEnabled)
            {
                Timer.Tick -= Timer_Tick;
                Timer.Stop();
            }
            CountDown.Content = "";
        }



        #region mouseHandle

        bool isDragging = false;
        Image selectedImage = null;
        int newi = -1;
        int newj = -1;
        int oldi = 2;
        int oldj = 2;
        private void Container_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isStart && !isOver)
            {
                var position = e.GetPosition(this);
                selectedImage = null;

                oldi = (int)(position.X / (imageWidth + 5));
                oldj = (int)(position.Y / (imageHeight + 5));

                if (oldi < 0 && oldj < 0)
                {
                    return;
                }
                else if (oldi < 3 && oldj < 3)
                {
                    selectedImage = images[oldi, oldj];
                }

                isDragging = selectedImage != null;
            }
            else
            {
                MessageBox.Show("Nhấn Start để bắt đầu chơi");
            }
        }
        private void Container_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var newPos = e.GetPosition(this);
                // Tao do moi
                var i = (int)(newPos.X / (imageWidth + 5));
                var j = (int)(newPos.Y / (imageHeight + 5));
                if (newPos.X < 5 || newPos.Y < 5)
                {
                    isDragging = false;

                    Canvas.SetLeft(selectedImage, oldi * (imageWidth + 5));
                    Canvas.SetTop(selectedImage, oldj * (imageHeight + 5));
                    return;
                }

                if (i < 3 && j < 3)
                {
                    newi = i;
                    newj = j;

                    Canvas.SetLeft(selectedImage, newPos.X - imageWidth / 2);
                    Canvas.SetTop(selectedImage, newPos.Y - imageHeight / 2);
                }

                else
                {
                    isDragging = false;

                    Canvas.SetLeft(selectedImage, oldi * (imageWidth + 5));
                    Canvas.SetTop(selectedImage, oldj * (imageHeight + 5));

                }

            }
        }

        private void Container_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var newPos = e.GetPosition(this);
            isDragging = false;
            if (selectedImage == null)
            {
                e.Handled = true;
                return;
            }
            if (oldi < 0 && oldj < 0)
            {
                return;
            }

            if (selectedImage != null)
            {
                if (((Math.Abs(newi - oldi) == 1 && newj == oldj) || (Math.Abs(newj - oldj) == 1 && newi == oldi)) && images[newi, newj] == null)
                {
                    Canvas.SetLeft(selectedImage, newi * (imageWidth + 5));
                    Canvas.SetTop(selectedImage, newj * (imageHeight + 5));
                    images[newi, newj] = images[oldi, oldj];
                    images[oldi, oldj] = null;
                    //SwapNum(ref imgID[newj, newi], ref imgID[oldj, oldi]);
                }
                else
                {
                    Canvas.SetLeft(selectedImage, oldi * (imageWidth + 5));
                    Canvas.SetTop(selectedImage, oldj * (imageHeight + 5));
                    isDragging = false;
                }
            }

            if (checkWin())
            {
                ResetTimer();
                MessageBox.Show("Bạn đã chiến thắng");
                resetGame();
            }
        }

        #endregion mouseHandle

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "All Pictures|*.emf;*wmf;*jpg;*jpeg;*jfif;*jpe;*png;*bmp;*dib;*rle;*gif;*emz;*wmz;*pcz;*tif;*tiff;*svg;*pct;*pict;*wpg";

            if (screen.ShowDialog() == true)
            {
                //dua ve trang thai ban dau
                Result.Children.Clear();
                Container.Children.Clear();
                images = new Image[3, 3];
                oldi = 2;
                oldj = 2;

                //
                fileName = screen.FileName;
               
                var bitmap = new BitmapImage(new Uri(screen.FileName));
                var originImage = new Image();
                originImage.Source = bitmap;
                originImage.Width = 230;
                originImage.Height = 230;
                Result.Children.Add(originImage);

                var pool = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
                var pooli = new List<int> { 0, 1, 2, 0, 1, 2, 0, 1 };
                var poolj = new List<int> { 0, 0, 0, 1, 1, 1, 2, 2 };
                imageWidth = (int)fullImageWidth / 3;
                imageHeight = (int)(fullImageWidth * bitmap.Height / bitmap.Width) / 3;
                int k = 0;

                for(int i = 0; i < 8; i++)
                    {
                        var cropped = new CroppedBitmap(bitmap, new Int32Rect(
                                    (int)(pooli[k] * bitmap.PixelWidth / 3), (int)(poolj[k] * bitmap.PixelHeight / 3),
                                    ((int)bitmap.PixelWidth / 3), ((int)bitmap.PixelHeight / 3)));

                        // Tao giao dien

                        var imageView = new Image();
                        imageView.Source = cropped;
                        imageView.Width = imageWidth;
                        imageView.Height = imageHeight;
                        imageView.Tag = k+1;
                        Container.Children.Add(imageView);
                        Canvas.SetLeft(imageView, pooli[k] * (imageWidth + padding));
                        Canvas.SetTop(imageView, poolj[k] * (imageHeight + padding));
                        images[pooli[k], poolj[k]] = imageView;
                        k++;
                    }

                // trộn mảng
                Random rdn = new Random();
                int random;
                for (int i=0; i < 120; i++)
                {
                    random = rdn.Next(1, 5);
                    swapImage(random);
                }

                //Đưa về trạng thái sau khi thêm ảnh
                isSelectedImage = true;
                isStart = false;

                ResetTimer();
                count = 180;

            }
            else
            {
                MessageBox.Show("Hãy chọn 1 ảnh để bắt đầu chơi", "WARING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        private void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isSelectedImage)
            {
                MessageBox.Show("Xin vui lòng chọn ảnh trước khi bắt đầu chơi");
            }
            else if(!isStart)
            {
                Timer.Interval = new TimeSpan(0, 0, 1);
                Timer.Tick += Timer_Tick;
                Timer.Start();
                isStart = true;
                isOver = false;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(count >0)
            {
                count--;
                CountDown.Content = string.Format(count / 60 + ":" + count % 60);
            }
            else
            {
                ResetTimer();
                MessageBox.Show("Bạn đã thua :(");
                resetGame();
            }

        }

        private void LoadButtonClick(object sender, RoutedEventArgs e)
        {
            var pool = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            var pooli = new List<int> { 0, 1, 2, 0, 1, 2, 0, 1 };
            var poolj = new List<int> { 0, 0, 0, 1, 1, 1, 2, 2 };

            var screen = new OpenFileDialog();
            screen.Filter = "Text|*.txt|All|*.*";

            if (screen.ShowDialog() == true)
            {
                //đưa về trạng thái ban đầu
                var filename = screen.FileName;
                Result.Children.Clear();
                Container.Children.Clear();
                images = new Image[3, 3];
                ResetTimer();
                //

                var reader = new StreamReader(filename);
                fileName = reader.ReadLine();
                count = int.Parse(reader.ReadLine());
                var bitmap = new BitmapImage(new Uri(fileName));
                var originImage = new Image();
                originImage.Source = bitmap;
                originImage.Width = 230;
                originImage.Height = 230;
                Result.Children.Add(originImage);
                imageWidth = (int)fullImageWidth / 3;
                imageHeight = (int)(fullImageWidth * bitmap.Height / bitmap.Width) / 3;
                for (int i = 0; i < 3; i++)
                {
                    var tokens = reader.ReadLine().Split(
                        new string[] { " " }, StringSplitOptions.None);
                    // Model

                    for (int j = 0; j < 3; j++)
                    {
                        if(int.Parse(tokens[j]) != 0)
                        {
                            var cropped = new CroppedBitmap(bitmap, new Int32Rect(
                                    (int)(pooli[int.Parse(tokens[j]) - 1] * bitmap.PixelWidth / 3), (int)(poolj[int.Parse(tokens[j]) -1] * bitmap.PixelHeight / 3),
                                    ((int)bitmap.PixelWidth / 3), ((int)bitmap.PixelHeight / 3)));
                            var imageView = new Image();
                            imageView.Source = cropped;
                            imageView.Width = imageWidth;
                            imageView.Height = imageHeight;
                            imageView.Tag = int.Parse(tokens[j]);
                            Container.Children.Add(imageView);
                            Canvas.SetLeft(imageView, j * (imageWidth + padding));
                            Canvas.SetTop(imageView, i * (imageHeight + padding));
                            images[j, i] = imageView;
                        }
                        else
                        {
                            oldi = j;
                            oldj = i;
                        }
                    }
                }
                reader.Close();
                MessageBox.Show("Nạp game thành công");
                isSelectedImage = true;
                isStart = false;
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if(isStart && !isOver)
            {
                string fileNameSave = "save.txt";
                var writer = new StreamWriter(fileNameSave);
                writer.WriteLine(fileName);
                writer.WriteLine(count);
                for (int j = 0; j < 3; j++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (images[i, j] != null)
                        {
                            writer.Write(images[i, j].Tag.ToString());
                        }
                        else
                        {
                            writer.Write("0");
                        }
                        if (i == 2)
                        {
                            writer.WriteLine("");
                        }
                        else
                        {
                            writer.Write(" ");
                        }
                    }
                }
                writer.Close();
                MessageBox.Show("Lưu game thành công!");
            }
            else
            {
                MessageBox.Show("game chưa bắt đầu!!");
            }
           

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (isStart)
            {
                int type = 0;
                if (e.Key == Key.Up)
                {
                    type = 1;
                }
                else if (e.Key == Key.Down)
                {
                    type = 2;
                }
                else if (e.Key == Key.Left)
                {
                    type = 3;
                }
                else if (e.Key == Key.Right)
                {
                    type = 4;
                }
                swapImage(type);
                if (checkWin())
                {
                    ResetTimer();
                    MessageBox.Show("Bạn đã chiến thắng");
                    resetGame();
                }
            }
            else
            {
                MessageBox.Show("Nhấn Start để bắt đầu chơi");
            }
        }
        private bool checkWin()
        {
            int k = 0;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (images[i, j] != null)
                    {
                        if (int.Parse(images[i, j].Tag.ToString()) != k + 1)
                        {
                            return false;
                        }
                    }
                    else
                    {

                    }
                    k++;
                }
            }
            return true;
        }

        private void swapImage(int type)
        {
            var posI = -1;
            var posJ = -1;
            switch (type)
            {
                case 1:
                    {
                        posI = oldi;
                        posJ = oldj + 1;
                        break;
                    }
                case 2:
                    {
                        posI = oldi;
                        posJ = oldj - 1;
                        break;
                    }
                case 3:
                    {
                        posI = oldi + 1;
                        posJ = oldj;
                        break;
                    }
                case 4:
                    {
                        posI = oldi - 1;
                        posJ = oldj;
                        break;
                    }

            }
            if (posI > -1 && posI < 3 && posJ > -1 && posJ < 3)
            {
                Image isMoved = images[posI,posJ];
                Canvas.SetLeft(isMoved, oldi * (imageWidth + padding));
                Canvas.SetTop(isMoved, oldj * (imageHeight + padding));
                images[oldi, oldj] = isMoved;
                images[posI,posJ] = null;
                oldi = posI;
                oldj = posJ;
            }
            else
            {
                return;
            }
        }

       
    }
}
