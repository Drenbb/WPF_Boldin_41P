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
    /// Логика взаимодействия для adminInfo.xaml
    /// </summary>
    public partial class adminInfo : Page
    {
        public adminInfo()
        {
            InitializeComponent();
            dgUsers.ItemsSource = BaseConnect.BaseModel.auth.ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BaseConnect.BaseModel.SaveChanges();
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            auth SelectedUser = (auth)dgUsers.SelectedItem;
            BaseConnect.BaseModel.auth.Remove(SelectedUser);
            BaseConnect.BaseModel.SaveChanges();
            MessageBox.Show("Пользователь " + SelectedUser.login + "Удален из системы!");
            dgUsers.ItemsSource = BaseConnect.BaseModel.auth.ToList(); 
        }

        private void btnChangeUser_Click(object sender, RoutedEventArgs e)
        {
            auth CurrentUser = (auth)dgUsers.SelectedItem;
            LoadPages.MainFrame.Navigate(new ChangeUser(CurrentUser));
        }
    }
}
