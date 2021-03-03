using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataProfiler
{
    public partial class DataScanForm2 : Form
    {
        public string nL = Environment.NewLine;
        public DataScanForm2()
        {
            InitializeComponent();
            textBox1.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void DataScanForm2_Load(object sender, EventArgs e)
        {
            textBox1.Text = "Veuillez attendre un peu..." + nL;
            textBox1.Text = Form1.sd;
        }
    }
}
