using SAEA.FileSocket;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using log4net;
using log4net.Config;
using log4net.Repository;
using Autofac;
using SuperSocket.WebSocket;
using System.IO;

namespace FileHelper
{
    public class SocketCenter
    {
        public FileTransfer fileTransfer { get; set; }

        public string FileStorePath { get; set; }

        public WsServer Server { get; set; }

        public WsClient Client { get; set; }

        ContainerBuilder builder = new ContainerBuilder();
        IContainer container { get; set; }

        public log4net.ILog log { get; set; }
        public async void StartWsserver(int port)
        {
            builder.RegisterType<Aes>().As<ICrypt>();
            builder.RegisterType<MessageJsonModel>().As<IMessageModel>().As<IMessage>();
            builder.RegisterType<FileCheck>().Named<IMessageBussiness>("FileCheck");
            builder.RegisterType<FileLog>().As<IMessageLog>();
            container = builder.Build();



            Server = new WsServer();
            Server.MessageAction += new Action<SuperSocket.WebSocket.Server.WebSocketSession,
                SuperSocket.WebSocket.WebSocketPackage>((session, msg) =>
                {
                    using (var scope = container.BeginLifetimeScope())
                    {
                        var result = MsgWork(msg.Message, scope);
                        if (result != null)
                        {
                            session.SendAsync(result.Jsonal());
                        }
                    }
                });
            await Server.Start();
        }

        private IMessageModel MsgWork(string msg, ILifetimeScope scope)
        {
            var imm = scope.Resolve<IMessageModel>();
            var result = imm.Objectal(msg);
            var logs = scope.Resolve<IMessageLog>();
            logs.InitLog(this.log);
            logs.InitSocket(this);
            try
            {
                if (result != null)
                {
                    logs.RecieveMessage(imm);
                    result.DecryptSelf(container.Resolve<ICrypt>());
                    var bussiness = scope.ResolveNamed<IMessageBussiness>(result.Key);
                    if (bussiness != null)
                    {
                        var workResponse = bussiness.Work(result);
                        logs.ResponseMessage(workResponse);
                        return workResponse;
                    }
                    else
                    {
                        result.SetMessageResponse("收到消息体解密失败的消息");
                        logs.LogInfo(result);
                    }
                }
                else
                {
                    result.SetMessageResponse("收到消息体为空的消息");
                    logs.LogInfo(result);
                }

            }
            catch (Exception ex)
            {
                result.SetMessageResponse(ex.Message + ex.StackTrace);
                logs.LogInfo(result);
            }
            return result;
        }

        public void Send(IMessageModel mjm, string ip, int port)
        {
            if (Client == null)
            {
                Client = new WsClient();
                Client.CreateClient(ip, port);
                Client.client.MessageReceived.Subscribe(msg =>
                {
                    using (var scope = container.BeginLifetimeScope())
                    {
                        //var result = MsgWork(msg.Text, scope);
                        //if (result != null)
                        //{
                        //    //Client.Send(result.Jsonal());
                        //}
                    }

                });
            }
            using (var scope = container.BeginLifetimeScope())
            {
                mjm.EncryptSelf(container.Resolve<ICrypt>());
            }
            Client.Send(mjm.Jsonal());
        }
    }
}
