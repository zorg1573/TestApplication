using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ivi.Visa;
using Keysight.KtNA;
using TestApp.MODEL;

namespace TestApp.FUNCTION
{
    public class ScpiDevice
    {
        private IMessageBasedSession _visaSession;
        private DeviceInfo _device;

        public ScpiDevice(DeviceInfo device)
        {
            _device = device;
        }

        public ScpiDevice() { }

        public bool IsConnected => _visaSession != null;

        // 新增：公开设备信息属性
        public DeviceInfo Device => _device;

        // 新增：方便直接获取地址
        public string Address => _device?.Address;


        public async Task<bool> ConnectAsync(string resourceString)
        {
            try
            {
                var rm = new NationalInstruments.Visa.ResourceManager();
                _visaSession = (IMessageBasedSession)rm.Open(resourceString);

                // 设置终止符
                _visaSession.TerminationCharacter = (byte)'\n';
                _visaSession.TerminationCharacterEnabled = true;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"VISA连接失败: {ex.Message}");
                return false;
            }
        }

        public async Task<string> QueryAsync(string command)
        {
            if (!IsConnected) return null;
            try
            {
                _visaSession.FormattedIO.WriteLine(command);
                return _visaSession.FormattedIO.ReadLine();
            }
            catch(Exception ex)
            {
                MessageBox.Show("查询失败，请检查设备连接或命令格式。" + ex);
                return null;
            }
        }

        public async Task<bool> SendCommandAsync(string command)
        {
            if (!IsConnected) return false;
            try
            {
                _visaSession.FormattedIO.WriteLine(command);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                _visaSession?.Dispose();
                _visaSession = null;
            }
            catch { }
        }

        public static async Task<string> IdentifyAsync(string resourceString)
        {
            var device = new ScpiDevice();
            bool connected = await device.ConnectAsync(resourceString);
            if (!connected) return null;

            string idn = await device.QueryAsync("*IDN?");
            device.Disconnect();
            return idn;
        }

        // SCPI命令封装（保持不变）
        public async Task SelectChannel(int ch) => await SendCommandAsync($"INST:NSEL {ch}");
        public async Task<string> GetInfo() => await QueryAsync("*IDN?");
        public async Task ClearInfo() => await SendCommandAsync("*CLS");
        public async Task SetVoltage(double voltage) => await SendCommandAsync($"VOLT {voltage}");
        public async Task SetCurrent(double current) => await SendCommandAsync($"CURR {current}");
        public async Task SetFrequency(double freq) => await SendCommandAsync($"FREQ {freq}");
        public async Task SetPower(double power) => await SendCommandAsync($"POW {power}");
        public async Task EnableOutput() => await SendCommandAsync("OUTP ON");
        public async Task DisableOutput() => await SendCommandAsync("OUTP OFF");
        public async Task ModON() => await SendCommandAsync("OUTP:MOD ON");
        public async Task ModOFF() => await SendCommandAsync("OUTP:MOD OFF");
        public async Task<double?> ReadVoltage()
        {
            string resp = await QueryAsync("MEAS:VOLT?");
            return double.TryParse(resp?.Trim(), out double val) ? (double?)val : null;
        }

        public async Task<double?> ReadCurrent()
        {
            string resp = await QueryAsync("MEAS:CURR?");
            return double.TryParse(resp?.Trim(), out double val) ? (double?)val : null;
        }

        public async Task<double?> ReadFrequency()
        {
            string resp = await QueryAsync("FREQ?");
            return double.TryParse(resp?.Trim(), out double val) ? (double?)val : null;
        }

        public async Task<double?> ReadPower()
        {
            string resp = await QueryAsync("POW?");
            return double.TryParse(resp?.Trim(), out double val) ? (double?)val : null;
        }


        #region 频谱
        // 设置频谱分析相关参数
        public async Task SetCenterFrequencyAsync(double freqHz) => await SendCommandAsync($"FREQ:CENT {freqHz}");
        public async Task SetSpanAsync(double spanHz) => await SendCommandAsync($"FREQ:SPAN {spanHz}");
        public async Task SetStartFrequencyAsync(double startHz) => await SendCommandAsync($"FREQ:STAR {startHz}");
        public async Task SetStopFrequencyAsync(double stopHz) => await SendCommandAsync($"FREQ:STOP {stopHz}");

        public async Task SetRBWAsync(double rbwHz) => await SendCommandAsync($"BAND:RES {rbwHz}");
        public async Task SetVBWAsync(double vbwHz) => await SendCommandAsync($"BAND:VID {vbwHz}");

        public async Task SetSweepTimeAsync(double timeSeconds) => await SendCommandAsync($"SWE:TIME {timeSeconds}");

        // 设置检波器：如 "NORM", "AVER", "POS", "NEG", "SAMP", "RMS"
        public async Task SetDetectorAsync(string mode) => await SendCommandAsync($":DET {mode}");

        // 设置触发源：如 "IMM", "EXT", "VID", "LINE"
        public async Task SetTriggerSourceAsync(string source) => await SendCommandAsync($"TRIG:SOUR {source}");

        // 启动单次扫描并等待完成
        public async Task StartSingleSweepAsync() => await SendCommandAsync(":INIT:CONT OFF; INIT; *WAI");
        // 连续扫描
        public async Task StartSweepAsync() => await SendCommandAsync(":INIT:CONT ON");
        // 标记相关功能
        public async Task SetMarkerToMaxAsync() => await SendCommandAsync("CALC:MARK1:MAX");

        public async Task<double?> ReadMarkerFrequencyAsync()
        {
            string resp = await QueryAsync("CALC:MARK1:X?");
            return double.TryParse(resp?.Trim(), out double val) ? (double?)val : null;
        }

        public async Task<double?> ReadMarkerPowerAsync()
        {
            string resp = await QueryAsync("CALC:MARK1:Y?");
            return double.TryParse(resp?.Trim(), out double val) ? (double?)val : null;
        }
        #endregion


        #region 矢网
        // 设置测量通道并选择参数（如 S12）
        public async Task<bool> SelectSParameterAsync(string name, string sparam)
        {
            await SendCommandAsync($"CALC:PAR:DEF '{name}', {sparam}");
            return await SendCommandAsync($"CALC:PAR:SEL '{name}'");
        }

        // 读取某格式下的 S 参数数据（返回第一点，或者你可以扩展为返回数组）
        private async Task<double?> ReadSParameterAsync(string traceName, string format)
        {
            await SelectSParameterAsync(traceName, "S12");
            await SendCommandAsync($"CALC:FORM {format}");
            /*            await SendCommandAsync("INIT:IMM; *WAI");
                        string data = await QueryAsync("CALC:DATA? FDATA");
                        if (string.IsNullOrWhiteSpace(data)) return null;

                        // 仅取第一个点
                        string[] parts = data.Split(',');
                        return double.TryParse(parts[0].Trim(), out double val) ? (double?)val : null;*/
            // 关闭连续扫描，改用手动单次扫描
            await SendCommandAsync("INIT:CONT OFF");

            // 触发单次测量
            await SendCommandAsync("INIT:IMM");

            // 等待操作完成，仪器返回“1”表示完成
            string opc = await QueryAsync("*OPC?");

            string data = await QueryAsync("CALC:DATA? FDATA");
            if (string.IsNullOrWhiteSpace(data)) return null;

            string[] parts = data.Split(',');
            return double.TryParse(parts[0].Trim(), out double val) ? (double?)val : null;
        }
        private async Task<string[]> GetAllData(string traceName, string format)
        {
            await SelectSParameterAsync(traceName, "S12");
            await SendCommandAsync($"CALC:FORM {format}");

            // 关闭连续扫描，改用手动单次扫描
            await SendCommandAsync("INIT:CONT OFF");

            // 触发单次测量
            await SendCommandAsync("INIT:IMM");

            // 等待操作完成，仪器返回“1”表示完成
            string opc = await QueryAsync("*OPC?");

            string data = await QueryAsync("CALC:DATA? FDATA");
            if (string.IsNullOrWhiteSpace(data)) return null;

            string[] parts = data.Split(',');
            return parts;
        }
        // 获取 S12 增益（对数幅度，dB）
        public async Task<double?> GetGainAsync()
        {
            return await ReadSParameterAsync("CH1_S21_3", "MLOG");
        }
        public async Task<string[]> GetGainStringAsync()
        {
            double freqHz = await GetFreqStart() ?? 0;
            await SelectSParameterAsync("CH1_S21_3", "S12");
            await SendCommandAsync("CALC:FORM MLOG");
            await SendCommandAsync("INIT:IMM; *WAI");
            string data = await QueryAsync("CALC:DATA? FDATA");
            string[] parts = data?.Split(',');
            return parts;
            //return await GetAllData("CH1_S21_3", "MLOG");
        }
        // 获取 S12 相位（单位度）
        public async Task<double?> GetPhaseAsync()
        {
            return await ReadSParameterAsync("CH1_S22_4", "PHAS");
        }
        public async Task<string[]> GetPhaseStringAsync()
        {
            return await GetAllData("CH1_S22_4", "PHAS");
        }
        // 获取 S11 驻波比（VSWR）
        public async Task<double?> GetInputVSWRAsync()
        {
            await SelectSParameterAsync("CH1_S11_1", "S11");
            await SendCommandAsync("CALC:FORM SWR");
            await SendCommandAsync("INIT:IMM; *WAI");
            string data = await QueryAsync("CALC:DATA? FDATA");
            string[] parts = data?.Split(',');
            return double.TryParse(parts?[0].Trim(), out double val) ? (double?)val : null;
        }
        public async Task<string[]> GetInputVSWRStringAsync()
        {
            await SelectSParameterAsync("CH1_S11_1", "S11");
            await SendCommandAsync("CALC:FORM SWR");
            await SendCommandAsync("INIT:IMM; *WAI");
            string data = await QueryAsync("CALC:DATA? FDATA");
            string[] parts = data?.Split(',');
            return parts;
        }
        // 获取 S22 驻波比（VSWR）
        public async Task<double?> GetOutputVSWRAsync()
        {
            await SelectSParameterAsync("CH1_S12_2", "S22");
            await SendCommandAsync("CALC:FORM SWR");
            await SendCommandAsync("INIT:IMM; *WAI");
            string data = await QueryAsync("CALC:DATA? FDATA");
            string[] parts = data?.Split(',');
            return double.TryParse(parts?[0].Trim(), out double val) ? (double?)val : null;
        }
        public async Task<string[]> GetOutputVSWRStringAsync()
        {
            await SelectSParameterAsync("CH1_S12_2", "S22");
            await SendCommandAsync("CALC:FORM SWR");
            await SendCommandAsync("INIT:IMM; *WAI");
            string data = await QueryAsync("CALC:DATA? FDATA");
            string[] parts = data?.Split(',');
            return parts;
        }
        // 获取初始相位
        public async Task<double?> GetInitialPhaseAsync()
        {
            double freqHz = await GetFreqStart() ?? 0;
            await SelectSParameterAsync("CH1_S22_4", "S12");
            await SendCommandAsync("CALC:FORM PHAS");
            await SendCommandAsync("INIT:IMM; *WAI");
            string data = await QueryAsync("CALC:DATA? FDATA");
            string[] parts = data?.Split(',');
            return double.TryParse(parts?[0].Trim(), out double val) ? (double?)val : null;
        }
        public async Task<string[]> GetInitialPhaseStringAsync()
        {
            double freqHz = await GetFreqStart() ?? 0;
            await SelectSParameterAsync("CH1_S22_4", "S12");
            await SendCommandAsync("CALC:FORM PHAS");
            await SendCommandAsync("INIT:IMM; *WAI");
            string data = await QueryAsync("CALC:DATA? FDATA");
            string[] parts = data?.Split(',');
            return parts;
        }
        public async Task<double?> GetFreqStart()
        {
            string freq = await QueryAsync(":SENS:FREQ:STAR?");
            return double.TryParse(freq?.Trim(), out double val) ? (double?)val : null;
        }
        public async Task<double?> GetFreqStop()
        {
            string freq = await QueryAsync(":SENS:FREQ:STOP?");
            return double.TryParse(freq?.Trim(), out double val) ? (double?)val : null;
        }
        public async Task<int?> GetPointCount()
        {
            string count = await QueryAsync(":SENS:SWE:POIN?");
            return int.TryParse(count?.Trim(), out int val) ? (int?)val : null;
        }
        public async Task<bool> AutoScan()
        {
            return await SendCommandAsync("INIT:IMM; *WAI");
        }
        public async Task<bool> LoadStateFile(string vnaPath)
        {
            string command = $":MMEM:LOAD:FILE \"{vnaPath}\"";
            return await SendCommandAsync(command);
        }

        public async Task<bool> SaveStateFile(string vnaPath)
        {
            string command = $":MMEM:STOR:FILE \"{vnaPath}\"";
            return await SendCommandAsync(command);
        }
        public async Task<bool> SetPointCount(int count)
        {
            return await SendCommandAsync($":SENS:SWE:POIN {count}");
        }
        #endregion

        #region 功率计
        // 获取当前功率值（单位 dBm）
        public async Task<double?> GetPowerDbm()
        {
            string result = await QueryAsync("MEAS:POW?");
            return double.TryParse(result?.Trim(), out double val) ? (double?)val : null;
        }

        // 设置功率单位（如 DBM、WATT）
        public async Task<bool> SetPowerUnit(string unit)
        {
            return await SendCommandAsync($":UNIT:POW {unit}");
        }

        // 查询功率单位
        public async Task<string> GetPowerUnit()
        {
            return await QueryAsync(":UNIT:POW?");
        }

        // 启用/关闭自动校零
        public async Task<bool> SetAutoZero(bool enable)
        {
            return await SendCommandAsync($":ZERO:AUTO {(enable ? "ON" : "OFF")}");
        }

        // 查询是否开启自动校零
        public async Task<bool?> IsAutoZeroEnabled()
        {
            string result = await QueryAsync(":ZERO:AUTO?");
            if (string.IsNullOrWhiteSpace(result))
                return null;

            string val = result.Trim().ToUpper();
            if (val == "ON")
                return true;
            else if (val == "OFF")
                return false;
            else
                return null;
        }


        // 设置触发方式（IMM 或 EXT）
        public async Task<bool> SetTriggerSource(string mode)
        {
            return await SendCommandAsync($":TRIG:SOUR {mode}");
        }

        // 获取触发方式
        public async Task<string> GetTriggerSource()
        {
            return await QueryAsync(":TRIG:SOUR?");
        }

        // 启动测量（立即触发）
        public async Task<bool> TriggerImmediate()
        {
            return await SendCommandAsync("INIT:IMM; *WAI");
        }

        public async Task<string> ReadData()
        {
            return await QueryAsync("READ?");
        }
        public async Task<bool> LoadGonglvState()
        {
            return await SendCommandAsync("*RCL 1");
        }
        public async Task<bool> SaveGonglvState()
        {
            return await SendCommandAsync("*SAV 1");
        }
        public async Task<double[]> ReadPulsePowerArrayAsync()
        {
            string result = await QueryAsync(":FETC:ARR:AME:POW?");
            if (string.IsNullOrWhiteSpace(result))
                return Array.Empty<double>();

            var parts = result.Split(',');
            var values = new List<double>();

            foreach (var part in parts)
            {
                if (double.TryParse(part.Trim(), out double val))
                {
                    // 过滤无效值（9.91e37 表示 NAN）
                    if (Math.Abs(val - 9.91e37) > 1e30)
                        values.Add(val);
                    else
                        values.Add(double.NaN);
                }
                else
                {
                    values.Add(double.NaN);
                }
            }

            return values.ToArray();
        }

        #endregion
    }
}
