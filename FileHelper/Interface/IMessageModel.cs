using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public interface IMessageModel : IMessage, IModelBussiness
    {
        public int BussinessID { get; set; }

        public int BussinessResult { get; set; }

        public string Key { get; set; }
        object Content { get; set; }

        object Response { get; set; }
        IMessageModel Objectal(string str);
        void EncryptSelf(ICrypt crypt);
        void DecryptSelf(ICrypt crypt);
        T GetContentChange<T>();
        void MessageErrorResponse();
        void SetMessageResponse(object value);
    }
}
