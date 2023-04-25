using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using ProServ.models;
using SQLite;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

public class OrderQueue : INotifyPropertyChanged, INotifyCollectionChanged
{
    private readonly ObservableCollection<QueuedItem> queue = new ObservableCollection<QueuedItem>();
    
    private readonly SQLiteAsyncConnection _connection;

    public event PropertyChangedEventHandler PropertyChanged;
    public event NotifyCollectionChangedEventHandler CollectionChanged;

    public ReadOnlyObservableCollection<QueuedItem> QueuedItems => new ReadOnlyObservableCollection<QueuedItem>(queue);


    public OrderQueue(SQLiteAsyncConnection connection)
    {
        _connection = connection;
        LoadQueueFromDatabaseAsync().ConfigureAwait(false);
    }

    public async Task AddOrderAsync(QueuedItem order)
    {
        queue.Add(order);
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, order));

        await _connection.InsertAsync(order).ConfigureAwait(false);
        NotifyPropertyChanged(nameof(Count));
        NotifyPropertyChanged(nameof(IsEmpty));
        NotifyPropertyChanged(nameof(NextOrder));
    }

    public async Task RemoveOrderAsync(QueuedItem order)
    {
        if (!queue.Contains(order))
            return;

        queue.Remove(order);
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, order));

        await _connection.DeleteAsync(order).ConfigureAwait(false);
        NotifyPropertyChanged(nameof(Count));
        NotifyPropertyChanged(nameof(IsEmpty));
        NotifyPropertyChanged(nameof(NextOrder));
    }


    public async Task<QueuedItem> GetNextOrderAsync()
    {
        QueuedItem order = queue[0];
        queue.RemoveAt(0);
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, order));
        await _connection.DeleteAsync(order).ConfigureAwait(false);
        NotifyPropertyChanged(nameof(Count));
        NotifyPropertyChanged(nameof(IsEmpty));
        NotifyPropertyChanged(nameof(NextOrder));
        return order;
    }

    public QueuedItem NextOrder => queue.Count > 0 ? queue[0] : default(QueuedItem);

    public int Count => queue.Count;

    public bool IsEmpty => queue.Count == 0;

    public async Task MoveOrderToTopAsync(QueuedItem order)
    {
        if (!queue.Contains(order))
            return;

        int index = queue.IndexOf(order);
        if (index > 0)
        {
            queue.Move(index, 0);
            await _connection.UpdateAsync(order).ConfigureAwait(false);
            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(nameof(IsEmpty));
            NotifyPropertyChanged(nameof(NextOrder));
        }
    }

    public async Task MoveOrderToBottomAsync(QueuedItem order)
    {
        if (!queue.Contains(order))
            return;

        int index = queue.IndexOf(order);
        if (index >= 0 && index < queue.Count - 1)
        {
            queue.Move(index, queue.Count - 1);
            await _connection.UpdateAsync(order).ConfigureAwait(false);
            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged(nameof(IsEmpty));
            NotifyPropertyChanged(nameof(NextOrder));
        }
    }



    private async Task LoadQueueFromDatabaseAsync()
    {
        List<QueuedItem> orders = await _connection.Table<QueuedItem>().ToListAsync().ConfigureAwait(false);
        foreach (QueuedItem order in orders)
        {
            queue.Add(order);
        }

        NotifyPropertyChanged(nameof(Count));
        NotifyPropertyChanged(nameof(IsEmpty));
        NotifyPropertyChanged(nameof(NextOrder));
    }


    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    
}
