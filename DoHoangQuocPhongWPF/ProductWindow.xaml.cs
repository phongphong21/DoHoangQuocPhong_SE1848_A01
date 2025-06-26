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
    public partial class ProductWindow : Window
    {
        private readonly ProductService productService = new();
        private List<Product> products = new();

        public ProductWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            products = productService.GetProducts().Where(p => !p.Discontinued).ToList();
            dgProducts.ItemsSource = null;
            dgProducts.ItemsSource = products;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new ProductEditWindow();
            if (addWindow.ShowDialog() == true)
            {
                var newProduct = addWindow.GetProduct();
                productService.AddProduct(newProduct);
                LoadProducts();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is not Product selectedProduct || selectedProduct.Discontinued)
            {
                MessageBox.Show("Please choose a continued product to update!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var editWindow = new ProductEditWindow(selectedProduct);
            if (editWindow.ShowDialog() == true)
            {
                var updatedProduct = editWindow.GetProduct();
                productService.UpdateProduct(updatedProduct);
                LoadProducts();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem == null)
            {
                MessageBox.Show("Please select a product to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var selectedProduct = dgProducts.SelectedItem as BusinessObjects.Product;
            if (selectedProduct != null)
            {
                var ret = MessageBox.Show($"Are you sure you want to delete product {selectedProduct.ProductName}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (ret == MessageBoxResult.Yes)
                {
                    bool deleted = productService.DeleteProduct(selectedProduct);
                    if (deleted)
                    {
                        MessageBox.Show("Product deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadProducts();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var keyword = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                dgProducts.ItemsSource = products;
            }
            else
            {
                var filtered = productService.GetProductByName(keyword)
                    .Where(p => !p.Discontinued)
                    .ToList();
                dgProducts.ItemsSource = filtered;
            }
        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to go back?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                new AdminMainWindow().Show();
                Close();
            }
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            LoadProducts();
        }
    }
}
