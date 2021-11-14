using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

using System.Net.Sockets;
using Modbus.Device;




namespace WindowsFormsApp1.Forms
{
    public partial class SensorSetting : Form
    {
        public SensorSetting()
        {
            InitializeComponent();

        }

        private void SensorSetting_Load(object sender, EventArgs e)
        {
            string ip = Config.ReadSetting("modbusIp");
            string port = Config.ReadSetting("portNum");
            string OChan = Config.ReadSetting("OChan");
            string PChan = Config.ReadSetting("PChan");
            string TChan = Config.ReadSetting("TChan");
            string Ttype = Config.ReadSetting("ThermalType");
            string Otype = Config.ReadSetting("OType");
            string Ptype = Config.ReadSetting("PType");
            string tem_modi = Config.ReadSetting("Tem_modification");
            string O_modi = Config.ReadSetting("O_modification");
            string P_modi = Config.ReadSetting("P_modification");
            string Axis_Tem_max = Config.ReadSetting("Axis_Tem_max");
            string Axis_Tem_min = Config.ReadSetting("Axis_Tem_min");
            string Axis_mV_max = Config.ReadSetting("Axis_mV_max");
            string Axis_mV_min = Config.ReadSetting("Axis_mV_min");
            string timespan = Config.ReadSetting("timeSpan");
            string period = Config.ReadSetting("period");


            this.textBox1.Text = ip;
            this.textBox2.Text = port;
            this.textBox3.Text = TChan;
            this.textBox4.Text = OChan;
            this.textBox5.Text = PChan;
            this.textBox3.ReadOnly = true;
            this.textBox4.ReadOnly = true;
            this.textBox5.ReadOnly = true;
            this.comboBox2.Text = Otype;
            this.comboBox3.Text = Ptype;
            this.Box_Ttype.Text = Ttype;
            this.textBox6.Text = tem_modi;
            this.textBox7.Text = O_modi;
            this.textBox8.Text = P_modi;
            this.textBox9.Text = Axis_Tem_max;
            this.textBox10.Text = Axis_Tem_min;
            this.textBox11.Text = Axis_mV_max;
            this.textBox12.Text = Axis_mV_min;
            this.textBox13.Text = timespan;
            this.textBox14.Text = period;


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Config.AddUpdateAppSettings("Tem_modification", this.textBox6.Text);
            Config.AddUpdateAppSettings("O_modification", this.textBox7.Text);
            Config.AddUpdateAppSettings("P_modification", this.textBox8.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Save_Click(object sender, EventArgs e)
        {
            string modbusIp = this.textBox1.Text;
            string portNum = (this.textBox2.Text);
            string TChan = this.textBox3.Text;
            string OChan = this.textBox4.Text;
            string PChan = this.textBox5.Text;
            Config.AddUpdateAppSettings("modbusIp", modbusIp);
            Config.AddUpdateAppSettings("portNum", portNum);
            Config.AddUpdateAppSettings("TChan", TChan);
            Config.AddUpdateAppSettings("OChan", OChan);
            Config.AddUpdateAppSettings("PChan", PChan);

        }



        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void Save_type_Click(object sender, EventArgs e)
        {
            string thermaltype = this.Box_Ttype.Text;
            string Otype = this.comboBox2.Text;
            string Ptype = this.comboBox3.Text;
            ushort code = ModbusTools.TypeToCode(thermaltype);
            int codeO = int.Parse(Otype);
            int codeP = int.Parse(Ptype);
            

            string ip = Config.ReadSetting("modbusIp");
            string port = Config.ReadSetting("portNum");
            int portnum = int.Parse(port);
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(ip, portnum);
            byte slaveID = 01;
            ModbusIpMaster master = ModbusIpMaster.CreateIp(tcpClient);
            master.WriteSingleRegister( slaveID, 201 , code );

            
            Config.AddUpdateAppSettings("ThermalType", thermaltype);
            Config.AddUpdateAppSettings("OType", Otype);
            Config.AddUpdateAppSettings("PType", Ptype);


        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click_1(object sender, EventArgs e)
        {

        }

        private void save_graph_Click(object sender, EventArgs e)
        {
            Config.AddUpdateAppSettings("Axis_Tem_max", this.textBox9.Text);
            Config.AddUpdateAppSettings("Axis_Tem_min", this.textBox10.Text);
            Config.AddUpdateAppSettings("Axis_mV_max", this.textBox11.Text);
            Config.AddUpdateAppSettings("Axis_mV_min", this.textBox12.Text);
            Config.AddUpdateAppSettings("timeSpan", this.textBox13.Text);
            Config.AddUpdateAppSettings("period", this.textBox14.Text);
            
            Form1 f1 = (Form1)this.Owner;
            f1.OxyInit(false);
        }

       

        private void inputCheck_digit(object sender, KeyPressEventArgs e)
        {
            
            TextBox target = (TextBox)sender;
            e.Handled = InputControl.CheckDigitFormat(target, e);
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
