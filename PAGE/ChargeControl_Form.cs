using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TestApp.FUNCTION;

namespace TestApp.PAGE
{
    public partial class ChargeControl_Form : Form
    {
        private Main_New mainForm;
        string deviceAddress = "";
        public ChargeControl_Form(Main_New mainForm)
        {
            InitializeComponent();
            this.Load += ChargeControl_Form_Load;
            this.mainForm = mainForm;
        }
        private void ChargeControl_Form_Load(object sender, EventArgs e)
        {
            GetAddress();
        }
        private void GetAddress()
        {
            try
            {
                string filePath = "DeviceAddressNew.json";
                if (!File.Exists(filePath))
                    return;

                string json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                data.TryGetValue("charge_textBox", out object charge);
                if (charge != null)
                {
                    deviceAddress = charge.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载DeviceAddress.json失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private async void start_output_button_Click(object sender, EventArgs e)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }

            string chString = ch_comboBox.Text.Replace("CH", "");
            int ch = int.Parse(chString);
            await scpiDevice.SelectChannel(ch);
            await scpiDevice.EnableOutput();
            mainForm.LogToConsole("输出已开启");
            scpiDevice.Disconnect(); // 释放资源
        }

        private async void stop_output_button_Click(object sender, EventArgs e)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            string chString = ch_comboBox.Text.Replace("CH", "");
            int ch = int.Parse(chString);
            await scpiDevice.SelectChannel(ch);
            await scpiDevice.DisableOutput();
            mainForm.LogToConsole("输出已关闭");
            scpiDevice.Disconnect(); // 释放资源
        }

        private async void manual_button_Click(object sender, EventArgs e)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            string chString = ch_comboBox.Text.Replace("CH", "");
            int ch = int.Parse(chString);
            double voltage = double.Parse(voltage_textBox.Text);
            double current = double.Parse(current_textBox.Text);
            await scpiDevice.SelectChannel(ch);
            await scpiDevice.SetVoltage(voltage);
            await scpiDevice.SetCurrent(current);
            mainForm.LogToConsole("执行修改:通道：" + ch_comboBox.Text + ", 输出电压：" + voltage + ", 输出电流：" + current);
            scpiDevice.Disconnect(); // 释放资源
        }

        private void disconnect_button_Click(object sender, EventArgs e)
        {

        }
    }
}
