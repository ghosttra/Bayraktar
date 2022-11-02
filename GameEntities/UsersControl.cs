using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameServerInterface
{
    public partial class UsersControl : Form
    {
        public UsersControl()
        {
            InitializeComponent();
        }
        

        private void dbButtons_Load(object sender, EventArgs e)
        {
            dbButtons.delBtn.Click += DelBtn_Click;
            dbButtons.updBtn.Click += UpdBtn_Click;
            dbButtons.addBtn.Click += AddBtn_Click;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {

        }

        private void UpdBtn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
