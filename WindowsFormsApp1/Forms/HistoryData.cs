using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace WindowsFormsApp1.Forms
{
    public partial class HistoryData : Form
    {
        private static string constr;
        private static SqlConnection connection;

        private PlotModel plotModel = new PlotModel
        {
            Title = "历史数据",
            DefaultFont = "微软雅黑",

        };

        /// <summary>
        /// 坐标轴
        /// </summary>
        private static LinearAxis _TemAxisHistory = new LinearAxis()
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


        private static LinearAxis _mVAxisHistory_H = new LinearAxis()
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
        LineSeries line_Tem_H = new LineSeries()
        {
            Title = $"温度",
            Color = OxyPlot.OxyColors.Red,
            StrokeThickness = 1,
            MarkerSize = 2,
            MarkerStroke = OxyColors.Red,
            MarkerType = MarkerType.Triangle,
            YAxisKey = "Tem",

        };

        LineSeries line_O_H = new LineSeries()
        {
            Title = $"氧电压",
            Color = OxyPlot.OxyColors.Green,
            StrokeThickness = 1,
            MarkerSize = 2,
            MarkerStroke = OxyColors.Green,
            MarkerType = MarkerType.Circle,
            YAxisKey = "mV",


        };

        LineSeries line_P_H = new LineSeries()
        {
            Title = $"磷电压",
            Color = OxyPlot.OxyColors.Blue,
            StrokeThickness = 1,
            MarkerSize = 2,
            MarkerStroke = OxyColors.Blue,
            MarkerType = MarkerType.Triangle,
            YAxisKey = "mV",

        };




        public HistoryData()
        {
            InitializeComponent();

            
        }

        private void HistoryData_Load(object sender, EventArgs e)
        {
            ///oxyplot init
            var maxValue = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(0));
            var minValue = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(-20));


            plotModel.Axes.Add(new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = minValue,
                Maximum = maxValue,
                //IntervalType = DateTimeIntervalType.Minutes,
            });

            plotModel.Axes.Add(_TemAxisHistory);
            plotModel.Axes.Add(_mVAxisHistory_H);
            plotModel.Series.Add(line_Tem_H);
            plotModel.Series.Add(line_P_H);
            plotModel.Series.Add(line_O_H);
            this.plotView1.Model = plotModel;


            ///database connection init
            constr = Config.ReadSetting("constr");
            Console.WriteLine(constr);
            connection = new SqlConnection(constr);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
            DateTime date = this.dateTimePicker1.Value;
            displayData(date);
            

        }

        private void displayData(DateTime date)
        {

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            
            DataSet dset = new DataSet();  //创建dataSet
            DataRow drow;


            string sql = "SELECT * FROM [TABLE] where datediff(d,@dts,time)=0";
               
            SqlParameter[] sp = new SqlParameter[]{
                new SqlParameter("@dts",date),
                
            };
            SqlDataAdapter dataAda = new SqlDataAdapter(sql, connection);
            dataAda.SelectCommand.Parameters.AddRange(sp);
            dataAda.Fill(dset);
            int maxrows = dset.Tables[0].Rows.Count;

            DateTime time;///2
            double t;
            double O_mV;///4
            double P_mV;///5
            double tem;///1
            float span = float.Parse(this.textBox1.Text);
            if(maxrows == 0)
            {
                MessageBox.Show("当天无可用数据");
                return;
            }
            else
            {
                line_Tem_H.Points.Clear();
                line_P_H.Points.Clear();
                line_O_H.Points.Clear();
            }
            for (int i = 0; i <= maxrows-1; i++)
            {
                drow = dset.Tables[0].Rows[i];
                time = (DateTime)drow.ItemArray.GetValue(2);
                t = DateTimeAxis.ToDouble(time);
                
                tem = (double)drow.ItemArray.GetValue(1);
                O_mV = (double)drow.ItemArray.GetValue(4);
                P_mV = (double)drow.ItemArray.GetValue(1);
                line_Tem_H.Points.Add(new DataPoint(t, tem));
                line_O_H.Points.Add(new DataPoint(t, O_mV));
                line_P_H.Points.Add(new DataPoint(t, P_mV));

            }
            
            DateTime time0 = (DateTime)dset.Tables[0].Rows[0].ItemArray.GetValue(2);
            plotModel.Axes[0].Minimum = DateTimeAxis.ToDouble(time0);
            plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(time0.AddMinutes(span));

            connection.Close();
            plotView1.InvalidatePlot(true);


        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime date = this.dateTimePicker1.Value.Date;
            TimeSpan time = this.dateTimePicker2.Value.TimeOfDay;
            DateTime obj = date.Add(time);
            float span = float.Parse(this.textBox1.Text);
            plotModel.Axes[0].Minimum = DateTimeAxis.ToDouble(obj);
            plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(obj.AddMinutes(span));
                
            plotView1.InvalidatePlot(true);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                return;
            };
            double dbtime = plotModel.Axes[0].Minimum;
            DateTime t0 = DateTime.FromOADate(dbtime);
            float span = float.Parse(this.textBox1.Text);
            plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(t0.AddMinutes(span));
            plotView1.InvalidatePlot(true);
        }

        private void inputCheck_digit(object sender, KeyPressEventArgs e)
        {

            TextBox target = (TextBox)sender;
            e.Handled = InputControl.CheckDigitFormat(target, e);
        }

        private void HistoryData_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
