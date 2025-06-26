using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoHoangQuocPhongWPF
{
    public partial class CustomerEditWindow : Window
    {
        private readonly bool isEditMode;
        private readonly Customer currentCustomer;

        public CustomerEditWindow(Customer customer = null)
        {
            InitializeComponent();
            if (customer == null)
            {
                currentCustomer = new Customer();
                isEditMode = false;
            }
            else
            {
                currentCustomer = new Customer
                {
                    CustomerId = customer.CustomerId,
                    CompanyName = customer.CompanyName,
                    ContactName = customer.ContactName,
                    ContactTitle = customer.ContactTitle,
                    Address = customer.Address,
                    Phone = customer.Phone
                };
                isEditMode = true;
                LoadData();
            }

            txtPhone.IsReadOnly = isEditMode;
        }

        private void LoadData()
        {
            txtCompanyName.Text = currentCustomer.CompanyName;
            txtContactName.Text = currentCustomer.ContactName;
            txtContactTitle.Text = currentCustomer.ContactTitle;
            txtAddress.Text = currentCustomer.Address;
            txtPhone.Text = currentCustomer.Phone;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCompanyName.Text) ||
                string.IsNullOrWhiteSpace(txtContactName.Text) ||
                string.IsNullOrWhiteSpace(txtContactTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentCustomer.CompanyName = txtCompanyName.Text.Trim();
            currentCustomer.ContactName = txtContactName.Text.Trim();
            currentCustomer.ContactTitle = txtContactTitle.Text.Trim();
            currentCustomer.Address = txtAddress.Text.Trim();
            currentCustomer.Phone = txtPhone.Text.Trim();

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public Customer GetCustomer() => currentCustomer;
    }
}