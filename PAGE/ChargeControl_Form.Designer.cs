namespace TestApp.PAGE
{
    partial class ChargeControl_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.manual_button = new System.Windows.Forms.Button();
            this.stop_output_button = new System.Windows.Forms.Button();
            this.start_output_button = new System.Windows.Forms.Button();
            this.ch_comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.current_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.voltage_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.disconnect_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // manual_button
            // 
            this.manual_button.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.manual_button.Location = new System.Drawing.Point(308, 134);
            this.manual_button.Name = "manual_button";
            this.manual_button.Size = new System.Drawing.Size(81, 39);
            this.manual_button.TabIndex = 23;
            this.manual_button.Text = "执行修改";
            this.manual_button.UseVisualStyleBackColor = true;
            this.manual_button.Click += new System.EventHandler(this.manual_button_Click);
            // 
            // stop_output_button
            // 
            this.stop_output_button.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stop_output_button.Location = new System.Drawing.Point(308, 82);
            this.stop_output_button.Name = "stop_output_button";
            this.stop_output_button.Size = new System.Drawing.Size(81, 39);
            this.stop_output_button.TabIndex = 20;
            this.stop_output_button.Text = "停止输出";
            this.stop_output_button.UseVisualStyleBackColor = true;
            this.stop_output_button.Click += new System.EventHandler(this.stop_output_button_Click);
            // 
            // start_output_button
            // 
            this.start_output_button.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.start_output_button.Location = new System.Drawing.Point(308, 35);
            this.start_output_button.Name = "start_output_button";
            this.start_output_button.Size = new System.Drawing.Size(81, 39);
            this.start_output_button.TabIndex = 19;
            this.start_output_button.Text = "启动输出";
            this.start_output_button.UseVisualStyleBackColor = true;
            this.start_output_button.Click += new System.EventHandler(this.start_output_button_Click);
            // 
            // ch_comboBox
            // 
            this.ch_comboBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ch_comboBox.FormattingEnabled = true;
            this.ch_comboBox.Items.AddRange(new object[] {
            "CH1",
            "CH2",
            "CH3"});
            this.ch_comboBox.Location = new System.Drawing.Point(137, 127);
            this.ch_comboBox.Name = "ch_comboBox";
            this.ch_comboBox.Size = new System.Drawing.Size(100, 22);
            this.ch_comboBox.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(38, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 19);
            this.label3.TabIndex = 17;
            this.label3.Text = "通道：";
            // 
            // current_textBox
            // 
            this.current_textBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.current_textBox.Location = new System.Drawing.Point(137, 84);
            this.current_textBox.Name = "current_textBox";
            this.current_textBox.Size = new System.Drawing.Size(100, 23);
            this.current_textBox.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(38, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 19);
            this.label2.TabIndex = 15;
            this.label2.Text = "输出电流：";
            // 
            // voltage_textBox
            // 
            this.voltage_textBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.voltage_textBox.Location = new System.Drawing.Point(137, 44);
            this.voltage_textBox.Name = "voltage_textBox";
            this.voltage_textBox.Size = new System.Drawing.Size(100, 23);
            this.voltage_textBox.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(38, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 19);
            this.label1.TabIndex = 13;
            this.label1.Text = "输出电压：";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // disconnect_button
            // 
            this.disconnect_button.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.disconnect_button.Location = new System.Drawing.Point(395, 82);
            this.disconnect_button.Name = "disconnect_button";
            this.disconnect_button.Size = new System.Drawing.Size(81, 39);
            this.disconnect_button.TabIndex = 24;
            this.disconnect_button.Text = "断开连接";
            this.disconnect_button.UseVisualStyleBackColor = true;
            this.disconnect_button.Visible = false;
            this.disconnect_button.Click += new System.EventHandler(this.disconnect_button_Click);
            // 
            // ChargeControl_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 209);
            this.Controls.Add(this.disconnect_button);
            this.Controls.Add(this.manual_button);
            this.Controls.Add(this.stop_output_button);
            this.Controls.Add(this.start_output_button);
            this.Controls.Add(this.ch_comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.current_textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.voltage_textBox);
            this.Controls.Add(this.label1);
            this.Name = "ChargeControl_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "电源控制";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button manual_button;
        private System.Windows.Forms.Button stop_output_button;
        private System.Windows.Forms.Button start_output_button;
        private System.Windows.Forms.ComboBox ch_comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox current_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox voltage_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button disconnect_button;
    }
}