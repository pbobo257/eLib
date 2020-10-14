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
            OnGotFocusPlaceholderAnim(LoginPlaceholder);
            
        }

        private void LoginInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if(LoginInput.Text != string.Empty)
            {
                return;
            }
            OnLostFocusPlaceholderAnim(LoginPlaceholder);
        }

        private void PasswordInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordInput.Password != string.Empty)
            {
                return;
            }
            OnGotFocusPlaceholderAnim(PasswordPlaceholder);
        }

        private void PasswordInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordInput.Password != string.Empty)
            {
                return;
            }
            OnLostFocusPlaceholderAnim(PasswordPlaceholder);
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

        void OnGotFocusPlaceholderAnim(TextBlock placeholder)
        {
            DoubleAnimation placeholderAnimation = new DoubleAnimation
            {
                From = placeholder.ActualHeight,
                To = placeholder.ActualHeight + 40,
                Duration = TimeSpan.FromSeconds(0.25)
            };
            placeholder.BeginAnimation(HeightProperty, placeholderAnimation);
            placeholder.Foreground = Brushes.Black;
        }

        void OnLostFocusPlaceholderAnim(TextBlock placeholder)
        {
            DoubleAnimation placeholderAnimation = new DoubleAnimation
            {
                From = placeholder.ActualHeight,
                To = placeholder.ActualHeight - 40,
                Duration = TimeSpan.FromSeconds(0.25)
            };
            placeholder.BeginAnimation(HeightProperty, placeholderAnimation);
            placeholder.Foreground = placeholderBrush;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(_context);
            registerWindow.ShowDialog();
        }
    }
}
