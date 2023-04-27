using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProServ.models;
using SQLite;

namespace ProServ.Views
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    /// 

    

    public partial class HomePage : Page , INotifyPropertyChanged
    {
        public List<TableControl> tableControls { get; set; }

        public ProServ.Views.TableControl selectedTable { get; set; }


        //this is for data-binding and UI updating dont remove <-- INotifyPropertyChanged
        private CustomerTab _selectedCustomerTab;
        public CustomerTab selectedCustomerTab
        {
            get { return _selectedCustomerTab; }
            set
            {
                if (_selectedCustomerTab != value)
                {
                    _selectedCustomerTab = value;
                    OnPropertyChanged(nameof(selectedCustomerTab));
                }
            }
        }


        private OrderQueue _orderQueue;

        public OrderQueue OrderQueue
        {
            get => _orderQueue;
            set
            {
                _orderQueue = value;
                NotifyPropertyChanged();
            }
        }



        public List<Item> Appetizers { get; set; }
        public List<Item> Salads { get; set; }
        public List<Item> Entrees { get; set; }
        public List<Item> Sides { get; set; }
        public List<Item> Sandwiches { get; set; }
        public List<Item> Wraps { get; set; }
        public List<Item> Burgers { get; set; }
        public List<Item> Beverages { get; set; }



        public HomePage()
        {

            InitializeComponent();

            SQLiteAsyncConnection connection = GlobalAccess.globalAccess.dbManager.GetConnection();

            OrderQueue = new OrderQueue(connection);

            _ = InitializeAndAddTableControls();

            _ = InititalizeFoodLists();


            DataContext = this;

            
        }


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<Task> InitializeAndAddTableControls()
        {
            List<models.Table> tables = GlobalAccess.globalAccess.GetTables();
            tableControls = new List<TableControl>();

            foreach (var i in tables)
            {
               

                if (i.tableStatus == 1)
                {
                    var tab = await GlobalAccess.globalAccess.dbManager.GetOpenTabByTableId(i.tableId).ConfigureAwait(false);
                    var foreignItems = await GlobalAccess.globalAccess.dbManager.GetForeignItemByTabId(tab.tabId).ConfigureAwait(false);


                    foreach(var n in foreignItems)
                    {
                        Item item = await GlobalAccess.globalAccess.dbManager.GetItemByID(n.itemId);
                        tab.items.Add(item);
                    }
                    
                    i.SetCustomerTab(tab);
                    i.currentTab.GetTabTotal();
                }

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    TableControl tableControl = new TableControl(i);
                    tableControl.MouseLeftButtonDown += TableControl_LeftMouseDown;
                    tableControls.Add(tableControl);

                    // Add the table control to the homepage
                    this.TableGrid.Children.Add(tableControl);
                    Grid.SetColumn(tableControl, i.columnID);
                    Grid.SetRow(tableControl, i.rowID);
                });
            }

            tables = null;
            return Task.CompletedTask;
        }




        //event handlers

        //Left click on table control
        private void TableControl_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(selectedTable != null)
            {
                selectedTable.Unselect();
            }

            //sets selected table to the table control that was clicked

            selectedTable = sender as TableControl;
            selectedTable.SetAsSelected();

            if(selectedTable.table.tableStatus == 1)
            {
                ShowCustomerTab();
            }
            else
            {
                //Show logic to give option to create new tab
                if(this.selectedCustomerTab != null)
                {
                    this.selectedCustomerTab = null;
                }
            }

        }

        private void ShowCustomerTab()
        {
            this.selectedCustomerTab = selectedTable.table.currentTab;
            
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            var tab = this.selectedCustomerTab;
            if(tab != null) {
                try
                {
                    tab.tabClosed = true;
                    tab.dateTimeClosed = DateTime.Now.ToString();              

                    await GlobalAccess.globalAccess.dbManager.UpdateCustomerTab(tab);
                    this.selectedCustomerTab = null;


                    //Update table to dirty. Update database as well
                    var table = this.selectedTable.table;
                    table.currentTab = null;
                    table.tableStatus = 2;

                    await GlobalAccess.globalAccess.dbManager.UpdateTable(table);

                    this.selectedTable = null;
                    
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Exception When closing Tab: " + ex);
                }
            }
        }

        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var contextMenu = menuItem.Parent as ContextMenu;
            var stackPanel = contextMenu.PlacementTarget as StackPanel;
            var listBoxItem = FindParent<ListBoxItem>(stackPanel);

            // Get the data context (Item) of the ListBoxItem
            var item = listBoxItem.DataContext as Item;
            if (item != null)
            {
                this.selectedCustomerTab.RemoveItemById(item);
            }
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            if (parent == null) return null;

            T parentAsT = parent as T;
            return parentAsT ?? FindParent<T>(parent);
        }

        public async Task<Task> InititalizeFoodLists()
        {
            var foodItems = await GlobalAccess.globalAccess.dbManager.GetItems();

            foreach (var i in foodItems)
            {
                i.InitImagePath();
            }

            //get all food items in list and add them to their respective food groups

            this.Appetizers = foodItems.Where(n => n.categoryName.Equals("Appetizers")).ToList();
            this.Salads = foodItems.Where(n => n.categoryName.Equals("Salads")).ToList();
            this.Entrees = foodItems.Where(n => n.categoryName.Equals("Entrees")).ToList();
            this.Sides = foodItems.Where(n => n.categoryName.Equals("Sides")).ToList();
            this.Sandwiches = foodItems.Where(n => n.categoryName.Equals("Sandwiches")).ToList();
            this.Wraps = foodItems.Where(n => n.categoryName.Equals("Wraps")).ToList();
            this.Burgers = foodItems.Where(n => n.categoryName.Equals("Burgers")).ToList();
            this.Beverages = foodItems.Where(n => n.categoryName.Equals("Beverages")).ToList();



            return Task.CompletedTask;
        }

        private async void MenuItem_AddToTab(object sender, RoutedEventArgs e)
        {
            if(this.selectedTable != null && this.selectedCustomerTab != null)
            {
                //Add item to customer tab
              
                var clickedItem = (((sender as MenuItem).Parent as ContextMenu).PlacementTarget as StackPanel).DataContext as Item;


                //Update database
                Application.Current.Dispatcher.Invoke(() => this.selectedCustomerTab.items.Add(clickedItem));


                await GlobalAccess.globalAccess.dbManager.InsertForeignItem(new ForeignItem(clickedItem.itemId, this.selectedCustomerTab.tabId));

                QueuedItem qItem = new QueuedItem(clickedItem.itemId, this.selectedCustomerTab.tabId, this.selectedTable.table.tableId, clickedItem.itemName);

                this.OrderQueue.AddOrderAsync(qItem);
                this.selectedCustomerTab.GetTabTotal();

                ShowCustomerTab();

            }
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (stackPanel != null)
            {
                ContextMenu contextMenu = stackPanel.ContextMenu;
                if (contextMenu != null)
                {
                    // Open the context menu manually
                    contextMenu.PlacementTarget = stackPanel;
                    contextMenu.IsOpen = true;

                    // Mark the event as handled to prevent it from being processed further
                    e.Handled = true;
                }
            }
        }


        





        private async void MoveToBottom_Click(object sender, RoutedEventArgs e)
        {
            var queuedItem = (((sender as MenuItem).Parent as ContextMenu).PlacementTarget as Grid).DataContext as QueuedItem;
            await OrderQueue.MoveOrderToBottomAsync(queuedItem);
        }

        private async void RemoveFromQueue_Click(object sender, RoutedEventArgs e)
        {
            var queuedItem = (((sender as MenuItem).Parent as ContextMenu).PlacementTarget as Grid).DataContext as QueuedItem;
            await OrderQueue.RemoveOrderAsync(queuedItem);
        }

        private async void MoveToTop_Click(object sender, RoutedEventArgs e)
        {
            var queuedItem = (((sender as MenuItem).Parent as ContextMenu).PlacementTarget as Grid).DataContext as QueuedItem;


            // Call the MoveOrderToTopAsync method with the queuedItem
            await OrderQueue.MoveOrderToTopAsync(queuedItem);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            String s = "";
        }

        private void Log_Out_Click(object sender, RoutedEventArgs e)
        {
            if(GlobalAccess.globalAccess.LogOut())
            {
                Login loginPage = new Login();
                GlobalAccess.globalAccess.dbManager.LogNow("Logged Out");
                NavigationService.Navigate(loginPage);
            }
        }
    }





}
