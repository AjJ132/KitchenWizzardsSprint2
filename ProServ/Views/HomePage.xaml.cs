﻿using System;
using System.Collections.Generic;
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

    

    public partial class HomePage : Page
    {
        public List<TableControl> tableControls { get; set; }

        public TableControl selectedTable { get; set; }



        public HomePage()
        {

            InitializeComponent();

            SetTableControls();
            AddTableControls();

            
        }

        //tables are imported and each table control is assigned a table
        public Task SetTableControls()
        {
            List<models.Table> tables = GlobalAccess.globalAccess.GetTables();
            tableControls = new List<TableControl>();
            
            foreach(var i  in tables)
            {
                TableControl tableControl = new TableControl(i);
                tableControl.MouseDown += TableControl_MouseDown;
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
        private void TableControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(selectedTable != null)
            {
                selectedTable.Unselect();
            }

            //sets selected table to the table control that was clicked
            selectedTable = sender as TableControl;
            selectedTable.SetAsSelected();

            Debug.WriteLine("Selected Table: " + this.selectedTable.table.tableId);
        }
        


    }




    
}
