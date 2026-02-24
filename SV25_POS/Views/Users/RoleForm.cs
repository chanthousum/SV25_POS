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
    public partial class RoleForm : Form
    {
        public RoleForm()
        {
            InitializeComponent();
        }
        Role role;
        private void btnCreate_Click(object sender, EventArgs e)
        {
            role = new Role();
            role.RoleName=txtRoleName.Text.Trim();
            role.Create(dgRole);
        }

        private void RoleForm_Load(object sender, EventArgs e)
        {
            Database.ConnectionDb();
            role=new Role();
            role.GetData(dgRole);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            role = new Role();
            role.DeleteById(dgRole);
        }

        private void dgRole_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            role = new Role();
            role.TransferDataToControls(dgRole, txtRoleName);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            role=new Role();
            role.RoleName = txtRoleName.Text.Trim();
            role.UpdateById(dgRole);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                role = new Role();
                role.RoleName=txtSearch.Text.Trim();
                role.Search(dgRole);
            }
        }
    }
}
