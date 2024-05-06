using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System.IO;

namespace PointOfSale
{
    public partial class MainWindow : Window
    {
        private List<Product> products;  // variable products is tpyeof List<t: Product>
        private List<Product> cart; 
        private decimal total;

        // Constructor of the class that add and update data.
        public MainWindow()
        {
            InitializeComponent();
            InitializeProducts();
            ProductListBox();
        }

        // Method to initialize sample products, cart, price.
        private void InitializeProducts()
        {
            products = new List<Product>();

            // Read product data from CSV file
            string[] lines = File.ReadAllLines("products.csv");
            foreach (string line in lines)
            {
                string[] fields = line.Split(',');
                if (fields.Length == 2)
                {
                    string name = fields[0];
                    decimal price = decimal.Parse(fields[1]);
                    products.Add(new Product { Name = name, Price = price});
                }
            }
            cart = new List<Product>();
            total = 0;  
        }

        private void ProductListBox()
        {
            // binding of products for productlistBox in XML 
            productListBox.ItemsSource = products;
        }

        private void UpdateCartListBox()
        {
            // set previous value null and update with new data
            cartListBox.ItemsSource = null;
            cartListBox.ItemsSource = cart;

        }

        private void UpdateTotal()
        {
            totalTextBlock.Text = $"Total: {total.ToString("0.00")} £";
        }

        //:- onClick event the routedEventArgs carries event data i.e selected Product Object
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
        // Get the selected product from the product list box.
        Product selectedProduct = (Product)productListBox.SelectedItem;

        // If a product is selected then update the cartlistbox and total amount.
        if (selectedProduct != null)
        {
            // Check if the selected product is already in the cart
            bool productAlreadyInCart = false;
            foreach (Product cartProduct in cart)
            {
                if (cartProduct.Name == selectedProduct.Name)
                {
                    // If the same product is found in the cart, increment its quantity instead of adding it again
                    cartProduct.Quantity++;
                    productAlreadyInCart = true;
                    break;
                }
            }

            // If the selected product is not already in the cart, add it with quantity 1
            if (!productAlreadyInCart)
            {
                selectedProduct.Quantity = 1;
                cart.Add(selectedProduct);
            }

            total += selectedProduct.Price;   // total + new product price
            UpdateCartListBox(); // method call to update the listbox object data 
            UpdateTotal(); //method call to update the total
        }
        }

         private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            // Get the product associated with the button that triggered the event
            Button removeButton = (Button)sender;
            Product productToRemove = (Product)removeButton.Tag;

            // Find and remove the product from the cart
            Product cartProductToRemove = cart.Find(p => p.Name == productToRemove.Name);
            if (cartProductToRemove != null)
            {
                cart.Remove(cartProductToRemove);
                total -= (cartProductToRemove.Price * cartProductToRemove.Quantity);
                UpdateCartListBox();
                UpdateTotal();
            }
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            // onClick to open box to show message or content
            MessageBox.Show($"Complete Checkout.. Total amount: £{total.ToString("0.00")}");
        }
    }

    public class Product
    {
        public string Name { get; set; } // auto auto implemented properties :- later stores values for the property
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
