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

namespace SV25_POS.Views.products
{
    public partial class SaleForm : Form
    {
        public SaleForm()
        {
            InitializeComponent();
        }
        public void ClearRecordDataGridView()
        {
            dgSale.Rows.Clear();
        }
        private void txtScanBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {

            if(e.KeyChar ==(char) Keys.Enter)
            {
                Sale sale = new Sale();
                sale.ScanBarcode(dgSale, txtScanBarcode);
            }
        }

        private void SaleForm_Load(object sender, EventArgs e)
        {

        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            PaymentForm paymentForm=new PaymentForm(this);
            Sale sale=new Sale();
            paymentForm.lblTotalAmount.Text=sale.CalculateTotalAmount().ToString("#,##0.00");
            paymentForm.ShowDialog();
        }
    }
}
