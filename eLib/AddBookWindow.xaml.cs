using eLib.Entities;
using eLib.Entities.Exceptions;
using eLib.Infrastructure;
using eLib.Logic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        private readonly IAppService _service;
        private byte[] Cover;
        private byte[] Book;

        public AddBookWindow(IAppService service)
        {
            InitializeComponent();

            _service = service;
        }

        private void LoadImageBtn_Click(object sender, RoutedEventArgs e)
        {
            var bitmap = new BitmapImage();
            MemoryStream ms = new MemoryStream();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image files(*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            string fileName = "";
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fileName, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();

            var image = new ImageBrush
            {
                ImageSource = bitmap,
                Stretch = Stretch.Fill
            };
            ImageContainer.Background = image;


            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                file.CopyTo(ms);
            }

            Cover = ms.ToArray();

            ms.Close();
            ms.Dispose();


        }

        private void LoadBookBtn_Click(object sender, RoutedEventArgs e)
        {
            MemoryStream ms = new MemoryStream();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Book files(*.pdf) | *.pdf";
            string fileName = "";
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            FileNameField.Text = System.IO.Path.GetFileName(fileName);

            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                file.CopyTo(ms);
            }

            Book = ms.ToArray();

            ms.Close();
            ms.Dispose();

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _service.CreateBook(BookNameInput.Text, AuthorInput.Text, GenreInput.Text, ReleaseDatePicker.SelectedDate, DescriptionInput.Text, Cover, Book);
                MessageBox.Show("Saved");
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
