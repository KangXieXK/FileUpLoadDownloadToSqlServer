using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public interface ICrypt
    {
        string Encrypt(string str, string key);
        string Decrypt(string str, string key);

        string Encrypt(string str);

        string Decrypt(string str); 
    }
}
