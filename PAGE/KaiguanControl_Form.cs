using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using TestApp.FUNCTION;

namespace TestApp.PAGE
{
    public partial class KaiguanControl_Form : MetroForm
    {
        private Main_New mainForm;
        string deviceAddress = "";
        public KaiguanControl_Form(Main_New mainForm)
        {
            InitializeComponent();
            this.Load += KaiguanControl_Form_Load;
            this.mainForm = mainForm;
        }
        private void KaiguanControl_Form_Load(object sender, EventArgs e)
        {
            GetAddress();
        }
        private void GetAddress()
        {
            try
            {
                string filePath = "DeviceAddressNew_KU.json";
                if (!File.Exists(filePath))
                    return;

                string json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                data.TryGetValue("kaiguan_textBox", out object device);
                if (device != null)
                {
                    deviceAddress = device.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载DeviceAddress.json失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private async void open_jieshouS(int chNum)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SendCommandAsync("CONNECT VNA_1 TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync($"CONNECT VNA_2 TX_OUT{chNum}/RX_IN{chNum}");
            mainForm.LogToConsole($"接收S参数(S12)通路{chNum}已开启");

            scpiDevice.Disconnect(); // 释放资源
        }
        private async void close_jieshouS(int chNum)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SendCommandAsync("DISCONNECT VNA_1 TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync($"DISCONNECT VNA_2 TX_OUT{chNum}/RX_IN{chNum}");
            mainForm.LogToConsole($"接收S参数(S12)通路{chNum}已关闭");

            scpiDevice.Disconnect(); // 释放资源
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            open_jieshouS(1);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            open_jieshouS(2);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            open_jieshouS(3);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            open_jieshouS(4);
        }

        private async void button28_Click(object sender, EventArgs e)
        {
            close_jieshouS(1);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            close_jieshouS(2);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            close_jieshouS(3);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            close_jieshouS(4);
        }

        private async void open_fasheS(int chNum)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SendCommandAsync("CONNECT VNA_1_AMP1 TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync($"CONNECT VNA_2_ATT1 TX_OUT{chNum}/RX_IN{chNum}");
            mainForm.LogToConsole($"发射S参数(S21)通路{chNum}已开启");

            scpiDevice.Disconnect(); // 释放资源
        }
        private async void close_fasheS(int chNum)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SendCommandAsync("DISCONNECT VNA_1_AMP1 TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync($"DISCONNECT VNA_2_ATT1 TX_OUT{chNum}/RX_IN{chNum}");
            mainForm.LogToConsole($"发射S参数(S21)通路{chNum}已关闭");

            scpiDevice.Disconnect(); // 释放资源
        }

        private void button12_Click(object sender, EventArgs e)
        {
            open_fasheS(1);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            open_fasheS(2);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            open_fasheS(3);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            open_fasheS(4);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            close_fasheS(1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            close_fasheS(2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            close_fasheS(3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            close_fasheS(4);
        }
        private async void open_sjjt(int chNum)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SendCommandAsync("CONNECT SIG_1_GELI2 TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync("CONNECT VNA_1_GELI1 TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync($"CONNECT SA TX_OUT{chNum}/RX_IN{chNum}");
            mainForm.LogToConsole($"频谱仪三阶交调通路{chNum}已开启");

            scpiDevice.Disconnect(); // 释放资源
        }
        private async void close_sjjt(int chNum)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SendCommandAsync("DISCONNECT SIG_1_GELI2 TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync("DISCONNECT VNA_1_GELI1 TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync($"DISCONNECT SA TX_OUT{chNum}/RX_IN{chNum}");
            mainForm.LogToConsole($"频谱仪三阶交调通路{chNum}已关闭");

            scpiDevice.Disconnect(); // 释放资源
        }

        private void button20_Click(object sender, EventArgs e)
        {
            open_sjjt(1);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            open_sjjt(2);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            open_sjjt(3);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            open_sjjt(4);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            close_sjjt(1);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            close_sjjt(2);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            close_sjjt(3);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            close_sjjt(4);
        }
        private async void open_fasheP(int chNum)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SendCommandAsync($"CONNECT SIG_1_AMP1 TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync($"CONNECT PA TX_OUT{chNum}/RX_IN{chNum}");
            mainForm.LogToConsole($"发射功率通路{chNum}已开启");

            scpiDevice.Disconnect(); // 释放资源
        }
        private async void close_fasheP(int chNum)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SendCommandAsync($"DISCONNECT SIG_1_AMP1 TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync($"DISCONNECT PA TX_OUT{chNum}/RX_IN{chNum}");
            mainForm.LogToConsole($"发射功率通路{chNum}已关闭");

            scpiDevice.Disconnect(); // 释放资源
        }

        private void button32_Click(object sender, EventArgs e)
        {
            open_fasheP(1);
        }

        private void button31_Click(object sender, EventArgs e)
        {
            open_fasheP(2);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            open_fasheP(3);
        }

        private void button29_Click(object sender, EventArgs e)
        {
            open_fasheP(4);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            close_fasheP(1);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            close_fasheP(2);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            close_fasheP(3);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            close_fasheP(4);
        }
        private async void open_zaosheng(int chNum)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SendCommandAsync("CONNECT SA TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync($"CONNECT NG TX_OUT{chNum}/RX_IN{chNum}");
            mainForm.LogToConsole($"噪声源频谱仪通路{chNum}已开启");

            scpiDevice.Disconnect(); // 释放资源
        }
        private async void close_zaosheng(int chNum)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SendCommandAsync("DISCONNECT SA TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync($"DISCONNECT NG TX_OUT{chNum}/RX_IN{chNum}");
            mainForm.LogToConsole($"噪声源频谱仪通路{chNum}已关闭");

            scpiDevice.Disconnect(); // 释放资源
        }

        private void button40_Click(object sender, EventArgs e)
        {
            open_zaosheng(1);
        }

        private void button39_Click(object sender, EventArgs e)
        {
            open_zaosheng(2);
        }

        private void button38_Click(object sender, EventArgs e)
        {
            open_zaosheng(3);
        }

        private void button37_Click(object sender, EventArgs e)
        {
            open_zaosheng(4);
        }

        private void button36_Click(object sender, EventArgs e)
        {
            close_zaosheng(1);
        }

        private void button35_Click(object sender, EventArgs e)
        {
            close_zaosheng(2);
        }

        private void button34_Click(object sender, EventArgs e)
        {
            close_zaosheng(3);
        }

        private void button33_Click(object sender, EventArgs e)
        {
            close_zaosheng(4);
        }
        private async void open_zasan(int chNum)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SendCommandAsync("CONNECT SIG_1_AMP1 TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync($"CONNECT SA TX_OUT{chNum}/RX_IN{chNum}");
            mainForm.LogToConsole($"频谱测杂散通路{chNum}已开启");

            scpiDevice.Disconnect(); // 释放资源
        }
        private async void close_zasan(int chNum)
        {
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(deviceAddress);
            if (!connected)
            {
                mainForm.LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SendCommandAsync("DISCONNECT SIG_1_AMP1 TX_IN/RX_OUT");
            await scpiDevice.SendCommandAsync($"DISCONNECT SA TX_OUT{chNum}/RX_IN{chNum}");
            mainForm.LogToConsole($"频谱测杂散通路{chNum}已关闭");

            scpiDevice.Disconnect(); // 释放资源
        }

        private void button48_Click(object sender, EventArgs e)
        {
            open_zasan(1);
        }

        private void button47_Click(object sender, EventArgs e)
        {
            open_zasan(2);
        }

        private void button46_Click(object sender, EventArgs e)
        {
            open_zasan(3);
        }

        private void button45_Click(object sender, EventArgs e)
        {
            open_zasan(4);
        }

        private void button44_Click(object sender, EventArgs e)
        {
            close_zasan(1);
        }

        private void button43_Click(object sender, EventArgs e)
        {
            close_zasan(2);
        }

        private void button42_Click(object sender, EventArgs e)
        {
            close_zasan(3);
        }

        private void button41_Click(object sender, EventArgs e)
        {
            close_zasan(4);
        }
    }
}
