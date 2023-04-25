using ProServ.models;
using System;
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
using static SQLite.SQLite3;

namespace ProServ.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page , INotifyPropertyChanged
    {
        private string _inputNumber;
        public string InputNumber
        {
            get => _inputNumber;
            set
            {
                _inputNumber = value;
                OnPropertyChanged(nameof(InputNumber));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;



        public Login()
        {
            InitializeComponent();
            
            this.DataContext = this;
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

                if(currentEmployee.employeeType.Equals("Manager"))
                {
                    ManagerView view = new ManagerView();
                    NavigationService.Navigate(view);
                }
                else if(currentEmployee.employeeType.Equals("Chef"))
                {
                    //need logic for chefs page!!!
                    string s = ";";
                }
                else
                {
                    NavigationService.Navigate(homePage);
                }
                
                
               
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

        private void NumberButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string number = button.Content.ToString();

            if (number == "↵")
            {
                if (InputNumber.Length > 0)
                {
                    InputNumber = InputNumber.Substring(0, InputNumber.Length - 1);
                }
            }
            else
            {
                InputNumber += number;
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void LoginViaPinBtn_Click(object sender, RoutedEventArgs e)
        {
            var userPin = 0;
            try
            {
                userPin = Convert.ToInt32(InputNumber);
                
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }

            Employee currentEmployee = await GlobalAccess.globalAccess.dbManager.GetEmployeeByPin(userPin);

            if(currentEmployee != null)
            {
                //if login is successful then navigate to the home page and set the current user to the user logging in
                HomePage homePage = new HomePage();

                GlobalAccess.globalAccess.LogIn(currentEmployee);

                if (currentEmployee.employeeType.Equals("Manager"))
                {
                    NavigationService.Navigate(homePage);
                }
                else if (currentEmployee.employeeType.Equals("Chef"))
                {
                    //need logic for chefs page!!!
                    string s = ";";
                }
                else
                {
                    NavigationService.Navigate(homePage);
                }
            }
            else
            {
                Debug.WriteLine("Employee Returned Null");
                this.LoginMessage_lb.Content = "There was an error in retrieving your information. Please contact system administrator";
                return;
            }
        }

        private void BackspaceButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputNumber))
            {
                InputNumber = InputNumber.Remove(InputNumber.Length - 1);
            }
        }

        private async void bypass_Click(object sender, RoutedEventArgs e)
        {

            var manager = await GlobalAccess.globalAccess.dbManager.GetEmployeeByID(4);
            GlobalAccess.globalAccess.LogIn(manager);
            ManagerView view = new ManagerView();
            NavigationService.Navigate(view);
        }
    }
}
