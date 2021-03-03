using System;
using System.IO;
using System.Globalization;
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
    
    public partial class Form1 : Form
    {
        private Point mouseloc;
        DateTime dt;
        private string s, path, fullPath2, fullPath3, fullPath4, name2, name3, name4;       
        private DirectoryInfo ddi;
        public static bool st, calf, cald;

        public static string ss = "";
        public static string sd="";
        public static string fnm;
        public string nL = Environment.NewLine;

        public static List<FileExt> sortedExt2;        
        public static List<FileInfo> Lddo = new List<FileInfo>();

        public List<DirectoryInfo> ldirs;
        public static FileExt[,] Mvar;        
        public static List<DB> Lsudir;
        public List<DB> BDdir;
        public Dictionary<int, DateTime> dict;

        private List<FileInfo> Lddi = new List<FileInfo>();
        public Form1()
        {
            InitializeComponent();
            hidepanels();
            iconPictureBox1.Visible = false;
            iconPictureBox2.Visible = false;
            iconPictureBox3.Visible = false;
            iconPictureBox4.Visible = false;
            iconPictureBox5.Visible = false;
            iconPictureBox6.Visible = false;
            iconPictureBox7.Visible = false;
            iconPictureBox8.Visible = false;
            textBox1.ReadOnly = true;
            st = false;
            calf = false;
            cald = false;
            dt = DateTime.Now;
            label1.Text= "Plateforme IA-Agrosanté "+"         Date:  " +dt.ToString("D", CultureInfo.CreateSpecificCulture("fr-FR"));

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseloc = new Point(-e.X, -e.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                Point mouseP = Control.MousePosition;
                mouseP.Offset(mouseloc.X, mouseloc.Y);
                Location = mouseP;
            }
        }
        private void hidepanels()
        {
            panel5.Visible = false;
            panel6.Visible = false;
        }
        private void hideONpanels()
        {
            if (panel5.Visible == true)
                panel5.Visible = false;
            if (panel6.Visible == true)
                panel6.Visible = false;
        }
        private void viewSPanel(Panel sPanel)
        {
            if (sPanel.Visible == false)
            {
                hideONpanels();
                sPanel.Visible = true;
            }
            else
                sPanel.Visible = false;
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {

            
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            if ((result == DialogResult.OK)&&(st==false))
            {
                dt = DateTime.Now;
                iconPictureBox1.Visible = true;
                iconPictureBox2.Visible = false;
                iconPictureBox3.Visible = false;
                iconPictureBox4.Visible = false;
                iconPictureBox5.Visible = false;
                iconPictureBox6.Visible = false;
                iconPictureBox7.Visible = false;
                iconPictureBox8.Visible = false;
                textBox1.Text = folderBrowserDialog.SelectedPath;
                Environment.SpecialFolder root = folderBrowserDialog.RootFolder;
                path = $@"{textBox1.Text}";
            
            ddi = new DirectoryInfo(path);
            name2=  "outdata_" + ddi.Name+ ".csv";
            name3 = "outExt_" + ddi.Name + ".csv";
            name4 = "outfiles_" + ddi.Name + ".csv";
            s = ddi.Name + "_"+dt.ToString("yyyy-MM-dd-HH-mmss");
            string fullPath = Path.Combine(path, s);          
            Directory.CreateDirectory(fullPath);          
            fullPath2 = Path.Combine(path, s, name2);
            fullPath3 = Path.Combine(path, s, name3);
            fullPath4 = Path.Combine(path, s, name4);
            fnm = ddi.FullName; 
            FileInfo[] dif = ddi.GetFiles("*", SearchOption.AllDirectories);
            Lddi = dif.ToList();
                st = true;

            }

        }
        private void iconButton2_Click(object sender, EventArgs e)
        {
            if (st)
            {
               iconPictureBox2.Visible = true;
               iconPictureBox1.Visible = false;
               iconPictureBox3.Visible = false;
               iconPictureBox4.Visible = false;
               iconPictureBox5.Visible = false;
               iconPictureBox6.Visible = false;
               iconPictureBox7.Visible = false;
               iconPictureBox8.Visible = false;           
               viewSPanel(panel5);
            }
            else
            {
                string message = "Veuillez d'abord ouvrir le dossier\r\n";
                       message += "que vous souhaitez scanner!\r\n";
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void iconButton3_Click(object sender, EventArgs e)
        {
            iconPictureBox3.Visible = true;
            iconPictureBox1.Visible = false;
            iconPictureBox2.Visible = false;
            iconPictureBox4.Visible = false;
            iconPictureBox5.Visible = false;
            iconPictureBox6.Visible = false;
            iconPictureBox7.Visible = false;
            iconPictureBox8.Visible = false;
            hideONpanels();
            sortedExt2 = new List<FileExt>();
            HashSet<string> setExt = new HashSet<string>();
            FileExt[] extF = new FileExt[setExt.Count];
            List<FileExt> extFA = new List<FileExt>();
            List<FileExt> sortedExt = new List<FileExt>();

            setExt = DBscan.AllfileExtensions(Lddi);  //set of file formats used in folder/DB
            extF = DBscan.filespExt(Lddi, setExt);  //total size for each of the used file formats  

            extFA = extF.ToList();
            sortedExt = extFA.OrderByDescending(c => c.Number).ToList();
           
            foreach (FileExt ee in sortedExt)
            {
                if (ee.Number == 0) break;
                sortedExt2.Add(ee);
            }
            string line;
            int Fnumber = 0;
            float TotKb = 0;
            foreach (FileExt ee in sortedExt2)
            {
                Fnumber = Fnumber + ee.NFiles;
                TotKb = TotKb + ee.Number;
            }       

            using (System.IO.StreamWriter file1 = new System.IO.StreamWriter(fullPath3, false, Encoding.Default))
            {
                line = "Extension;";
                foreach (var extName in sortedExt2)
                {
                    line += extName.format + ";";
                }
                string fline = line.Remove(line.Length - 1, 1);
                file1.WriteLine(fline);
                line = "Total size (Kb);";
                foreach (var ext in sortedExt2)
                {
                    line += Convert.ToString(ext.Number) + ";";
                }
                fline = line.Remove(line.Length - 1, 1);
                file1.WriteLine(fline);
                line = "Number of files;";
                foreach (var ext in sortedExt2)
                {
                    line += Convert.ToString(ext.NFiles) + ";";
                }
                fline = line.Remove(line.Length - 1, 1);
                file1.WriteLine(fline);
                line = "Mean size (Kb);";
                foreach (var ext in sortedExt2)
                {
                    line += Convert.ToString(ext.Number / ext.NFiles) + ";";
                }
                fline = line.Remove(line.Length - 1, 1);
                file1.WriteLine(fline);
            }
            Lddo = Lddi.OrderByDescending(f => f.LastWriteTime).ToList();
            using (System.IO.StreamWriter file0 = new System.IO.StreamWriter(fullPath4, false, Encoding.Default))
            {
                line = "File_Name" + ";" + "Path" + ";" + "Last_write_time" + ";" + "Last_year_time" + ";" + "Last_access_time" + ";" + "Creation_time" + ";" + "File_size (Kb)" + ";" + "File_extension";
                file0.WriteLine(line);
                foreach (var fi in Lddo)
                {
                    line = fi.Name + ";" + fi.FullName + ";" + fi.LastWriteTime + ";" + +fi.LastWriteTime.Year + ";" + fi.LastAccessTime + ";" + fi.CreationTime + ";" + fi.Length/1024 + ";" + Path.GetExtension(fi.FullName);
                    file0.WriteLine(line);
                    //LsudirW[qq] = line;
                }
            }
            ss = "Dossier " + ddi.Name + ":" + nL + nL + "Nombre de fichiers: " + Convert.ToString(Fnumber) + nL + nL + "Espace disque: " + Convert.ToString(TotKb) + " Kb"+ nL +nL;
            ss +=  "Dans le dossier, " + s + " ,les fichiers, " + name3 + " et " + name4 + " , ";
            ss += "contenant des informations plus détaillées ont été enregistrés." + nL;
            DataScanForm childForm = new DataScanForm();
            openChildFormInPanel(childForm);            
            calf = true;
        }
        private void iconButton4_Click(object sender, EventArgs e)
        {
            if (calf)
            {
                iconPictureBox4.Visible = true;
            iconPictureBox3.Visible = false;
            iconPictureBox1.Visible = false;
            iconPictureBox2.Visible = false;
            iconPictureBox5.Visible = false;
            iconPictureBox6.Visible = false;
            iconPictureBox7.Visible = false;
            iconPictureBox8.Visible = false;
            hideONpanels();
            string line;          

            ldirs = DBscan.ListsubDirDB(path);          //top subdirectories
            Mvar = new FileExt[ldirs.Count, sortedExt2.Count]; //FileExt[,] Mvar = new FileExt[ldirs.Count, 9];
            FileExt[] Rvar = new FileExt[sortedExt2.Count];               //FileExt[] Rvar = new FileExt[9]
            Rvar = sortedExt2.ToArray();

            BDdir = new List<DB>();
            int q = 0;
            DateTime Recent = DateTime.Now;

            foreach (var fi in ldirs)
            {
                string FNm = fi.FullName;
                DB indDB = new DB(FNm, "All");
                indDB.DSize = DBscan.DirSize(FNm);

                FileInfo[] fif = fi.GetFiles("*", SearchOption.AllDirectories);
                DirectoryInfo[] fid = fi.GetDirectories("*", SearchOption.AllDirectories);
                List<FileInfo> fis = fif.ToList();

                //introducir valores cambiar funcion filesExt

                for (int i = 0; i < Rvar.Length; i++) Rvar[i].Number = 0;
                for (int i = 0; i < Rvar.Length; i++) Rvar[i].NFiles = 0;
                Rvar = DBscan.filesExtRvar(fis, Rvar);
                for (int i = 0; i < Rvar.Length; i++) Mvar[q, i] = Rvar[i]; //9

                List<DirectoryInfo> fids = fid.ToList();
                indDB.AgeY = DBscan.YearDiff(Recent, indDB.OldestFLWTime);
                indDB.DFileNumber = DBscan.nfiles(fis);
                indDB.DSubDirNumber = DBscan.ndir(fids);                
                if (DBscan.nfiles(fis) > 0)
                {
                    indDB.AvgAgeY = DBscan.MeanYFTime(fis, Recent);
                    indDB.StdAgeY = (float)Math.Sqrt(DBscan.Mean2YFTime(fis, Recent) - (indDB.AvgAgeY * indDB.AvgAgeY));
                }
                else //if (DBscan.nfiles(fis) == 0)
                {
                    indDB.AvgAgeY = 0;
                    indDB.StdAgeY = 0;
                }
                BDdir.Add(indDB);
                q++;
                //     file2.Write(fi.Name); Console.Write(";");                                   Console.Write(fi.CreationTime);  Console.Write("\t"); Console.Write(fi.LastWriteTime); Console.Write("\t"); Console.Write(fi.LastAccessTime);
              }
            string line1 = "DirName" + ";" + "Path_DirName" + ";" + "LastWriteTime" + ";" + "CT_OldestFile" + ";" + "LWT_OldestFile" + ";" + "LAT_OldestFile" + ";" + "maximum age (years)" + ";" + "average age (years)" + ";" + "std" + ";" + "Size of DB (Kb)" + ";";
            for (int i = 0; i < Rvar.Length; i++) line1 = line1 + Mvar[0, i].format + "(Kb);"; // for (int i = 0; i < 9; i++) line1 = line1 + Mvar[0, i].format + "(Kb);";
            line1 = line1 + "number of files" + ";" + "number of subdirectories";

            dict = new Dictionary<int, DateTime>();
            int y = 0;
            foreach (var b in BDdir)
            {
                dict[y] = b.OldestFLWTime;
                y++;
            }
            var dictr = dict.OrderByDescending(x => x.Value);
            Lsudir = new List<DB>();
            Lsudir = BDdir.OrderByDescending(f => f.OldestFLWTime).ToList(); // 
            string[] LsudirW = new string[Lsudir.Count];
            int qq = 0;
            foreach (var bd in Lsudir)
            {
                line = bd.DName + ";" + bd.PDName + ";" + bd.LWTime + ";" + bd.OldestFCTTime + ";" + bd.OldestFLWTime + ";" + bd.OldestFLATime + ";" + bd.AgeY + ";" + bd.AvgAgeY + ";" + bd.StdAgeY + ";" + bd.DSize + ";";
                LsudirW[qq] = line;
                qq++;
            }
            qq = 0;
            foreach (var ee in dictr)
            {
                line = LsudirW[qq];
                for (int i = 0; i < Rvar.Length; i++)
                    line = line + Convert.ToString(Mvar[ee.Key, i].Number) + ";";
                LsudirW[qq] = line;
                qq++;
            }
            qq = 0;
            foreach (var bd in Lsudir)
            {
                line = LsudirW[qq];
                line = line + bd.DFileNumber + ";" + bd.DSubDirNumber;
                LsudirW[qq] = line;
                qq++;
            }
            using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(fullPath2, false, Encoding.Default))
            {
                file2.WriteLine(line1);
                for (int i = 0; i < LsudirW.Length; i++)
                {
                    line = LsudirW[i];
                    file2.WriteLine(line);
                }
            }
            sd = "Dossier " + ddi.Name + ":" + nL + nL + "Nombre de sous-dossiers supérieurs: " + Convert.ToString(Lsudir.Count) + nL + nL ;
            sd += "Dans le dossier, " + s + " , le fichier, "+ name2+", contenant des informations plus detaillees a été enregistré." ;
            DataScanForm2 childForm = new DataScanForm2();
            openChildFormInPanel(childForm);            
            cald = true;
            }
            else
            {
                string message;
                 message = "Veuillez d'abord scanner les fichiers\r\n";                
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void iconButton5_Click(object sender, EventArgs e)
        {
            if ((cald)||(calf))
            {
            iconPictureBox5.Visible = true;
            iconPictureBox1.Visible = false;
            iconPictureBox2.Visible = false;
            iconPictureBox3.Visible = false;
            iconPictureBox4.Visible = false;
            iconPictureBox6.Visible = false;
            iconPictureBox7.Visible = false;
            iconPictureBox8.Visible = false;
            viewSPanel(panel6);
              }
            else
            {
                string message;
                if (st) message = "Veuillez scanner les donnees\r\n";
                else message = "Veuillez d'abord choisir un dossier et scanner les donnees\r\n";                
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    
        private void iconButton6_Click(object sender, EventArgs e)
        {
            iconPictureBox6.Visible = true;
            iconPictureBox1.Visible = false;
            iconPictureBox2.Visible = false;
            iconPictureBox3.Visible = false;
            iconPictureBox4.Visible = false;
            iconPictureBox5.Visible = false;
            iconPictureBox7.Visible = false;
            iconPictureBox8.Visible = false;
            hideONpanels();
            fileFormat childForm = new fileFormat();
            openChildFormInPanel(childForm);
          
        }
        private void iconButton7_Click(object sender, EventArgs e)
        {
            iconPictureBox7.Visible = true;
            iconPictureBox1.Visible = false;
            iconPictureBox2.Visible = false;
            iconPictureBox3.Visible = false;
            iconPictureBox4.Visible = false;
            iconPictureBox5.Visible = false;
            iconPictureBox6.Visible = false;
            iconPictureBox8.Visible = false;
            hideONpanels();
            LWtimeForm childForm = new LWtimeForm();
            openChildFormInPanel(childForm);
        }

        private void iconButton8_Click(object sender, EventArgs e)
        {
            iconPictureBox7.Visible = false;
            iconPictureBox1.Visible = false;
            iconPictureBox2.Visible = false;
            iconPictureBox3.Visible = false;
            iconPictureBox4.Visible = false;
            iconPictureBox5.Visible = false;
            iconPictureBox6.Visible = false;
            iconPictureBox8.Visible = false;
            textBox1.Text = "";
            ss = "";
            sd = "";
            st = false;
            cald = false;
            calf= false;
            if (activeForm != null)
                activeForm.Close();
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            iconPictureBox8.Visible = true;
            iconPictureBox7.Visible = false;
            iconPictureBox1.Visible = false;
            iconPictureBox2.Visible = false;
            iconPictureBox3.Visible = false;
            iconPictureBox4.Visible = false;
            iconPictureBox5.Visible = false;
            iconPictureBox6.Visible = false;

            DialogResult result;
            result = MessageBox.Show("Voulez-vous vraiment quitter l'application ?", "Caption", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                System.Windows.Forms.Application.Exit();
            }
         }

        private Form activeForm = null;

        public int BorderStyle { get; private set; }

        private void openChildFormInPanel(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel3.Controls.Add(childForm);
            panel3.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

      
    }
}
