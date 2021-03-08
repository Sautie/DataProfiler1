using System;
using System.IO;
using System.Collections;
using System.Globalization;
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
    public partial class LWtimeForm : Form
    {
        string ChosenItem;
        public LWtimeForm()
        {
            InitializeComponent();          
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LWtimeForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = comboBox1.FindStringExact("Heures");
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series("Heures");
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series("Heures_Bar");
            int [] t = new int [] {0, 0, 0, 0, 0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0};             
            foreach (FileInfo ee in Form1.Lddo)
            {
                int h = ee.LastWriteTime.Hour;
                t[h] = t[h] + 1;
            }
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Points.DataBindY(t);
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            series2.Points.DataBindY(t);
            chart1.Series.Clear();
            chart1.Series.Add(series1);
            chart1.Series.Add(series2);

            // additional styling
            chart1.ResetAutoValues();
            chart1.Titles.Clear();
            chart1.Titles.Add($"Frequences vs heures");
            chart1.ChartAreas[0].AxisX.Title = "Heures";
            chart1.ChartAreas[0].AxisY.Title = "Frequences";
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
        }
        public List<string> GetWeekDaysByCulture(CultureInfo culture)
        {
            return new List<string>
          {

        culture.DateTimeFormat.GetDayName(DayOfWeek.Monday),
        culture.DateTimeFormat.GetDayName(DayOfWeek.Tuesday),
        culture.DateTimeFormat.GetDayName(DayOfWeek.Wednesday),
        culture.DateTimeFormat.GetDayName(DayOfWeek.Thursday),
        culture.DateTimeFormat.GetDayName(DayOfWeek.Friday),
        culture.DateTimeFormat.GetDayName(DayOfWeek.Saturday),
        culture.DateTimeFormat.GetDayName(DayOfWeek.Sunday)
          };
        }
        public List<string> GetMonthNamesByCulture(CultureInfo culture)
        {
          return new List<string>
             {
        culture.DateTimeFormat.GetMonthName(1),
        culture.DateTimeFormat.GetMonthName(2),
        culture.DateTimeFormat.GetMonthName(3),
        culture.DateTimeFormat.GetMonthName(4),
        culture.DateTimeFormat.GetMonthName(5),
        culture.DateTimeFormat.GetMonthName(6),
        culture.DateTimeFormat.GetMonthName(7),
        culture.DateTimeFormat.GetMonthName(8),
        culture.DateTimeFormat.GetMonthName(9),
        culture.DateTimeFormat.GetMonthName(10),
        culture.DateTimeFormat.GetMonthName(11),
        culture.DateTimeFormat.GetMonthName(12),
            };
        }
        private void button6_Click(object sender, EventArgs e)
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
                        case 1: this.chart1.SaveImage(sfd.FileName, ChartImageFormat.Png); break;
                        case 2: this.chart1.SaveImage(sfd.FileName, ChartImageFormat.Jpeg); break;
                        case 3: this.chart1.SaveImage(sfd.FileName, ChartImageFormat.Bmp); break;
                        case 4: this.chart1.SaveImage(sfd.FileName, ChartImageFormat.Gif); break;
                        case 5: this.chart1.SaveImage(sfd.FileName, ChartImageFormat.Tiff); break;
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          ChosenItem = comboBox1.Text;
            if (ChosenItem=="Mois")
            {
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series("Mois");
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series("Mois_Bar");
            int[] t = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<string> months = GetMonthNamesByCulture(new CultureInfo("fr-FR"));
           
            foreach (FileInfo ee in Form1.Lddo)
            {               
                    int h = ee.LastWriteTime.Month;
                    t[h-1] = t[h-1] + 1;
            }
                string[] monthsa = months.ToArray();
                series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Points.DataBindXY(monthsa, t);
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            series4.Points.DataBindXY(monthsa, t);
            chart1.Series.Clear();
            chart1.Series.Add(series3);
            chart1.Series.Add(series4);

            // additional styling
            chart1.ResetAutoValues();
            chart1.Titles.Clear();
            chart1.Titles.Add($"Frequences vs mois");
            chart1.ChartAreas[0].AxisX.Title = "Mois";
            chart1.ChartAreas[0].AxisY.Title = "Frequences";           
                chart1.ChartAreas[0].AxisX.MajorTickMark.Interval = 1;
              chart1.ChartAreas[0].AxisX.MinorTickMark.Interval = 0.5;
                chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
             chart1.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
            }
            else if (ChosenItem == "Jours_Semaine")
            {
                System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series("Jours");
                System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series("Jours_Bar");
                int[] t = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                List<string> daysw = GetWeekDaysByCulture(new CultureInfo("fr-FR"));
                foreach (FileInfo ee in Form1.Lddo)
                {
                    int h = (int)ee.LastWriteTime.DayOfWeek;
                    t[h] = t[h] + 1;
                }               
                string[] dayswa = daysw.ToArray();
                series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                series3.Points.DataBindXY(dayswa, t);
                series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                series4.Points.DataBindXY(dayswa, t);
                chart1.Series.Clear();
                chart1.Series.Add(series3);
                chart1.Series.Add(series4);

                // additional styling
                chart1.ResetAutoValues();
                chart1.Titles.Clear();
                chart1.Titles.Add($"Frequences vs Jours de la semaine");
                chart1.ChartAreas[0].AxisX.Title = "Jours de la semaine";
                chart1.ChartAreas[0].AxisY.Title = "Frequences";
                chart1.ChartAreas[0].AxisX.MajorTickMark.Interval = 1;
                chart1.ChartAreas[0].AxisX.MinorTickMark.Interval = 0.5;
                chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
                chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;

            }
            else if (ChosenItem == "Ans")
            {
                System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series("Ans");
                System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series("Ans_Bar");
                int[] a = new int[] { 0, 0, 0, 0, 0,  0, 0, 0,0,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
                string[] aa = new string[a.Length];
                int uu = 0;
                while (uu < a.Length)
                {
                    aa[uu] = " "; uu++;
                }
                int gg = 0;
                foreach (FileInfo ee in Form1.Lddo)
                {
                    string h = Convert.ToString(ee.LastWriteTime.Year);
                    int g = 0;
                    while ((g < aa.Length)&&(aa[g] != h)) g++;
                    if (g == aa.Length)
                    {
                          aa[gg] = h;
                          a[gg] = a[gg] + 1;
                          gg++;                        
                    }
                    else
                    {
                        a[g] = a[g] + 1;
                    }                   
                }
                int u = 0;
                while ((u < aa.Length)&&(aa[u] != " ")) u++;
                string[] aan;
                int[] an;
                if (u < 26)
                {
                    aan = new string[26];
                    an = new int[26];
                    u = 0;
                    while (u < 26) 
                    {
                        aan[u] = aa[u];
                        an[u] = a[u];
                        u++;
                    }
                }
                else
                {
                 aan = new string [u + 1];
                 an = new int[u + 1];
                    u = 0;
                    while ((u < aa.Length) && (aa[u] != " "))
                    {
                        aan[u] = aa[u];
                        an[u] = a[u];
                        u++;
                    }
                }
               
                series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                series5.Points.DataBindXY(aan, an);
                series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                series6.Points.DataBindXY(aan, an);
                //chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                chart1.Series.Clear();
                chart1.Series.Add(series5);
                chart1.Series.Add(series6);
                // additional styling
                //chart1.ResetAutoValues();
                chart1.Titles.Clear();
                chart1.Titles.Add($"Frequences vs Ans");
                chart1.ChartAreas[0].AxisX.Title = "Ans";
                chart1.ChartAreas[0].AxisY.Title = "Frequences";
                chart1.ChartAreas[0].AxisX.MajorTickMark.Interval = 1;
                chart1.ChartAreas[0].AxisX.MinorTickMark.Interval = 0.5;
                chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
                chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;

            }
            else if (ChosenItem == "Heures")
            {
                System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series("Heures");
                System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series("Heures_Bar");
                int[] t = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                foreach (FileInfo ee in Form1.Lddo)
                {
                    int h = ee.LastWriteTime.Hour;
                    t[h] = t[h] + 1;
                }
                series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                series1.Points.DataBindY(t);
                series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                series2.Points.DataBindY(t);
                chart1.Series.Clear();
                chart1.Series.Add(series1);
                chart1.Series.Add(series2);

                // additional styling
                chart1.ResetAutoValues();
                chart1.Titles.Clear();
                chart1.Titles.Add($"Frequences vs heures");
                chart1.ChartAreas[0].AxisX.Title = "Heures";
                chart1.ChartAreas[0].AxisY.Title = "Frequences";
                chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
                chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            }
        }

    }

  
}
