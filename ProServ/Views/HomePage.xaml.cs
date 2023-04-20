﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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

namespace ProServ.Views
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    /// 

    

    public partial class HomePage : Page , INotifyPropertyChanged
    {
        public List<TableControl> tableControls { get; set; }

        public TableControl selectedTable { get; set; }

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



        public HomePage()
        {

            InitializeComponent();

            SetTableControls();
            AddTableControls();

            DataContext = this;

            
        }

        //tables are imported and each table control is assigned a table
        public async Task<Task> SetTableControls()
        {
            List<models.Table> tables = GlobalAccess.globalAccess.GetTables();
            tableControls = new List<TableControl>();
            
            foreach(var i  in tables)
            {
                /*
                if (i.tableStatus == 1)
                {
                    var tab = await GlobalAccess.globalAccess.dbManager.GetOpenTabByTableId(i.tableId);
                    i.SetCustomerTab(tab);
                } */

                TableControl tableControl = new TableControl(i);
                tableControl.MouseLeftButtonDown += TableControl_LeftMouseDown;
                tableControls.Add(tableControl);

                
            }

            tables = null;
            return Task.CompletedTask;
        }

        //table controls are added to the homepage
        public Task AddTableControls()
        {
            var grid = this.TableGrid;

            foreach(var i in tableControls)
            {

                this.TableGrid.Children.Add(i);
                Grid.SetColumn(i, i.table.columnID);
                Grid.SetRow(i, i.table.rowID);
            }
            
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
            }

            Debug.WriteLine("Selected Table: " + this.selectedTable.table.tableId);
        }

        private async void ShowCustomerTab()
        {
            this.selectedCustomerTab = selectedTable.table.currentTab as CustomerTab;
            string s = "";
        }

        private async void AllowCreateCustomerTab()
        {
            Debug.WriteLine("You can create a tab");
        }






        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }




    
}
