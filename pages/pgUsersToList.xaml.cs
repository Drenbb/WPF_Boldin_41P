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

namespace WpfApp.pages
{
    /// <summary>
    /// Логика взаимодействия для pgUsersToList.xaml
    /// </summary>
    public partial class pgUsersToList : Page
    {
        List<users> users;
        public pgUsersToList()
        {
            InitializeComponent();
            users = BaseConnect.BaseModel.users.ToList();
            lbUsers.ItemsSource = users;
            List<genders> genders = BaseConnect.BaseModel.genders.ToList();
            cbGenderS.ItemsSource = genders;
            cbGenderS.SelectedValuePath = "id";
            cbGenderS.DisplayMemberPath = "gender";
           
        }

        private void lbTraits_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            int id = Convert.ToInt32(lb.Uid);
            lb.ItemsSource = BaseConnect.BaseModel.users_to_traits.Where(x => x.id_user == id).ToList();
            lb.DisplayMemberPath = "traits.trait";
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.Uid);
            auth CurrentUser = BaseConnect.BaseModel.auth.FirstOrDefault(x => x.id == id);
            LoadPages.MainFrame.Navigate(new ChangeUser(CurrentUser));
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            List<users> listUsers = users;

            if (tbStart.Text != "" && tbFinish.Text != "")
            {
                int start = Convert.ToInt32(tbStart.Text) - 1;
                int finish = Convert.ToInt32(tbFinish.Text);
                listUsers = users.Skip(start).Take(finish - start).ToList();

            }
            if (dpDate.SelectedDate != null)
            {
                listUsers = users.Where(x => x.dr == (DateTime)dpDate.SelectedDate).ToList();
            }
            

            if (cbGenderS.SelectedItem != null)
            {
                if (cbGenderS.SelectedValue.ToString()=="1")
                {
                    listUsers = users.Where(x => x.gender ==1).ToList();
                }
                if (cbGenderS.SelectedValue.ToString() == "2")
                {
                    listUsers = users.Where(x => x.gender == 2).ToList();
                }
                if (cbGenderS.SelectedValue.ToString() == "3")
                {
                    listUsers = users.Where(x => x.gender == 3).ToList();
                }
            }
            lbUsers.ItemsSource = listUsers;
        }

        private void btnRset_Click(object sender, RoutedEventArgs e)
        {
            lbUsers.ItemsSource = users;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.Uid);
            auth CurrentUser = BaseConnect.BaseModel.auth.FirstOrDefault(x => x.id == id);
            BaseConnect.BaseModel.auth.Remove(CurrentUser);
            BaseConnect.BaseModel.SaveChanges();
            MessageBox.Show("Пользователь успешно удален!");
        }

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            LoadPages.MainFrame.Navigate(new reg());
        }
    }
}
