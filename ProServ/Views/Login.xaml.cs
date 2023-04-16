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
            if (await CheckLoginCredentials())
            {
                this.LoginMessage_lb.Content = "Login Successful";

                //if login is successful then navigate to the home page
                HomePage homePage = new HomePage();
                NavigationService.Navigate(homePage);
            }
            else
            {
                //display error message for incorrect login info
                this.LoginMessage_lb.Content = "Incorrect username or password";
            }

            
            
        }



        private async Task<bool> CheckLoginCredentials()
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
                    return true;
                }
                else
                {
                    //return false due to incorrect password
                    return false;
                }
            }
            else
            {
                //username was not found so return false
                return false;
            }


        }
    }
}
