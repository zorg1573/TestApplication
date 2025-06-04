using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestApp.PAGE;
using AxDSOFramer;
using System.IO;
using System.Text.Json;
using Excel;
using TestApp.FUNCTION;
using Keysight.KtNA;

namespace TestApp
{
    public partial class Main_New : Form
    {
        string excelPath;
        string vnaPath;
        static string ifaceName = @"\Device\NPF_{3A0CA248-4796-4CBA-B275-E9C8E0A766CF}"; // 注意：需要和系统中接口名称完全匹配
        static string dstMacStr = "00:0a:35:01:fe:c0";
        static string srcMacStr; //上位机MAC地址
        static string srcMacAddress;
        static string srcIpStr = "192.168.0.3";
        static string dstIpStr = "192.168.0.2";
        static ushort srcPort = 8080;
        static ushort dstPort = 8080;
        // byte[] headValue = StringToByteArray("00 0a 35 01 fe c0 00 2b 67 f1 71 51 08 00 45 00 b4 68 00 00 80 11 00 00 c0 a8 00 03 c0 a8 00 02 1f 90 1f 90 00 23 81 70");
        byte[] headValue;
        byte[] modelValue = StringToByteArray("01 03 02 00");
        byte[] emptyValue = StringToByteArray("00 00 00 00 00 00 00 00");
        private AxFramerControl _axFramerControl;

        public Main_New()
        {
            InitializeComponent();
            //InitializeDSO();
            this.Load += Main_New_Load;
        }
        private void Main_New_Load(object sender, EventArgs e)
        {
            GetAddress();
            GetDeviceFilesJson();
            InitializeDSO();
        }
        private void GetAddress()
        {
            string filePath = "DeviceAddress.json";
            if (!File.Exists(filePath))
                return;

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            data.TryGetValue("mac_textBox", out object value);
            srcMacStr = value.ToString().Replace(":", " ");
            srcMacAddress = value.ToString();
            headValue = StringToByteArray("00 0a 35 01 fe c0 " + srcMacStr + " 08 00 45 00 b4 68 00 00 80 11 00 00 c0 a8 00 03 c0 a8 00 02 1f 90 1f 90 00 23 81 70");
        }
        private void GetDeviceFilesJson()
        {
            string filePath = "DeviceFiles.json";
            if (!File.Exists(filePath))
                return;

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            data.TryGetValue("textBox6", out object value);
            excelPath = value.ToString();
            data.TryGetValue("textBox6", out object value2);
            vnaPath = value2.ToString();
        }
        private void InitializeDSO()
        {
            try
            {
                _axFramerControl = new AxFramerControl();
                _axFramerControl.Dock = DockStyle.Fill;

                this.splitContainer2.Panel2.Controls.Add(_axFramerControl);

                _axFramerControl.CreateControl(); // 强制初始化

                _axFramerControl.Titlebar = false;

                //string excelPath = "C:\\Users\\Administrator\\Desktop\\test.xlsx";
                excelPath = Path.Combine(excelPath, "test.xlsx");
                if (File.Exists(excelPath))
                {
                    _axFramerControl.Open(excelPath, false, "Excel.Sheet", "", "");
                }
                else
                {
                    MessageBox.Show($"找不到 Excel 文件：{excelPath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化 DSOFramer 出错：" + ex.ToString());
            }
        }
        /*        private void WriteArrayToExcelColumn(string[] data, int columnIndex)
                {
                    try
                    {
                        var excelApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
                        var workbook = excelApp.ActiveWorkbook;
                        var worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                        for (int i = 0; i < data.Length; i++)
                        {
                            worksheet.Cells[8 + i, columnIndex] = data[i];
                        }

                        workbook.Save();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("写入 Excel 失败：" + ex.Message);
                    }
                }*/
        private void WriteArrayToExcelColumn(string[] data, int columnIndex)
        {
            try
            {
                // 去除每个字符串的空格
                string[] cleanedData = data.Select(s => s.Replace("\n", "")).ToArray();

                // 获取已打开的 Excel 应用
                var excelApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
                Excel.Workbook workbook = excelApp.ActiveWorkbook;
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                // 写入到 Excel，从第8行开始
                for (int i = 0; i < cleanedData.Length; i++)
                {
                    worksheet.Cells[8 + i, columnIndex] = cleanedData[i];
                }

                workbook.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入 Excel 失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 控制台输出
        /// </summary>
        /// <param name="message"></param>
        public void LogToConsole(string message)
        {
            if (console_textBox.InvokeRequired)
            {
                console_textBox.Invoke(new System.Action(() => {
                    console_textBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                }));
            }
            else
            {
                console_textBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
            }
        }
        #region 弹出界面
        /// <summary>
        /// 设备地址设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void device_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new DeviceAddres_Form();
            form.ShowDialog();
        }
        /// <summary>
        /// 设备文件路径设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deviceFile_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new DeviceFiles_Form();
            form.ShowDialog();
        }
        /// <summary>
        /// 测试设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testSet_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new TestSet_Form();
            form.ShowDialog();
        }
        /// <summary>
        /// 手动发码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void manualSend_button_Click(object sender, EventArgs e)
        {
            Form form = new ManualSend_Form();
            form.ShowDialog();
        }
        /// <summary>
        /// 矢网文件设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vnaFile_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new VNAFile_Form();
            form.ShowDialog();
        }
        /// <summary>
        /// 设备管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form form = new DeviceManage_Form(this);
            form.ShowDialog();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form form = new ChargeControl_Form(this);
            form.ShowDialog();
        }
        #endregion

        #region 按钮
        /// <summary>
        /// 发射测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendTest_button_Click(object sender, EventArgs e)
        {
            LogToConsole("开始发射测试");
            string ch1send = ch1_checkBox.Checked ? "0" : "1";
            string ch2send = ch2_checkBox.Checked ? "0" : "1";
            string ch3send = ch3_checkBox.Checked ? "0" : "1";
            string ch4send = ch4_checkBox.Checked ? "0" : "1";
            string ch1 = ch1send + new string('0', 24) + "1";
            string ch2 = ch2send + new string('0', 24) + "1";
            string ch3 = ch3send + new string('0', 24) + "1";
            string ch4 = ch4send + new string('0', 24) + "1";
            string ch5 = new string('0', 16);

            var codeValue = GenerateCodeValueFromBits(new[] { ch1, ch2, ch3, ch4, ch5 });

            SendCustomPacket(headValue, modelValue, emptyValue, codeValue);
        }
        /// <summary>
        /// 接收测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void receiveTest_button_Click(object sender, EventArgs e)
        {
            LogToConsole("开始接收测试");
            string ch1recive = ch1_checkBox.Checked ? "0" : "1";
            string ch2recive = ch2_checkBox.Checked ? "0" : "1";
            string ch3recive = ch3_checkBox.Checked ? "0" : "1";
            string ch4recive = ch4_checkBox.Checked ? "0" : "1";
            string ch1 = ch1recive + new string('0', 24) + "0";
            string ch2 = ch2recive + new string('0', 24) + "0";
            string ch3 = ch3recive + new string('0', 24) + "0";
            string ch4 = ch4recive + new string('0', 24) + "0";
            string ch5 = new string('0', 16);

            var codeValue = GenerateCodeValueFromBits(new[] { ch1, ch2, ch3, ch4, ch5 });

            SendCustomPacket(headValue, modelValue, emptyValue, codeValue);
        }
        #endregion

        #region UDP发送
        public void SendCustomPacket(byte[] headValue, byte[] modelValue, byte[] emptyValue, byte[] codeValue)
        {
            var payload = headValue.Concat(modelValue).Concat(emptyValue).Concat(codeValue).ToArray();

            PhysicalAddress srcMac = PhysicalAddress.Parse(srcMacAddress.Trim().Replace(":", "-").ToUpperInvariant());
            PhysicalAddress dstMac = PhysicalAddress.Parse(dstMacStr.Trim().Replace(":", "-").ToUpperInvariant());
            IPAddress srcIp = IPAddress.Parse(srcIpStr);
            IPAddress dstIp = IPAddress.Parse(dstIpStr);

            // 创建 UDP 数据包
            var udpPacket = new UdpPacket(srcPort, dstPort)
            {
                PayloadData = payload
            };

            // 创建 IP 数据包
            var ipPacket = new IPv4Packet(srcIp, dstIp)
            {
                Protocol = ProtocolType.Udp,
                TimeToLive = 128
            };
            ipPacket.PayloadPacket = udpPacket;

            // 创建以太网帧
            var ethernetPacket = new EthernetPacket(srcMac, dstMac, EthernetType.IPv4)
            {
                PayloadPacket = ipPacket
            };

            // 选择接口
            var devices = CaptureDeviceList.Instance;
            var device = CaptureDeviceList.Instance.FirstOrDefault(d => d.Name == ifaceName);
            if (device == null)
            {
                LogToConsole("找不到接口：" + ifaceName);
                return;
            }

            device.Open();
            device.SendPacket(ethernetPacket);
            device.Close();

            LogToConsole($"发送数据包：Payload长度={payload.Length}字节");
            LogToConsole($"Payload (Hex): {BitConverter.ToString(payload).Replace("-", " ")}");
            LogToConsole("数据包已发送。\n");
        }

        static byte[] GenerateCodeValueFromBits(string[] bitStrings)
        {
            if (bitStrings.Length != 5)
                throw new ArgumentException("应包含5个通道的比特串");

            int[] expectedLengths = { 26, 26, 26, 26, 16 };
            string allBits = "";

            for (int i = 0; i < 5; i++)
            {
                var bits = bitStrings[i].Replace(" ", "");
                if (bits.Length != expectedLengths[i])
                    throw new ArgumentException($"通道{i + 1} 应为 {expectedLengths[i]} 位，但提供了 {bits.Length} 位");

                allBits += bits;
            }

            if (allBits.Length != 120)
                throw new ArgumentException("总位数应为120");

            byte[] codeBytes = new byte[15];
            for (int i = 0; i < 15; i++)
            {
                codeBytes[i] = Convert.ToByte(allBits.Substring(i * 8, 8), 2);
            }

            return codeBytes;
        }

        static byte[] StringToByteArray(string hex)
        {
            try
            {
                return hex.Split(' ')
                  .Select(s => Convert.ToByte(s, 16))
                  .ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine("转换十六进制字符串到字节数组失败: " + ex.Message);
                return new byte[0];
            }

        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _axFramerControl.Titlebar = false;
            var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = System.Environment.CurrentDirectory;
            openFileDialog.Filter = "Excel 文件 (*.xls;*.xlsx)|*.xls;*.xlsx";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this._axFramerControl.Open(openFileDialog.FileName);
            }
        }
        #endregion

        private async void button1_Click(object sender, EventArgs e)
        {
            foreach (var dev in CaptureDeviceList.Instance)
            {
                LogToConsole($"Name: {dev.Name} - Description: {dev.Description}");
            }

        }

        /*        private async void button2_Click(object sender, EventArgs e)
                {
                    string visaAddress = "TCPIP0::Lucky::hislip_PXI10_CHASSIS1_SLOT1_INDEX0::INSTR";

                    ScpiDevice scpiDevice = new ScpiDevice();

                    bool connected = await scpiDevice.ConnectAsync(visaAddress);
                    if (!connected)
                    {
                        LogToConsole("连接失败");
                        return;
                    }
                    await scpiDevice.LoadStateFile();
                    double? gain = await scpiDevice.GetGainAsync();               // 增益（dB）
                    double? initial = await scpiDevice.GetInitialPhaseAsync();
                    double? inputVswr = await scpiDevice.GetInputVSWRAsync();     // 输入驻波比
                    double? outputVswr = await scpiDevice.GetOutputVSWRAsync();   // 输出驻波比

                    LogToConsole($"增益: {gain:F2} dB, 初相: {initial:F2} °, 输入驻波: {inputVswr:F2}, 输出驻波: {outputVswr:F2}");

                    scpiDevice.Disconnect(); // 释放资源
                }*/
        private async void button2_Click(object sender, EventArgs e)
        {
            string visaAddress = "TCPIP0::Lucky::hislip_PXI10_CHASSIS1_SLOT1_INDEX0::INSTR";

            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(visaAddress);
            if (!connected)
            {
                LogToConsole("连接失败");
                return;
            }
            await scpiDevice.LoadStateFile(vnaPath);
            string[] gain = await scpiDevice.GetGainStringAsync();               // 增益（dB）
            string[] initial = await scpiDevice.GetInitialPhaseStringAsync();    // 初相（°）
            string[] inputVswr = await scpiDevice.GetInputVSWRStringAsync();     // 输入驻波比
            string[] outputVswr = await scpiDevice.GetOutputVSWRStringAsync();   // 输出驻波比

            LogToConsole($"数据读取完成");
            LogToConsole("开始写入 Excel...");
            WriteArrayToExcelColumn(gain, 2);        // B列
            WriteArrayToExcelColumn(initial, 3);     // C列
            WriteArrayToExcelColumn(inputVswr, 4);   // D列
            WriteArrayToExcelColumn(outputVswr, 5);  // E列
            LogToConsole("写入完成");
            scpiDevice.Disconnect(); // 释放资源
        }

        private void ChargeSet_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new ChargeControl_Form(this);
            form.ShowDialog();
        }

        private async void RecievePower_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string visaAddress = "TCPIP0::192.168.0.8::INSTR";

            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(visaAddress);
            if (!connected)
            {
                LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SelectChannel(2);
            await scpiDevice.EnableOutput();
            await scpiDevice.SetVoltage(5);
            await scpiDevice.SetCurrent(1.5);

            scpiDevice.Disconnect();
        }

        private async void SendPower_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string visaAddress = "TCPIP0::192.168.0.8::INSTR";

            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(visaAddress);
            if (!connected)
            {
                LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SelectChannel(1);
            await scpiDevice.EnableOutput();
            await scpiDevice.SetVoltage(8.5);
            await scpiDevice.SetCurrent(3);

            await scpiDevice.SelectChannel(2);
            await scpiDevice.EnableOutput();
            await scpiDevice.SetVoltage(5);
            await scpiDevice.SetCurrent(1.5);

            scpiDevice.Disconnect();
        }

        private async void Close_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string visaAddress = "TCPIP0::192.168.0.8::INSTR";

            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(visaAddress);
            if (!connected)
            {
                LogToConsole("连接失败");
                return;
            }
            await scpiDevice.SelectChannel(1);
            await scpiDevice.DisableOutput();
            await scpiDevice.SelectChannel(2);
            await scpiDevice.DisableOutput();
            await scpiDevice.SelectChannel(3);
            await scpiDevice.DisableOutput();

            scpiDevice.Disconnect();
        }
    }
}
