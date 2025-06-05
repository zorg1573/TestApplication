using Ivi.Visa;
using NationalInstruments.Visa;
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

namespace TestApp.PAGE
{
    public partial class DeviceAddressNew_Form : Form
    {
        public DeviceAddressNew_Form()
        {
            InitializeComponent();
            this.Load += DeviceAddressNew_Form_Load;
        }
        private void DeviceAddressNew_Form_Load(object sender, EventArgs e)
        {
            LoadFromJson();
        }
        private void SaveToJson()
        {
            var data = new Dictionary<string, object>();

            foreach (Control ctrl in tableLayoutPanel1.Controls)
            {
                if (ctrl is TextBox textBox)
                {
                    data[textBox.Name] = textBox.Text;
                }
            }

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("DeviceAddressNew.json", json);
        }

        private void LoadFromJson()
        {
            string filePath = "DeviceAddressNew.json";
            if (!File.Exists(filePath))
                return;

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            if (data == null)
                return;

            foreach (Control ctrl in tableLayoutPanel1.Controls)
            {
                if (data.TryGetValue(ctrl.Name, out object value))
                {
                    if (ctrl is TextBox textBox)
                    {
                        textBox.Text = value.ToString();
                    }
                }
            }
        }
        private Task<string> TryConnectAndGetIdnAsync(string visaAddress)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var resourceManager = new ResourceManager())
                    using (var session = resourceManager.Open(visaAddress) as IMessageBasedSession)
                    {
                        if (session == null)
                            return null;

                        session.TimeoutMilliseconds = 2000;
                        session.FormattedIO.WriteLine("*IDN?");
                        string response = session.FormattedIO.ReadLine();

                        Console.WriteLine($"{visaAddress} → {response}");
                        return response; // 返回 *IDN 的结果
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect to {visaAddress}: {ex.Message}");
                    return null; // 表示失败
                }
            });
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            SaveToJson();
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(shiwang_textBox.Text))
            {
                shiwangName_textBox.Text = await TryConnectAndGetIdnAsync(shiwang_textBox.Text);
            }
            if (!string.IsNullOrEmpty(charge_textBox.Text))
            {
                chargeName_textBox.Text = await TryConnectAndGetIdnAsync(charge_textBox.Text);
            }
        }
    }
}
