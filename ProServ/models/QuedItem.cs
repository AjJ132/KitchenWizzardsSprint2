using SQLite;
using System;


namespace ProServ.models;
public class QueuedItem
{
    [PrimaryKey, AutoIncrement]
    public int index { get; set; }

    [NotNull]
    public int ItemId { get; set; }

    [NotNull]
    public string ItemName { get; set; }

    [NotNull]
    public int TabId { get; set; }

    [NotNull]
    public int TableId { get; set; }

    [NotNull]
    public DateTime TimeInserted { get; set; }

    [NotNull]
    public bool IsReady { get; set; }

    public QueuedItem()
    {

    }

    public QueuedItem(int itemId, int tabId, int tableId, string itemName)
    {
        ItemId = itemId;
        TabId = tabId;
        TableId = tableId;
        TimeInserted = DateTime.Now;
        IsReady = false;
        ItemName = itemName;
    }

   

}
