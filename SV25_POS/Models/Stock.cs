using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SV25_POS.Models
{
    internal class Stock
    {
        private string _sql;
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public double Cost { get; set; }
        public double CalculateTotal()
        {
            return this.Qty * this.Cost;
        }
        public void SetSupplierName(ComboBox cboSupllierName)
        {
            try
            {
                this._sql = "select * from tblSupplier";
                Database.Cmd = new SqlCommand(this._sql, Database.Con);
                Database.Cmd.ExecuteNonQuery();
                Database.da = new SqlDataAdapter(Database.Cmd);
                Database.tbl = new DataTable();
                Database.da.Fill(Database.tbl);
                cboSupllierName.Items.Clear();
                foreach (DataRow r in Database.tbl.Rows)
                {
                    cboSupllierName.Items.Add(r["Name"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error set Supllier Name:" + ex.Message);
            }
        }
        public int GetSupplierId(ComboBox cboSupplierName)
        {
            int id = 0;
            try
            {
                this._sql = "select * from tblSupplier where Name=@Name";
                Database.Cmd = new SqlCommand(this._sql, Database.Con);
                Database.Cmd.Parameters.AddWithValue("@Name", cboSupplierName.Text);
                Database.Cmd.ExecuteNonQuery();
                Database.da = new SqlDataAdapter(Database.Cmd);
                Database.tbl = new DataTable();
                Database.da.Fill(Database.tbl);
                if (Database.tbl.Rows.Count > 0)
                {
                    id = int.Parse(Database.tbl.Rows[0]["Id"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error get Supplier Id:" + ex.Message);
            }
            return id;
        }

        public void TransferData(DataGridView dg, Label lblId, Label lblName)
        {
            if (dg.Rows.Count == 0) { return; }
            DataGridViewRow DGV = new DataGridViewRow();
            DGV = dg.SelectedRows[0];
            lblId.Text = DGV.Cells[0].Value.ToString();
            lblName.Text = DGV.Cells[1].Value.ToString();
        }

        public void AddStock()
        {
            SqlTransaction sqlTransaction = null;
            try
            {
                this._sql = "insert into tblStock(SupplierId,ProductId,Qty,Cost,Total,CreateBy,CreateAt)values(@SupplierId,@ProductId,@Qty,@Cost,@Total,@CreateBy,GETDATE());";
                sqlTransaction=Database.Con.BeginTransaction();
                Database.Cmd = new SqlCommand(this._sql,Database.Con,sqlTransaction);
           
                Database.Cmd.Parameters.AddWithValue("@SupplierId", this.SupplierId);
                Database.Cmd.Parameters.AddWithValue("@ProductId", this.ProductId);
                Database.Cmd.Parameters.AddWithValue("@Qty", this.Qty);
                Database.Cmd.Parameters.AddWithValue("@Cost", this.Cost);
                Database.Cmd.Parameters.AddWithValue("@Total", this.CalculateTotal());
                Database.Cmd.Parameters.AddWithValue("@CreateBy", this.SupplierId);
                Database.Cmd.ExecuteNonQuery();

                //update stock
                this._sql = "update tblProduct set UnitInStock=UnitInStock + @Qty where Id=@ProductId";
                Database.Cmd = new SqlCommand(this._sql, Database.Con, sqlTransaction);

                Database.Cmd.Parameters.AddWithValue("@Qty", this.Qty);
                Database.Cmd.Parameters.AddWithValue("@ProductId",this.ProductId);
                Database.Cmd.ExecuteNonQuery ();
                sqlTransaction.Commit();
                MessageBox.Show("Add Stock Successfully");
            }
            catch(Exception ex)
            {
                sqlTransaction.Rollback();
                MessageBox.Show("Error Add Stock:"+ex.Message);
            }
        }
    }
}
