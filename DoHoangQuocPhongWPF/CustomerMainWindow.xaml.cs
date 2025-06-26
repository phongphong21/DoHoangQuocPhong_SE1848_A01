using BusinessObjects;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoHoangQuocPhongWPF
{
    public partial class CustomerMainWindow : Window
    {
        private readonly Customer currentCustomer;
        private readonly OrderService orderService = new();
        private readonly CustomerService customerService = new();

        public CustomerMainWindow(Customer customerlogined)
        {
            InitializeComponent();
            currentCustomer = customerlogined;
            LoadCustomerProfile();
            LoadOrderHistory();
        }

        private void LoadCustomerProfile()
        {
            txtCompanyName.Text = currentCustomer.CompanyName;
            txtContactName.Text = currentCustomer.ContactName;
            txtContactTitle.Text = currentCustomer.ContactTitle;
            txtAddress.Text = currentCustomer.Address;
            txtPhone.Text = currentCustomer.Phone;
        }

        private void LoadOrderHistory()
        {
            lvOrders.ItemsSource = orderService.GetOrderByCustomerId(currentCustomer.CustomerId);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                new LoginWindow().Show();
                Close();
            }
        }

        private void btnUpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            currentCustomer.CompanyName = txtCompanyName.Text;
            currentCustomer.ContactName = txtContactName.Text;
            currentCustomer.ContactTitle = txtContactTitle.Text;
            currentCustomer.Address = txtAddress.Text;

            bool updated = customerService.UpdateCustomer(currentCustomer);
            MessageBox.Show(updated ? "Profile updated successfully." : "Failed to update profile.",
                            updated ? "Success" : "Error",
                            MessageBoxButton.OK,
                            updated ? MessageBoxImage.Information : MessageBoxImage.Error);
        }
    }
}

