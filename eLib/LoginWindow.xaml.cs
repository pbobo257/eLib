using eLib.Entities;
using eLib.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eLib
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        protected readonly Brush placeholderBrush;

        private readonly AppDbContext _context;
        private readonly DbSet<Account> _set;

        public LoginWindow(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            _set = _context.Set<Account>();
            placeholderBrush = LoginPlaceholder.Foreground;
        }
        

        private void LoginInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginInput.Text != string.Empty)
            {
                return;
            }
            DoubleAnimation placeholderAnimation = new DoubleAnimation();
            placeholderAnimation.From = LoginPlaceholder.ActualHeight;
            placeholderAnimation.To = LoginPlaceholder.ActualHeight+40;
            placeholderAnimation.Duration = TimeSpan.FromSeconds(0.25);
            placeholderAnimation.Completed += (sender, Args) => Panel.SetZIndex(LoginInput, 1);
            LoginPlaceholder.BeginAnimation(HeightProperty, placeholderAnimation);
            LoginPlaceholder.Foreground = Brushes.Black;
            
        }

        private void LoginInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if(LoginInput.Text != string.Empty)
            {
                return;
            }
            DoubleAnimation placeholderAnimation = new DoubleAnimation();
            placeholderAnimation.From = LoginPlaceholder.ActualHeight;
            placeholderAnimation.To = LoginPlaceholder.ActualHeight - 40;
            placeholderAnimation.Duration = TimeSpan.FromSeconds(0.25);
            LoginPlaceholder.BeginAnimation(HeightProperty, placeholderAnimation);
            Panel.SetZIndex(LoginInput, 0);
            LoginPlaceholder.Foreground = placeholderBrush;
        }

        private void PasswordInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordInput.Password != string.Empty)
            {
                return;
            }
            DoubleAnimation placeholderAnimation = new DoubleAnimation();
            placeholderAnimation.From = PasswordPlaceholder.ActualHeight;
            placeholderAnimation.To = PasswordPlaceholder.ActualHeight + 40;
            placeholderAnimation.Duration = TimeSpan.FromSeconds(0.25);
            placeholderAnimation.Completed += (sender, Args) => Panel.SetZIndex(PasswordInput, 1);
            PasswordPlaceholder.BeginAnimation(HeightProperty, placeholderAnimation);
            PasswordPlaceholder.Foreground = Brushes.Black;
        }

        private void PasswordInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordInput.Password != string.Empty)
            {
                return;
            }
            DoubleAnimation placeholderAnimation = new DoubleAnimation();
            placeholderAnimation.From = PasswordPlaceholder.ActualHeight;
            placeholderAnimation.To = PasswordPlaceholder.ActualHeight - 40;
            placeholderAnimation.Duration = TimeSpan.FromSeconds(0.25);
            PasswordPlaceholder.BeginAnimation(HeightProperty, placeholderAnimation);
            Panel.SetZIndex(PasswordInput, 0);
            PasswordPlaceholder.Foreground = placeholderBrush;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var acc = _set.FirstOrDefault(x => x.Login.Equals(LoginInput.Text));
            if (acc == null || acc.Password != PasswordInput.Password)
            {
                MessageBox.Show("Wrong login or password", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MainWindow mainWindow = new MainWindow(_context, acc);
            mainWindow.Show();
            Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
