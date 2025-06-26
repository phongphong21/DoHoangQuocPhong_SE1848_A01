using BusinessObjects;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DoHoangQuocPhongWPF {
    public partial class OrderWindow : Window
    {
        private readonly OrderService orderService = new();
        private readonly OrderDetailService orderDetailService = new();
        private List<Order> orders = new();
        private List<OrderDetail> orderDetails = new();

        public OrderWindow()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            orders = orderService.GetOrders();
            dgOrders.ItemsSource = orders;
            dgOrderDetail.ItemsSource = null;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtsearch.Text.Trim().ToLower();
            orders = string.IsNullOrWhiteSpace(keyword)
                ? orderService.GetOrders()
                : orderService.GetOrdersbyCustomerName(keyword);

            dgOrders.ItemsSource = orders;
            dgOrderDetail.ItemsSource = null;
        }

        private void dgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgOrders.SelectedItem is not Order selectedOrder)
            {
                dgOrderDetail.ItemsSource = null;
                return;
            }

            orderDetails = orderDetailService.GetDetailsByOrderId(selectedOrder.OrderId);
            dgOrderDetail.ItemsSource = orderDetails;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new OrderEditWindow();
            if (addWindow.ShowDialog() == true)
            {
                orderService.AddOrder(addWindow.GetOrder());
                LoadOrders();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrders.SelectedItem is not Order selectedOrder)
            {
                MessageBox.Show("Please select an order to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var editWindow = new OrderEditWindow(selectedOrder);
            if (editWindow.ShowDialog() == true)
            {
                orderService.UpdateOrder(editWindow.GetOrder());
                LoadOrders();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrders.SelectedItem is not Order selectedOrder)
            {
                MessageBox.Show("Please select an order to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (orderDetailService.HasDetails(selectedOrder.OrderId))
            {
                MessageBox.Show("Cannot delete order with existing details.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete order {selectedOrder.OrderId}?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool deleted = orderService.DeleteOrder(selectedOrder.OrderId);
                MessageBox.Show(deleted ? "Order deleted successfully." : "Failed to delete order.",
                    deleted ? "Success" : "Error", MessageBoxButton.OK,
                    deleted ? MessageBoxImage.Information : MessageBoxImage.Error);

                if (deleted) LoadOrders();
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
    }
}
