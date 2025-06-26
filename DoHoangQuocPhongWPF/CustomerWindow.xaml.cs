using BusinessObjects;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace DoHoangQuocPhongWPF
{
    public partial class CustomerWindow : Window
    {
        private readonly CustomerService customerService = new();
        private List<Customer> customers = new();

        public CustomerWindow()
        {
            InitializeComponent();
            LoadCustomerList();
        }

        private void LoadCustomerList()
        {
            customers = customerService.GetCustomers();
            dgCustomers.ItemsSource = customers;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new CustomerEditWindow();
            if (addWindow.ShowDialog() == true)
            {
                var newCustomer = addWindow.GetCustomer();
                customerService.AddCustomer(newCustomer);
                LoadCustomerList();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem is not Customer selectedCustomer)
            {
                MessageBox.Show("Please select a customer to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editWindow = new CustomerEditWindow(selectedCustomer);
            if (editWindow.ShowDialog() == true)
            {
                var updatedCustomer = editWindow.GetCustomer();
                customerService.UpdateCustomer(updatedCustomer);
                LoadCustomerList();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem is not Customer selectedCustomer)
                return;

            if (selectedCustomer.Orders.Any())
            {
                MessageBox.Show("Cannot delete a customer with existing orders.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete customer {selectedCustomer.CompanyName}?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                bool deleted = customerService.DeleteCustomer(selectedCustomer);
                MessageBox.Show(deleted ? "Customer deleted successfully." : "Failed to delete customer.",
                    deleted ? "Success" : "Error", MessageBoxButton.OK,
                    deleted ? MessageBoxImage.Information : MessageBoxImage.Error);
                if (deleted) LoadCustomerList();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to go back?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                new AdminMainWindow().Show();
                Close();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            var filtered = customers.Where(c =>
                (!string.IsNullOrWhiteSpace(c.CompanyName) && c.CompanyName.ToLower().Contains(keyword)) ||
                (!string.IsNullOrWhiteSpace(c.ContactName) && c.ContactName.ToLower().Contains(keyword)) ||
                (!string.IsNullOrWhiteSpace(c.Phone) && c.Phone.ToLower().Contains(keyword))
            ).ToList();

            dgCustomers.ItemsSource = filtered;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            dgCustomers.ItemsSource = customers;
        }
    }
}
