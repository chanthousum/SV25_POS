using SV25_POS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SV25_POS.Views.Products
{
    public partial class AddStockForm : Form
    {
        Stock stock;
        public AddStockForm()
        {
            InitializeComponent();
            stock = new Stock();
            stock.SetSupplierName(cboSupplierName);
        }

        private void AddStockForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAddStock_Click(object sender, EventArgs e)
        {
            Stock stock = new Stock();
            stock.SupplierId = stock.GetSupplierId(cboSupplierName);
            stock.ProductId=Convert.ToInt32(lblId.Text);
            stock.Qty = Convert.ToInt32(txtQty.Text.Trim());
            stock.Cost = Convert.ToDouble(txtCost.Text.Trim());
            stock.AddStock();
        }
    }
}
