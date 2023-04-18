using ProServ.models;
using System;
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
using static SQLite.SQLite3;

namespace ProServ.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {

            Tuple<bool, int> result = await CheckLoginCredentials();
            if (result.Item1)
            {
                this.LoginMessage_lb.Content = "Login Successful";



                //if login is successful then navigate to the home page and set the current user to the user logging in
                HomePage homePage = new HomePage();
                Employee currentEmployee = await GlobalAccess.globalAccess.dbManager.GetEmployeeByID(result.Item2);

                if(currentEmployee == null)
                {
                    Debug.WriteLine("Employee Returned Null");
                    this.LoginMessage_lb.Content = "There was an error in retrieving your information. Please contact system administrator";
                    return;
                }

                GlobalAccess.globalAccess.LogIn(currentEmployee);
                
                NavigationService.Navigate(homePage);
               
            }
            else
            {
                //display error message for incorrect login info
                this.LoginMessage_lb.Content = "Incorrect username or password";
            }

            
            
        }

        private async Task<Tuple<bool, int>> CheckLoginCredentials()
        {
            string username = this.Username_tb.Text;
            string password = this.Password_tb.Text;


            Debug.WriteLine("Checking for username: " + username);

            int userID = await GlobalAccess.globalAccess.dbManager.CheckUsername(username);

            if(userID != -1)
            {
                bool passwordMatch = await GlobalAccess.globalAccess.dbManager.CheckPasswordMatch(password, userID);
                if(passwordMatch)
                {
                    //passwords match so return true
                    return Tuple.Create(true, userID);
                }
                else
                {
                    //return false due to incorrect password
                    return Tuple.Create(false, -1);
                }
            }
            else
            {
                //username was not found so return false
                return Tuple.Create(false,-1);
            }


        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //This is temporary code to bypass typing in a password. This autmatically selects a user and uses that as a login
            HomePage homePage = new HomePage();
            var emplpoyees = await GlobalAccess.globalAccess.dbManager.GetEmployees();
            Employee current = emplpoyees.FirstOrDefault();

            if (current == null)
            {
                Debug.WriteLine("Employee Returned Null");
                this.LoginMessage_lb.Content = "There was an error in retrieving your information. Please contact system administrator";
                return;
            }

            GlobalAccess.globalAccess.LogIn(current);

            NavigationService.Navigate(homePage);
        }
    }
}
