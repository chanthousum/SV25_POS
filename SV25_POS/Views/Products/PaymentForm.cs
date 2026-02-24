using SV25_POS.Models;
using SV25_POS.Views.products;
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
    public partial class PaymentForm : Form
    {
        private SaleForm _saleForm;
        public PaymentForm(SaleForm saleForm)
        {
            InitializeComponent();
            this._saleForm=saleForm;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            Sale sale = new Sale();
            sale.Payment(this);
            _saleForm.ClearRecordDataGridView();
        }

        private void txtCashRecieve_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                double totalAmount, cashRecieve, cashReturn;
                totalAmount = Convert.ToDouble(lblTotalAmount.Text);
                cashRecieve=Convert.ToDouble(txtCashRecieve.Text.Trim());
                if (cashRecieve >= totalAmount)
                {
                    cashReturn = cashRecieve - totalAmount;
                    lblCashReturn.Text=cashReturn.ToString("#,##0.00");
                }
                else
                {
                    
                    txtCashRecieve.Focus();
                }
            }
        }

        private void PaymentForm_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
