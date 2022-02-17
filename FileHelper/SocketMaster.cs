using SAEA.FileSocket;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;

namespace FileHelper
{
    public class SocketCenter
    {
        public FileTransfer fileTransfer { get; set; }

        public string FileStorePath { get; set; }

        public Encoding Codeing = Encoding.UTF8;
        public void StartReceive(string path)
        {
            fileTransfer?.Dispose();
            fileTransfer = new FileTransfer(path);
            //fileTransfer.OnReceiveEnd += fileTransfer_OnReceiveEnd;

            //fileTransfer.OnDisplay += fileTransfer_OnDisplay;
            fileTransfer.Start();
        }

        public void StartWsserver(int port)
        {
            //wsServer = new WSServer(port, System.Security.Authentication.SslProtocols.None, "", "", 1024);
            //wsServer.OnMessage += WsServer_OnMessage;
            //wsServer.Start();
            SuperSocket.WebSocket.WebSocketServer webSocketServer = new SuperSocket.WebSocket.WebSocketServer();
            webSocketServer.
            webSocketServer.Start();
        }
        


        public void UploadFileListFirstStep(string path, string ip, int port)
        {
            var fc = new FileCheck();
            var result = fc.CheckBaseFolder(path);
            if (result?.Count > 0)
            {
                MessageJsonModel mjm = new MessageJsonModel()
                {
                    BussinessID = 1,
                    BussinessResult = 0,
                    Content = result,
                };
                string resultstr = Newtonsoft.Json.JsonConvert.SerializeObject(mjm);

            }
        }
        public List<FileInfoxk> GetUploadFile(object obj)
        {
            List<FileInfoxk> mjm = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileInfoxk>>(obj.ToString());
            List<FileInfoxk> temp = new List<FileInfoxk>();
            if (mjm?.Count > 0)
            {
                var fc = new FileCheck();
                var old = fc.CheckBaseFolder(this.FileStorePath);
                var result = fc.Compare(mjm, old);
                if (result?.Count > 0)
                {
                    var deletelist = result.FindAll(i => i.result == 2 || i.result == 3);
                    var uploadlist = result.FindAll(i => i.result == 1 || i.result == 2);
                    foreach (var item in deletelist)
                    {
                        fc.DeleteFile(item.fileInfoxk, this.FileStorePath);
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
                        GetUploadFile(mjm.Content); break;
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
                            var result = GetUploadFile(mjm.Content);
                            MessageJsonModel mjm2 = new MessageJsonModel()
                            {
                                BussinessID = 2,
                                BussinessResult = 0,
                                Content = result,
                            };
                            string mjm2str = Newtonsoft.Json.JsonConvert.SerializeObject(mjm2);
                            byte[] byteArray = Codeing.GetBytes(mjm2str);

                        }; break;
                    default: break;
                }
            }
        }


    }
}
