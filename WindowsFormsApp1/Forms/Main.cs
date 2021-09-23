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
using OxyPlot;
using OxyPlot.Series;
using System.IO.Ports;
using System.Threading;




namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {


        

        public Form1()
        {
            InitializeComponent();
            var myModel = new PlotModel { Title = "Example 1" };
            myModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            this.plotView1.Model = myModel;

            

            
        }
        private void TestThread()
        {
            DateTime now = System.DateTime.Now;
            string str_now = now.ToString()+"testflag";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("---------Main_Load--------");
            timer1.Start();
            SettingInit();
            


        }

        private void 系统设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            SysSetting subform = new SysSetting();
            //subform.MdiParent = this;
            subform.StartPosition = FormStartPosition.CenterScreen;
            subform.ShowDialog();


        }


        private void 检测设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SensorSetting subform = new SensorSetting();
            subform.StartPosition = FormStartPosition.CenterScreen;
            //subform.MdiParent = this;
            //subform.Show()
            subform.ShowDialog();
        }

        private void 历史数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryData subform = new HistoryData();
            subform.TopLevel = false;
            subform.Parent = this.panel1;
            subform.StartPosition = FormStartPosition.CenterScreen;
            subform.Show();
            subform.BringToFront();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = System.DateTime.Now.ToString();
        }

        private void SettingInit()
        {
            Console.WriteLine("-----Init-----");
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            this.label2.Text = DateTime.Now.ToString("yyyy.MM.dd. hh时mm分ss秒");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread Test = new Thread(TestThread);
        }
    }
}
