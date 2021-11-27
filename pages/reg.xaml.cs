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
    /// Логика взаимодействия для reg.xaml
    /// </summary>
    public partial class reg : Page
    {
        public int ii;
        public reg(int ii)
        {
            this.ii = ii;
            InitializeComponent();
            txtLog.Focus();
            List<genders> genders = BaseConnect.BaseModel.genders.ToList();
            listGenders.ItemsSource = genders;
            listGenders.SelectedValuePath = "id";
            listGenders.DisplayMemberPath = "gender";
            string[] traits2 = new string[3];
            List<traits> traits1 = BaseConnect.BaseModel.traits.ToList();
            int i = 0;
            foreach (traits tr in traits1)
            {
                traits2[i] = tr.trait;
                i++;
            }
            cb1.Content = traits2[0];
            cb2.Content = traits2[1];
            cb3.Content = traits2[2];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ii == 1)
            {
                LoadPages.MainFrame.GoBack();
            }
            else
            {
                LoadPages.MainFrame.Navigate(new pgUsersToList());
            }
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                auth Find = BaseConnect.BaseModel.auth.FirstOrDefault(x => x.login == txtLog.Text);
                if (Find == null)
                {
                    auth logPass = new auth() { login = txtLog.Text, password = txtPass.Password, role = 2 };
                    BaseConnect.BaseModel.auth.Add(logPass);
                    BaseConnect.BaseModel.SaveChanges();

                    users Users = new users() { name = txtName.Text, id = logPass.id, gender = (int)listGenders.SelectedValue, dr = (DateTime)date.SelectedDate };
                    BaseConnect.BaseModel.users.Add(Users);
                    if (cb1.IsChecked == true)
                    {
                        users_to_traits UTT = new users_to_traits();
                        UTT.id_user = Users.id;
                        UTT.id_trait = 1;
                        BaseConnect.BaseModel.users_to_traits.Add(UTT);
                    }
                    if (cb2.IsChecked == true)
                    {
                        users_to_traits UTT = new users_to_traits();
                        UTT.id_user = Users.id;
                        UTT.id_trait = 2;
                        BaseConnect.BaseModel.users_to_traits.Add(UTT);
                    }
                    if (cb3.IsChecked == true)
                    {
                        users_to_traits UTT = new users_to_traits();
                        UTT.id_user = Users.id;
                        UTT.id_trait = 3;
                        BaseConnect.BaseModel.users_to_traits.Add(UTT);
                    }
                    BaseConnect.BaseModel.SaveChanges();
                    MessageBox.Show("Пользователь успешно добавлен!!!");
                }
                else
                {
                    MessageBox.Show("Пользоваетль с таким Логином уже существует, придумайте другой !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка, проверьте заполненность полей");
            }
        }

    }
}
