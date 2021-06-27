using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerStoreAPI.Data
{
    public class ElephantSqlConnectionString
    {
        public static string Get()
        {
            var uriString = "postgres://bdyktwyr:aSRcMlA6bCeTBBfSACwx_PvQ-T5kn_cZ@satao.db.elephantsql.com:5432/bdyktwyr";
            var uri = new Uri(uriString);
            var db = uri.AbsolutePath.Trim('/');
            var user = uri.UserInfo.Split(':')[0];
            var passwd = uri.UserInfo.Split(':')[1];
            var port = uri.Port > 0 ? uri.Port : 5432;
            var connStr = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}",
                uri.Host, db, user, passwd, port);
            return connStr;
        }
    }
}
// Метод преобразовывает uriString в connectionString

//Server=satao.db.elephantsql.com;Database=bbzvbrxc;User Id=bbzvbrxc;Password=3UDkrnG03oEpr9xB7qpZ2QngL0DVWZX4;Port=5432
//Server=satao.db.elephantsql.com;Database=bdyktwyr;User Id=bdyktwyr;Password=aSRcMlA6bCeTBBfSACwx_PvQ-T5kn_cZ;Port=5432

// postgres://kqgqgbwk:P3vPUl6i2Mjv9twMnGtbA7BJviWSNYW6@satao.db.elephantsql.com:5432/kqgqgbwk

//Server=satao.db.elephantsql.com;Database=kqgqgbwk;User Id=kqgqgbwk;Password=P3vPUl6i2Mjv9twMnGtbA7BJviWSNYW6;Port=5432

//postgres://kqgqgbwk:P3vPUl6i2Mjv9twMnGtbA7BJviWSNYW6@satao.db.elephantsql.com:5432/kqgqgbwk

//Server = satao.db.elephantsql.com; Database = kqgqgbwk; User Id = kqgqgbwk; Password = P3vPUl6i2Mjv9twMnGtbA7BJviWSNYW6; Port = 5432