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
    /// Логика взаимодействия для pgUsersToList.xaml
    /// </summary>
    public partial class pgUsersToList : Page
    {
        List<users> users;
        List<users> listUsers;
        public pgUsersToList()
        {
            InitializeComponent();
            spPages.Visibility = Visibility.Hidden;
            users = BaseConnect.BaseModel.users.ToList();
            lbUsers.ItemsSource = users;
            listUsers = users;
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

        private void Sort(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(listUsers.Count.ToString());
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
        }

        private void btnRset_Click(object sender, RoutedEventArgs e)
        {
            cbGenderS.SelectedItem = null;
            dpDate.Text = null;
            tbStart.Text = null;
            tbFinish.Text = null;
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



        int currpg = 1;
        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TextBlock tb = (TextBlock)sender;
                int countList = users.Count;
                int countzap = Convert.ToInt32(txtPageCount.Text);
                int countpage = countList / countzap;
                txtNext.Visibility = Visibility.Visible;
                txtPrev.Visibility = Visibility.Visible;
                switch (tb.Uid)
                {
                    case "prev":
                        currpg--;
                        break;
                    case "1":
                        if (currpg < 3) currpg = 1;
                        else if (currpg > countpage) currpg = countpage - 4;
                        else currpg -= 2;
                        break;
                    case "2":
                        if (currpg < 3) currpg = 2;
                        else if (currpg > countpage) currpg = countpage - 3;
                        else currpg--;
                        break;
                    case "3":
                        if (currpg < 3) currpg = 3;
                        else if (currpg > countpage) currpg = countpage - 2;
                        break;
                    case "4":
                        if (currpg < 3) currpg = 4;
                        else if (currpg > countpage) currpg = countpage - 1;
                        else currpg++;
                        break;
                    case "5":
                        if (currpg < 3) currpg = 5;
                        else if (currpg > countpage) currpg = countpage;
                        else currpg += 2;
                        break;
                    case "next":
                        currpg++;
                        break;
                    default:
                        currpg = 1;
                        break;
                }

                //отрисовка
                if (currpg < 3)
                {
                    txt1.Text = " 1 ";
                    txt2.Text = " 2 ";
                    txt3.Text = " 3 ";
                    txt4.Text = " 4 ";
                    txt5.Text = " 5 ";
                }
                else if (currpg > countpage - 2)
                {
                    txt1.Text = " " + (countpage - 4).ToString() + " ";
                    txt2.Text = " " + (countpage - 3).ToString() + " ";
                    txt3.Text = " " + (countpage - 2).ToString() + " ";
                    txt4.Text = " " + (countpage - 1).ToString() + " ";
                    txt5.Text = " " + (countpage).ToString() + " ";

                }
                else
                {
                    txt1.Text = " " + (currpg - 2).ToString() + " ";
                    txt2.Text = " " + (currpg - 1).ToString() + " ";
                    txt3.Text = " " + (currpg).ToString() + " ";
                    txt4.Text = " " + (currpg + 1).ToString() + " ";
                    txt5.Text = " " + (currpg + 2).ToString() + " ";

                }
                txtCurentPage.Text = "Текущая страница: " + (currpg).ToString();

                if (currpg < 1)
                {
                    currpg = 1;
                    txtPrev.Visibility = Visibility.Hidden;
                }

                if (currpg == countpage)
                {
                    currpg = countpage;
                    txtNext.Visibility = Visibility.Hidden;
                }

                listUsers = users.Skip(currpg * countzap - countzap).Take(countzap).ToList();
                lbUsers.ItemsSource = listUsers;
            }
            catch
            {

            }
        }

        private void txtPageCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtPageCount.Text == "")
                {
                    listUsers = users.ToList();
                    spPages.Visibility = Visibility.Hidden;
                }
                else
                {
                    listUsers = users.Take(Convert.ToInt32(txtPageCount.Text)).ToList();
                    spPages.Visibility = Visibility.Visible;
                }


                lbUsers.ItemsSource = listUsers;
            }
            catch
            {

            }
        }
    }
}
