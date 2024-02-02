using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SqlServerController : ControllerBase
    {
        public void InsertData()
        {
            string connectionString = "Server=DESKTOP-Q17JBL1;Database=admindb;User Id=sa;Password=xuyi;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("数据库连接成功!");
                    // 这里可以执行查询、插入、更新、删除等操作
                }
                catch (SqlException e)
                {
                    Console.WriteLine("数据库连接失败: " + e.Message);
                }

                SqlCommand cmdInsert = connection.CreateCommand();

                for (var i = 2; i < 100000; i++)
                {
                    cmdInsert.CommandText = "INSERT INTO test VALUES(" + i + ", '小红')";//插入几条数据
                    cmdInsert.ExecuteNonQuery();
                }
            }

           

        }
    }
}
