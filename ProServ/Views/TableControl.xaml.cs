using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            DataContext = this;
            ConfigureContextMenu();
        }

        public void SetAsSelected()
        {
            this.TableRectangle.Height = 120;
            this.TableRectangle.Width = 120;
        }

        public void Unselect()
        {
            this.TableRectangle.Width = 100;
            this.TableRectangle.Height = 100;
        }


        private void ConfigureContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem item1 = new MenuItem { Header = "Check Table", FontSize = 16, Height = 40, Width = 200, Padding = new Thickness(5) };
            item1.Click += Item1_Click;
            contextMenu.Items.Add(item1);

            MenuItem item2 = new MenuItem { Header = "Mark as dirty", FontSize = 16, Height = 40, Width = 200, Padding = new Thickness(5) };
            item2.Click += Item2_Click;
            contextMenu.Items.Add(item2);

            MenuItem item3 = new MenuItem { Header = "Not sure yet", FontSize = 16, Height = 40, Width = 200, Padding = new Thickness(5) };
            item3.Click += Item3_Click;
            contextMenu.Items.Add(item3);

            this.ContextMenu = contextMenu;
        }


        private void Item1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Check table");
        }

        private void Item2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mark as dirty");
        }

        private void Item3_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("not sure yet");
        }


    }
}
