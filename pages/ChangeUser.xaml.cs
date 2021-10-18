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
    /// Логика взаимодействия для ChangeUser.xaml
    /// </summary>
    public partial class ChangeUser : Page
    {
        public string sLog, sPass;
        public ChangeUser(auth CurrentUser)
        {
            InitializeComponent();
            tbLogin.Focus();
            try
            {
                tbLogin.Text = CurrentUser.login;
                tbPass.Text = CurrentUser.password;
                sLog = tbLogin.Text;
                sPass = tbPass.Text;
                tbName.Text = CurrentUser.users.name;
                tbGender.Text = CurrentUser.users.genders.gender;
                dpDR.Text = CurrentUser.users.dr.ToString("yyyy MMMM dd");
                List<users_to_traits> LUTT = BaseConnect.BaseModel.users_to_traits.Where(x => x.id_user == CurrentUser.id).ToList();
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
                foreach (users_to_traits tr in LUTT)
                {
                    if (cb1.Content == tr.traits.trait)
                    {
                        cb1.IsChecked = true;
                    }
                    if (cb2.Content == tr.traits.trait)
                    {
                        cb2.IsChecked = true;
                    }
                    if (cb3.Content == tr.traits.trait)
                    {
                        cb3.IsChecked = true;
                    }
                }
                List<genders> genders = BaseConnect.BaseModel.genders.Where(x => x.gender != tbGender.Text).ToList();
                listGenders.ItemsSource = genders;
                listGenders.SelectedValuePath = "id";
                listGenders.DisplayMemberPath = "gender";
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибочка" + e.Message);
            }

            users NullInfo = BaseConnect.BaseModel.users.FirstOrDefault(x => x.id == CurrentUser.id);


        }

        private void btnChangeUser_Click(object sender, RoutedEventArgs e)
        {
            auth User = BaseConnect.BaseModel.auth.FirstOrDefault(x => x.login == sLog && x.password == sPass);
            users mbUser = BaseConnect.BaseModel.users.FirstOrDefault(x => x.id == User.id);
            if (mbUser == null)
            {
                users newUser = new users { id = User.id, name = tbName.Text, dr = (DateTime)dpDR.SelectedDate, gender = (int)listGenders.SelectedValue };
                BaseConnect.BaseModel.users.Add(newUser);
                if (cb1.IsChecked == true)
                {
                    users_to_traits UTT = new users_to_traits();
                    UTT.id_user = newUser.id;
                    UTT.id_trait = 1;
                    BaseConnect.BaseModel.users_to_traits.Add(UTT);
                }
                if (cb2.IsChecked == true)
                {
                    users_to_traits UTT = new users_to_traits();
                    UTT.id_user = newUser.id;
                    UTT.id_trait = 2;
                    BaseConnect.BaseModel.users_to_traits.Add(UTT);
                }
                if (cb3.IsChecked == true)
                {
                    users_to_traits UTT = new users_to_traits();
                    UTT.id_user = newUser.id;
                    UTT.id_trait = 3;
                    BaseConnect.BaseModel.users_to_traits.Add(UTT);
                }
                BaseConnect.BaseModel.SaveChanges();
            }
            else
            {
                mbUser.dr = (DateTime)dpDR.SelectedDate;
                mbUser.name = tbName.Text;
                User.login = tbLogin.Text;
                User.password = tbPass.Text;
                users_to_traits trait1 = BaseConnect.BaseModel.users_to_traits.FirstOrDefault(x => x.id_user == User.id && x.id_trait == 1);
                users_to_traits trait2 = BaseConnect.BaseModel.users_to_traits.FirstOrDefault(x => x.id_user == User.id && x.id_trait == 2);
                users_to_traits trait3 = BaseConnect.BaseModel.users_to_traits.FirstOrDefault(x => x.id_user == User.id && x.id_trait == 3);
                try
                {
                    if (listGenders.SelectedItem != null)
                    {
                        mbUser.gender = (int)listGenders.SelectedValue;
                    }
                    if (cb1.IsChecked == false && trait1 != null)
                    {
                        BaseConnect.BaseModel.users_to_traits.Remove(trait1);
                    }
                    else if (cb1.IsChecked == true && trait1 == null)
                    {
                        CreateTrait(User, 1);
                    }
                    if (cb2.IsChecked == false && trait2 != null)
                    {
                        BaseConnect.BaseModel.users_to_traits.Remove(trait2);
                    }
                    else if (cb2.IsChecked == true && trait2 == null)
                    {
                        CreateTrait(User, 2);
                    }
                    if (cb3.IsChecked == false && trait3 != null)
                    {
                        BaseConnect.BaseModel.users_to_traits.Remove(trait3);
                    }
                    else if (cb3.IsChecked == true && trait3 == null)
                    {
                        CreateTrait(User, 3);
                    }
                    BaseConnect.BaseModel.SaveChanges();
                    MessageBox.Show("Изменения успешно внесены");
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                }
            }


        }

        public static void CreateTrait(auth User, int i)
        {
            users_to_traits UTT = new users_to_traits();
            UTT.id_user = User.id;
            UTT.id_trait = i;
            BaseConnect.BaseModel.users_to_traits.Add(UTT);
        }




        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoadPages.MainFrame.Navigate(new pgUsersToList());
        }
    }
}
