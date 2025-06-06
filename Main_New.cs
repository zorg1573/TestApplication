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
using TestApp.DAL.Dapper;
using TestApp.MODEL;
using TestApp.DAL;

namespace TestApp
{
    public partial class Main_New : Form
    {
    //DeviceFiles.json
        string excelPath = "";
        string vnaFilePath = "";
        string excelMobanPath = "";

    //DeviceAddressNew.json
        string chargeAddress = "";
        string vnaAddress = "";
        string gonglvAddress = "";
        string xinhaoAddress = "";
        string ifaceName = "";
        //string ifaceName = @"\Device\NPF_{3A0CA248-4796-4CBA-B275-E9C8E0A766CF}"; // 注意：需要和系统中接口名称完全匹配
        //static string dstMacStr = "00:0a:35:01:fe:c0";
        string dstMacStr = "";
        string srcMacStr = ""; //上位机MAC地址
        string srcMacAddress = "";
        string srcIpStr = "";
        string dstIpStr = "";
        //static string srcIpStr = "192.168.0.3";
        //static string dstIpStr = "192.168.0.2";

        //XinhaoSet.json
        double startFreq = -1;
        double stopFreq = -1;
        int pointCount = -1;

        static ushort srcPort = 8080;
        static ushort dstPort = 8080;
        // byte[] headValue = StringToByteArray("00 0a 35 01 fe c0 00 2b 67 f1 71 51 08 00 45 00 b4 68 00 00 80 11 00 00 c0 a8 00 03 c0 a8 00 02 1f 90 1f 90 00 23 81 70");
        byte[] headValue;
        //byte[] modelValue = StringToByteArray("01 03 02 00");
        byte[] modelValue;
        byte[] emptyValue = StringToByteArray("00 00 00 00 00 00 00 00");
        private AxFramerControl _axFramerControl;
        private Main_DAL main_DAL = new Main_DAL();
        private OperateLog_DAL operateLog_DAL = new OperateLog_DAL();

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
            GetXinhaoSetJson();
            InitializeDSO();
        }
        #region 通用方法
        private void GetAddress()
        {
            try
            {
                string filePath = "DeviceAddressNew.json";
                if (!File.Exists(filePath))
                    return;

                string json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                data.TryGetValue("pc_mac_textBox", out object pcMac);
                if (pcMac != null)
                {
                    srcMacStr = pcMac.ToString().Replace(":", " ");
                    srcMacAddress = pcMac.ToString();
                }
                headValue = StringToByteArray("00 0a 35 01 fe c0 " + srcMacStr + " 08 00 45 00 b4 68 00 00 80 11 00 00 c0 a8 00 03 c0 a8 00 02 1f 90 1f 90 00 23 81 70");

                data.TryGetValue("pc_jiekou_textBox", out object pcJiekou);
                if (pcJiekou != null)
                {
                    ifaceName = pcJiekou.ToString();
                }

                data.TryGetValue("pc_ip_textBox", out object pcIp);
                if (pcIp != null)
                {
                    srcIpStr = pcIp.ToString();
                }

                data.TryGetValue("fpga_ip_textBox", out object fpgaIp);
                if (fpgaIp != null)
                {
                    dstIpStr = fpgaIp.ToString();
                }

                data.TryGetValue("fpga_mac_textBox", out object fpgaMac);
                if (fpgaMac != null)
                {
                    dstMacStr = fpgaMac.ToString();
                }

                data.TryGetValue("shiwang_textBox", out object shiwang);
                if (shiwang != null)
                {
                    vnaAddress = shiwang.ToString();
                }

                data.TryGetValue("charge_textBox", out object charge);
                if (charge != null)
                {
                    chargeAddress = charge.ToString();
                }

                data.TryGetValue("gonglv_textBox", out object gonglv);
                if (gonglv != null)
                {
                    gonglvAddress = gonglv.ToString();
                }

                data.TryGetValue("xinhao_textBox", out object xinhao);
                if (xinhao != null)
                {
                    xinhaoAddress = xinhao.ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("加载DeviceAddress.json失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void GetDeviceFilesJson()
        {
            try
            {
                string filePath = "DeviceFiles.json";
                if (!File.Exists(filePath))
                    return;

                string json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                data.TryGetValue("textBox6", out object value);
                if (value != null)
                {
                    excelPath = value.ToString();
                }

                data.TryGetValue("textBox1", out object value2);
                if (value2 != null)
                {
                    vnaFilePath = value2.ToString();
                }

                data.TryGetValue("textBox7", out object value3);
                if (value3 != null)
                {
                    excelMobanPath = value3.ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("加载DeviceFiles.json失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GetXinhaoSetJson()
        {
            try
            {
                string filePath = "XinhaoSet.json";
                if (!File.Exists(filePath))
                    return;

                string json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                string startFreqDanwei = "";
                string stopFreqDanwei = "";

                data.TryGetValue("comboBox1", out object start_freq_danwei);
                if (start_freq_danwei != null)
                {
                    startFreqDanwei = start_freq_danwei.ToString();
                }
                data.TryGetValue("comboBox2", out object stop_freq_danwei);
                if (stop_freq_danwei != null)
                {
                    stopFreqDanwei = stop_freq_danwei.ToString();
                }

                data.TryGetValue("start_freq_textBox", out object start_freq);
                if (start_freq != null)
                {
                    if(startFreqDanwei == "MHz")
                        startFreq = double.Parse(start_freq.ToString()) * 1e6; // 转换为Hz
                    else if (startFreqDanwei == "GHz")
                        startFreq = double.Parse(start_freq.ToString()) * 1e9; // 转换为Hz
                }

                data.TryGetValue("stop_freq_textBox", out object stop_freq);
                if (stop_freq != null)
                {
                    if (stopFreqDanwei == "MHz")
                        stopFreq = double.Parse(stop_freq.ToString()) * 1e6; // 转换为Hz
                    else if (stopFreqDanwei == "GHz")
                        stopFreq = double.Parse(stop_freq.ToString()) * 1e9; // 转换为Hz
                }

                data.TryGetValue("point_count_textBox", out object point_count);
                if (point_count != null)
                {
                    pointCount = int.Parse(point_count.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载DeviceFiles.json失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                excelPath = Path.Combine(excelPath, "test.xls");
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
        #endregion

        #region 弹出界面
        /// <summary>
        /// 设备地址设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void device_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new DeviceAddressNew_Form();
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
            Form form = new ManualSend_Form(this);
            form.ShowDialog();
        }
        /// <summary>
        /// 信号发生器设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Form form = new XinhaoControl_Form(this);
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
        /// <summary>
        /// 电源参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargeSet_ToolStripMenuItem_Click(object sender, EventArgs e)
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
        private async void SendTestUDP()
        {
            try
            {
                string ch1send = ch1_checkBox.Checked ? "0" : "1";
                string ch2send = ch2_checkBox.Checked ? "0" : "1";
                string ch3send = ch3_checkBox.Checked ? "0" : "1";
                string ch4send = ch4_checkBox.Checked ? "0" : "1";
                string ch1 = "1" + new string('0', 24) + ch1send;
                string ch2 = "1" + new string('0', 24) + ch2send;
                string ch3 = "1" + new string('0', 24) + ch3send;
                string ch4 = "1" + new string('0', 24) + ch4send;
                string ch5 = new string('0', 16);
                byte[] modelValue = StringToByteArray("01 03 01 00");
                var codeValue = GenerateCodeValueFromBits(new[] { ch1, ch2, ch3, ch4, ch5 });
                string chSum = "";
                if (ch1_checkBox.Checked)
                {
                    chSum += "通道1 ";
                }
                if (ch2_checkBox.Checked)
                {
                    chSum += " 通道2 ";
                }
                if (ch3_checkBox.Checked)
                {
                    chSum += " 通道3 ";
                }
                if (ch4_checkBox.Checked)
                {
                    chSum += " 通道4 ";
                }

                LogToConsole("FPGA发包:" + chSum);
                SendCustomPacket(headValue, modelValue, emptyValue, codeValue);

                operateLog_DAL.InsertOperateLog_DT("发射测试", $"{ch1send},{ch2send},{ch3send},{ch4send}");

            }
            catch (Exception ex)
            {
                LogToConsole("发射测试失败: " + ex);
                operateLog_DAL.InsertOperateLog_DT("发射测试失败", ex.ToString());
            }
        }
        private void sendTest_button_Click(object sender, EventArgs e)
        {
            LogToConsole("开始发射测试...");
            ChargeSendPowerON(); // 发射加电
            LoadGonglvState(); // 调用功率计文件
            SendTestUDP(); //FPGA发包
            //信号发生器
            LogToConsole("发射测试已完成");
        }
        /// <summary>
        /// 接收测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void receiveTest_button_Click(object sender, EventArgs e)
        {
            LogToConsole("开始接收测试...");
            ChargeRecievePowerON(); // 接收加电
            LoadVNAState(); // 调用矢网文件
            RecieveTestUDP(); //FPGA发包
            LoadVNAData(); // 写入矢网数据
            LogToConsole("接收测试已完成");
        }
        private async void RecieveTestUDP()
        {
            try
            {
                string ch1recive = ch1_checkBox.Checked ? "0" : "1";
                string ch2recive = ch2_checkBox.Checked ? "0" : "1";
                string ch3recive = ch3_checkBox.Checked ? "0" : "1";
                string ch4recive = ch4_checkBox.Checked ? "0" : "1";
                string ch1 = ch1recive + new string('0', 24) + "1";
                string ch2 = ch2recive + new string('0', 24) + "1";
                string ch3 = ch3recive + new string('0', 24) + "1";
                string ch4 = ch4recive + new string('0', 24) + "1";
                string ch5 = new string('0', 16);
                byte[] modelValue = StringToByteArray("01 03 02 00");
                var codeValue = GenerateCodeValueFromBits(new[] { ch1, ch2, ch3, ch4, ch5 });
                string chSum = "";
                if(ch1_checkBox.Checked)
                {
                    chSum += "通道1 ";
                }
                if(ch2_checkBox.Checked)
                {
                    chSum += " 通道2 ";
                }
                if(ch3_checkBox.Checked)
                {
                    chSum += " 通道3 ";
                }
                if(ch4_checkBox.Checked)
                {
                    chSum += " 通道4 ";
                }

                LogToConsole("FPGA发包:" + chSum);
                SendCustomPacket(headValue, modelValue, emptyValue, codeValue);

                operateLog_DAL.InsertOperateLog_DT("接收测试", $"{ch1recive},{ch2recive},{ch3recive},{ch4recive}");
            }
            catch (Exception ex)
            {
                LogToConsole("接收测试失败: " + ex);
                operateLog_DAL.InsertOperateLog_DT("接收测试失败", ex.ToString());
            }
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            foreach (var dev in CaptureDeviceList.Instance)
            {
                LogToConsole($"Name: {dev.Name} - Description: {dev.Description}");
            }

        }
        /// <summary>
        /// 调用矢网文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LoadVNAState()
        {
            string visaAddress = vnaAddress;
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(visaAddress);
            if (!connected)
            {
                LogToConsole("连接失败");
                return;
            }
            
            await scpiDevice.LoadStateFile(vnaFilePath);
            LogToConsole("调用矢网文件");
            scpiDevice.Disconnect(); // 释放资源
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            string visaAddress = vnaAddress;
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(visaAddress);
            if (!connected)
            {
                LogToConsole("连接失败");
                return;
            }

            await scpiDevice.LoadStateFile(vnaFilePath);

            string[] gain = await scpiDevice.GetGainStringAsync();               // 增益（dB）
            string[] initial = await scpiDevice.GetInitialPhaseStringAsync();    // 初相（°）
            string[] inputVswr = await scpiDevice.GetInputVSWRStringAsync();     // 输入驻波比
            string[] outputVswr = await scpiDevice.GetOutputVSWRStringAsync();   // 输出驻波比
/*            string[] gain = { "1.111", "2.222", "3.333" };               // 增益（dB）
            string[] initial = { "1.111", "2.222", "3.333" };     // 初相（°）
            string[] inputVswr = { "1.111", "2.222", "3.333" };      // 输入驻波比
            string[] outputVswr = { "1.111", "2.222", "3.333" };    // 输出驻波比*/

            LogToConsole($"数据读取完成");
            LogToConsole("开始写入 Excel...");

            // 写入测量数据
            WriteArrayToExcelColumn(gain, 2);        // B列
            WriteArrayToExcelColumn(initial, 3);     // C列
            WriteArrayToExcelColumn(inputVswr, 4);   // D列
            WriteArrayToExcelColumn(outputVswr, 5);  // E列

            // 写入频率（从 A8 开始）
            double startFreq = await scpiDevice.GetFreqStart() ?? -1;  // 单位 Hz
            double stopFreq = await scpiDevice.GetFreqStop() ?? -1;    // 单位 Hz
            int pointCount = await scpiDevice.GetPointCount() ?? -1;


            if (startFreq < 0 || stopFreq < 0 || pointCount <= 0)
            {
                LogToConsole("获取频率或点数失败，请检查设备连接或设置。");
                scpiDevice.Disconnect();
                return;
            }

            double step = (stopFreq - startFreq) / (pointCount - 1);
            string[] freqArray = new string[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                double freqGHz = (startFreq + step * i) / 1e9;
                freqArray[i] = freqGHz.ToString("F6"); // 保留6位小数（GHz）
            }

            WriteArrayToExcelColumn(freqArray, 1);  // A列，从第8行开始
            LogToConsole("写入Excel完成");

            // 写入数据库
            LogToConsole("开始写入数据库...");
            try
            {
                int batchId = main_DAL.GetBatchId();

                var batch = new MeasurementBatch
                {
                    Operator = "操作员",
                    Description = "自动测试批次",
                    UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                main_DAL.InsertTestBatch_DT(batch);

                for (int i = 0; i < gain.Length; i++)
                {
                    var result = new MeasurementResult
                    {
                        BatchId = batchId + 1,
                        PointIndex = i,
                        Gain = double.Parse(gain[i]),
                        InitialPhase = double.Parse(initial[i]),
                        InputVSWR = double.Parse(inputVswr[i]),
                        OutputVSWR = double.Parse(outputVswr[i]),
                    };

                    main_DAL.InsertTestData_DT(result);
                }
                LogToConsole("写入数据库完成");
            }
            catch (Exception ex)
            {
                LogToConsole("写入数据库出错: " + ex.Message);
            }

            scpiDevice.Disconnect(); // 释放资源

            operateLog_DAL.InsertOperateLog_DT("调用矢网文件", "");
        }
        private async void LoadVNAData()
        {
            string visaAddress = vnaAddress;
            ScpiDevice scpiDevice = new ScpiDevice();

            bool connected = await scpiDevice.ConnectAsync(visaAddress);
            if (!connected)
            {
                LogToConsole("连接失败");
                return;
            }

            string[] gain = await scpiDevice.GetGainStringAsync();               // 增益（dB）
            string[] initial = await scpiDevice.GetInitialPhaseStringAsync();    // 初相（°）
            string[] inputVswr = await scpiDevice.GetInputVSWRStringAsync();     // 输入驻波比
            string[] outputVswr = await scpiDevice.GetOutputVSWRStringAsync();   // 输出驻波比

            LogToConsole($"数据读取完成");
            LogToConsole("开始写入 Excel...");

            // 写入测量数据
            WriteArrayToExcelColumn(gain, 2);        // B列
            WriteArrayToExcelColumn(initial, 3);     // C列
            WriteArrayToExcelColumn(inputVswr, 4);   // D列
            WriteArrayToExcelColumn(outputVswr, 5);  // E列

            // 写入频率（从 A8 开始）
            double startFreq = await scpiDevice.GetFreqStart() ?? -1;  // 单位 Hz
            double stopFreq = await scpiDevice.GetFreqStop() ?? -1;    // 单位 Hz
            int pointCount = await scpiDevice.GetPointCount() ?? -1;


            if (startFreq < 0 || stopFreq < 0 || pointCount <= 0)
            {
                LogToConsole("获取频率或点数失败，请检查设备连接或设置。");
                scpiDevice.Disconnect();
                return;
            }

            double step = (stopFreq - startFreq) / (pointCount - 1);
            string[] freqArray = new string[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                double freqGHz = (startFreq + step * i) / 1e9;
                freqArray[i] = freqGHz.ToString("F6"); // 保留6位小数（GHz）
            }

            WriteArrayToExcelColumn(freqArray, 1);  // A列，从第8行开始
            LogToConsole("写入Excel完成");

            // 写入数据库
            LogToConsole("开始写入数据库...");
            try
            {
                int batchId = main_DAL.GetBatchId();

                var batch = new MeasurementBatch
                {
                    Operator = "操作员",
                    Description = "自动测试批次",
                    UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                main_DAL.InsertTestBatch_DT(batch);

                for (int i = 0; i < gain.Length; i++)
                {
                    var result = new MeasurementResult
                    {
                        BatchId = batchId + 1,
                        PointIndex = i,
                        Gain = double.Parse(gain[i]),
                        InitialPhase = double.Parse(initial[i]),
                        InputVSWR = double.Parse(inputVswr[i]),
                        OutputVSWR = double.Parse(outputVswr[i]),
                    };

                    main_DAL.InsertTestData_DT(result);
                }
                LogToConsole("写入数据库完成");
            }
            catch (Exception ex)
            {
                LogToConsole("写入数据库出错: " + ex.Message);
            }

            scpiDevice.Disconnect(); // 释放资源
        }
        /// <summary>
        /// 接收加电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RecievePower_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //string visaAddress = "TCPIP0::192.168.0.8::INSTR";
                string visaAddress = chargeAddress;

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
                LogToConsole("接收加电");
                scpiDevice.Disconnect();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"接收加电失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                operateLog_DAL.InsertOperateLog_DT("接收加电失败", ex.ToString());
            }

        }
        private async void ChargeRecievePowerON()
        {
            try
            {
                string visaAddress = chargeAddress;

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
                LogToConsole("接收加电...");
                scpiDevice.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"接收加电失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                operateLog_DAL.InsertOperateLog_DT("接收加电失败", ex.ToString());
            }
        }
        /// <summary>
        /// 发射加电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SendPower_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //string visaAddress = "TCPIP0::192.168.0.8::INSTR";
                string visaAddress = chargeAddress;

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
            catch(Exception ex)
            {
                MessageBox.Show($"发射加电失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                operateLog_DAL.InsertOperateLog_DT("发射加电失败", ex.ToString());
            }

        }
        private async void ChargeSendPowerON()
        {
            try
            {
                //string visaAddress = "TCPIP0::192.168.0.8::INSTR";
                string visaAddress = chargeAddress;

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
            catch (Exception ex)
            {
                MessageBox.Show($"发射加电失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                operateLog_DAL.InsertOperateLog_DT("发射加电失败", ex.ToString());
            }
        }
        /// <summary>
        /// 电源关电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Close_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //string visaAddress = "TCPIP0::192.168.0.8::INSTR";
                string visaAddress = chargeAddress;

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
            catch(Exception ex)
            {
                MessageBox.Show($"电源关电失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                operateLog_DAL.InsertOperateLog_DT("电源关电失败", ex.ToString());
            }

        }
        /// <summary>
        /// 新建Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (!File.Exists(excelMobanPath))
            {
                MessageBox.Show("模板文件不存在，请检查路径。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                string now = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                dialog.Title = "保存新建的 Excel 文件";
                dialog.Filter = "Excel 文件 (*.xlsx)|*.xlsx";
                dialog.FileName = "测试结果" + now + ".xlsx";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.Copy(excelMobanPath, dialog.FileName, overwrite: true);
                        MessageBox.Show("Excel 文件已成功创建。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"创建 Excel 文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        operateLog_DAL.InsertOperateLog_DT("创建 Excel 文件失败", ex.ToString());
                    }
                }
            }
        }
        /// <summary>
        /// 打开Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
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
            catch(Exception ex)
            {
                MessageBox.Show($"打开Excel失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                operateLog_DAL.InsertOperateLog_DT("打开Excel失败", ex.ToString());
            }

        }
        #endregion

        #region UDP发送
        public void SendCustomPacket(byte[] headValue, byte[] modelValue, byte[] emptyValue, byte[] codeValue)
        {
            try
            {
                if (string.IsNullOrEmpty(srcIpStr) || string.IsNullOrEmpty(dstIpStr) || string.IsNullOrEmpty(srcMacAddress) || string.IsNullOrEmpty(dstMacStr))
                {
                    MessageBox.Show("IP或MAC地址为空，请设置后再操作。");
                    return;
                }

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
            catch(Exception ex)
            {
                MessageBox.Show($"UDP发送失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                operateLog_DAL.InsertOperateLog_DT("UDP发送失败", ex.ToString());
            }

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




        #endregion

        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string visaAddress = gonglvAddress;

                ScpiDevice scpiDevice = new ScpiDevice();

                bool connected = await scpiDevice.ConnectAsync(visaAddress);
                if (!connected)
                {
                    LogToConsole("连接失败");
                    return;
                }
                //await scpiDevice.TriggerImmediate();
                double[] result = await scpiDevice.ReadPulsePowerArrayAsync();
                LogToConsole("当前峰值功率: "+ result[0] + "dBm, " + "平均功率: " + result[1] + "dBm");

                scpiDevice.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取功率失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                operateLog_DAL.InsertOperateLog_DT("读取功率失败", ex.ToString());
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string visaAddress = gonglvAddress;

                ScpiDevice scpiDevice = new ScpiDevice();

                bool connected = await scpiDevice.ConnectAsync(visaAddress);
                if (!connected)
                {
                    LogToConsole("连接失败");
                    return;
                }
                bool state = await scpiDevice.LoadGonglvState();
                if (state)
                {
                    LogToConsole("调用功率计文件");
                }
                
                scpiDevice.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"调用功率计文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                operateLog_DAL.InsertOperateLog_DT("调用功率计文件失败", ex.ToString());
            }
        }
        private async void LoadGonglvState()
        {
            try
            {
                string visaAddress = gonglvAddress;

                ScpiDevice scpiDevice = new ScpiDevice();

                bool connected = await scpiDevice.ConnectAsync(visaAddress);
                if (!connected)
                {
                    LogToConsole("连接失败");
                    return;
                }
                bool state = await scpiDevice.LoadGonglvState();
                if (state)
                {
                    LogToConsole("调用功率计文件");
                }

                scpiDevice.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"调用功率计文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                operateLog_DAL.InsertOperateLog_DT("调用功率计文件失败", ex.ToString());
            }
        }
    }
}
