using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Transactions;
using System.Windows.Controls.Primitives;
using SQLite;

namespace ProServ.models
{
    public class CustomerTab : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int tabId { get; set; } = 1001;
        public int customerId { get; set; }

        public bool tabClosed { get; set; }

        //String is beacuase SQLite does not have a DateTime type
        public String dateOpened { get; set; }

        public String dateTimeClosed { get; set; }

        
        private ObservableCollection<Item> _items;
        [Ignore]
        public ObservableCollection<Item> items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged(nameof(items));
                }
            }
        }


        public double tabTotal { get; set; }

        public int tableId { get; set; }

        [Ignore]
        public bool inDatabase { get; set; }

        public CustomerTab()
        {
            this.items = new ObservableCollection<Item>();
            this.tabTotal = 0;
        }
        public CustomerTab(int tableId)
        {
            this.items = new ObservableCollection<Item>();
            this.tabTotal = 0;
            this.tableId = tableId;
            this.customerId = 0;
            this.tabClosed = false;
            this.dateOpened = DateTime.Now.ToString();
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double GetTabTotal()
        {
            foreach (var item in items)
            {
                //this.tabTotal += item.
    
            }

            return tabTotal;
        }

        public async void RemoveItemById(Item itemToRemove)
        {
            if (itemToRemove != null)
            {
                this.items.Remove(itemToRemove);

                //Update customer tab in database
                ForeignItem foreignItem = await GlobalAccess.globalAccess.dbManager.GetForeignItemByTabAndItemId(tabId, itemToRemove.itemId);

                //Update foreign item tables
                await GlobalAccess.globalAccess.dbManager.DeleteForeignItem(foreignItem);

                this.items.Remove(itemToRemove);


                return;
            }
            else { return; }
            
        }
    }
}
