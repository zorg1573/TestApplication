namespace TestApp.PAGE
{
    partial class XinhaoControl_Form
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
            this.power_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.start_freq_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.point_count_textBox = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.stop_freq_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // manual_button
            // 
            this.manual_button.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.manual_button.Location = new System.Drawing.Point(377, 137);
            this.manual_button.Name = "manual_button";
            this.manual_button.Size = new System.Drawing.Size(81, 39);
            this.manual_button.TabIndex = 23;
            this.manual_button.Text = "保存设置";
            this.manual_button.UseVisualStyleBackColor = true;
            this.manual_button.Click += new System.EventHandler(this.manual_button_Click);
            // 
            // stop_output_button
            // 
            this.stop_output_button.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stop_output_button.Location = new System.Drawing.Point(377, 83);
            this.stop_output_button.Name = "stop_output_button";
            this.stop_output_button.Size = new System.Drawing.Size(104, 39);
            this.stop_output_button.TabIndex = 20;
            this.stop_output_button.Text = "停止射频输出";
            this.stop_output_button.UseVisualStyleBackColor = true;
            this.stop_output_button.Click += new System.EventHandler(this.stop_output_button_Click);
            // 
            // start_output_button
            // 
            this.start_output_button.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.start_output_button.Location = new System.Drawing.Point(377, 30);
            this.start_output_button.Name = "start_output_button";
            this.start_output_button.Size = new System.Drawing.Size(104, 39);
            this.start_output_button.TabIndex = 19;
            this.start_output_button.Text = "启动射频输出";
            this.start_output_button.UseVisualStyleBackColor = true;
            this.start_output_button.Click += new System.EventHandler(this.start_output_button_Click);
            // 
            // power_textBox
            // 
            this.power_textBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.power_textBox.Location = new System.Drawing.Point(133, 112);
            this.power_textBox.Name = "power_textBox";
            this.power_textBox.Size = new System.Drawing.Size(127, 23);
            this.power_textBox.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(32, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 19);
            this.label2.TabIndex = 15;
            this.label2.Text = "功率：";
            // 
            // start_freq_textBox
            // 
            this.start_freq_textBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.start_freq_textBox.Location = new System.Drawing.Point(133, 31);
            this.start_freq_textBox.Name = "start_freq_textBox";
            this.start_freq_textBox.Size = new System.Drawing.Size(127, 23);
            this.start_freq_textBox.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(32, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 19);
            this.label1.TabIndex = 13;
            this.label1.Text = "起始频率：";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "GHz",
            "MHz",
            "Hz"});
            this.comboBox1.Location = new System.Drawing.Point(276, 33);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(64, 20);
            this.comboBox1.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(274, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "dBm";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(507, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 39);
            this.button1.TabIndex = 26;
            this.button1.Text = "启用调制功能";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(507, 83);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 39);
            this.button2.TabIndex = 27;
            this.button2.Text = "关闭调制功能";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(32, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 19);
            this.label4.TabIndex = 28;
            this.label4.Text = "测试点数：";
            // 
            // point_count_textBox
            // 
            this.point_count_textBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.point_count_textBox.Location = new System.Drawing.Point(133, 153);
            this.point_count_textBox.Name = "point_count_textBox";
            this.point_count_textBox.Size = new System.Drawing.Size(127, 23);
            this.point_count_textBox.TabIndex = 29;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "GHz",
            "MHz",
            "Hz"});
            this.comboBox2.Location = new System.Drawing.Point(276, 73);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(64, 20);
            this.comboBox2.TabIndex = 32;
            // 
            // stop_freq_textBox
            // 
            this.stop_freq_textBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stop_freq_textBox.Location = new System.Drawing.Point(133, 71);
            this.stop_freq_textBox.Name = "stop_freq_textBox";
            this.stop_freq_textBox.Size = new System.Drawing.Size(127, 23);
            this.stop_freq_textBox.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(32, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 19);
            this.label5.TabIndex = 30;
            this.label5.Text = "终止频率：";
            // 
            // XinhaoControl_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 218);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.stop_freq_textBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.point_count_textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.manual_button);
            this.Controls.Add(this.stop_output_button);
            this.Controls.Add(this.start_output_button);
            this.Controls.Add(this.power_textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.start_freq_textBox);
            this.Controls.Add(this.label1);
            this.Name = "XinhaoControl_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "信号发生器控制";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button manual_button;
        private System.Windows.Forms.Button stop_output_button;
        private System.Windows.Forms.Button start_output_button;
        private System.Windows.Forms.TextBox power_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox start_freq_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox point_count_textBox;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox stop_freq_textBox;
        private System.Windows.Forms.Label label5;
    }
}