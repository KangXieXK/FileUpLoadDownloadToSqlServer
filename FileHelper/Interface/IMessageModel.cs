using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public interface IMessageModel: IMessage,IModelBussiness
    {

        IMessageModel Objectal(string str);
        void EncryptSelf(ICrypt crypt);

        void DecryptSelf(ICrypt crypt);


    }
}
