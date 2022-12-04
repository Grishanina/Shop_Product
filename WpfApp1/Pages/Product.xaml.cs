using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Product.xaml
    /// </summary>
    public partial class Product : Page
    {
		PageChange pc = new PageChange();  // создаем объект класса для отображения страниц
		List<T_Product> CatsFilter = new List<T_Product>();
		public Product()
        {
            InitializeComponent();
            ClassBase.BD = new Entities14();
			CatsFilter = ClassBase.BD.T_Product.ToList();
            listProduct.ItemsSource = ClassBase.BD.T_Product.ToList();
            pc.CountPage = ClassBase.BD.T_Product.ToList().Count;
			DataContext = pc;  // добавляем объект для отображения страниц

            List<T_Type> TT = ClassBase.BD.T_Type.ToList();

            // программное заполнение выпадающего списка
            cmbType.Items.Add("Все типы товаров");  // первый элемент выпадающего списка, который сбрасывает фильтрацию
            for (int i = 0; i < TT.Count; i++)  // цикл для записи в выпадающий список всех типов оваров из БД
            {
                cmbType.Items.Add(TT[i].Type);
            }

            cmbType.SelectedIndex = 0;  // выбранное по умолчанию значение в списке с типами товаров ("Все типы товаров")
            cmbSort.SelectedIndex = 0;  // выбранное по умолчанию значение в списке с видами сортировки ("Без сортировки")

            tbCount.Text = "По данным запросам найдено количество записей: " + ClassBase.BD.T_Product.ToList().Count;

		}


		// Список состава продукта
        private void SostavPR_Loaded(object sender, RoutedEventArgs e)
        {
			TextBlock tb = (TextBlock)sender;
			int index = Convert.ToInt32(tb.Uid);

			List<T_Sostav_Product> TC = ClassBase.BD.T_Sostav_Product.Where(x => x.id_product == index).ToList();

			string str = "";

			foreach (T_Sostav_Product tc in TC)
			{
				str += tc.T_Sostav.Sostav + ", ";
			}

			tb.Text = "Состав: " + str.Substring(0, str.Length - 2) + ".";
		}


		// Расчет цены товара с учетом скидки (и без нее)
        private void PricePT_Loaded(object sender, RoutedEventArgs e)
        {
			TextBlock tb = (TextBlock)sender;
			int index = Convert.ToInt32(tb.Uid);

			List<T_Product> TP = ClassBase.BD.T_Product.Where(x => x.id_product == index).ToList();

			double sum = 0;
			double pr = 0;

			foreach (T_Product prd in TP)
			{
				sum += Convert.ToDouble(prd.Price - (prd.Price * (prd.T_Discount.Discount / 100)));
				pr += Convert.ToDouble(prd.Price);
			}

			tb.Text = "Цена: " + sum.ToString("F2") + " руб. (Без скидки: " + pr.ToString("F2") + " руб.)";
		}


		// Ограничение знаков после запятой у значения скидки
		private void DiscountPT_Loaded(object sender, RoutedEventArgs e)
		{
			TextBlock tb = (TextBlock)sender;
			int index = Convert.ToInt32(tb.Uid);

			List<T_Product> TP = ClassBase.BD.T_Product.Where(x => x.id_product == index).ToList();

			int dis = 0;

			foreach (T_Product p in TP)
			{

				dis += Convert.ToInt32(p.T_Discount.Discount);
			}

			tb.Text = "Скидка: " + dis.ToString() + " %";
		}

		private void btnCreateProduct_Click(object sender, RoutedEventArgs e)
		{
			Class1.Mfrm.Navigate(new CreateProduct());
		}

		private void btnupdate_Click(object sender, RoutedEventArgs e)
		{
			Button btn = (Button)sender;  // получаем доступ к Button из шаблона
			int index = Convert.ToInt32(btn.Uid);   // получаем числовой Uid элемента списка (в разметке предварительно нужно связать номер ячейки с номером кота в базе данных)

			// создаем объект, который содержит кота, информацию о котором нужно отредактировать
			T_Product product = ClassBase.BD.T_Product.FirstOrDefault(x => x.id_product == index);

			// переход на страницу с редактированием (на ту же самую, где и добавляли кота)
			Class1.Mfrm.Navigate(new CreateProduct(product));
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			Button btn = (Button)sender;  // получаем доступ к Button из шаблона
			int index = Convert.ToInt32(btn.Uid);

			T_Product product = ClassBase.BD.T_Product.FirstOrDefault(x => x.id_product == index);

			ClassBase.BD.T_Product.Remove(product); // удаление кота из базы            
			ClassBase.BD.SaveChanges();  // сохранение изменений в базе данных

			Class1.Mfrm.Navigate(new Product());
		}

        void Filter()  // метод для одновременной фильтрации, поиска и сортировки
        {
            List<T_Product> productList = new List<T_Product>();  // пустой список, который далее будет заполнять элементами, удавлетворяющими условиям фильтрации, поиска и сортировки

            string type = cmbType.SelectedValue.ToString();  // выбранное пользователем название типа
            int index = cmbType.SelectedIndex;

            // поиск значений, удовлетворяющих условия фильтра
            if (index != 0)
            {
                productList = ClassBase.BD.T_Product.Where(x => x.T_Type.Type == type).ToList();
            }
            else  // если выбран пункт "Все типы товаров", то сбрасываем фильтрацию:
            {
                productList = ClassBase.BD.T_Product.ToList();
            }


            // поиск совпадений по названию продукта
            if (!string.IsNullOrWhiteSpace(tbSearch.Text))  // если строка не пустая и если она не состоит из пробелов
            {
                productList = productList.Where(x => x.Title.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
            }


            // выбор элементов только со скидкой
            if (cbPhoto.IsChecked == true)
            {
                productList = productList.Where(x => x.T_Discount.Discount != 0).ToList();
            }

            // сортировка
            switch (cmbSort.SelectedIndex)
            {
                case 1:
                    {
                        productList.Sort((x, y) => x.Price.CompareTo(y.Price));
                    }
                    break;
                case 2:
                    {
                        productList.Sort((x, y) => x.Price.CompareTo(y.Price));
                        productList.Reverse();
                    }
                    break;
            }

            listProduct.ItemsSource = productList;
            if (productList.Count == 0)
            {
                MessageBox.Show("нет записей");
            }
            tbCount.Text = "По данным запросам найдено количество записей: " + productList.Count;
        }

        // далее во всех обработчиках событий применяем один и тот же метод Filter, который позволяет находить условия, удовлетворяющие всем сразу выбранным параметрам

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cbPhoto_Checked(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

		private void txtPageCount_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				pc.CountPage = Convert.ToInt32(txtPageCount.Text); // если в текстовом поле есnь значение, присваиваем его свойству объекта, которое хранит количество записей на странице
			}
			catch
			{
				pc.CountPage = CatsFilter.Count; // если в текстовом поле значения нет, присваиваем свойству объекта, которое хранит количество записей на странице количество элементов в списке
			}
			pc.Countlist = CatsFilter.Count;  // присваиваем новое значение свойству, которое в объекте отвечает за общее количество записей
			listProduct.ItemsSource = CatsFilter.Skip(0).Take(pc.CountPage).ToList();  // отображаем первые записи в том количестве, которое равно CountPage
			pc.CurrentPage = 1; // текущая страница - это страница 1
		}

		private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)  // обработка нажатия на один из Textblock в меню с номерами страниц
		{
			TextBlock tb = (TextBlock)sender;

			switch (tb.Uid)  // определяем, куда конкретно было сделано нажатие
			{
				case "prev":
					pc.CurrentPage--;
					break;
				case "next":
					pc.CurrentPage++;
					break;
				default:
					pc.CurrentPage = Convert.ToInt32(tb.Text);
					break;
			}
			listProduct.ItemsSource = CatsFilter.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();  // оображение записей постранично с определенным количеством на каждой странице
																															 // Skip(pc.CurrentPage* pc.CountPage - pc.CountPage) - сколько пропускаем записей
																															 // Take(pc.CountPage) - сколько записей отображаем на странице
		}

		private void btn_Click(object sender, RoutedEventArgs e)
		{
			pc.CurrentPage = 1;

			try
			{
				pc.CountPage = Convert.ToInt32(txtPageCount.Text); // если в текстовом поле есnь значение, присваиваем его свойству объекта, которое хранит количество записей на странице
			}
			catch
			{
				pc.CountPage = CatsFilter.Count; // если в текстовом поле значения нет, присваиваем свойству объекта, которое хранит количество записей на странице количество элементов в списке
			}
			pc.Countlist = CatsFilter.Count;  // присваиваем новое значение свойству, которое в объекте отвечает за общее количество записей
			listProduct.ItemsSource = CatsFilter.Skip(0).Take(pc.CountPage).ToList();  // отображаем первые записи в том количестве, которое равно CountPage
		}
	}
}
