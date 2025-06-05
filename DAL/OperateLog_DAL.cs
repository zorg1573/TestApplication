using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.MODEL;

namespace TestApp.DAL
{
    public class OperateLog_DAL
    {
        public int InsertOperateLog_DT(string operation, string description)
        {
            string updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = $@"insert operate_log
	                ([operation],[description],[update_time])  
                values('{operation}','{description}','{updateTime}')";
            return Dapper.DbHelper.UpdateBySql(sql);
        }
    }
}
