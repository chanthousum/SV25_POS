using SV25_POS.Models;
using SV25_POS.Views.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SV25_POS.Views.Users
{
    public partial class ProductForm : Form
    {
        Product product;
        
        public ProductForm()
        {
            InitializeComponent();
            product = new Product();
            product.SetCategoryName(cboCategoryName);
        }
        
        private void btnCreate_Click(object sender, EventArgs e)
        {
            product = new Product();
            product.ProductName = txtName.Text.Trim();
            product.Barcode=Convert.ToInt64(txtBarcode.Text.Trim());
            product.SellPrice=Convert.ToDouble(txtSellPrice.Text.Trim());
            product.CategoryId = product.GetCategoryId(cboCategoryName);
            product.Photo = Product.PathPhoto;
            product.Create(dgProduct);
            Product.PathPhoto = "";
            
                
        }

        private void RoleForm_Load(object sender, EventArgs e)
        {
            product = new Product();
            product.GetData(dgProduct);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            product = new Product();
            product.DeleteById(dgProduct);
        }

        private void dgRole_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            product = new Product();
            product.TransferDataToControls(dgProduct,txtName, txtBarcode, txtSellPrice, picPhoto, cboCategoryName);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            product = new Product();
            product.ProductName = txtName.Text.Trim();
            product.Barcode = Convert.ToInt64(txtBarcode.Text.Trim());
            product.SellPrice = Convert.ToDouble(txtSellPrice.Text.Trim());
            product.CategoryId = product.GetCategoryId(cboCategoryName);
            product.Photo = Product.PathPhoto;
            product.UpdateById(dgProduct);
            Product.PathPhoto = "";
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                product = new Product();
                product.ProductName = txtSearch.Text.Trim();
                product.Search(dgProduct);

            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                picPhoto.Image=Image.FromFile(openFileDialog.FileName);
                Product.PathPhoto = openFileDialog.FileName;
            }
        }

        private void addStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddStockForm addStockForm = new AddStockForm();
            Stock stock=new Stock();
            stock.TransferData(dgProduct, addStockForm.lblId, addStockForm.lblName);
            addStockForm.ShowDialog();
        }
    }
}
