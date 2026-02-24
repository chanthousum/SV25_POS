using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SV25_POS.Models
{
    public class Action : IAction
    {
        public virtual void Create(DataGridView dg)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteById(DataGridView dg)
        {
            throw new NotImplementedException();
        }

        public virtual void GetData(DataGridView dg)
        {
            throw new NotImplementedException();
        }

        public virtual void Search(DataGridView dg)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateById(DataGridView dg)
        {
            throw new NotImplementedException();
        }
    }
}
