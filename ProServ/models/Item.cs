using System;
using SQLite;

public class Item
{
    [PrimaryKey, AutoIncrement]
    public int itemId { get; set; }



	public Item()
	{
	}
}
