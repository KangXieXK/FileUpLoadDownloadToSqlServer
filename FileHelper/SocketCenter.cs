﻿using SAEA.FileSocket;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Autofac;

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
        public async void StartWsserver(int port)
        {
            builder.RegisterType<Aes>().As<ICrypt>();
            builder.RegisterType<MessageJsonModel>().As<IMessageModel>();
            builder.RegisterType<MessageJsonModel>().As<IMessage>();
            container = builder.Build();

            Server = new WsServer();
            Server.MessageAction += new Action<SuperSocket.WebSocket.Server.WebSocketSession,
                SuperSocket.WebSocket.WebSocketPackage>((session, msg) =>
                {
                    using (var scope = container.BeginLifetimeScope())
                    {
                        var imm = scope.Resolve<IMessageModel>();
                        var result = imm.Objectal(msg.Message);
                        result.DecryptSelf(container.Resolve<ICrypt>());
                        Console.WriteLine(result.Jsonal());
                    }
                });
            await Server.Start();
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
                        var imm = scope.Resolve<IMessageModel>();
                        var result = imm.Objectal(msg.Text);
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
