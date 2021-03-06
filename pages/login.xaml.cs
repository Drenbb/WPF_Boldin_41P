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
using System.Windows.Threading;

namespace WpfApp.pages
{
    /// <summary>
    /// Логика взаимодействия для login.xaml
    /// </summary>
    public partial class login : Page
    {
        public login()
        {
            InitializeComponent();
            txtLogin.Focus();
        }
        string kode;
        bool flagKode = false;

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            auth CurrentUser = BaseConnect.BaseModel.auth.FirstOrDefault(x => x.login == txtLogin.Text && x.password == txtPassword.Password);
            if (CurrentUser != null)
            {
                if (flagKode == false)
                {
                    generateKey();
                    flagKode = true;
                }
                else if (kode == txtKey.Text)
                {
                    switch (CurrentUser.role)
                    {
                        case 1:
                            MessageBox.Show("Вы администратор");
                            LoadPages.MainFrame.Navigate(new pgUsersToList());
                            break;
                        case 2:
                        default:
                            MessageBox.Show("Вы рядовой пользователь");
                            LoadPages.MainFrame.Navigate(new info(CurrentUser));
                            break;
                    }

                }
                else
                {
                    MessageBox.Show("Введенный код не подходит");
                }
            }
            else { MessageBox.Show("Не верный логин или пароль"); }
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            LoadPages.MainFrame.Navigate(new reg(1));
        }

        Random random = new Random();
        private void generateKey()
        {
            imgRefresh.IsEnabled = false;
            kode = "";

            for (int i = 0; i < 8; i++)
            {
                kode += ((char)random.Next(33, 122)).ToString();
            }
            txtKey.Text = kode;
            MessageBox.Show(kode, "введите код в соответствующее поле на форме.", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            imgRefresh.IsEnabled = true;
            kode = ((char)random.Next(33, 122)).ToString();
            // MessageBox.Show("время вышло");
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            generateKey();
            flagKode = true;
        }


    }
}
