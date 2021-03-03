using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProfiler
{
    public class DB
    {
        public DB(string path, string b = "files")
        {
            DirectoryInfo p = new DirectoryInfo(path);
            DirectoryInfo[] diArr = p.GetDirectories("*", SearchOption.AllDirectories);
            FileInfo[] fiArr = p.GetFiles("*", SearchOption.AllDirectories);

            DName = p.Name;
            PDName = p.FullName;
            CTime = p.CreationTime;
            LWTime = p.LastWriteTime;
            LATime = p.LastAccessTime;
            OldestDLWTime = DateTime.Now;
            OldestDCTTime = DateTime.Now;
            OldestDLATime = DateTime.Now;

            OldestFCTTime = DateTime.Now;
            OldestFLWTime = DateTime.Now;
            OldestFLATime = DateTime.Now;

            if (((b == "files") || (b == "All")) && (fiArr.Length > 0))
            {
                OldestFLWTime = GetOldestFile(fiArr, "lastWriteTime");
                OldestFCTTime = GetOldestFile(fiArr, "CreationTime");
                OldestFLATime = GetOldestFile(fiArr, "lastAccessTime");
            }
            if ((b == "All") && (diArr.Length > 0))
            {
                OldestDLWTime = GetOldestSubDir(diArr, "lastWriteTime");
                OldestDCTTime = GetOldestSubDir(diArr, "CreationTime");
                OldestDLATime = GetOldestSubDir(diArr, "lastAccessTime");
            }
            DSize = -1;
            DFileNumber = -1;
            DSubDirNumber = -1;
        }
        public string PDName { get; }
        public string DName { get; }
        public DateTime CTime { get; }  //temps folder
        public DateTime LWTime { get; }
        public DateTime LATime { get; }
        public DateTime OldestFLWTime { get; } //temps oldest file
        public DateTime OldestFCTTime { get; }
        public DateTime OldestFLATime { get; }
        public DateTime OldestDLWTime { get; set; } //temps oldest sub-folder
        public DateTime OldestDCTTime { get; set; }
        public DateTime OldestDLATime { get; set; }
        public float DSize { get; set; }
        public int DFileNumber { get; set; }
        public int DSubDirNumber { get; set; }
        public int AgeY { get; set; }
        public float AvgAgeY { get; set; }
        public float StdAgeY { get; set; }
        //public float KBxls { get; set; }
        //public float KBxlsx { get; set; }

        private List<FileInfo> Newest2Oldest_Ct(FileInfo[] diArr)
        {
            List<FileInfo> di = new List<FileInfo>();
            List<FileInfo> diout = new List<FileInfo>();
            di = diArr.ToList();
            diout = di.OrderBy(f => f.CreationTime).ToList();
            return diout;
        }
        private List<FileInfo> Newest2Oldest_Wt(FileInfo[] diArr)
        {
            List<FileInfo> di = new List<FileInfo>();
            List<FileInfo> diout = new List<FileInfo>();
            di = diArr.ToList();
            diout = di.OrderBy(f => f.LastWriteTime).ToList();
            return diout;
        }
        private List<FileInfo> Newest2Oldest_LAT(FileInfo[] diArr)
        {
            List<FileInfo> di = new List<FileInfo>();
            List<FileInfo> diout = new List<FileInfo>();
            di = diArr.ToList();
            diout = di.OrderBy(f => f.LastAccessTime).ToList();
            return diout;
            //return new DirectoryInfo(location).GetFiles("*", SearchOption.AllDirectories)
            //                                  .OrderByDescending(f => f.LastAccessTime)
            //                                  .ToList();
        }
        private List<DirectoryInfo> Newest2OldestCTDIR(DirectoryInfo[] diArr)
        {
            List<DirectoryInfo> di = new List<DirectoryInfo>();
            List<DirectoryInfo> diout = new List<DirectoryInfo>();
            di = diArr.ToList();
            diout = di.OrderBy(f => f.CreationTime).ToList();
            return diout;
        }
        private List<DirectoryInfo> Newest2OldestLWTDIR(DirectoryInfo[] diArr)
        {
            List<DirectoryInfo> di = new List<DirectoryInfo>();
            List<DirectoryInfo> diout = new List<DirectoryInfo>();
            di = diArr.ToList();
            diout = di.OrderBy(f => f.LastWriteTime).ToList();
            return diout;
        }
        private List<DirectoryInfo> Newest2OldestLATDIR(DirectoryInfo[] diArr)
        {
            List<DirectoryInfo> di = new List<DirectoryInfo>();
            List<DirectoryInfo> diout = new List<DirectoryInfo>();
            di = diArr.ToList();
            diout = di.OrderBy(f => f.LastAccessTime).ToList();
            return diout;
            //return new DirectoryInfo(location).GetDirectories("*", SearchOption.AllDirectories)
            //                                  .OrderByDescending(f => f.LastAccessTime)
            //                                  .ToList();
        }
        public DateTime GetOldestFile(FileInfo[] Af, string temps)
        {
            List<FileInfo> Lfiles;
            FileInfo Oldest;
            if (temps == "lastWriteTime")
            {
                Lfiles = Newest2Oldest_Wt(Af);
                Oldest = Lfiles.First();
                return Oldest.LastWriteTime;
            }
            else if (temps == "lastAccessTime")
            {
                Lfiles = Newest2Oldest_LAT(Af);
                Oldest = Lfiles.First();
                return Oldest.LastAccessTime;
            }
            else
            {
                Lfiles = Newest2Oldest_Ct(Af);
                Oldest = Lfiles.First();
            }
            return Oldest.CreationTime;
        }
        public DateTime GetOldestSubDir(DirectoryInfo[] ASubDir, string temps)
        {
            List<DirectoryInfo> LSubDir;
            DirectoryInfo Oldest;
            if (temps == "lastWriteTime")
            {
                LSubDir = Newest2OldestLWTDIR(ASubDir);
                Oldest = LSubDir.First();
                return Oldest.LastWriteTime;
            }
            else if (temps == "lastAccessTime")
            {
                LSubDir = Newest2OldestLATDIR(ASubDir);
                Oldest = LSubDir.First();
                return Oldest.LastAccessTime;
            }
            else
            {
                LSubDir = Newest2OldestCTDIR(ASubDir);
                Oldest = LSubDir.First();
            }
            return Oldest.CreationTime;

        }
    }
}
