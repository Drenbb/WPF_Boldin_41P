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
//using DLLCSharp;

namespace WpfApp.pages
{
    /// <summary>
    /// Логика взаимодействия для pgUsersToList.xaml
    /// </summary>
    public partial class pgUsersToList : Page
    {
        List<users> users;
        List<users> listUsers;
        PageChange pc = new PageChange();
        public pgUsersToList()
        {
            InitializeComponent();
            pgPanel.Visibility = Visibility.Collapsed;
            users = BaseConnect.BaseModel.users.ToList();
            lbUsers.ItemsSource = users;
            listUsers = users;
            List<genders> genders = BaseConnect.BaseModel.genders.ToList();
            cbGenderS.ItemsSource = genders;
            cbGenderS.SelectedValuePath = "id";
            cbGenderS.DisplayMemberPath = "gender";
            DataContext = pc;

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

        private void Sort(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbStart.Text != "" && tbFinish.Text != "")
                {
                    int start = Convert.ToInt32(tbStart.Text) - 1;
                    int finish = Convert.ToInt32(tbFinish.Text);
                    listUsers = users.Skip(start).Take(finish - start).ToList();
                }
                if (dpDate.SelectedDate != null)
                    listUsers = listUsers.Where(x => x.dr == (DateTime)dpDate.SelectedDate).ToList();
                if (cbGenderS.SelectedIndex != -1)
                    listUsers = listUsers.Where(x => x.gender == Convert.ToInt32(cbGenderS.SelectedValue)).ToList();
                if (tbInputName.Text != "")
                    listUsers = listUsers.Where(x => x.name.Contains(tbInputName.Text)).ToList();
            }
            catch (Exception d) { MessageBox.Show(d.Message); }
            lbUsers.ItemsSource = listUsers;
            pc.Countlist = listUsers.Count;
        }

        private void btnRset_Click(object sender, RoutedEventArgs e)
        {
            cbGenderS.SelectedItem = null;
            dpDate.Text = null;
            tbStart.Text = null;
            tbFinish.Text = null;
            lbUsers.ItemsSource = users;
            listUsers = users;
            txtPageCount.Text = null;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.Uid);
            auth CurrentUser = BaseConnect.BaseModel.auth.FirstOrDefault(x => x.id == id);
            BaseConnect.BaseModel.auth.Remove(CurrentUser);
            BaseConnect.BaseModel.SaveChanges();
            MessageBox.Show("Пользователь успешно удален!");
            users = BaseConnect.BaseModel.users.ToList();
            lbUsers.ItemsSource = users;

        }

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            LoadPages.MainFrame.Navigate(new reg(2));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoadPages.MainFrame.Navigate(new login());
        }

        private void btnDLL_Click(object sender, RoutedEventArgs e)
        {
            LoadPages.MainFrame.Navigate(new pgDLL());
        }




        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;//определяем, какой текстовый блок был нажат           
                                             //изменение номера страници при нажатии на кнопку
            switch (tb.Uid)
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


            //определение списка
            lbUsers.ItemsSource = listUsers.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();

            txtCurrentPage.Text = "Текущая страница: " + (pc.CurrentPage).ToString();

        }

        private void txtPageCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtPageCount.Text != "")
                {
                    pc.CountPage = Convert.ToInt32(txtPageCount.Text);
                    pgPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    pgPanel.Visibility = Visibility.Collapsed;
                    txtCurrentPage.Visibility = Visibility.Collapsed;
                }
                
            }
            catch
            {
                pc.CountPage = listUsers.Count;
            }
            pc.Countlist = users.Count;
            lbUsers.ItemsSource = listUsers.Skip(0).Take(pc.CountPage).ToList();
            pc.CurrentPage = 1;
        }
    }
}
