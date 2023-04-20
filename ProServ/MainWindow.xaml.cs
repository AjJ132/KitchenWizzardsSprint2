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
using System.Diagnostics;
using ProServ.models;
using ProServ.Views;

namespace ProServ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

           //disallow resizing  
              //ResizeMode = ResizeMode.NoResize;

            Debug.WriteLine("MainWindow.xaml.cs");

            //This loads the login page in the main window
            //MainWindow is the border or parent that holds a page
            Loaded += (s, e) => { MainFrame.NavigationService.Navigate(new Login()); };

            this.Loaded += MainWindow_Loaded;


        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.WorkArea.Height;
            this.Top = 0;
            this.Left = 0;
        }


    }
}
