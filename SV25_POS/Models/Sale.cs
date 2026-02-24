using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using SV25_POS.Views.Products;
namespace SV25_POS.Models
{
    internal class Sale:Product
    {
        public int Qty { get; set; } = 0;
        public  static List<Sale> saleItem = new List<Sale>();
        public int SaleId { get; set; }
        public double CalculateAmount()
        {
            return this.Qty * this.SellPrice;
        }
        public void ScanBarcode(DataGridView dgSale,TextBox txtScanBarcode)
        {
            try
            {
                this.Barcode=Convert.ToInt64(txtScanBarcode.Text.Trim());
                this.Sql = "select * from tblProduct where Barcode=@Barcode";
                Database.Cmd=new SqlCommand(this.Sql,Database.Con);
                Database.Cmd.Parameters.AddWithValue("@Barcode", this.Barcode);
                Database.Cmd.ExecuteNonQuery();
                Database.da = new SqlDataAdapter(Database.Cmd);
                Database.tbl = new System.Data.DataTable();
                Database.da.Fill(Database.tbl);
                if (Database.tbl.Rows.Count>0)
                {

                    foreach (DataGridViewRow DGV in dgSale.Rows)
                    {
                        long existingBarcode = Convert.ToInt64(DGV.Cells[1].Value);
                        if (existingBarcode == this.Barcode)
                        {
                            this.Qty = Convert.ToInt32(DGV.Cells[3].Value) + 1;
                            DGV.Cells[3].Value = this.Qty;
                            this.SellPrice = Convert.ToDouble(DGV.Cells[4].Value);
                            DGV.Cells[5].Value = this.CalculateAmount();

                            int rowIndex = DGV.Index;
                            saleItem[rowIndex].Qty = this.Qty;
                            txtScanBarcode.Clear();
                            txtScanBarcode.Focus();
                            return;
                        }
                    }

                    this.Id = Convert.ToInt32(Database.tbl.Rows[0]["Id"].ToString());
                    this.Barcode = Convert.ToInt64(Database.tbl.Rows[0]["Barcode"].ToString());
                    this.ProductName = Database.tbl.Rows[0]["Name"].ToString();
                    this.Qty = 1;
                    this.SellPrice = Convert.ToDouble(Database.tbl.Rows[0]["SellPrice"].ToString());
                    Object[] row = { this.Id, this.Barcode, this.ProductName, this.Qty, this.SellPrice.ToString("#,##0.00"), this.CalculateAmount().ToString("#,##0.00") };

                    dgSale.Rows.Add(row);
                    saleItem.Add(this);
                    txtScanBarcode.Clear();
                    txtScanBarcode.Focus();

                }
                else
                {
                    MessageBox.Show("Barcode not found:" + this.Barcode);
                    txtScanBarcode.Clear();
                    txtScanBarcode.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Scan barcode:" + ex.Message);
            }
        }
        public double CalculateTotalAmount()
        {
            double sum = 0;
            for (int i = 0; i <saleItem.Count; i++) { 
                sum +=Convert.ToDouble(saleItem[i].CalculateAmount());
            }
            return sum;
        }
        public void Payment(PaymentForm paymentForm)
        {
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlTransaction = Database.Con.BeginTransaction();
                this.Sql = "insert into tblSale(SaleDate,UserId,TotalAmount)values( GETDATE(),@UserId,@TotalAmount);select SCOPE_IDENTITY()";
                Database.Cmd = new SqlCommand(this.Sql, Database.Con, sqlTransaction);
                Database.Cmd.Parameters.AddWithValue("@UserId", User.LogInUserId);
                Database.Cmd.Parameters.AddWithValue("@TotalAmount", this.CalculateTotalAmount());
               this.SaleId=Convert.ToInt32(Database.Cmd.ExecuteScalar());

                //insert data to table sale detail
                foreach (var item in saleItem)
                {
                    this.Id= item.Id;
                    this.Qty = item.Qty;
                    this.SellPrice= item.SellPrice;
                    this.Sql = "insert into tblSaleDetail(SaleId,ProductId,Qty,Price,Amount)values(@SaleId,@ProductId,@Qty,@Price,@Amount)";
                    Database.Cmd = new SqlCommand(this.Sql, Database.Con, sqlTransaction);
                    Database.Cmd.Parameters.AddWithValue("@SaleId",this.SaleId);
                    Database.Cmd.Parameters.AddWithValue("@ProductId",this.Id);
                    Database.Cmd.Parameters.AddWithValue("@Qty",this.Qty);
                    Database.Cmd.Parameters.AddWithValue("@Price",this.SellPrice);
                    Database.Cmd.Parameters.AddWithValue("@Amount",this.CalculateAmount());
                    Database.Cmd.ExecuteNonQuery();

                    //update stock in table product
                    this.Sql = "update tblProduct set UnitInStock=UnitInStock - @Qty where Id=@ProductId";
                    Database.Cmd = new SqlCommand(this.Sql, Database.Con, sqlTransaction);
                    Database.Cmd.Parameters.AddWithValue("@Qty", this.Qty);
                    Database.Cmd.Parameters.AddWithValue("@ProductId", this.Id);
                    Database.Cmd.ExecuteNonQuery();

                }

                sqlTransaction.Commit();
                MessageBox.Show("Order successs","Order",MessageBoxButtons.OK);
                paymentForm.Dispose();
                saleItem.Clear();
            }catch(Exception ex)
            {
                sqlTransaction.Rollback();
                MessageBox.Show($"Error Payament:{ex.Message}");
            }
        }
           
    }
   
}
