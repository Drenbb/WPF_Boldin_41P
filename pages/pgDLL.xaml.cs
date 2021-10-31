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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DLLCSharp;

namespace WpfApp.pages
{
    /// <summary>
    /// Логика взаимодействия для pgDLL.xaml
    /// </summary>
    public partial class pgDLL : Page
    {
        List<users> users;
        public pgDLL()
        {
            InitializeComponent();
            users = BaseConnect.BaseModel.users.ToList();
        }

        private void btnAverage_Click(object sender, RoutedEventArgs e)
        {
            List<DateTime> dt = new List<DateTime>();
            foreach (users u in users)
            {
                dt.Add(u.dr);
            }
            MessageBox.Show("Средний возраст пользователей" + Class1.averageDate(dt));

        }

        private void btnFindName_Click(object sender, RoutedEventArgs e)
        {
            tbOutput.Text = null; 
            List<string> names = new List<string>();
            foreach (users u in users)
            {
                names.Add(u.name);
            }
            foreach (string s in Class1.findName(names, tbPathName.Text))
            {
                tbOutput.Text += s + "\n";
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoadPages.MainFrame.GoBack();
        }
    }
}
