using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace ProServ.models
{

    public class DBLog
    {


        [PrimaryKey, AutoIncrement]
        public int LogId { get; set; }
        public string LogAction { get; set; }

        public string Employee { get; set; }

        public string LogDateTime { get; set; }

        public DBLog()
        {

        }

        public DBLog(string logAction, string employee)
        {
            LogAction = logAction;  
            Employee = employee;
            LogDateTime = DateTime.Now.ToString();
        }

      
    }
}
