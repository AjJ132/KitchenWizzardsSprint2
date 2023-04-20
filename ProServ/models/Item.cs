using System;
using SQLite;

namespace ProServ.models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int itemId { get; set; }

        public string itemName { get; set; }

        public int categoryId { get; set; }

        public double itemPrice { get; set; }

        public Item()
        {
        }

        public Item(string itemName)
        {
            this.itemName = itemName;
        }

        public Item(string itemName, int categoryId, double itemPrice)
        {
            this.itemName = itemName;
            this.categoryId = categoryId;
            this.itemPrice = itemPrice;

        }
    }
}
