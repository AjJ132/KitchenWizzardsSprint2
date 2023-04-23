using System;
using SQLite;

namespace ProServ.models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int itemId { get; set; }

        public string itemName { get; set; }

        public string categoryName { get; set; }

        public double itemPrice { get; set; }

        [Ignore]
        public string imagePath { get; set; }

        
        public Item()
        {
            this.imagePath = "/Images/" + this.itemName + ".jpg";
        }

        public Item(string itemName)
        {
            this.itemName = itemName;

            this.imagePath = "/Images/" + this.itemName + ".jpg";
        }

        public Item(string itemName, string category, double itemPrice)
        {
            this.itemName = itemName;
            this.categoryName = category;
            this.itemPrice = itemPrice;

            this.imagePath = "/Images/" + this.itemName + ".jpg";

        }

        public void InitImagePath()
        {
            this.imagePath = "/Images/" + this.itemName + ".jpg";
        }
    }
}
