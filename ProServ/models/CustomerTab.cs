using System;
using System.Collections.Generic;
using System.Transactions;
using SQLite;

namespace ProServ.models
{
    public class CustomerTab
    {
        [PrimaryKey, AutoIncrement]
        public int tabId { get; set; } = 1001;
        public int customerId { get; set; }

        public bool tabClosed { get; set; }

        public DateTime dateOpened { get; set; }

        public DateTime dateTimeClosed { get; set; }

        [Ignore]
        public List<ProServ.models.Item> items { get; set; }
        public double tabTotal { get; set; }

        public int tableId { get; set; }

        [Ignore]
        public bool inDatabase { get; set; }

        public CustomerTab()
        {
            this.items = new List<ProServ.models.Item>();
            this.tabTotal = 0;
        }
        public CustomerTab(int tableId)
        {
            this.items = new List<ProServ.models.Item>();
            this.tabTotal = 0;
            this.tableId = tableId;
            this.customerId = 0;
            this.tabClosed = false;
            this.dateOpened = DateTime.Now;
        }

      

        public double GetTabTotal()
        {
            foreach (var item in items)
            {
                //this.tabTotal += item.
    
            }

            return tabTotal;
        }
    }
}
