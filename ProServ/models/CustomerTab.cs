using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Windows.Controls.Primitives;
using SQLite;

namespace ProServ.models
{
    public class CustomerTab
    {
        [PrimaryKey, AutoIncrement]
        public int tabId { get; set; } = 1001;
        public int customerId { get; set; }

        public bool tabClosed { get; set; }

        //String is beacuase SQLite does not have a DateTime type
        public String dateOpened { get; set; }

        public String dateTimeClosed { get; set; }

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
            this.dateOpened = DateTime.Now.ToString();
        }

      

        public double GetTabTotal()
        {
            foreach (var item in items)
            {
                //this.tabTotal += item.
    
            }

            return tabTotal;
        }

        public async void RemoveItemById(int itemID)
        {
            Item itemToRemove = this.items.Where(n => n.itemId == itemID).FirstOrDefault();
            if (itemToRemove != null)
            {
                this.items.Remove(itemToRemove);

                //Update customer tab in database
                await GlobalAccess.globalAccess.dbManager.UpdateCustomerTab(this);

                //Update foreign item tables
                List<ForeignItem> itemsUnderTab = await GlobalAccess.globalAccess.dbManager.GetForeignItemByTabAndItemId(this.tabId, itemID);

                await GlobalAccess.globalAccess.dbManager.DeleteForeignItem(itemsUnderTab[0]);


                return;
            }
            else { return; }
            
        }
    }
}
