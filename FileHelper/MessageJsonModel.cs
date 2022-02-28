using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public class MessageJsonModel : IMessageModel
    {
        public int BussinessID { get; set; }

        public int BussinessResult { get; set; }

        public string Key { get; set; }

        public object Content { get; set; }
        public bool IsEncrpyt { get; set; }

        public object Response { get; set; }
        public void DecryptSelf(ICrypt crypt)
        {
            if (Content != null)
            {
                try
                {
                    Content = crypt.Decrypt(Content.ToString());
                }
                catch
                {
                    throw new Exception("解密失败");
                }
                IsEncrpyt = false;
            }
            else
            {
                IsEncrpyt = true;
            }
        }

        public void EncryptSelf(ICrypt crypt)
        {
            if (Content != null)
            {
                Content = crypt.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(Content));
                IsEncrpyt = true;
            }
            else
            {
                IsEncrpyt = false;
            }
        }

        public string Jsonal()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public IMessageModel Objectal(string str)
        {
            IMessageModel mjm = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageJsonModel>(str);
            return mjm;
        }

        public string GetKey()
        {
            return this.Key;
        }

        public T GetContentChange<T>()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Content.ToString());
        }

        public void MessageErrorResponse()
        {
            this.Response = "ErrorMessage";
        }

        public void SetMessageResponse(object value)
        {
            this.Response = value;
        }
    }
}
