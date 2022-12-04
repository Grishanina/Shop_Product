using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class T_Users
    {
        public string Name_Surname_Patronimyc
        {
            get
            {
                return "" + Name + " " + Surname + " " + Patronymic;
            }
        }

    }
}
