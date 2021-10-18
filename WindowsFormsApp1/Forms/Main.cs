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
using OxyPlot.Axes;

using System.IO.Ports;
using System.Threading;




namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        
        public delegate void RefreshDelegate();

        private DateTimeAxis _dateAxis;//X轴
        private LinearAxis _valueAxis;//Y轴
        private PlotModel TestLine = new PlotModel { Title = "TestLine" };

        LineSeries line1 = new LineSeries()
        {
            Title = $"温度",
            Color = OxyPlot.OxyColors.Blue,
            StrokeThickness = 2,
            MarkerSize = 3,
            MarkerStroke = OxyColors.DarkGreen,

        };

        LineSeries line2 = new LineSeries()
        {
            Title = $"电压",
            Color = OxyPlot.OxyColors.Red,
            StrokeThickness = 2,
            MarkerSize = 3,
            MarkerStroke = OxyColors.Red,
            MarkerType = MarkerType.Circle,

        };


        public Form1()
        {
            InitializeComponent();
            

            Thread Test = new Thread(TestThread);

            
            
            var maxValue = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(0));
            var minValue = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(-20));

            TestLine.Axes.Add(new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = minValue,
                Maximum = maxValue,
                //IntervalType = DateTimeIntervalType.Minutes,
            });

            _valueAxis = new LinearAxis()
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IntervalLength = 80,
                Angle = 0,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                Maximum = 1,
                Minimum = -1,
                Title = "数值"
            };
            TestLine.Axes.Add(_valueAxis);


            TestLine.Series.Add(line1);


            this.plotView1.Model = TestLine;
            

            
        }
        private void TestThread()
        {
            DateTime now = System.DateTime.Now;
            string str_now = now.ToString()+"testflag";


        }

        public void UpdateText(string text, LineSeries line)
        {
            double t = DateTimeAxis.ToDouble(DateTime.Now);
            double value = double.Parse(text);
            line.Points.Add(new DataPoint(t, value));
        }

        public void UpdatePlot(string text, LineSeries line)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("---------Main_Load--------");


            timer1.Start();
            SettingInit();

            serialPort1.ReceivedBytesThreshold = 1;
            serialPort1.PortName = "COM1";

            serialPort1.BaudRate = 9600;
            serialPort1.Open();//打开串口
            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serialPort1_DataReceived);

            serialPort2.PortName = "COM2";
            serialPort2.Open();

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                Console.WriteLine("datareceived");
                int len = serialPort1.BytesToRead;
                Byte[] buf = new byte[len];
                int length = serialPort1.Read(buf, 0, len);
                string result = System.Text.Encoding.ASCII.GetString(buf);
                
                double value = double.Parse(result);
                double t = DateTimeAxis.ToDouble(DateTime.Now);
                double res1 = System.Math.Sin(t / 60);
                
                line1.Points.Add(new DataPoint(t, value));
                
                var date = DateTime.Now;
                TestLine.Axes[0].Maximum = DateTimeAxis.ToDouble(date.AddSeconds(1));
                TestLine.Axes[0].Minimum = DateTimeAxis.ToDouble(date.AddSeconds(-599));
                Console.WriteLine(result);
                plotView1.InvalidatePlot(true);

                
            }
            catch (Exception ex)
            {
                //MyDelegate dele1 = new MyDelegate(UpdateText);
                string result = ex.Message;
                

            }
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

       

        private void button3_Click(object sender, EventArgs e)
        {
            System.Timers.Timer t = new System.Timers.Timer(100);
            t.Elapsed += new System.Timers.ElapsedEventHandler(tick);
            t.AutoReset = true;
            t.Enabled = true;
            Console.Write( "----timer start-----" );
            t.Start();
            
            
        }

        private void tick(object source, System.Timers.ElapsedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            float sec = dt.Second;
            string msg = System.Math.Sin(sec *3 / 180 *3.14).ToString();
            Console.WriteLine( "--port writeline-" );
            this.serialPort2.WriteLine(msg);
        }


    }
}
