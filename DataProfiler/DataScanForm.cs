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
    public partial class DataScanForm : Form
    {

        //public static string SetValueForText3 = "";
        public static string sg;
        public DataScanForm()
        {
            InitializeComponent();
            textBox2.ReadOnly = true;
             //sg = Form1.ss;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataScanForm_Load(object sender, EventArgs e)
        {

            //string nL = Environment.NewLine; "Veuillez attendre un peu..." +
            textBox2.Text = Form1.ss;

        }

    }
}
