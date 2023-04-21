using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProServ.models
{
    public class ForeignItem
    {
        [PrimaryKey, AutoIncrement]
        public int index { get; set; }

        
        public int itemId { get; set; }

        public int customerTabId { get; set; }

        public ForeignItem()
        {

        }

        public ForeignItem(int itemId,int customerTabId)
        {
            this.itemId = itemId;
            this.customerTabId = customerTabId;
        }
        
    }
}
