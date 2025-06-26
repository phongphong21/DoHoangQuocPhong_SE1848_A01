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
    public partial class ReportWindow : Window
    {
        private readonly ReportService _reportService;
        private readonly OrderService _orderService;

        public ReportWindow()
        {
            InitializeComponent();
            _reportService = new ReportService();
            _orderService = new OrderService();
            LoadAllOrders();
        }

        private void LoadAllOrders()
        {
            var allOrders = _reportService.GetOrdersByDateRange(null, null);
            dgOrderReport.ItemsSource = allOrders;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            var startDate = dpFrom.SelectedDate;
            var endDate = dpTo.SelectedDate;

            if (!IsDateRangeValid(startDate, endDate))
                return;

            var filteredOrders = _reportService.GetOrdersByDateRange(startDate, endDate);
            dgOrderReport.ItemsSource = filteredOrders;
        }

        private bool IsDateRangeValid(DateTime? start, DateTime? end)
        {
            if (end.HasValue && end.Value > DateTime.Now)
            {
                MessageBox.Show("To date cannot be in the future.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (start.HasValue && end.HasValue && start.Value > end.Value)
            {
                MessageBox.Show("From date must not be later than To date.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to go back?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                new AdminMainWindow().Show();
                Close();
            }
        }
    }
}
