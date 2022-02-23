using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public class TelnetServer : AppServer<TelnetSession>
    {

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            var m_PolicyFile = config.Options.GetValue("policyFile");

            //if (string.IsNullOrEmpty(m_PolicyFile))
            //{
            //    if (Logger.IsErrorEnabled)
            //        Logger.Error("Configuration option policyFile is required!");
            //    return false;
            //}

            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }
    }
}
