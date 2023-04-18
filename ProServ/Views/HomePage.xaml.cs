using System;
using System.Collections.Generic;
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
                tableControls.Add(new TableControl(i));
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
    }
}
