using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Forms;




namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

        }

        private void 系统设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            SysSetting subform = new SysSetting();
            subform.MdiParent = this;
            subform.Show();

        }


        private void 检测设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SensorSetting subform = new SensorSetting();
            subform.MdiParent = this;
            subform.Show();
        }

        private void 历史数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryData subform = new HistoryData();
            subform.MdiParent = this;
            subform.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = System.DateTime.Now.ToString();
        }
    }
}
