using DataModels;
using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.DataProvider.SqlServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileUpdate
{
    public class FileUpdateIns
    {
        public string ProgramProcessName { get; set; }

        public string FilePath { get; set; }


        public string ProgramVersion { get; set; }

        public string UpLoadPath { get; set; }
        public string ConnectionString { get; set; }


        LinqToDbConnectionOptions opt;


        public FileUpdateIns(string db)
        {
            this.ConnectionString = db;
            InitDbFactory();
        }
        /// <summary>
        /// 初始化数据库操作对象
        /// </summary>
        private void InitDbFactory()
        {
            LinqToDbConnectionOptionsBuilder builder = new LinqToDbConnectionOptionsBuilder();
            builder.UseSqlServer(ConnectionString);
            opt = builder.Build();
            SqlServerTools.Provider = SqlServerProvider.MicrosoftDataSqlClient;
        }


        public void UpLoadBaseFolder(string path)
        {
            Console.WriteLine("开始检测文件夹");
            DirectoryInfo di = new DirectoryInfo(path);
            Console.WriteLine("检测文件夹结束" + di?.FullName);
            DirectoryInfo[] difirstchild = di.GetDirectories();
            Console.WriteLine("检测程序文件结束-共" + (difirstchild == null ? "0" : difirstchild.Length) + "程序");
            foreach (DirectoryInfo firstChild in difirstchild)
            {
                string ProgramName = firstChild.Name;
                Console.WriteLine("检测" + ProgramName + "程序开始");
                DirectoryInfo[] diSecondchild = firstChild.GetDirectories();
                Console.WriteLine("检测" + ProgramName + "----版本数" + (diSecondchild == null ? "0" : diSecondchild.Length));
                foreach (DirectoryInfo SecondChild in diSecondchild)
                {
                    string VersionName = SecondChild.Name;
                    Console.WriteLine("上传" + ProgramName + "的" + VersionName + "版本开始");
                    UpLoadChildFolder(SecondChild, ProgramName, VersionName, "");
                }
            }
        }

        public void UpLoadChildFolder(DirectoryInfo SecondChild, string Program, string Version, string ParentDir)
        {
            DirectoryInfo[] dichild = SecondChild.GetDirectories();
            FileInfo[] fileInfos = SecondChild.GetFiles();
            foreach (FileInfo item in fileInfos)
            {
                string str;
                byte[] btye = this.GetFileBytes(item.FullName, out str);
                string strname = Path.Combine(ParentDir, item.Name);
                Insert(strname, btye, str, Program, Version);
                Console.WriteLine("检测" + Program + "----版本" + Version + "文件" + strname + "上传完成");
            }
            foreach (DirectoryInfo item in dichild)
            {
                UpLoadChildFolder(item, Program, Version, Path.Combine(ParentDir, item.Name));
            }
        }

        public byte[] GetFileBytes(string SingleFileName, out string md5)
        {
            InitDbFactory();
            byte[] bFile;
            md5 = "";
            try
            {
                using (FileStream fs = new FileStream(SingleFileName, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        bFile = new byte[fs.Length];
                        fs.Read(bFile, 0, (int)fs.Length);
                        MD5 mds5 = new MD5CryptoServiceProvider();
                        byte[] retVal = mds5.ComputeHash(bFile);
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < retVal.Length; i++)
                        {
                            sb.Append(retVal[i].ToString("x2"));
                        }
                        md5 = sb.ToString();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return bFile;

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return null;
        }

        public void Insert(string fileName, byte[] bFile, string strmd5, string ProgramName, string Version)
        {
            using (var db = new DataModels.TestForUploadDB(opt))
            {
                var queryExistSame = from p in db.SysFileversions
                                     where p.VERSION == Version && p.ProgramName == ProgramName && p.FILENAME == fileName && p.MD5 == strmd5
                                     select p;
                if (queryExistSame.Any())
                {
                    return;
                }
                SysFileversion sysFileversions = new SysFileversion()
                {
                    FILENAME = fileName,
                    MD5 = strmd5,
                    IMG = bFile,
                    UploadTime = DateTime.Now,
                    ProgramName = ProgramName,
                    VERSION = Version
                };

                var query = from p in db.SysFileversions
                            where p.VERSION == Version && p.ProgramName == ProgramName && p.FILENAME == fileName
                            select p;
                if (query.Any())
                {
                    db.SysFileversions.Where(p => p.VERSION == Version && p.ProgramName == ProgramName && p.FILENAME == fileName)
                        .Set(p => p.MD5, strmd5).Set(p => p.IMG, bFile).Update();
                }
                else
                {
                    db.Insert<SysFileversion>(sysFileversions);
                }

                //db.CaVoiceTts.Where(p => p.STATE == 1).Set(p => p.STATE, 0).Update();///测试用
            }


        }

        public bool CheckMd5(string path, string Md5)
        {

            try
            {
                var md5 = GetMD5HashFromFile(path);
                if (md5 == Md5)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }


        public List<string> GetCanDownLoadProgram()
        {
            using (var db = new DataModels.TestForUploadDB(opt))
            {
                var query = (from p in db.SysFileversions
                             select p.ProgramName).Distinct();
                var list = query.ToList();
                return list;
            }
        }

        public List<string> GetCanDownLoadProgramVersion(string ProgramName)
        {
            if (string.IsNullOrEmpty(ProgramName))
            {
                return null;
            }
            using (var db = new DataModels.TestForUploadDB(opt))
            {
                var query = (from p in db.SysFileversions
                             where p.ProgramName == ProgramName
                             orderby p.UploadTime descending
                             select p.VERSION).Distinct();
                var list = query.ToList();
                return list;
            }
        }

        public void DownLoadFolder(string path, string Program, string Version)
        {
            List<SysFileversion> list;
            using (var db = new DataModels.TestForUploadDB(opt))
            {
                var query = from p in db.SysFileversions
                            where p.VERSION == Version && p.ProgramName == Program
                            select new SysFileversion { FILENAME = p.FILENAME, MD5 = p.MD5 };
                list = query.ToList();
            }
            Console.WriteLine("正在检测文件完整性");
            List<SysFileversion> NeedList = new List<SysFileversion>();
            if (list != null)
            {
                foreach (SysFileversion item in list)
                {
                    string fullName = Path.Combine(path, item.FILENAME);
                    if (!File.Exists(fullName))
                    {
                        NeedList.Add(item);
                    }
                    else
                    {
                        if (!CheckMd5(fullName, item.MD5))
                        {
                            NeedList.Add(item);
                        }
                    }
                }
            }
            Console.WriteLine("需要更新文件数目" + NeedList.Count);
            int count = 1;
            using (var db = new DataModels.TestForUploadDB(opt))
            {
                var q = from p in db.SysFileversions
                        where p.VERSION == Version && p.ProgramName == Program && NeedList.Select(a => a.FILENAME).Contains(p.FILENAME)
                        select p;
                NeedList = q.ToList();
            }
            foreach (SysFileversion item in NeedList)
            {
                Console.WriteLine("正在更新文件" + count++ + "/" + NeedList.Count);
                if (ActionForCount != null)
                {
                    Task.Run(() => ActionForCount.Invoke(count, NeedList.Count));
                }
                string stringfile = Path.Combine(path, item.FILENAME);
                GetFile(item.IMG, stringfile);
            }
        }
        public Action<int, int> ActionForCount;

        public void DownLoadFolder(string path, string Program)
        {
            List<string> vs = GetCanDownLoadProgramVersion(Program);
            if (vs?.Count > 0)
            {
                DownLoadFolder(path, Program, vs.First());
            }
        }
        public string GetMD5HashFromFile(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        var bFile = new byte[fs.Length];
                        fs.Read(bFile, 0, (int)fs.Length);
                        MD5 mds5 = new MD5CryptoServiceProvider();
                        byte[] retVal = mds5.ComputeHash(bFile);
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < retVal.Length; i++)
                        {
                            sb.Append(retVal[i].ToString("x2"));
                        }
                        return sb.ToString();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        public static void GetFile(byte[] bfile, String filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (FileStream fs = new FileStream(filePath, FileMode.CreateNew))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(bfile, 0, bfile.Length);
                    bw.Close();
                    fs.Close();
                }
            }
        }

        //下载文件
    }
}
