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
        public void DecryptSelf(ICrypt crypt)
        {
            if (Content != null)
            {

                Content = crypt.Decrypt(Content.ToString());
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
                Content = crypt.Encrypt(Content.ToString());
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
    }
}
