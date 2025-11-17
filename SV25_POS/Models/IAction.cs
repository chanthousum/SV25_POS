using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SV25_POS.Models
{
    internal interface IAction
    {
        void Create(DataGridView dg);
        void UpdateById(DataGridView dg);
        void DeleteById(DataGridView dg);
        void Search(DataGridView dg);
        void GetData(DataGridView dg);
    }
}
