using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
namespace SV25_POS.Models
{
    internal class Role:Action
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        private string _sql="";
        private int _RowEffectd;
        DataGridViewRow DGV = null;
        public override void Create(DataGridView dg)
        {
            try
            {
                this._sql = "insert into tblRole(RoleName) values(@RoleName);";
                Database.Cmd = new SqlCommand(this._sql, Database.Con);
                Database.Cmd.Parameters.AddWithValue("@RoleName", this.RoleName);
               this._RowEffectd=Database.Cmd.ExecuteNonQuery();
                if (this._RowEffectd == 1)
                {
                    MessageBox.Show("Role created successfully");
                    this.GetData(dg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error create Role:" + ex.Message);
            }
        }
        public override void GetData(DataGridView dg)
        {
            try
            {
                this._sql = "select * from tblRole;";
                Database.Cmd=new SqlCommand(this._sql, Database.Con);
                Database.Cmd.ExecuteNonQuery();
                Database.da = new SqlDataAdapter(Database.Cmd);
                Database.tbl = new DataTable();
                Database.da.Fill(Database.tbl);
                dg.Rows.Clear();
                foreach (DataRow r in Database.tbl.Rows)
                {
                    this.Id = int.Parse(r["Id"].ToString());
                    this.RoleName=r["RoleName"].ToString();
                    Object[] row = {this.Id,this.RoleName};
                    dg.Rows.Add(row);
                }
            }
            catch (Exception ex) { 
                MessageBox.Show("Error get Data Role:"+ex.Message);
            }
        }
        public override void DeleteById(DataGridView dg)
        {
            try
            {
                if(dg.Rows.Count == 0)
                {
                    return;
                }
                var click = MessageBox.Show("Do you want to delete record?","Delete",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (click != DialogResult.Yes)
                {
                    return;
                }
                this.DGV = new DataGridViewRow();
                this.DGV = dg.SelectedRows[0];
                this.Id = int.Parse(this.DGV.Cells[0].Value.ToString());
                this._sql = "delete from tblRole where Id=@Id";
                Database.Cmd=new SqlCommand(this._sql,Database.Con);
                Database.Cmd.Parameters.AddWithValue("@Id",this.Id);
                this._RowEffectd=Database.Cmd.ExecuteNonQuery();
                if (this._RowEffectd == 1)
                {
                    MessageBox.Show("Role deleted successfully");
                    dg.Rows.Remove(this.DGV);
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Error delete role:" + ex.Message)
 ;           }
            
        }
        public void TransferDataToControls(DataGridView dg,TextBox txtRoleName)
        {
            if (dg.Rows.Count == 0)
            {
                return;
            }
            this.DGV = new DataGridViewRow();
            this.DGV=dg.SelectedRows[0];
            txtRoleName.Text = this.DGV.Cells[1].Value.ToString();
        }
        public override void UpdateById(DataGridView dg)
        {
            try
            {
                if (dg.Rows.Count == 0) {
                    return;

                }
                this.DGV=new DataGridViewRow();
                this.DGV = dg.SelectedRows[0];
                this.Id=int.Parse(this.DGV.Cells[0].Value.ToString());
                this._sql = "update tblRole set RoleName=@RoleName where Id=@Id";
                Database.Cmd=new SqlCommand(this._sql,Database.Con);
                Database.Cmd.Parameters.AddWithValue("@RoleName", this.RoleName);
                Database.Cmd.Parameters.AddWithValue("@Id", this.Id);
                this._RowEffectd=Database.Cmd.ExecuteNonQuery();
                if (this._RowEffectd == 1)
                {
                    MessageBox.Show("Role updated successfully");
                    this.GetData(dg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error update role:" + ex.Message);
            }
        }
        public override void Search(DataGridView dg)
        {
            try
            {
                this._sql = "select * from tblRole where RoleName like CONCAT('%',@RoleName,'%');";
                Database.Cmd = new SqlCommand(this._sql, Database.Con);
                Database.Cmd.Parameters.AddWithValue("@RoleName", this.RoleName);
                Database.Cmd.ExecuteNonQuery();
                Database.da = new SqlDataAdapter(Database.Cmd);
                Database.tbl = new DataTable();
                Database.da.Fill(Database.tbl);
                dg.Rows.Clear();
                foreach (DataRow r in Database.tbl.Rows)
                {
                    this.Id = int.Parse(r["Id"].ToString());
                    this.RoleName = r["RoleName"].ToString();
                    Object[] row = { this.Id, this.RoleName };
                    dg.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error search Role:" + ex.Message);
            }
        }
    }
    
}
