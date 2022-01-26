using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileUpdateFrameWork
{
    public class FileUpdateIns
    {
        public string ProgramProcessName { get; set; }

        public string FilePath { get; set; }
        public string ProgramVersion { get; set; }

        public string UpLoadPath { get; set; }
        public string ConnectionString { get; set; }

        public FileUpdateIns(string db)
        {
            this.ConnectionString = db;
            InitDbFactory();
        }

        SqlSugarClient db;
        /// <summary>
        /// 初始化数据库操作对象
        /// </summary>
        private void InitDbFactory()
        {
            db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = this.ConnectionString,
                DbType = DbType.SqlServer,//设置数据库类型
                IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            });
        }


        public void UpLoadBaseFolder(string path)
        {
            Console.WriteLine("开始检测文件夹");
            DirectoryInfo di = new DirectoryInfo(path);
            Console.WriteLine("检测文件夹结束" + di?.FullName);
            DirectoryInfo[] difirstchild = di.GetDirectories();
            Console.WriteLine("检测程序文件结束-共" + (difirstchild == null ? "0" : difirstchild.Length.ToString()) + "程序");
            foreach (DirectoryInfo firstChild in difirstchild)
            {
                string ProgramName = firstChild.Name;
                Console.WriteLine("检测" + ProgramName + "程序开始");
                DirectoryInfo[] diSecondchild = firstChild.GetDirectories();
                Console.WriteLine("检测" + ProgramName + "----版本数" + (diSecondchild == null ? "0" : diSecondchild.Length.ToString()));
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
            var queryExistSame = db.Queryable<SYS_FILEVERSION>().Where(p => p.VERSION == Version && p.PROGRAM_NAME == ProgramName && p.FILENAME == fileName && p.MD5 == strmd5);
            if (queryExistSame.Any())
            {
                return;
            }
            SYS_FILEVERSION sysFileversions = new SYS_FILEVERSION()
            {
                FILENAME = fileName,
                MD5 = strmd5,
                IMG = bFile,
                UPLOAD_TIME = DateTime.Now,
                PROGRAM_NAME = ProgramName,
                VERSION = Version
            };
            var queryExist = db.Queryable<SYS_FILEVERSION>().Where(p => p.VERSION == Version && p.PROGRAM_NAME == ProgramName && p.FILENAME == fileName);
            if (queryExist.Any())
            {
                db.Updateable<SYS_FILEVERSION>().UpdateColumns(it => new SYS_FILEVERSION() { MD5 = strmd5, IMG = bFile }).Where(p => p.VERSION == Version && p.PROGRAM_NAME == ProgramName && p.FILENAME == fileName).ExecuteCommand();
            }
            else
            {
                var t2 = db.Insertable(sysFileversions).ExecuteCommand();
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
            var group = db.Queryable<SYS_FILEVERSION>().GroupBy(it => it.PROGRAM_NAME).Select(i => i.PROGRAM_NAME).ToList();
            return group.ToList();
        }

        public List<string> GetCanDownLoadProgramVersion(string ProgramName)
        {
            if (string.IsNullOrEmpty(ProgramName))
            {
                return null;
            }
            return db.Queryable<SYS_FILEVERSION>().GroupBy(it => it.VERSION)
                .Select(i => i.VERSION).ToList();
        }

        public void DownLoadFolder(string path, string Program, string Version)
        {
            List<SYS_FILEVERSION> NeedList = CheckNeedList(path, Program, Version);
            NeedList = StartDownload(path, Program, Version, NeedList);
        }

        public List<SYS_FILEVERSION> CheckNeedList(string path, string Program, string Version)
        {
            List<SYS_FILEVERSION> list = db.Queryable<SYS_FILEVERSION>().Where(it => it.PROGRAM_NAME == Program && it.VERSION == Version).
                Select(i => new SYS_FILEVERSION { FILENAME = i.FILENAME, MD5 = i.MD5 }).ToList();
            ActionMessage("正在检测文件完整性");
            List<SYS_FILEVERSION> NeedList = new List<SYS_FILEVERSION>();
            if (list != null)
            {
                int ccount = 0;
                foreach (SYS_FILEVERSION item in list)
                {
                    string fullName = Path.Combine(path, Program, Version, item.FILENAME);
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
                    ActionMessage("正在检测文件完整性" + ccount++ + "/" + list.Count);
                    ActionCount(list.Count, ccount);
                }
            }

            return NeedList;
        }

        public List<SYS_FILEVERSION> StartDownload(string path, string Program, string Version, List<SYS_FILEVERSION> NeedList)
        {
            Console.WriteLine("需要更新文件数目" + NeedList.Count);
            int count = 0;
            string[] Namelist = NeedList.Select(a => a.FILENAME).ToArray();
            foreach (SYS_FILEVERSION item in NeedList)
            {
                var thisitem = db.Queryable<SYS_FILEVERSION>().Where(it => it.VERSION == Version && it.PROGRAM_NAME == Program && it.FILENAME == item.FILENAME).First();
                ActionMessage("正在更新文件" + count++ + "/" + NeedList.Count);
                ActionCount(NeedList.Count, count);
                string stringfile = Path.Combine(path, item.FILENAME);
                GetFile(thisitem.IMG, stringfile);
            }
            ActionFinish();
            return NeedList;
        }

        private void ActionCount(int all, int now)
        {
            if (ActionForCount != null)
            {
                Task.Factory.StartNew(() => ActionForCount.Invoke(now, all));
            }
        }

        private void ActionMessage(string str)
        {
            if (ActionForMes != null)
            {
                Task.Factory.StartNew(() => ActionForMes.Invoke(str));
            }
        }

        private void ActionFinish()
        {
            if (ActionForFinish != null)
            {
                Task.Factory.StartNew(() => ActionForFinish.Invoke("更新结束"));
            }
        }

        public Action<int, int> ActionForCount;
        public Action<string> ActionForMes;
        public Action<string> ActionForFinish;

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
    }

    //下载文件
}
