using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для ProfileUser.xaml
    /// </summary>
    public partial class ProfileUser : Page
    {
        T_Users user;
        public ProfileUser(T_Users user)
        {
            InitializeComponent();
            this.user = user;
			tbName.Text = user.Name + " " + user.Patronymic + " " + user.Surname;
			tbBirthday.Text = Convert.ToString(user.Birthday);
			tbPol.Text = user.T_Pol.Pol;
			List<T_Userphoto> u = ClassBase.BD.T_Userphoto.Where(x => x.id_user == user.id_user).ToList();
            if (u != null)
            {
                byte[] Bar = u[u.Count - 1].photoBinary;
                showImage(Bar, imUser);
            }
        }
		private void showImage(byte[] Barray, System.Windows.Controls.Image img)
		{
			BitmapImage BI = new BitmapImage();
			using (MemoryStream m = new MemoryStream(Barray))
			{
				BI.BeginInit();
				BI.StreamSource = m;
				BI.CacheOption = BitmapCacheOption.OnLoad;
				BI.EndInit();
			}
			img.Source = BI;
			img.Stretch = Stretch.Uniform;
		}
		

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			List<T_Userphoto> u = ClassBase.BD.T_Userphoto.Where(x => x.id_user == user.id_user).ToList();
			n--;
			if (Next.IsEnabled == false)
			{
				Next.IsEnabled = true;
			}
			if (u != null) 
			{

				byte[] Bar = u[n].photoBinary;  
				BitmapImage BI = new BitmapImage();  
				showImage(Bar, imgGallery);
			}
			if (n == 0)
			{
				Back.IsEnabled = false;
			}
		}

		private void Next_Click(object sender, RoutedEventArgs e)
		{
			List<T_Userphoto> u = ClassBase.BD.T_Userphoto.Where(x => x.id_user == user.id_user).ToList();
			n++;
			if (Back.IsEnabled == false)
			{
				Back.IsEnabled = true;
			}
			if (u != null)
			{

				byte[] Bar = u[n].photoBinary;
				showImage(Bar, imgGallery);
			}
			if (n == u.Count - 1)
			{
				Next.IsEnabled = false;
			}
		}

		private void btnOld_Click(object sender, RoutedEventArgs e)
		{
			List<T_Userphoto> u = ClassBase.BD.T_Userphoto.Where(x => x.id_user == user.id_user).ToList();
			byte[] Bar = u[n].photoBinary;
			showImage(Bar, imUser);
		}

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
			WindowProfile windowProfile = new WindowProfile(user);
			windowProfile.ShowDialog();
			Class1.Mfrm.Navigate(new ProfileUser(user));
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			try
			{
				T_Userphoto u = new T_Userphoto();
				u.id_user = user.id_user;

				OpenFileDialog OFD = new OpenFileDialog();
				OFD.ShowDialog();
				string path = OFD.FileName;
				System.Drawing.Image SDI = System.Drawing.Image.FromFile(path);
				ImageConverter IC = new ImageConverter();
				byte[] Barray = (byte[])IC.ConvertTo(SDI, typeof(byte[]));
				u.photoBinary = Barray;
				ClassBase.BD.T_Userphoto.Add(u);
				ClassBase.BD.SaveChanges();
				MessageBox.Show("Фото добавлено");
				Class1.Mfrm.Navigate(new ProfileUser(user));

			}
			catch
			{
				MessageBox.Show("ОШИБКА");
			}
		}

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
			try
			{
				OpenFileDialog OFD = new OpenFileDialog();  
				OFD.Multiselect = true;  
				if (OFD.ShowDialog() == true)  
				{
					foreach (string file in OFD.FileNames)  
					{
						T_Userphoto u = new T_Userphoto();  
						u.id_user = user.id_user;  
						string path = file;  
						System.Drawing.Image SDI = System.Drawing.Image.FromFile(file);  
						ImageConverter IC = new ImageConverter();  
						byte[] Barray = (byte[])IC.ConvertTo(SDI, typeof(byte[]));  
						u.photoBinary = Barray; 
						ClassBase.BD.T_Userphoto.Add(u); 
					}
					ClassBase.BD.SaveChanges();
					MessageBox.Show("Фото добавлены");
				}
			}
			catch
			{
				MessageBox.Show("ОШИБКА");
			}
		}

		int n = 0;

		private void Button_Click_6(object sender, RoutedEventArgs e)
        {
			spGallery.Visibility = Visibility.Visible;
			List<T_Userphoto> u = ClassBase.BD.T_Userphoto.Where(x => x.id_user == user.id_user).ToList();
			if (u != null)  
			{

				byte[] Bar = u[n].photoBinary;   
				showImage(Bar, imgGallery); 
			}
		}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
			WindowLoginPassword windowLoginPassword = new WindowLoginPassword(user);
			windowLoginPassword.ShowDialog();
			Class1.Mfrm.Navigate(new ProfileUser(user));
		}
    }
}

