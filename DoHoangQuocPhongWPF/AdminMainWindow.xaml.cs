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
using System.Windows.Shapes;

namespace DoHoangQuocPhongWPF
{
    /// <summary>
    /// Interaction logic for AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();
        }

        private void OpenWindow(Window window)
        {
            window.Show();
            this.Hide();
        }
        private void btnCustomer_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new CustomerWindow());

        }

        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new ProductWindow());
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new OrderWindow());
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new ReportWindow());
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var ret = MessageBox.Show("Are you sure want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(ret == MessageBoxResult.Yes)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}
