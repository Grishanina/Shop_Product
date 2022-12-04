using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp1
{
	public partial class T_Product
	{
		// Изменение цвета в зависимости от скидки
		public SolidColorBrush DiscountColor
		{
			get
			{
				switch (id_discount)
				{
					case 1: // 5%
						return Brushes.LawnGreen;
					case 2: // 10%
						return Brushes.LawnGreen;
					case 3: // 15%
						return Brushes.LawnGreen;
					case 4: // 25%
						return Brushes.LawnGreen;
					case 5: // 30%
						return Brushes.LawnGreen;
					case 6: // 50%
						return Brushes.LawnGreen;
					case 7: // 0% (нет скидки)
						return Brushes.LightGoldenrodYellow;
					default:
						return Brushes.LightGoldenrodYellow;
				}
			}
		}
		public string Manufact_Provider
		{
			get
			{
				return "Производ.: " + T_Manufacturer.Manufacturer + " | " + "Постав.:" + T_Provider.Provider; 
			}
		}
	}
}
