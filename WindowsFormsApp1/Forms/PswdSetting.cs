using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Forms
{
    public partial class PswdSetting : Form
    {
        public PswdSetting()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string oldpswd = textBox1.Text;
            MessageBox.Show(oldpswd);

        }
    }
}
