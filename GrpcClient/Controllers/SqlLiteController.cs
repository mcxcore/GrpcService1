using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SqlLiteController : ControllerBase
    {
        public void InsertData() 
        {
            SQLiteConnection conn = null;

            string dbPath = "Data Source =" + "D://Downloads//test.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置
            conn.Open();//打开数据库，若文件不存在会自动创建

            SQLiteCommand cmdInsert = new SQLiteCommand(conn);

            for (var i = 2; i < 100000; i++) 
            {
                cmdInsert.CommandText = "INSERT INTO test VALUES("+i+", '小红')";//插入几条数据
                cmdInsert.ExecuteNonQuery();
            }
            
        }
    }
}
