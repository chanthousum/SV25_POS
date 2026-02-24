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
    public class Database
    {
        public static SqlConnection Con = new SqlConnection(@"Data Source=.;Initial Catalog=SV25_POS_DB;Persist Security Info=True;User ID=sa;Password=Password123@;Encrypt=False");
        public static SqlCommand Cmd=null;
        public static SqlDataAdapter da = null;
        public static DataTable tbl = null;
        public static void ConnectionDb()
        {
            try
            {
                if (Con.State == System.Data.ConnectionState.Closed)
                {
                    Con.Open();
                   // MessageBox.Show("Database connected");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Connection to Database:" + ex.Message);
            }
        }
    }
}
