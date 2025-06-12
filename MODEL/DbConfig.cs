using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.MODEL
{
    public class DbConfig
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }

        public string GetConnectionString()
        {
            return $"Server={Server};Database={Database};User Id={UserId};Password={Password};";
        }
    }

}
