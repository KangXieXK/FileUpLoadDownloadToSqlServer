using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileUpdateFrameWork
{
    public class Create
    {
        SqlSugarClient db;
        public void InitConnect()
        {
             db = new SqlSugarClient(
   new ConnectionConfig()
   {
       ConnectionString = "server=172.17.23.156;uid=sa;pwd=clims1.0;database=FICS_GMYG",
       DbType = DbType.SqlServer,//设置数据库类型
       IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
       InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
   });
        }

        public void CreateTemplate()
        {
            db.DbFirst.CreateClassFile(@"D:\doc\2021\FileUpdate\UpdateForCSKYPF\FileUpdateFrameWork\DataModels", "FileUpdateFrameWork");
            db.DbFirst.Where("SYS_FILEVERSION").CreateClassFile(@"D:\doc\2021\FileUpdate\UpdateForCSKYPF\FileUpdateFrameWork\DataModels2", "FileUpdateFrameWork");
        }
    }
}
