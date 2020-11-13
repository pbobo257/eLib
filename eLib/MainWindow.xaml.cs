using eLib.Entities;
using eLib.Infrastructure;
using eLib.Logic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Grid libGrid;

        protected readonly IAppService _service;

        private string searchValue = string.Empty;
        private string genreValue = string.Empty;
        private string yearValue = string.Empty;

        public MainWindow(IAppService service, Account acc)
        {
            InitializeComponent();
            if(acc.UserType == UserType.User)
            {
                AddButton.Visibility = Visibility.Collapsed;
            }
            _service = service;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(1);
        }

        private void LibViewer_Loaded(object sender, RoutedEventArgs e)
        {
            renderLibGrid(searchValue, genreValue, yearValue);
        }

        private Grid GenerateLibGrid(int columnCount, double rowCount)
        {
            libGrid = new Grid();
            var columnSize = LibViewer.ActualWidth / columnCount;
            for (int i = 0; i < columnCount; i++)
            {
                var column = new ColumnDefinition
                {
                    Width = new GridLength(columnSize)
                };

                libGrid.ColumnDefinitions.Add(column);
            }


            for (int i = 0; i < rowCount; i++)
            {
                var row = new RowDefinition();
                {
                    Height = 400;
                };

                libGrid.RowDefinitions.Add(row);
            }

            return libGrid;
        }

        private Border GenerateBookItem(BookHeader bookHeader, int index, int columnCount)
        {
            var book = new Border
            {
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(20),
                BorderBrush = Brushes.Black,
                Margin = new Thickness(5),
                Height = 300,
                Width = 200,
                Cursor = Cursors.Hand,
                ToolTip = bookHeader.Id.ToString() + ' ' + bookHeader.Name
            };
            book.MouseLeftButtonDown += Border_MouseLeftButtonDown;

            var bitmap = new BitmapImage();
            using (var ms = new System.IO.MemoryStream(bookHeader.Cover))
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

            book.Background = image;

            if (index == 0)
            {
                Grid.SetRow(book, 0);
                Grid.SetColumn(book, 0);
            }
            else
            {
                var row = (int)Math.Floor(index / (double)columnCount);
                var column = (index % columnCount == 0) ? 0 : (index % columnCount);
                Grid.SetRow(book, row);
                Grid.SetColumn(book, column);
            }

            return book;
        }

        private void renderLibGrid(string searchString, string genreString, string yearString)
        {
            var bookList = _service.GetBooks();

            var columnCount = Convert.ToInt32(Math.Floor(LibViewer.ActualWidth / 200));
            var rowCount = (Math.Ceiling((double)bookList.Count() / columnCount) == 1) ? 2 : Math.Ceiling((double)bookList.Count() / columnCount);

            libGrid = GenerateLibGrid(columnCount, rowCount);

            if(searchString != string.Empty)
            {
                bookList = bookList.FindAll(x => x.Name == searchString);
            }

            if (genreString != string.Empty)
            {
                bookList = bookList.FindAll(x => x.Details.Genre == genreString);
            }

            if(yearString != string.Empty)
            {
                bookList = bookList.FindAll(x => x.Details.ReleaseDate.Year.ToString() == yearString);
            }

            for (int i = 0; i < bookList.Count(); i++)
            {
                var border = GenerateBookItem(bookList[i], i, columnCount);

                libGrid.Children.Add(border);
            }

            LibViewer.Content = libGrid;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = (Border)sender;
            var id = Convert.ToInt32(border.ToolTip.ToString().Split(' ')[0]);
            var book = _service.GetBookById(id);
            var bookView = new ViewBookWindow(book);
            bookView.ShowDialog();
        }

        private void LibViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderLibGrid(searchValue, genreValue, yearValue);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBookWindow = new AddBookWindow(_service);
            addBookWindow.ShowDialog();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchValue = searchField.Text;
            renderLibGrid(searchValue, genreValue, yearValue);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var books = _service.GetAllDetails();

            var genres = books.Select(x => x.Genre).Distinct().ToList();

            genres.Sort();
            genres.Insert(0, "Genre");
            var years = books.Select(x => x.ReleaseDate.Year.ToString()).Distinct().ToList();

            years.Sort();
            years.Insert(0, "Year");

            GenreList.ItemsSource = genres;
            YearList.ItemsSource = years;
            
            GenreList.SelectedIndex = 0;
            YearList.SelectedIndex = 0;
        }

        private void GenreList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = (ComboBox)sender;

            genreValue = combobox.SelectedItem.ToString() == "Genre" ? string.Empty : combobox.SelectedItem.ToString();

            renderLibGrid(searchValue, genreValue, yearValue);
        }

        private void YearList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = (ComboBox)sender;

            yearValue = combobox.SelectedItem.ToString() == "Year" ? string.Empty : combobox.SelectedItem.ToString();

            renderLibGrid(searchValue, genreValue, yearValue);
        }
    }
}
