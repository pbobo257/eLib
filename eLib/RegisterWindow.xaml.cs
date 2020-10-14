using eLib.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        protected readonly DbContext _context;

        public RegisterWindow(DbContext context)
        {
            InitializeComponent();
            _context = context;
        }


        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (LoginField.Text == string.Empty || MailField.Text == string.Empty
                || PasswordField.Password == string.Empty || PasswordConfirmField.Password == string.Empty)
            {
                MessageBox.Show("All fields must be filled");
                return;
            }

            if (!new EmailAddressAttribute().IsValid(MailField.Text))
            {
                MessageBox.Show("Enter valid E-Mail!");
                return;
            }

            if(PasswordField.Password != PasswordConfirmField.Password)
            {
                MessageBox.Show("Passwords must match!");
                return;
            }

            var acc = new Account()
            {
                Login = LoginField.Text,
                Email = MailField.Text,
                Password = PasswordField.Password,
                UserType = UserType.User
            };

            _context.Set<Account>().Add(acc);
            _context.SaveChanges();

            MessageBox.Show("Account created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
