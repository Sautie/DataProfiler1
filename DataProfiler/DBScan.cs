using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProfiler
{
    public struct FileExt
    {
        public FileExt(string Ext="", float numb=0, int nf=0)
        {
            format = Ext;
            Number = numb;
            NFiles = nf;
        }
        public string format { get; }
        public float Number { get; set; }
        public int NFiles { get; set; }
    }

    static class DBscan
    {
        static public List<DirectoryInfo> ListsubDirDB(string location)
        {
            return new DirectoryInfo(location).GetDirectories("*", SearchOption.TopDirectoryOnly)
                                              .OrderByDescending(f => f.LastWriteTime)
                                              .ToList();
        }
        static public float DirSize(string location)
        {
            var di = new DirectoryInfo(location);
            long size = 0;
            foreach (FileInfo fi in di.GetFiles("*", SearchOption.AllDirectories))
            {
                size += fi.Length;
            }
            return (size / 1024);
        }
        static public int YearDiff(DateTime Recent, DateTime old)
        {
            return Math.Abs(Recent.Year - old.Year);
        }
        static public int MeanYFTime(List<FileInfo> fis, DateTime Recent, string Temps = "LastWriteTime")
        {
            int s = 0, n = fis.Count;
            DateTime T;
            foreach (var fi in fis)
            {
                if (Temps == "CreationTime")
                {
                    T = fi.CreationTime;
                    s += YearDiff(Recent, T);
                }
                else if (Temps == "LastAccessTime")
                {
                    T = fi.LastAccessTime;
                    s += YearDiff(Recent, T);
                }
                else
                {
                    T = fi.LastWriteTime;
                    s += YearDiff(Recent, T);
                }
            }
            return (s / n);
        }
        static public int Mean2YFTime(List<FileInfo> fis, DateTime Recent, string Temps = "LastWriteTime")
        {
            int s = 0, n = fis.Count;
            DateTime T;
            foreach (var fi in fis)
            {
                if (Temps == "CreationTime")
                {
                    T = fi.CreationTime;
                    s += YearDiff(Recent, T) * YearDiff(Recent, T);
                }
                else if (Temps == "LastAccessTime")
                {
                    T = fi.LastAccessTime;
                    s += YearDiff(Recent, T) * YearDiff(Recent, T);
                }
                else
                {
                    T = fi.LastWriteTime;
                    s += YearDiff(Recent, T) * YearDiff(Recent, T);
                }
            }
            return (s / n);
        }
        static public int nfiles(List<FileInfo> Fis)
        {
            return Fis.Count;
        }
        static public int ndir(List<DirectoryInfo> Fis)
        {
            return Fis.Count;
        }
        static public FileExt[] filesExtRvar(List<FileInfo> fis, FileExt[] Rvar)
        {
            foreach (FileInfo fi in fis)
            {
                string fname = fi.FullName;
                long size = fi.Length;
                string fex = Path.GetExtension(fname);
                int i = 0;
                while ((i < Rvar.Length) && (fex != Rvar[i].format)) i++;
                if (i < Rvar.Length)
                {
                    Rvar[i].Number = Rvar[i].Number + size;
                    Rvar[i].NFiles = Rvar[i].NFiles + 1;
                }
            }
            int ii = 0;
            while (ii < Rvar.Length)
            {
                Rvar[ii].Number = Rvar[ii].Number / 1024;
                ii++;
            }
            return Rvar;
        }
        //static public FileExt[] filesExt(List<FileInfo> fis)
        //{
        //    float[] exts = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //    foreach (FileInfo fi in fis)
        //    {
        //        string fname = fi.FullName;
        //        long size = fi.Length;
        //        string fex = Path.GetExtension(fname);
        //        if (fex == ".xls")
        //            exts[0] = exts[0] + size;
        //        else if (fex == ".xlsx")
        //            exts[1] = exts[1] + size;
        //        else if (fex == ".sas")
        //            exts[2] = exts[2] + size;
        //        else if (fex == ".rtf")
        //            exts[3] = exts[3] + size;
        //        else if (fex == ".doc")
        //            exts[4] = exts[4] + size;
        //        else if (fex == ".docx")
        //            exts[5] = exts[5] + size;
        //        else if (fex == ".txt")
        //            exts[6] = exts[6] + size;
        //        else if (fex == ".accdb")
        //            exts[7] = exts[7] + size;
        //        else if (fex == ".mdb")
        //            exts[8] = exts[8] + size;
        //        else if (fex == ".pdf")
        //            exts[9] = exts[9] + size;
        //    }
        //    FileExt[] FE = new FileExt[10];
        //    FE[0] = new FileExt(".xls", exts[0] / 1024);
        //    FE[1] = new FileExt(".xlsx", exts[1] / 1024);
        //    FE[2] = new FileExt(".sas", exts[2] / 1024);
        //    FE[3] = new FileExt(".rtf", exts[3] / 1024);
        //    FE[4] = new FileExt(".doc", exts[4] / 1024);
        //    FE[5] = new FileExt(".docx", exts[5] / 1024);
        //    FE[6] = new FileExt(".txt", exts[6] / 1024);
        //    FE[7] = new FileExt(".accdb", exts[7] / 1024);
        //    FE[8] = new FileExt(".mdb", exts[8] / 1024);
        //    FE[9] = new FileExt(".pdf", exts[9] / 1024);

        //    //FEL = FE.ToList();
        //    return FE;
        //}

        static public HashSet<string> AllfileExtensions(List<FileInfo> fis)
        {
            HashSet<string> myext = new HashSet<string>();  //extensiones presentes
            foreach (FileInfo fi in fis)
            {
                string fname = fi.FullName;
                string fex = Path.GetExtension(fname);
                myext.Add(fex);
            }
            return myext;
        }
        //compute total size for each of the extensions with larger size
        static public FileExt[] filespExt(List<FileInfo> fis, HashSet<string> myext)
        {
            FileExt[] FE = new FileExt[fis.Count];
            int[] exts = new int[myext.Count];
            int i = 0;
            foreach (string e in myext)
            {
                FE[i] = new FileExt(e, 0, 0);
                i++;
            }
            foreach (FileInfo fi in fis)  //computing size for each extension
            {
                string fname = fi.FullName;
                float size = fi.Length;
                string fex = Path.GetExtension(fname);
                i = 0;
                while (fex != FE[i].format) i++;
                if (i < myext.Count)
                {
                    FE[i].Number = FE[i].Number + size;
                    FE[i].NFiles = FE[i].NFiles + 1;
                }
            }
            var FEX = FE.OrderByDescending(x => x.Number).ToArray(); //sorting total size per extension decreasing order
            i = 0;
            foreach (string e in myext)
            {
                FEX[i].Number = (float)FEX[i].Number / (float)1024;
                i++;
            }
            return FEX;
        }
        static public void FilesDirPrint(List<FileInfo> fis)
        {
            Console.Write("Name                     "); Console.Write("\t"); Console.Write("Size  "); Console.Write("\t"); Console.Write("Ext "); Console.Write("\t"); Console.Write("CreationTime      "); Console.Write("\t"); Console.Write("LastWriteTime      "); Console.Write("\t"); Console.Write("LastAccessTime      ");
            Console.WriteLine();
            foreach (var fi in fis)
            {
                string fname = fi.Name;
                string ssname;
                int L = fname.Length;
                if (L > 25)
                    ssname = fname.Substring(0, 25);
                else
                {
                    string ss = "";
                    for (int i = 0; i < (25 - L); i++)
                        ss += " ";
                    ssname = fname + ss;
                }
                string fn = fi.FullName;
                string fex = Path.GetExtension(fn);
                Console.Write(ssname);
                Console.Write("\t"); Console.Write(fi.Length); Console.Write("\t"); Console.Write(fex); Console.Write("\t"); Console.Write(fi.CreationTime); Console.Write("\t"); Console.Write(fi.LastWriteTime); Console.Write("\t"); Console.Write(fi.LastAccessTime);
                Console.WriteLine();
            }
        }
    }
}
