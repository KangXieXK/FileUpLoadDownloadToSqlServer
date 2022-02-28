using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public class FileCheck : IMessageBussiness
    {
        public IMessageModel Work(IMessageModel messageModel)
        {
            var msg = messageModel.GetContentChange<FileCheckQuest>();
            if (msg == null)
            {
                messageModel.MessageErrorResponse();
            }
            else
            {
                if (msg.quest == 0)
                {
                    if (!msg.ServerPathAbs)
                    {
                        msg.ServerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, msg.ServerPath);
                    }

                    var old = this.CheckBaseFolder(msg.ServerPath);
                    var result = this.Compare(msg.listfile, old);
                    if (result?.Count > 0)
                    {
                        var deletelist = result.FindAll(i => i.result == 2 || i.result == 3);
                        var uploadlist = result.FindAll(i => i.result == 1 || i.result == 2);
                        foreach (var item in deletelist)
                        {
                            this.DeleteFile(item.fileInfoxk, msg.ServerPath);
                        }
                        if (uploadlist?.Count > 0)
                        {
                            FileCheckQuest responsemsg = new FileCheckQuest()
                            {
                                ClientPath = msg.ClientPath,
                                ClientPathAbs = msg.ClientPathAbs,
                                ServerPath = msg.ServerPath,
                                ServerPathAbs = msg.ServerPathAbs,
                                listfile = uploadlist.Select(i => i.fileInfoxk).ToList(),
                                quest = msg.quest + 1
                            };
                            messageModel.SetMessageResponse(responsemsg);
                        }
                    }
                }
            }
            return messageModel;
        }

        public string GetKey()
        {
            return "Check";
        }
        public Encoding Codeing = Encoding.UTF8;
        public List<FileInfoxk> CheckBaseFolder(string path)
        {
            List<FileInfoxk> fileModes = new List<FileInfoxk>();
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
                    fileModes.AddRange(CheckChildFolder(SecondChild, ProgramName, VersionName, ""));
                }
            }
            return fileModes;
        }

        public List<FileInfoxk> CheckChildFolder(DirectoryInfo SecondChild, string Program, string Version, string ParentDir)
        {
            List<FileInfoxk> fileModes = new List<FileInfoxk>();
            DirectoryInfo[] dichild = SecondChild.GetDirectories();
            FileInfo[] fileInfos = SecondChild.GetFiles();
            foreach (FileInfo item in fileInfos)
            {
                string str;
                byte[] btye = this.GetFileBytes(item.FullName, out str);
                string strname = Path.Combine(ParentDir, item.Name);
                fileModes.Add(GetfileMode(strname, str, Program, Version, item.FullName));
                Console.WriteLine("检测" + Program + "----版本" + Version + "文件" + strname + "上传完成");
            }
            foreach (DirectoryInfo item in dichild)
            {
                CheckChildFolder(item, Program, Version, Path.Combine(ParentDir, item.Name));
            }
            return fileModes;
        }

        public byte[] GetFileBytes(string SingleFileName, out string md5)
        {
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

        public FileInfoxk GetfileMode(string fileName, string strmd5, string ProgramName, string Version, string fullName, byte[] bFile)
        {
            FileInfoxk model = new FileInfoxk()
            {
                FILENAME = fileName,
                IMG = bFile,
                MD5 = strmd5,
                ProgramName = ProgramName,
                VERSION = Version,

            };
            return model;
        }

        public FileInfoxk GetfileMode(string fileName, string strmd5, string ProgramName, string Version, string fullName)
        {
            FileInfoxk model = new FileInfoxk()
            {
                FILENAME = fileName,
                IMG = null,
                MD5 = strmd5,
                ProgramName = ProgramName,
                VERSION = Version,
            };
            return model;
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="fn">新文件信息列表</param>
        /// <param name="fo">原始文件信息列表</param>
        /// <returns></returns>
        public List<FileInfoCompareResult> Compare(List<FileInfoxk> fn, List<FileInfoxk> fo)
        {
            List<FileInfoCompareResult> resultlist = new List<FileInfoCompareResult>();
            foreach (var itemn in fn)
            {
                if (!fo.Exists(i => i.FILENAME == itemn.FILENAME))
                {
                    resultlist.Add(new FileInfoCompareResult() { fileInfoxk = itemn, result = 1 });
                }
                else
                {
                    if (fo.Find(i => i.FILENAME == itemn.FILENAME).MD5 != itemn.MD5)
                    {
                        resultlist.Add(new FileInfoCompareResult() { fileInfoxk = itemn, result = 2 });
                    }
                    else
                    {
                        resultlist.Add(new FileInfoCompareResult() { fileInfoxk = itemn, result = 0 });
                    }
                }
            }
            foreach (var itemn in fo)
            {
                if (!fn.Exists(i => i.FILENAME == itemn.FILENAME))
                {
                    resultlist.Add(new FileInfoCompareResult() { fileInfoxk = itemn, result = 3 });
                }
            }
            return resultlist;

        }


        public void DeleteFile(FileInfoxk info, String LocalPath)
        {
            string filePath = Path.Combine(LocalPath, info.ProgramName, info.VERSION, info.FILENAME);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public void CreateFile(FileInfoxk fileinfo, String LocalPath)
        {
            string filePath = Path.Combine(LocalPath, fileinfo.ProgramName, fileinfo.VERSION, fileinfo.FILENAME);
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
                    bw.Write(fileinfo.IMG, 0, fileinfo.IMG.Length);
                    bw.Close();
                    fs.Close();
                }
            }
        }


        public string UploadFileListFirstStep(string path)
        {
            var fc = new FileCheck();
            var result = fc.CheckBaseFolder(path);
            string resultstr = string.Empty;
            if (result?.Count > 0)
            {
                MessageJsonModel mjm = new MessageJsonModel()
                {
                    BussinessID = 1,
                    BussinessResult = 0,
                    Content = result,
                };
                resultstr = Newtonsoft.Json.JsonConvert.SerializeObject(mjm);
            }
            return resultstr;
        }


        public List<FileInfoxk> GetUploadFile(object obj, string FileStorePath)
        {
            List<FileInfoxk> mjm = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileInfoxk>>(obj.ToString());
            List<FileInfoxk> temp = new List<FileInfoxk>();
            if (mjm?.Count > 0)
            {
                var fc = new FileCheck();
                var old = fc.CheckBaseFolder(FileStorePath);
                var result = fc.Compare(mjm, old);
                if (result?.Count > 0)
                {
                    var deletelist = result.FindAll(i => i.result == 2 || i.result == 3);
                    var uploadlist = result.FindAll(i => i.result == 1 || i.result == 2);
                    foreach (var item in deletelist)
                    {
                        fc.DeleteFile(item.fileInfoxk, FileStorePath);
                    }
                    temp.AddRange(uploadlist.Select(i => i.fileInfoxk));
                }
            }
            return temp;
        }

        private void WsClient_OnMessage()
        {
            string str = string.Empty;
            MessageJsonModel mjm = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageJsonModel>(str);
            if (mjm != null)
            {
                switch (mjm.BussinessID)
                {
                    case 1:
                    //GetUploadFile(mjm.Content); break;
                    default: break;
                }
            }
        }



        private void WsServer_OnMessage(string id, byte[] data)
        {
            string str = Codeing.GetString(data);
            MessageJsonModel mjm = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageJsonModel>(str);

            if (mjm != null)
            {

                switch (mjm.BussinessID)
                {
                    case 1:
                        {
                            //var result = GetUploadFile(mjm.Content);
                            //MessageJsonModel mjm2 = new MessageJsonModel()
                            //{
                            //    BussinessID = 2,
                            //    BussinessResult = 0,
                            //    Content = result,
                            //};
                            //string mjm2str = Newtonsoft.Json.JsonConvert.SerializeObject(mjm2);
                            //byte[] byteArray = Codeing.GetBytes(mjm2str);

                        }; break;
                    default: break;
                }
            }
        }


    }
}
