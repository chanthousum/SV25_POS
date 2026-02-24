using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using SV25_POS.Views.Users;
using System.Windows.Forms.VisualStyles;

using System.Drawing;
namespace SV25_POS.Models
{
    public class Product : Action
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public long Barcode { get; set; }
        public double SellPrice { get; set; }
        public int UnitInStock { get; set; } = 0;
        public string Photo { get; set; }
        public string Sql { get; set; } = "";
        private string _sql = "";
        private int _RowEffectd;
        DataGridViewRow DGV = null;
        public int CategoryId { get; set; }
        public static string PathPhoto { get; set; } = string.Empty;
        public void SetCategoryName(ComboBox cboCategoryName)
        {
            try
            {
                this._sql = "select * from tblCategory";
                Database.Cmd=new SqlCommand(this._sql,Database.Con);
                Database.Cmd.ExecuteNonQuery ();
                Database.da=new SqlDataAdapter(Database.Cmd);
                Database.tbl = new DataTable();
                Database.da.Fill(Database.tbl);
                cboCategoryName.Items.Clear();
                foreach(DataRow r in Database.tbl.Rows) {
                    cboCategoryName.Items.Add(r["CategoryName"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error set Category Name:"+ex.Message);
            }
        }
        public int GetCategoryId(ComboBox cboCategoryName)
        {
            int id = 0;
            try
            {
                this._sql = "select * from tblCategory where CategoryName=@CategoryName";
                Database.Cmd=new SqlCommand(this._sql,Database.Con);
                Database.Cmd.Parameters.AddWithValue("@CategoryName", cboCategoryName.Text);
                Database.Cmd.ExecuteNonQuery ();
                Database.da=new SqlDataAdapter( Database.Cmd);
                Database.tbl = new DataTable();
                Database.da.Fill(Database.tbl);
                if (Database.tbl.Rows.Count > 0)
                {
                    id = int.Parse(Database.tbl.Rows[0]["Id"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error get Category Id:"+ex.Message);
            }
            return id;
        }
        public override void Create(DataGridView dg)
        {

            try
            {
                this._sql = "insert into tblProduct(Name,Barcode,SellPrice,UnitInStock,Photo,CategoryId,CreateBy,CreateAt)values(@Name,@Barcode,@SellPrice,0,@Photo,@CategoryId,@CreateBy,GETDATE());";
                Database.Cmd = new SqlCommand(this._sql, Database.Con);
                Database.Cmd.Parameters.AddWithValue("@Name", this.ProductName);
                Database.Cmd.Parameters.AddWithValue("@Barcode", this.Barcode);
                Database.Cmd.Parameters.AddWithValue("@SellPrice", this.SellPrice);
                Database.Cmd.Parameters.AddWithValue("@Photo", this.Photo);
                Database.Cmd.Parameters.AddWithValue("@CategoryId", this.CategoryId);
                Database.Cmd.Parameters.AddWithValue("@CreateBy",User.LogInUserId);
                this._RowEffectd = Database.Cmd.ExecuteNonQuery();
                if (this._RowEffectd == 1)
                {
                    MessageBox.Show("Product created successfully");
                }
            }
            catch (Exception ex)
            {
                 
                MessageBox.Show("Error create Product:" + ex.Message);
            }
        }
        public override void GetData(DataGridView dg)
        {
            try
            {
                this._sql = "select Id,Name as \"Product Name\",Barcode,UnitInStock from tblProduct;";
                Database.Cmd = new SqlCommand(this._sql, Database.Con);
                Database.Cmd.ExecuteNonQuery();
                Database.da = new SqlDataAdapter(Database.Cmd);
                Database.tbl = new DataTable();
                Database.da.Fill(Database.tbl);
                dg.Rows.Clear();
                dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                dg.DataSource = Database.tbl;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error get Data Product:" + ex.Message);
            }
        }
        public override void DeleteById(DataGridView dg)
        {
            try
            {
                if (dg.Rows.Count == 0)
                {
                    return;
                }
                var click = MessageBox.Show("Do you want to delete record?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (click != DialogResult.Yes)
                {
                    return;
                }
                this.DGV = new DataGridViewRow();
                this.DGV = dg.SelectedRows[0];
                this.Id = int.Parse(this.DGV.Cells[0].Value.ToString());
                this._sql = "delete from tblRole where Id=@Id";
                Database.Cmd = new SqlCommand(this._sql, Database.Con);
                Database.Cmd.Parameters.AddWithValue("@Id", this.Id);
                this._RowEffectd = Database.Cmd.ExecuteNonQuery();
                if (this._RowEffectd == 1)
                {
                    MessageBox.Show("Role deleted successfully");
                    dg.Rows.Remove(this.DGV);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error delete role:" + ex.Message)
 ;
            }

        }
        public void TransferDataToControls(DataGridView dg, TextBox txtProductName,TextBox txtBarcode,TextBox txtSellPricee,PictureBox picPhoto,ComboBox cboCategoryName)
        {
            if (dg.Rows.Count == 0)
            {
                return;
            }
            this.DGV = new DataGridViewRow();
            this.DGV = dg.SelectedRows[0];
            this.Id=int.Parse(DGV.Cells[0].Value.ToString());
            this._sql = "select * from View_Product_Category where Id=@Id";
            Database.Cmd=new SqlCommand(this._sql, Database.Con);
            Database.Cmd.Parameters.AddWithValue("@Id", this.Id);
            Database.da=new SqlDataAdapter(Database.Cmd);
            Database.tbl = new DataTable();
            Database.da.Fill(Database.tbl);
            if (Database.tbl.Rows.Count > 0)
            {
                txtProductName.Text = Database.tbl.Rows[0]["Name"].ToString();
                txtBarcode.Text =Database.tbl.Rows[0]["Barcode"].ToString();

                txtSellPricee.Text = Database.tbl.Rows[0]["SellPrice"].ToString();
                cboCategoryName.Text = Database.tbl.Rows[0]["CategoryName"].ToString();
                this.Photo = Database.tbl.Rows[0]["Photo"].ToString();
                if(this.Photo != "")
                {
                    picPhoto.Image=Image.FromFile(this.Photo);
                }
                else
                {
                    picPhoto.Image=null;
                }
            }
           
             
        }
        public override void UpdateById(DataGridView dg)
        {

            try
            {
                if(dg.Rows.Count ==0) {return; }
                this.DGV = new DataGridViewRow();
                this.DGV=dg.SelectedRows[0];
                this.Id=Convert.ToInt32(DGV.Cells[0].Value);
                this._sql = "update tblProduct set Name=@Name,Barcode=@Barcode,SellPrice=@SellPrice,Photo=@Photo,CategoryId=@CategoryId,UpdateBy=@UpdateBy,UpdateAt=GETDATE() where Id=@Id";
                Database.Cmd = new SqlCommand(this._sql, Database.Con);
                Database.Cmd.Parameters.AddWithValue("@Name", this.ProductName);
                Database.Cmd.Parameters.AddWithValue("@Barcode", this.Barcode);
                Database.Cmd.Parameters.AddWithValue("@SellPrice", this.SellPrice);
                Database.Cmd.Parameters.AddWithValue("@Photo", this.Photo);
                Database.Cmd.Parameters.AddWithValue("@CategoryId", this.CategoryId);
                Database.Cmd.Parameters.AddWithValue("@UpdateBy", User.LogInUserId);
                Database.Cmd.Parameters.AddWithValue("@Id",this.Id);
                this._RowEffectd = Database.Cmd.ExecuteNonQuery();
                if (this._RowEffectd == 1)
                {
                    MessageBox.Show("Product updated successfully");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error update Product:" + ex.Message);
            }

        }
        public override void Search(DataGridView dg)
        {
            try
            {
                this._sql = "select Id,Name as \"Product Name\",Barcode,UnitInStock from tblProduct where Name like CONCAT('%',@Name,'%');";
                Database.Cmd = new SqlCommand(this._sql, Database.Con);
                Database.Cmd.Parameters.AddWithValue("@Name", this.ProductName);
                Database.Cmd.ExecuteNonQuery();
                Database.da = new SqlDataAdapter(Database.Cmd);
                Database.tbl = new DataTable();
                Database.da.Fill(Database.tbl);
                dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                dg.DataSource = Database.tbl;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error search Product:" + ex.Message);
            }
        }
    }
}
