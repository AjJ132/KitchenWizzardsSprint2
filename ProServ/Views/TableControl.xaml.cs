using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
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
using Zone = ProServ.models.Zone;

namespace ProServ.Views
{
    /// <summary>
    /// Interaction logic for TableControl.xaml
    /// </summary>
    public partial class TableControl : UserControl , INotifyPropertyChanged
    {
        public static readonly DependencyProperty TableProperty = DependencyProperty.Register(
                nameof(models.Table),
                typeof(models.Table),
                typeof(TableControl),
                new PropertyMetadata(null));

        public event PropertyChangedEventHandler? PropertyChanged;


        public models.Table table
        {
            get { return (models.Table)GetValue(TableProperty); }
            set { SetValue(TableProperty, value); }
        }

        public TableControl(ProServ.models.Table table)
        {
            this.table = table;
            InitializeComponent();

            if(this.table.tableStatus != 0)
            {
                Debug.WriteLine("Table: " + this.table.tableId + " is taken");
            }

            DataContext = this;
        }

        public void SetAsSelected()
        {
            this.TableRectangle.Height = 110;
            this.TableRectangle.Width = 110;
            this.TableRectangle.StrokeThickness = 5;
            this.TableRectangle.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));
        }

        public void Unselect()
        {
            this.TableRectangle.Height = 100;
            this.TableRectangle.Width = 100;
            this.TableRectangle.StrokeThickness = 1;
            this.TableRectangle.Stroke = Brushes.Black;
        }
    

        

        private async void Check_Table_Click(object sender, RoutedEventArgs e)
        {
            //logic to check customer into their table
            CustomerTab newTab = new CustomerTab(this.table.tableId);

            //Write to database
            await GlobalAccess.globalAccess.dbManager.InsertCustomerTab(newTab);

            this.table.SetCustomerTab(newTab);

            this.table.tableStatus = 1;

            await GlobalAccess.globalAccess.dbManager.UpdateTable(this.table);
        }

        private void Mark_As_Dirty_Click(object sender, RoutedEventArgs e)
        {
            this.table.tableStatus = 2;

            GlobalAccess.globalAccess.dbManager.UpdateTable(this.table);

            this.TableRectangle.Fill = Brushes.Red;
        }

        private void Mark_As_Clean(object sender, RoutedEventArgs e)
        {
            this.table.tableStatus = 0;

            GlobalAccess.globalAccess.dbManager.UpdateTable(this.table);

            this.TableRectangle.Fill = Brushes.Green;
        }

       

       



    }
}

