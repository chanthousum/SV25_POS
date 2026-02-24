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

namespace SV25_POS.Views.Users
{
    public partial class UserForm : Form
    {
        User user;
        Role role;
        public UserForm()
        {
            InitializeComponent();
            user = new User();
            user.SetRoleName(cboRoleName);
        }
        
        private void btnCreate_Click(object sender, EventArgs e)
        {
            user = new User();
            user.UserName=txtUserName.Text.Trim();
            if (rMale.Checked)
            {
                user.Gender = rMale.Text;
            }
            else
            {
                user.Gender = rFemale.Text;
            }
            user.Password = txtPassword.Text.Trim();
            user.Email = txtEmail.Text.Trim();
            if (rActive.Checked)
            {
                user.Status = true;
            }
            else
            {
                user.Status = false;
            }
            user.RoleId = user.GetRoleId(cboRoleName);
            user.Create(dgUser);
                
        }

        private void RoleForm_Load(object sender, EventArgs e)
        {
            user=new User();
            user.GetData(dgUser);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            role = new Role();
            role.DeleteById(dgUser);
        }

        private void dgRole_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            user = new User();
            user.TransferDataToControls(dgUser, txtUserName, rMale, rFemale, txtPassword, txtEmail, rActive, rInactive, cboRoleName);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            user=new User();
            user.UserName = txtUserName.Text.Trim();
            if (rMale.Checked)
            {
                user.Gender = rMale.Text;
            }
            else
            {
                user.Gender = rFemale.Text;
            }
            user.Password = txtPassword.Text.Trim();
            user.Email = txtEmail.Text.Trim();
            if (rActive.Checked)
            {
                user.Status = true;
            }
            else
            {
                user.Status = false;
            }
            user.RoleId = user.GetRoleId(cboRoleName);
            user.UpdateById(dgUser);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                user = new User();
                user.UserName=txtSearch.Text.Trim();
                user.Search(dgUser);
            }
        }
    }
}
