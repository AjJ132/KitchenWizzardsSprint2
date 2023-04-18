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

           WindowState = WindowState.Normal;
           //disallow resizing  
              ResizeMode = ResizeMode.NoResize;

            Debug.WriteLine("MainWindow.xaml.cs");

            Loaded += (s, e) => { MainFrame.NavigationService.Navigate(new Login()); };
            
            

        }


    }
}
