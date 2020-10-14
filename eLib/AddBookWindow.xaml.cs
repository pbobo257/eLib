using eLib.Entities;
using eLib.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly AppDbContext _context;
        private readonly DbSet<BookHeader> _headersSet;
        private BookHeader newHeader = new BookHeader();
        private BookDetails newDetails = new BookDetails();

        public AddBookWindow(AppDbContext context)
        {
            InitializeComponent();

            _context = context;
            _headersSet = _context.Set<BookHeader>();
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

            newHeader.Cover = ms.ToArray();

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

            newDetails.Book = ms.ToArray();

            ms.Close();
            ms.Dispose();

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(BookNameInput.Text) || string.IsNullOrEmpty(AuthorInput.Text)
                || string.IsNullOrEmpty(GenreInput.Text) || ReleaseDatePicker.SelectedDate == null
                || string.IsNullOrEmpty(DescriptionInput.Text) || newHeader.Cover == null 
                || newDetails.Book == null)
            {
                MessageBox.Show("Fill All Fields!!!");
                return;
            }
            newHeader.Name = BookNameInput.Text;

            newDetails.Author = AuthorInput.Text;
            newDetails.Genre = GenreInput.Text;
            newDetails.Description = DescriptionInput.Text;
            newDetails.ReleaseDate = (DateTime)ReleaseDatePicker.SelectedDate;

            newHeader.Details = newDetails;
            _headersSet.Add(newHeader);
            _context.SaveChanges();


            MessageBox.Show("Saved");
            Close();
        }
    }
}
