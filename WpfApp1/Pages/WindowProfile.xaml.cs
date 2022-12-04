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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
	/// <summary>
	/// Логика взаимодействия для WindowProfile.xaml
	/// </summary>
	public partial class WindowProfile : Window
	{
		T_Users user;
		public WindowProfile(T_Users user)
		{
			InitializeComponent();
			this.user = user;
			tbName.Text = user.Name;
			tbSurname.Text = user.Surname;
			tbPatronymic.Text = user.Patronymic;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			int g = 0;
			if (rbMen.IsChecked == true) g = 1;
			if (rbWomen.IsChecked == true) g = 2;

			user.Name = tbName.Text;  
			user.Surname = tbSurname.Text;
			user.Patronymic = tbPatronymic.Text;
			user.Birthday = Convert.ToDateTime(tbBirthday.SelectedDate);
			user.id_pol = g;
			ClassBase.BD.SaveChanges();
			MessageBox.Show("Личные данные изменены");
			this.Close();  
		}
	}
}
