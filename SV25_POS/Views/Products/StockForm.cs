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

namespace SV15_POS.Views.products
{
    public partial class StockForm : Form
    {
        public StockForm()
        {
            InitializeComponent();
        }

        private void StockForm_Load(object sender, EventArgs e)
        {
            Stock stock = new Stock();
            stock.SetSupplierName(cboSupplierName);
        }

        private void btnAddStock_Click(object sender, EventArgs e)
        {
            Stock stock = new Stock();
            stock.SupplierId = stock.GetSupplierId(cboSupplierName);
            stock.ProductId=Convert.ToInt32(lblId.Text);
            stock.Qty = Convert.ToInt32(txtQty.Text.Trim());
            stock.Cost = Convert.ToDouble(txtPrice.Text.Trim());
            stock.AddStock();
        }
    }
}
