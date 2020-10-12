using eLib.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace eLib
{
    /// <summary>
    /// Interaction logic for ViewBookWindow.xaml
    /// </summary>
    public partial class ViewBookWindow : Window
    {
        protected readonly BookHeader _header;
        protected readonly BookDetails _details;
        private string _path;
        public ViewBookWindow(BookHeader header)
        {
            InitializeComponent();
            _header = header;
            _details = _header.Details;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ReadBtn_Click(object sender, RoutedEventArgs e)
        {
            var path = _header.Name + ".pdf";
            File.WriteAllBytes(path, _details.Book);
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            };
            p.Start();

            _path = path;

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var bitmap = new BitmapImage();
            using (var ms = new System.IO.MemoryStream(_header.Cover))
            {

                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();

            }

            var image = new ImageBrush
            {
                ImageSource = bitmap,
                Stretch = Stretch.Fill
            };

            ImageContainer.Background = image;

            NameField.Text = _header.Name;
            AuthorField.Text = _details.Author;
            GenreField.Text = _details.Genre;
            DateField.Text = _details.ReleaseDate.ToShortDateString();
            DescriptionField.Text = _details.Description;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
        }
    }
}
