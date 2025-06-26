using BusinessObjects;
using Services;
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
    public partial class OrderEditWindow : Window
    {
        private readonly OrderService orderService = new();
        private readonly CustomerService customerService = new();
        private readonly EmployeeService employeeService = new();
        private readonly bool isEditMode;
        private Order currentOrder;

        public OrderEditWindow(Order order = null)
        {
            InitializeComponent();
            LoadComboBoxData();

            if (order == null)
            {
                isEditMode = false;
                currentOrder = new Order { OrderDate = DateTime.Now };
                dpOrderDate.SelectedDate = currentOrder.OrderDate;
            }
            else
            {
                isEditMode = true;
                currentOrder = order;
                cmbCustomer.SelectedItem = customerService.GetCustomers().FirstOrDefault(c => c.CustomerId == order.CustomerId);
                cmbEmployee.SelectedItem = employeeService.GetEmployees().FirstOrDefault(e => e.EmployeeId == order.EmployeeId);
                dpOrderDate.SelectedDate = order.OrderDate;
            }
        }

        private void LoadComboBoxData()
        {
            cmbCustomer.ItemsSource = customerService.GetCustomers();
            cmbCustomer.DisplayMemberPath = "ContactName";

            cmbEmployee.ItemsSource = employeeService.GetEmployees();
            cmbEmployee.DisplayMemberPath = "Name";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCustomer.SelectedItem is not Customer customer ||
                cmbEmployee.SelectedItem is not Employee employee ||
                dpOrderDate.SelectedDate == null)
            {
                MessageBox.Show("Please select a customer, employee, and order date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentOrder.CustomerId = customer.CustomerId;
            currentOrder.EmployeeId = employee.EmployeeId;
            currentOrder.OrderDate = dpOrderDate.SelectedDate.Value;

            bool success = isEditMode
                ? orderService.UpdateOrder(currentOrder)
                : orderService.AddOrder(currentOrder);

            if (!success)
            {
                MessageBox.Show(isEditMode ? "Failed to update order." : "Failed to add order.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!isEditMode)
                currentOrder = orderService.GetOrderById(currentOrder.OrderId);

            MessageBox.Show(isEditMode ? "Order updated successfully." : "Order added successfully.",
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DialogResult = false;
                Close();
            }
        }

        public Order GetOrder() => currentOrder;
    }
}
