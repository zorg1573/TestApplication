using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestApp.DAL;

namespace TestApp.PAGE
{
    public partial class ManualSend_Form : Form
    {
        //string ifaceName = @"\Device\NPF_{3A0CA248-4796-4CBA-B275-E9C8E0A766CF}"; // 注意：需要和系统中接口名称完全匹配
        string ifaceName = "";
        //static string dstMacStr = "00:0a:35:01:fe:c0";
        string dstMacStr = "";
        string srcMacStr = ""; //上位机MAC地址
        string srcMacAddress = "";
        //static string srcIpStr = "192.168.0.3";
        //static string dstIpStr = "192.168.0.2";
        string srcIpStr = "";
        string dstIpStr = "";

        static ushort srcPort = 8080;
        static ushort dstPort = 8080;
        // byte[] headValue = StringToByteArray("00 0a 35 01 fe c0 00 2b 67 f1 71 51 08 00 45 00 b4 68 00 00 80 11 00 00 c0 a8 00 03 c0 a8 00 02 1f 90 1f 90 00 23 81 70");
        byte[] headValue;
        //byte[] modelValue = StringToByteArray("01 03 01 00");
        byte[] modelValue;
        byte[] emptyValue = StringToByteArray("00 00 00 00 00 00 00 00");
        private Main_New mainForm;
        string ch1Yixiang = "000000";
        string ch2Yixiang = "000000";
        string ch3Yixiang = "000000";
        string ch4Yixiang = "000000";
        string ch1Shuaijian = "000000";
        string ch2Shuaijian = "000000";
        string ch3Shuaijian = "000000";
        string ch4Shuaijian = "000000";
        private OperateLog_DAL operateLog_DAL = new OperateLog_DAL();
        /*        public ManualSend_Form()
                {
                    InitializeComponent();
                    this.Load += ManualSend_Form_Load;
                    radioButton4.Checked = true;
                    radioButton6.Checked = true;
                }*/
        public ManualSend_Form(Main_New mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.Load += ManualSend_Form_Load;
            radioButton4.Checked = true;
            radioButton6.Checked = true;
        }
        private void ManualSend_Form_Load(object sender, EventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载DeviceAddress.json失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void send_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked)
                {
                    MessageBox.Show("请选择发送或接收模式！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ch1Yixiang = GetBinaryFromTextBox(ch1_yixiang_textBox, 6);
                ch2Yixiang = GetBinaryFromTextBox(ch2_yixiang_textBox, 6);
                ch3Yixiang = GetBinaryFromTextBox(ch3_yixiang_textBox, 6);
                ch4Yixiang = GetBinaryFromTextBox(ch4_yixiang_textBox, 6);
                ch1Shuaijian = GetBinaryFromTextBox(ch1_shuaijian_textBox, 6);
                ch2Shuaijian = GetBinaryFromTextBox(ch2_shuaijian_textBox, 6);
                ch3Shuaijian = GetBinaryFromTextBox(ch3_shuaijian_textBox, 6);
                ch4Shuaijian = GetBinaryFromTextBox(ch4_shuaijian_textBox, 6);

                if (radioButton2.Checked)
                {
                    mainForm.LogToConsole("开始发射测试"); //接收开关 发射移相 接收移相 发射衰减 接收衰减 发射开关
                    string ch1send = checkBox1.Checked ? "0" : "1";
                    string ch2send = checkBox2.Checked ? "0" : "1";
                    string ch3send = checkBox3.Checked ? "0" : "1";
                    string ch4send = checkBox4.Checked ? "0" : "1";
                    string ch1 = "1" + ch1Yixiang + "000000" + ch1Shuaijian + "000000" + ch1send;
                    string ch2 = "1" + ch2Yixiang + "000000" + ch2Shuaijian + "000000" + ch2send;
                    string ch3 = "1" + ch3Yixiang + "000000" + ch3Shuaijian + "000000" + ch3send;
                    string ch4 = "1" + ch4Yixiang + "000000" + ch4Shuaijian + "000000" + ch4send;
                    string ch5 = new string('0', 16);
                    modelValue = StringToByteArray("01 03 01 00");
                    var codeValue = GenerateCodeValueFromBits(new[] { ch1, ch2, ch3, ch4, ch5 });

                    SendCustomPacket(headValue, modelValue, emptyValue, codeValue);
                    //operateLog_DAL.InsertOperateLog_DT("手动发码|发射测试", $"{ch1send},{ch2send},{ch3send},{ch4send}");
                }
                else if (radioButton1.Checked)
                {
                    mainForm.LogToConsole("开始接收测试");
                    string ch1recive = checkBox1.Checked ? "0" : "1";
                    string ch2recive = checkBox2.Checked ? "0" : "1";
                    string ch3recive = checkBox3.Checked ? "0" : "1";
                    string ch4recive = checkBox4.Checked ? "0" : "1";
                    string ch1 = ch1recive + "000000" + ch1Yixiang + "000000" + ch1Shuaijian + "1";
                    string ch2 = ch2recive + "000000" + ch2Yixiang + "000000" + ch2Shuaijian + "1";
                    string ch3 = ch3recive + "000000" + ch3Yixiang + "000000" + ch3Shuaijian + "1";
                    string ch4 = ch4recive + "000000" + ch4Yixiang + "000000" + ch4Shuaijian + "1";
                    string ch5 = new string('0', 16);
                    modelValue = StringToByteArray("01 03 02 00");
                    var codeValue = GenerateCodeValueFromBits(new[] { ch1, ch2, ch3, ch4, ch5 });

                    SendCustomPacket(headValue, modelValue, emptyValue, codeValue);
                    //operateLog_DAL.InsertOperateLog_DT("手动发码|接收测试", $"{ch1recive},{ch2recive},{ch3recive},{ch4recive}");
                }
                else if (radioButton3.Checked)
                {
                    mainForm.LogToConsole("负载模式");
                    string ch1 = "1" + "000000" + "000000" + "000000" + "000000" + "1";
                    string ch2 = "1" + "000000" + "000000" + "000000" + "000000" + "1";
                    string ch3 = "1" + "000000" + "000000" + "000000" + "000000" + "1";
                    string ch4 = "1" + "000000" + "000000" + "000000" + "000000" + "1";
                    string ch5 = new string('0', 16);
                    modelValue = StringToByteArray("01 03 03 00");
                    var codeValue = GenerateCodeValueFromBits(new[] { ch1, ch2, ch3, ch4, ch5 });

                    SendCustomPacket(headValue, modelValue, emptyValue, codeValue);
                    //operateLog_DAL.InsertOperateLog_DT("负载模式","");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("手动发码失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //operateLog_DAL.InsertOperateLog_DT("手动发码失败", ex.ToString());
            }


        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
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
                    mainForm.LogToConsole("找不到接口：" + ifaceName);
                    return;
                }

                device.Open();
                device.SendPacket(ethernetPacket);
                device.Close();

                mainForm.LogToConsole($"发送数据包：Payload长度={payload.Length}字节");
                mainForm.LogToConsole($"Payload (Hex): {BitConverter.ToString(payload).Replace("-", " ")}");
                mainForm.LogToConsole("数据包已发送。\n");
            }
            catch(Exception ex)
            {
                MessageBox.Show("UDP发送失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //operateLog_DAL.InsertOperateLog_DT("UDP发送失败", ex.ToString());
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
        private string GetBinaryFromTextBox(TextBox textBox, int bitLength)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
                return new string('0', bitLength);

            if (int.TryParse(textBox.Text, out int value))
            {
                if (value >= 0 && value <= 63)
                {
                    return DecimalToBinary(value, bitLength);
                }
            }

            // 超出范围或格式错误，返回默认值或抛异常，按需修改
            MessageBox.Show($"请输入一个 0 到 63 之间的整数：{textBox.Name}", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return new string('0', bitLength);
        }

        public string DecimalToBinary(int number, int totalBits)
        {
            return Convert.ToString(number, 2).PadLeft(totalBits, '0');
        }
        #endregion
    }
}
