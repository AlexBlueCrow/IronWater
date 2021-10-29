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

using NModbus;
using NModbus.Extensions;
using System.Net.Sockets;
using System.IO.Ports;
using System.Threading;
using Modbus.Device;
using System.Configuration;




namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        
        public delegate void RefreshDelegate();
        private DateTimeAxis _dateAxis; //X轴
        private LinearAxis   _TemAxis;    //Y轴
        private LinearAxis   _mVAxis;
        private PlotModel plotModel = new PlotModel 
        { 
            Title = "plotModel",
            DefaultFont = "微软雅黑",

        };
        

        LineSeries line_Tem = new LineSeries()
        {
            Title = $"温度",
            Color = OxyPlot.OxyColors.Red,
            StrokeThickness = 1,
            MarkerSize = 2,
            MarkerStroke = OxyColors.Red,
            MarkerType = MarkerType.Triangle,

        };

        LineSeries line_O = new LineSeries()
        {
            Title = $"氧电压",
            Color = OxyPlot.OxyColors.Green,
            StrokeThickness = 1,
            MarkerSize = 2,
            MarkerStroke = OxyColors.Green,
            MarkerType = MarkerType.Circle,

        };

        LineSeries line_P = new LineSeries()
        {
            Title = $"磷电压",
            Color = OxyPlot.OxyColors.Blue,
            StrokeThickness = 1,
            MarkerSize = 2,
            MarkerStroke = OxyColors.Red,
            MarkerType = MarkerType.Triangle,

        };


        /// <summary>
        /// /////
        /// </summary>
        public string ipAddress = "192.168.0.20";
        public int tcpPort = 8000;
        public static TcpClient tcpClient = new TcpClient();



        /// <summary>
        /// //////////
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            

            Thread Test = new Thread(TestThread);

            
            
            var maxValue = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(0));
            var minValue = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(-20));

            plotModel.Axes.Add(new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = minValue,
                Maximum = maxValue,
                //IntervalType = DateTimeIntervalType.Minutes,
            });

            _TemAxis = new LinearAxis()
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IntervalLength = 80,
                Angle = 0,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                Maximum = 1900,
                Minimum = 0,
                Title = "温度"
            };
            
            plotModel.Axes.Add(_TemAxis);


            plotModel.Series.Add(line_Tem);
            plotModel.Series.Add(line_P);
            plotModel.Series.Add(line_O);
            

            this.plotView1.Model = plotModel;
            

            
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
                
                line_Tem.Points.Add(new DataPoint(t, value));
                plotView1.InvalidatePlot(true);
                var date = DateTime.Now;
                plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(date.AddSeconds(1));
                plotModel.Axes[0].Minimum = DateTimeAxis.ToDouble(date.AddSeconds(-599));
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
            System.Timers.Timer t = new System.Timers.Timer(1000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(tick);
            t.AutoReset = true;
            t.Enabled = true;
            Console.Write( "----timer start-----" );
            t.Start();
            return;
            
            
        }


        private void tick(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (!tcpClient.Connected)
                {
                    tcpClient.Connect(ipAddress, tcpPort);
                }
                
            }
            finally
            {
                ModbusIpMaster master = ModbusIpMaster.CreateIp(tcpClient);
                ushort startRef, noOfRefs;
                startRef = 0;
                noOfRefs = 8;

                byte slaveID = 01;
                ushort[] outputRegisterData = master.ReadHoldingRegisters(slaveID, startRef, noOfRefs);
                string output = String.Join(" | ", outputRegisterData);
                
                double t = DateTimeAxis.ToDouble(DateTime.Now);

                var date = DateTime.Now;
                plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(date.AddSeconds(1));
                plotModel.Axes[0].Minimum = DateTimeAxis.ToDouble(date.AddSeconds(-599));

                float value_00 = convertor(outputRegisterData[0], 03);
                float value_01 = convertor(outputRegisterData[1], 03);
                float value_02 = convertor(outputRegisterData[2], 03);
                line_Tem.Points.Add(new DataPoint( t , value_00 ));
                line_O.Points.Add(new DataPoint(t, value_01));
                line_O.Points.Add(new DataPoint(t, value_02));
                Console.WriteLine(value_00);

                plotView1.InvalidatePlot(true);
            }


        }

        private float convertor(int value,int type)
        {
            float max, min;
            switch (type)
            {
                case 0:
                    max = 15;
                    min = -15;
                    break;
                case 1:
                    max = 50;
                    min = -50;
                    break;
                case 02:
                    max = 100;
                    min = -100;
                    break;
                case 03:
                    max = 500;
                    min = -500;
                    break;
                case 04:
                    max = 1000;
                    min = -1000;
                    break;
                case 05:
                    max = 2500;
                    min = 0;
                    break;
                case 06:
                    max = 20;
                    min = -20;
                    break;
                case 07:
                    max = 20;
                    min = 4;
                    break;
                case 08:
                    max = 1100;
                    min = -200;
                    break;
                case 09:
                    max = 1400;
                    min = -250;
                    break;
                case 10:
                    max = 400;
                    min = -250;
                    break;
                case 11:
                    max = 900;
                    min = -250;
                    break;
                case 12:
                    max = 1750;
                    min = 0;
                    break;
                case 13:
                    max = 1750;
                    min = 0;
                    break;
                case 14:
                    max = 1800;
                    min = 0;
                    break;
                case 15:
                    max = 1300;
                    min = -250;
                    break;

            }

            float res = value * ( max - min ) / 65535 + (min);
            return res;
        }


    }
}
