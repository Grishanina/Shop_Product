using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для WindowLoginPassword.xaml
    /// </summary>
    public partial class WindowLoginPassword : Window
    {
        T_Users user;
        public WindowLoginPassword(T_Users user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var password = tbPassword.Password;
            Regex regex = new Regex("(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,}$");
            if (regex.IsMatch(password))
            {
                user.Login = tbLogin.Text;
                user.Password = tbPassword.Password.GetHashCode();

                ClassBase.BD.SaveChanges();
                MessageBox.Show("Данные входа изменены");
                this.Close();
            }
            else
            {
                MessageBox.Show("Пароль должен содержать: \n" +
                    " - минимум 1 заглавную латинскую букву; \n" +
                    " - минимум 3 строчные латинские буквы; \n" +
                    " - минимум 2 цыфры; \n" +
                    " - минимум 1 спец. символ (!@#$%^&*()+=); \n" +
                    " - минимум 8 символов."
                    );
            } 
        }
    }
}
