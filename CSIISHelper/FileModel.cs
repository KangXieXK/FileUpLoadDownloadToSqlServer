using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSIISHelper
{
    public class FileModel
    {
        public int ID { get; set; } // int
        public string VERSION { get; set; } // varchar(50)
        public string FILENAME { get; set; } // varchar(200)
        public string MD5 { get; set; } // varchar(1024)
        public byte[] IMG { get; set; } // image
        public string ProgramName { get; set; } // varchar(50)
        public DateTime? UploadTime { get; set; } // datetime
    }
}
