using ProServ.models;
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
    /// Interaction logic for MenuItemControl.xaml
    /// </summary>
    public partial class MenuItemControl : UserControl
    {

        
        public List<Item> Appetizers { get; set; }
        public List<Item> Salads { get; set; }
        public List<Item> Entrees { get; set; }
        public List<Item> Sides { get; set; }
        public List<Item> Sandwhiches { get; set; }
        public List<Item> Wraps { get; set; }
        public List<Item> Burgers { get; set; }
        public List<Item> Beverages { get; set; }


        public MenuItemControl()
        {
            

            InititalizeFoodLists();

            
            InitializeComponent();

            DataContext = this;
        }

        public async void InititalizeFoodLists()
        {
            var foodItems = await GlobalAccess.globalAccess.dbManager.GetItems();

            foreach(var i in foodItems)
            {
                i.InitImagePath();
            }

            //get all food items in list and add them to their respective food groups

            this.Appetizers = foodItems.Where(n => n.categoryName.Equals("Appetizers")).ToList();
            this.Salads = foodItems.Where(n => n.categoryName.Equals("Salads")).ToList();
            this.Entrees = foodItems.Where(n => n.categoryName.Equals("Entrees")).ToList();
            this.Sides = foodItems.Where(n => n.categoryName.Equals("Sides")).ToList();
            this.Sandwhiches = foodItems.Where(n => n.categoryName.Equals("Sandwhiches")).ToList();
            this.Wraps = foodItems.Where(n => n.categoryName.Equals("Wraps")).ToList();
            this.Burgers = foodItems.Where(n => n.categoryName.Equals("Burgers")).ToList();
            this.Beverages = foodItems.Where(n => n.categoryName.Equals("Beverages")).ToList();

            

            return;
        }

        
    }
}
