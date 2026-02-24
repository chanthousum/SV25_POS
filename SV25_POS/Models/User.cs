using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using SV25_POS.Views.Users;
namespace SV25_POS.Models
{
    public class User : Action
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public string Sql { get; set; } = "";
        public static string LogInUserName { get; set; }
        public static int LogInUserId { get; set; }
        public static string RoleName { get; set; }="";
        private string _sql = "";
        private int _RowEffectd;
        DataGridViewRow DGV = null;
        public int RoleId { get; set; }
        public void Authentication(Form form)
        {
            try
            {
                Database.ConnectionDb();
                this.Sql = "select * from View_User_Role1 where UserName=@UserName and Password=@Password and Status=1";
                Database.Cmd=new SqlCommand(this.Sql,Database.Con);
                Database.Cmd.Parameters.AddWithValue("@UserName", this.UserName);
                Database.Cmd.Parameters.AddWithValue("@Password", this.Password);
                Database.Cmd.ExecuteNonQuery();
                Database.da=new SqlDataAdapter(Database.Cmd);
                Database.tbl = new DataTable();
                Database.da.Fill(Database.tbl);
                if(Database.tbl.Rows.Count > 0)
                {
                    DashbardForm dashbardForm = new DashbardForm();
                    User.LogInUserName = this.UserName;
                    User.LogInUserId = int.Parse(Database.tbl.Rows[0]["Id"].ToString());
                    User.RoleName = Database.tbl.Rows[0]["RoleName"].ToString();
                    form.Hide();
                    dashbardForm.Show();
                }
                else
                {
                    MessageBox.Show("Username and Password is invalid!","Invalid",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error login:" + ex.Message);
            }
        }
        public void SetRoleName(ComboBox cboRoleName)
        {
            try
            {
                this._sql = "select * from tblRole";
                Database.Cmd=new SqlCommand(this._sql,Database.Con);
                Database.Cmd.ExecuteNonQuery ();
                Database.da=new SqlDataAdapter(Database.Cmd);
                Database.tbl = new DataTable();
                Database.da.Fill(Database.tbl);
                cboRoleName.Items.Clear();
                foreach(DataRow r in Database.tbl.Rows) {
                    cboRoleName.Items.Add(r["RoleName"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error set Role Name:"+ex.Message);
            }
        }
        public int GetRoleId(ComboBox cboRoleName)
        {
            int id = 0;
            try
            {
                this._sql = "select * from tblRole where RoleName=@RoleName";
                Database.Cmd=new SqlCommand(this._sql,Database.Con);
                Database.Cmd.Parameters.AddWithValue("@RoleName",cboRoleName.Text);
                Database.Cmd.ExecuteNonQuery ();
                Database.da=new SqlDataAdapter( Database.Cmd);
                Database.tbl = new DataTable();
                Database.da.Fill(Database.tbl);
                if (Database.tbl.Rows.Count > 0)
                {
                    id = int.Parse(Database.tbl.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error get Role Id:"+ex.Message);
            }
            return id;
        }
        public override void Create(DataGridView dg)
        {
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlTransaction=Database.Con.BeginTransaction();
                this._sql = "insert into tblUser(UserName,Gender,Password,Email,Status,CreateBy,CreateAt)values(@UserName,@Gender,@Password,@Email,@Status,@CreateBy,GETDATE());select SCOPE_IDENTITY();";
                Database.Cmd = new SqlCommand(this._sql, Database.Con,sqlTransaction);
                Database.Cmd.Parameters.AddWithValue("@UserName", this.UserName);
                Database.Cmd.Parameters.AddWithValue("@Gender", this.Gender);
                Database.Cmd.Parameters.AddWithValue("@Password", this.Password);
                Database.Cmd.Parameters.AddWithValue("@Email", this.Email);
                Database.Cmd.Parameters.AddWithValue("@Status", this.Status);
                Database.Cmd.Parameters.AddWithValue("@CreateBy",User.LogInUserId);
                this.Id=Convert.ToInt32(Database.Cmd.ExecuteScalar());


                this._sql = "insert into tblUserRole(UserId,RoleId)values(@UserId,@RoleId);";
                Database.Cmd=new SqlCommand(this._sql, Database.Con,sqlTransaction);
                Database.Cmd.Parameters.AddWithValue("@UserId",this.Id);
                Database.Cmd.Parameters.AddWithValue("@RoleId", this.RoleId);
                Database.Cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
                MessageBox.Show("User created successfully");
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                MessageBox.Show("Error create User:" + ex.Message);
            }
        }
        public override void GetData(DataGridView dg)
        {
            try
            {
                this._sql = "select * from View_User_Role";
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
                MessageBox.Show("Error get Data User:" + ex.Message);
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
        public void TransferDataToControls(DataGridView dg, TextBox txtUserName,RadioButton rMale,RadioButton rFemale,TextBox txtPassword,TextBox txtEmail,RadioButton rActive,RadioButton rInactive,ComboBox cboRoleName)
        {
            if (dg.Rows.Count == 0)
            {
                return;
            }
            this.DGV = new DataGridViewRow();
            this.DGV = dg.SelectedRows[0];
            this.Id=int.Parse(DGV.Cells[0].Value.ToString());
            this._sql = "select * from View_User_Role1 where id=@Id";
            Database.Cmd=new SqlCommand(this._sql, Database.Con);
            Database.Cmd.Parameters.AddWithValue("@Id", this.Id);
            Database.da=new SqlDataAdapter(Database.Cmd);
            Database.tbl = new DataTable();
            Database.da.Fill(Database.tbl);
            if (Database.tbl.Rows.Count > 0)
            {
                txtUserName.Text = Database.tbl.Rows[0]["UserName"].ToString();
                this.Gender = Database.tbl.Rows[0]["Gender"].ToString();
                if (this.Gender == "Male")
                {
                    rMale.Checked = true;
                }
                else
                {
                    rFemale.Checked = true;
                }
                txtPassword.Text = Database.tbl.Rows[0]["Password"].ToString();
                txtEmail.Text = Database.tbl.Rows[0]["Email"].ToString();
                this.Status = bool.Parse(Database.tbl.Rows[0]["Status"].ToString());
                if(this.Status == true)
                {
                    rActive.Checked = true;
                }
                else
                {
                    rInactive.Checked = true;
                }
                cboRoleName.Text= Database.tbl.Rows[0]["RoleName"].ToString();
            }

           
             
        }
        public override void UpdateById(DataGridView dg)
        {
            SqlTransaction sqlTransaction = null;
            try
            {
                if (dg.Rows.Count == 0)
                {
                    return;

                }
                sqlTransaction=Database.Con.BeginTransaction();
                this.DGV = new DataGridViewRow();
                this.DGV = dg.SelectedRows[0];
                this.Id = int.Parse(this.DGV.Cells[0].Value.ToString());
                this._sql = "update tblUser set UserName=@UserName,Gender=@Gender,Password=@Password,Email=@Email,Status=@Status,UpdateBy=@UpdateBy,UpdateAt=GETDATE() where Id=@Id;";
                Database.Cmd = new SqlCommand(this._sql, Database.Con, sqlTransaction);
                Database.Cmd.Parameters.AddWithValue("@UserName", this.UserName);
                Database.Cmd.Parameters.AddWithValue("@Gender", this.Gender);
                Database.Cmd.Parameters.AddWithValue("@Password", this.Password);
                Database.Cmd.Parameters.AddWithValue("@Email", this.Email);
                Database.Cmd.Parameters.AddWithValue("@Status", this.Status);
                Database.Cmd.Parameters.AddWithValue("@UpdateBy", User.LogInUserId);
                Database.Cmd.Parameters.AddWithValue("@Id",this.Id);
                Database.Cmd.ExecuteNonQuery();

                this._sql = "update tblUserRole set RoleId=@RoleId where UserId=@UserId";
                Database.Cmd = new SqlCommand(this._sql, Database.Con, sqlTransaction);
                Database.Cmd.Parameters.AddWithValue("@RoleId", this.RoleId);
                Database.Cmd.Parameters.AddWithValue("@UserId", this.Id);
                Database.Cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
                MessageBox.Show("User updated successfully");

            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                MessageBox.Show("Error update User:" + ex.Message);
            }
        }
        public override void Search(DataGridView dg)
        {
            try
            {
                this._sql = "select * from View_User_Role where [User Name] like CONCAT('%',@UserName,'%');";
                Database.Cmd = new SqlCommand(this._sql, Database.Con);
                Database.Cmd.Parameters.AddWithValue("@UserName", this.UserName);
                Database.Cmd.ExecuteNonQuery();
                Database.da = new SqlDataAdapter(Database.Cmd);
                Database.tbl = new DataTable();
                Database.da.Fill(Database.tbl);
                dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                dg.DataSource = Database.tbl;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error search User:" + ex.Message);
            }
        }
    }
}
