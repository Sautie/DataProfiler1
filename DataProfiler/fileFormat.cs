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

namespace DataProfiler
{
    public partial class fileFormat : Form
    {
        bool protect;
        public fileFormat()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fileFormat_Load(object sender, EventArgs e)
        {
            int ii = 0;
            protect = false;
            foreach (FileExt ee in Form1.sortedExt2)
            {
                if (ii < 15)
                    PieExt.Series["Ext"].Points.AddXY(ee.format, Convert.ToString(ee.Number));
                else break;
                    ii++;
            }
            this.PieExt.Series["Ext"]["PieLabelStyle"] = "Outside";
            this.PieExt.Series["Ext"].BorderWidth = 1;
            this.PieExt.Series["Ext"].BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
            this.PieExt.Series["Ext"].Label = "#PERCENT{P2}";      
            //this.PieExt.Legends.Add("Legend1");
            this.PieExt.Legends[0].Enabled = true;
            this.PieExt.Legends[0].Docking = Docking.Bottom;
            this.PieExt.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
            this.PieExt.Series["Ext"].LegendText = "#VALX (#PERCENT)";
            this.PieExt.DataManipulator.Sort(PointSortOrder.Descending, PieExt.Series["Ext"]);
            //PieExt.Series["Ext"]["PieLabelStyle"] = "Disabled";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Form1.fnm; ;
            sfd.RestoreDirectory = true;
            sfd.Filter = "Png Image|*.png|JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Tiff Image|*.tiff";
            sfd.Title = "Sauvegarder un fichier image";
            sfd.DefaultExt = "png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FileName != "")
                {
                    switch (sfd.FilterIndex)
                    {
                        case 1: this.PieExt.SaveImage(sfd.FileName, ChartImageFormat.Png); break;
                        case 2: this.PieExt.SaveImage(sfd.FileName, ChartImageFormat.Jpeg); break;
                        case 3: this.PieExt.SaveImage(sfd.FileName, ChartImageFormat.Bmp); break;
                        case 4: this.PieExt.SaveImage(sfd.FileName, ChartImageFormat.Gif); break;
                        case 5: this.PieExt.SaveImage(sfd.FileName, ChartImageFormat.Tiff); break;
                    }                    
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Series series2 = new Series("Dossiers");
            if ((Form1.cald)&&(protect == false))
            {
                this.PieExt.Series["Ext"].Points.Clear();
                this.PieExt.Series["Ext"].IsVisibleInLegend = false;
                this.PieExt.Titles.RemoveAt(0);           
                this.PieExt.Series.Add(series2);
                series2.ChartArea = "ChartArea1";
                series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                series2.Name = "Dossiers";
                this.PieExt.Legends.Add(new Legend("Legend2"));
                Title title2 = new Title("t2");
                title2.Text = "Sous-Dossiers superieurs";
                this.PieExt.Titles.Add(title2);

                int ii = 0;
                foreach (DB ee in Form1.Lsudir)
                {
                    if (ii < 15)
                        series2.Points.AddXY(ee.DName, Convert.ToString(ee.DSize));
                    else break;
                    ii++;
                }
                this.PieExt.Series["Dossiers"]["PieLabelStyle"] = "Outside";
                this.PieExt.Series["Dossiers"].BorderWidth = 1;
                this.PieExt.Series["Dossiers"].BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                this.PieExt.Series["Dossiers"].Label = "#PERCENT{P2}";

                this.PieExt.Legends["Legend2"].Enabled = true;
                this.PieExt.Legends["Legend2"].Docking = Docking.Bottom;                
                this.PieExt.Legends["Legend2"].Alignment = System.Drawing.StringAlignment.Center;
                series2.LegendText = "#VALX (#PERCENT)";
                 this.PieExt.DataManipulator.Sort(PointSortOrder.Descending, series2);
                protect = true;

            }
            else if (protect)
            {
                this.PieExt.Series["Dossiers"]["PieLabelStyle"] = "Outside";
                this.PieExt.Series["Dossiers"].BorderWidth = 1;
                this.PieExt.Series["Dossiers"].BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                this.PieExt.Series["Dossiers"].Label = "#PERCENT{P2}";

                this.PieExt.Legends["Legend2"].Enabled = true;
                this.PieExt.Legends["Legend2"].Docking = Docking.Bottom;
                this.PieExt.Legends["Legend2"].Alignment = System.Drawing.StringAlignment.Center;
                series2.LegendText = "#VALX (#PERCENT)";
                this.PieExt.DataManipulator.Sort(PointSortOrder.Descending, series2);
            }
            else
            {
                string message;
                message = "Veuillez d'abord scanner les sous-dossiers superieurs\r\n";
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }
    }
}
