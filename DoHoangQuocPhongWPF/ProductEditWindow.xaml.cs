using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace DoHoangQuocPhongWPF
{
    public partial class ProductEditWindow : Window
    {
        public Product currentProduct;

        public ProductEditWindow(Product product = null)
        {
            InitializeComponent();

            currentProduct = product == null ? new Product() : new Product
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsOnOrder = product.UnitsOnOrder,
                UnitsInStock = product.UnitsInStock,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued
            };

            if (product != null) LoadData();
        }

        private void LoadData()
        {
            txtProductName.Text = currentProduct.ProductName;
            txtSupplier.Text = currentProduct.SupplierId?.ToString();
            txtCategory.Text = currentProduct.CategoryId?.ToString();
            txtQuantity.Text = currentProduct.QuantityPerUnit;
            txtUnitPrice.Text = currentProduct.UnitPrice?.ToString("F2");
            txtStock.Text = currentProduct.UnitsInStock.ToString();
            txtOnOrder.Text = currentProduct.UnitsOnOrder.ToString();
            txtReorder.Text = currentProduct.ReorderLevel.ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text) ||
                string.IsNullOrWhiteSpace(txtSupplier.Text) ||
                string.IsNullOrWhiteSpace(txtCategory.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                string.IsNullOrWhiteSpace(txtUnitPrice.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text) ||
                string.IsNullOrWhiteSpace(txtOnOrder.Text) ||
                string.IsNullOrWhiteSpace(txtReorder.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                currentProduct.ProductName = txtProductName.Text.Trim();
                currentProduct.SupplierId = int.Parse(txtSupplier.Text);
                currentProduct.CategoryId = int.Parse(txtCategory.Text);
                currentProduct.QuantityPerUnit = txtQuantity.Text.Trim();
                currentProduct.UnitPrice = decimal.Parse(txtUnitPrice.Text);
                currentProduct.UnitsInStock = int.Parse(txtStock.Text);
                currentProduct.UnitsOnOrder = int.Parse(txtOnOrder.Text);
                currentProduct.ReorderLevel = int.Parse(txtReorder.Text);

                DialogResult = true;
                Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid numeric values entered.", "Format Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public Product GetProduct() => currentProduct;
    }
}
