using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public interface IMessageBussiness
    {
        IMessageModel Work(IMessageModel messageModel);

        string GetKey();
    }
}
