using eLib.Entities;
using eLib.Entities.Exceptions;
using eLib.Logic;
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
        protected readonly IAppService _service;

        public RegisterWindow(IAppService service)
        {
            InitializeComponent();
            _service = service;
        }


        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _service.CreateAccount(LoginField.Text, MailField.Text, PasswordField.Password, PasswordConfirmField.Password);

                MessageBox.Show("Account created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
