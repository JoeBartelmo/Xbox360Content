using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Xbox360Content;
using Xbox360Content.XDBF.GPD;
namespace Testing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Log.ConsoleLogging = Log.EnableLogging = true;

            GPD gpd = new GPD(new IO(@"C:\Users\Joe\Desktop\FFFE07D1.gpd"));
            gpd.Update();
            gpd.Close();
        }
    }
}
