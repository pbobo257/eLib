using eLib.Entities;
using eLib.Infrastructure;
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

        protected readonly AppDbContext _context;
        protected readonly DbSet<BookHeader> _headerSet;
        protected readonly DbSet<BookDetails> _detailsSet;

        private string searchValue = string.Empty;
        private string genreValue = string.Empty;
        private string yearValue = string.Empty;

        public MainWindow(AppDbContext context, Account acc)
        {
            InitializeComponent();
            if(acc.UserType == UserType.User)
            {
                AddButton.Visibility = Visibility.Collapsed;
            }
            _context = context;
            _headerSet = _context.Set<BookHeader>();
            _detailsSet = _context.Set<BookDetails>();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(1);
        }

        private void LibViewer_Loaded(object sender, RoutedEventArgs e)
        {
            renderLibGrid(searchValue, genreValue, yearValue);
        }

        private void renderLibGrid(string searchString, string genreString, string yearString)
        {
            libGrid = new Grid();
            var columnCount = Convert.ToInt32(Math.Floor(LibViewer.ActualWidth / 200));
            var columnSize = LibViewer.ActualWidth / columnCount;
            for (int i = 0; i < columnCount; i++)
            {
                var column = new ColumnDefinition
                {
                    Width = new GridLength(columnSize)
                };

                libGrid.ColumnDefinitions.Add(column);
            }

            var rowCount = (Math.Ceiling((double)_headerSet.Count() / columnCount) == 1) ? 2 : Math.Ceiling((double)_headerSet.Count() / columnCount);

            for (int i = 0; i < rowCount; i++)
            {
                var row = new RowDefinition();
                {
                    Height = 400;
                };

                libGrid.RowDefinitions.Add(row);
            }

            var bookList = _headerSet.Where(x => x.Name.Contains(searchString)).Include(x=>x.Details).ToList();

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
                var border = new Border
                {
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(20),
                    BorderBrush = Brushes.Black,
                    Margin = new Thickness(5),
                    Height = 300,
                    Width = 200,
                    Cursor = Cursors.Hand,
                    ToolTip = bookList[i].Id.ToString()+' '+bookList[i].Name
                };
                border.MouseLeftButtonDown += Border_MouseLeftButtonDown;

                var bitmap = new BitmapImage();
                using (var ms = new System.IO.MemoryStream(bookList[i].Cover))
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

                border.Background = image;

                if (i == 0)
                {
                    Grid.SetRow(border, 0);
                    Grid.SetColumn(border, 0);
                }
                else
                {
                    var row = (int)Math.Floor(i / (double)columnCount);
                    var column = (i % columnCount == 0) ? 0 : (i % columnCount) ;
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, column);
                }

                libGrid.Children.Add(border);
            }

            LibViewer.Content = libGrid;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = (Border)sender;
            var id = Convert.ToInt32(border.ToolTip.ToString().Split(' ')[0]);
            var book = _headerSet.Where(x => x.Id.Equals(id)).Include(b => b.Details).FirstOrDefault();
            var bookView = new ViewBookWindow(book);
            bookView.ShowDialog();
        }

        private void LibViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderLibGrid(searchValue, genreValue, yearValue);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBookWindow = new AddBookWindow(_context);
            addBookWindow.ShowDialog();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchValue = searchField.Text;
            renderLibGrid(searchValue, genreValue, yearValue);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var books = _detailsSet.ToList();
            GenreList.Items.Add("Genre");
            YearList.Items.Add("Year");
            foreach (var book in books)
            {
                if (!GenreList.Items.Contains(book.Genre))
                {
                    GenreList.Items.Add(book.Genre);
                }
                if (!YearList.Items.Contains(book.ReleaseDate.Year.ToString()))
                {
                    YearList.Items.Add(book.ReleaseDate.Year.ToString());
                }
            }

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
