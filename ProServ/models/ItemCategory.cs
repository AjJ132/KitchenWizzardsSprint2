using System;
using SQLite;

namespace ProServ.models
{
    public class ItemCategory
    {
        [PrimaryKey, AutoIncrement]
        public int categoryId { get; set; }
        public string categoryName { get; set; }



        public ItemCategory()
        {
        }

        public ItemCategory(string categoryName)
        {
            this.categoryName = categoryName;
        }
    }
}
