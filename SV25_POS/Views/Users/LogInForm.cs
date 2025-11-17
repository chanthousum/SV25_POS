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
    public partial class LogInForm : Form
    {
        public LogInForm()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            User user = new User();
            user.UserName=txtUserName.Text.Trim();
            user.Password=txtPassword.Text.Trim();
            user.Authentication(this);
        }
    }
}
