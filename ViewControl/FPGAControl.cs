using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TestApp.FUNCTION;

namespace TestApp.ViewControl
{
    public partial class FPGAControl : UserControl
    {
        private Timer _timer;
        private Random _rand = new Random();
        private int _timeCounter = 0;
        private Form Main => this.FindForm();

        public FPGAControl()
        {
            InitializeComponent();
        }
        private void Log(string msg)
        {
            if (Main is Main parent)
            {
                parent.LogToConsole(msg);
            }
        }



    }
}
