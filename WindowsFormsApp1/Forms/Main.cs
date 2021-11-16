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
using System.Data.SqlClient;
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
        
        public delegate void Refresher(string text1, string text2, string text3);
        

        private static string constr;


        private static SqlConnection connection;

        public static TcpClient tcpClient = new TcpClient();

        public delegate void MyDelegate(string s);
       
        private static System.Timers.Timer read_timer = new System.Timers.Timer(200);
       

        private PlotModel plotModel = new PlotModel
        {
            Title = "plotModel",
            DefaultFont = "微软雅黑",

        };

        /// <summary>
        /// 坐标轴
        /// </summary>
        private static LinearAxis _TemAxis = new LinearAxis()
        {
            Position = AxisPosition.Left,
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot,
            IntervalLength = 80,
            Angle = 0,
            IsZoomEnabled = false,
            IsPanEnabled = false,
            Maximum = 1900,
            Minimum = 0,
            Title = "温度",
            Key = "Tem",
            MajorStep = 100,
            MinorStep = 50,
           /* AxislineColor = OxyColors.White,
            TextColor = OxyColors.White,
            TitleColor = OxyColors.White,*/


        };   //Y轴

        private static  LinearAxis   _mVAxis = new LinearAxis()
        {
            Position = AxisPosition.Right,
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot,
            IntervalLength = 80,
            Angle = 0,
            IsZoomEnabled = false,
            IsPanEnabled = false,
            Maximum = 500,
            Minimum = -500,
            Title = "mV",
            Key = "mV",
            

        }; 
        
        /// <summary>
        /// 曲线设置
        /// </summary>
        LineSeries line_Tem = new LineSeries()
        {
            Title = $"温度",
            Color = OxyPlot.OxyColors.Red,
            StrokeThickness = 1,
            MarkerSize = 2,
            MarkerStroke = OxyColors.Red,
            MarkerType = MarkerType.Triangle,
            YAxisKey = "Tem",
            
        };

        LineSeries line_O = new LineSeries()
        {
            Title = $"氧电压",
            Color = OxyPlot.OxyColors.Green,
            StrokeThickness = 1,
            MarkerSize = 2,
            MarkerStroke = OxyColors.Green,
            MarkerType = MarkerType.Circle,
            YAxisKey = "mV",


        };

        LineSeries line_P = new LineSeries()
        {
            Title = $"磷电压",
            Color = OxyPlot.OxyColors.Blue,
            StrokeThickness = 1,
            MarkerSize = 2,
            MarkerStroke = OxyColors.Blue,
            MarkerType = MarkerType.Triangle,
            YAxisKey = "mV",

        };


        /// <summary>
        /// /////
        /// </summary>
        


        /// <summary>
        /// //////////
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            var maxValue = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(0));
            var minValue = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(-20));

            plotModel.Axes.Add(new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = minValue,
                Maximum = maxValue,
                //IntervalType = DateTimeIntervalType.Minutes,
            });
            plotModel.Axes.Add(_TemAxis);
            plotModel.Axes.Add(_mVAxis);
            plotModel.Series.Add(line_Tem);
            plotModel.Series.Add(line_P);
            plotModel.Series.Add(line_O);
            

            this.plotView1.Model = plotModel;
            

            
        }


       

        public void UpdateText(string text, LineSeries line)
        {
            double t = DateTimeAxis.ToDouble(DateTime.Now);
            double value = double.Parse(text);
            line.Points.Add(new DataPoint(t, value));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;
            Console.WriteLine("---------Main_Load--------");
            ConfigInit();
            SettingInit();
            DataBaseInit();
            TimerInit();
            
            OxyInit(true);
            //timer1.Start();



        }



        private void 系统设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SysSetting subform = new SysSetting();
            //subform.MdiParent = this;
            subform.Owner = this;
            subform.StartPosition = FormStartPosition.CenterScreen;
            subform.ShowDialog();

        }


        private void 检测设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SensorSetting subform = new SensorSetting();
            subform.Owner = this;
            subform.StartPosition = FormStartPosition.CenterScreen;
            //subform.MdiParent = this;
            //subform.Show()
            subform.ShowDialog();
        }

        private void 历史数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            HistoryData subform = new HistoryData();
            subform.Owner = this;
            subform.TopLevel = false;
            subform.Parent = this.panel1;
            subform.StartPosition = FormStartPosition.CenterScreen;
            subform.Show();
            subform.BringToFront();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label2.Text = DateTime.Now.ToString("yyyy.MM.dd. hh时mm分ss秒");
        }

        private void TimerInit()
        {
            read_timer.Elapsed += new System.Timers.ElapsedEventHandler(tick);
            read_timer.AutoReset = true;
            read_timer.Enabled = true;
            try
            {
                int period = int.Parse(Config.ReadSetting("period"));
                //Console.WriteLine(period);
                read_timer.Interval = period;
            }
            catch
            {
                //Console.WriteLine(Config.ReadSetting("period"));

            }
        }

        private void ConfigInit()
        {
           
        }
        private void DataBaseInit()
        {
            //constr = Properties.Settings.Default.ConfigurationDataConnectionString;
            string dbfile = Environment.CurrentDirectory;
            
            constr = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = 'D:\\working place\\WindowsFormsApp1\\dbfile\\Database1.mdf'; Integrated Security = True";
            string conName = "WindowsFormsApp1.Properties.Settings.ConfigurationDataConnectionString";
            //constr = ConfigurationManager.ConnectionStrings[conName].ConnectionString;
            Config.AddUpdateAppSettings("constr", constr);
            connection = new SqlConnection(constr);
        }

        private void SettingInit()
        {
            string ip = "192.168.0.1";
            string portnum = "8000";
            Config.AddUpdateAppSettings("modbusIp", ip);
            Config.AddUpdateAppSettings("portNum", portnum);

            try
            {
                string Axis_Tem_max = Config.ReadSetting("Axis_Tem_max");
                int T_ax = Convert.ToInt32(float.Parse(Axis_Tem_max));
            }
            catch
            {
                MessageBox.Show("未检测到设置文件，初始化为默认配置");
                Config.AddUpdateAppSettings("Axis_Tem_max","1800");
                Config.AddUpdateAppSettings("Axis_Tem_min","0");
                Config.AddUpdateAppSettings("Axis_mV_max","500");
                Config.AddUpdateAppSettings("Axis_mV_min","-500");
                Config.AddUpdateAppSettings("timeSpan","20");
                Config.AddUpdateAppSettings("ThermalType","B");
                Config.AddUpdateAppSettings("pericd", "500");

            }
            
        }

        public void OxyInit(bool resettime)
        {
            
            
            var time = DateTime.Now;
            string Axis_Tem_max = Config.ReadSetting("Axis_Tem_max");
            
            string Axis_Tem_min = Config.ReadSetting("Axis_Tem_min");
            string Axis_mV_max = Config.ReadSetting("Axis_mV_max");
            string Axis_mV_min = Config.ReadSetting("Axis_mV_min");
            string timespan = Config.ReadSetting("timeSpan");
            int seconds = 600;
            int T_max = 1800;
            int T_min = 0;
            int mV_max = 500;
            int mV_min = -500;
            try
            {
                seconds = (int.Parse(timespan))*60;
                T_max = Convert.ToInt32(float.Parse(Axis_Tem_max));
                T_min = Convert.ToInt32(float.Parse(Axis_Tem_min));
                mV_max = Convert.ToInt32(float.Parse(Axis_mV_max));
                mV_min = Convert.ToInt32(float.Parse(Axis_mV_min));
            }
            catch
            {
                seconds = 600;
                T_max = 1800;
                T_min = 0;
                mV_max = 500;
                mV_min = -500;
            }


            


            if (resettime == true)
            {
                plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(time.AddSeconds(seconds));
                plotModel.Axes[0].Minimum = DateTimeAxis.ToDouble(time.AddSeconds(0));
            }
            else
            {
                
                plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.FromOADate(plotModel.Axes[0].Minimum).AddSeconds(seconds));

            }

            plotModel.Axes[1].Maximum = T_max;
            plotModel.Axes[1].Minimum = T_min;
            plotModel.Axes[2].Maximum = mV_max;
            plotModel.Axes[2].Minimum = mV_min;

            plotView1.InvalidatePlot(true);

        }

        

     

       

        private void start_Click(object sender, EventArgs e)
        {
            
            
            

            var date = DateTime.Now;
            string timespan = Config.ReadSetting("timeSpan");
            int seconds = (int.Parse(timespan)) * 60;
            plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(date.AddSeconds(seconds));
            plotModel.Axes[0].Minimum = DateTimeAxis.ToDouble(date.AddSeconds(0));

            read_timer.Start();
            return;
            
            
        }


        private void tick(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
     
                if (!tcpClient.Connected)
                {
                    
                    string ipAddress = Config.ReadSetting("modbusIp");
                   
                    int tcpPort = int.Parse(Config.ReadSetting("portNum"));
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
                DateTime time = DateTime.Now;
               
                double t = DateTimeAxis.ToDouble(time);

                var date = DateTime.Now;
                if ( DateTimeAxis.ToDouble(date) > plotModel.Axes[0].Maximum )
                {
                    plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(date);
                }
                

                string thermaltype = Config.ReadSetting( "ThermalType");
                
                ushort code = ModbusTools.TypeToCode(thermaltype);

                float value_01 = ModbusTools.convertor(outputRegisterData[1], code);
                float value_02 = ModbusTools.convertor(outputRegisterData[2], 03);
                float value_03 = ModbusTools.convertor(outputRegisterData[3], 03);
                string text_01 = value_01.ToString("f1");
                string text_02 = value_02.ToString("f2");
                string text_03 = value_03.ToString("f2");
                line_Tem.Points.Add(new DataPoint( t , value_01 ));
                line_O.Points.Add(new DataPoint(t, value_02));
                line_P.Points.Add(new DataPoint(t, value_03));
               
                string identity = "test";
                /* 数据存入数据库*/
                string sql = "INSERT INTO [Table](time,tem,type,[identity],O_mv,P_mv) VALUES(@time,@tem,@type,@identity,@O_mv,@P_mv)";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@tem", value_01);
                cmd.Parameters.AddWithValue("@O_mv", value_02);
                cmd.Parameters.AddWithValue("@P_mv", value_03);
                cmd.Parameters.AddWithValue("@time", time);
                cmd.Parameters.AddWithValue("@identity", identity);
                cmd.Parameters.AddWithValue("@type", thermaltype);

                if( connection.State == ConnectionState.Closed )
                {
                    connection.Open();
                }
                if (connection.State == ConnectionState.Open)
                {
                    int i = cmd.ExecuteNonQuery();
                }
                
                //connection.Close();
                
                /*if (i == 0)
                {
                    Console.WriteLine("Data Saving error");
                }
*/
                /*刷新控件*/
                Refresher rs = new Refresher( renewText );
                label8.Invoke(rs, text_01, text_02, text_03);
                plotView1.InvalidatePlot(true);


            }


        }

        

        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void renewText(string text1, string text2, string text3)
        {
            this.label8.Text = text1;
            this.label9.Text = text2;
            this.label10.Text = text3;
        }

        private void stop_Click(object sender, EventArgs e)
        {
            read_timer.Stop();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void resetpraph()
        {
            


            plotView1.InvalidatePlot(true);
        }

        private void plotView1_MouseHover(object sender, EventArgs e)
        {

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            int width = this.Size.Width;
            int height = this.Size.Height;
            int bar_h = 200;

            this.panel1.Size = this.Size;
            this.plotView1.Size = new Size(width, (int)(height * 0.9));
            this.btn_start.Location = new Point((int)(width*0.5 - 100), (int)(height * 0.9));
            this.btn_stop.Location = new Point((int)(width * 0.5 + 100), (int)(height * 0.9));

            
            

        }
    }
}
